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
        foreach (var habit in savedHabits)
            habits.Add(habit);
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
        if (checkBox.BindingContext is Habit habit)
        {
            habit.IsDone = e.Value;
            await database.SaveHabitAsync(habit);
        }
    }

    async void OnDeleteClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var habit = (Habit)button.BindingContext;
        await database.DeleteHabitAsync(habit);
        habits.Remove(habit);
    }
}