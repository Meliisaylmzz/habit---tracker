using System.Collections.ObjectModel;

namespace HabitTracker;

public partial class MainPage : ContentPage
{
    readonly HabitDatabase database = new();
    readonly ObservableCollection<Habit> habits = new();

    public MainPage()
    {
        InitializeComponent();
        HabitList.ItemsSource = habits;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        habits.Clear();
        var savedHabits = await database.GetHabitsAsync();
        var today = DateTime.Today;

        foreach (var habit in savedHabits)
        {
            bool changed = false;
            var last = habit.LastCompletedDate?.Date;

            // Yeni bir gün başladıysa kutucuk boşalsın
            if (last != today && habit.IsDone)
            {
                habit.IsDone = false;
                changed = true;
            }

            // Dünden önce yapıldıysa (ya da hiç yapılmadıysa) seri kopmuştur
            if ((last is null || last < today.AddDays(-1)) && habit.Streak != 0)
            {
                habit.Streak = 0;
                changed = true;
            }

            if (changed)
                await database.SaveHabitAsync(habit);

            habits.Add(habit);
        }
    }

    async void OnAddClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(HabitEntry.Text))
            return;

        var habit = new Habit { Name = HabitEntry.Text };
        await database.SaveHabitAsync(habit);
        habits.Add(habit);
        HabitEntry.Text = "";
    }

    async void OnCheckChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkBox = (CheckBox)sender;
        if (checkBox.BindingContext is not Habit habit)
            return;

        var today = DateTime.Today;
        var last = habit.LastCompletedDate?.Date;

        if (e.Value)
        {
            // İşaretlendi: bugün daha önce sayılmadıysa seriyi hesapla
            if (last != today)
            {
                habit.Streak = (last == today.AddDays(-1)) ? habit.Streak + 1 : 1;
                habit.LastCompletedDate = today;
            }
        }
        else if (last == today)
        {
            // İşaret kaldırıldı: bugünkü sayımı geri al
            habit.Streak = Math.Max(0, habit.Streak - 1);
            habit.LastCompletedDate = habit.Streak > 0 ? today.AddDays(-1) : null;
        }

        habit.IsDone = e.Value;
        await database.SaveHabitAsync(habit);
    }

    async void OnDeleteClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var habit = (Habit)button.BindingContext;
        await database.DeleteHabitAsync(habit);
        habits.Remove(habit);
    }
}