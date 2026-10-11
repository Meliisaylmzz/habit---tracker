using System.Collections.ObjectModel;

namespace HabitTracker;

public partial class MainPage : ContentPage
{
    ObservableCollection<Habit> habits = new();

    public MainPage()
    {
        InitializeComponent();
        HabitList.ItemsSource = habits;
    }

    void OnAddClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(HabitEntry.Text))
            return;

        habits.Add(new Habit { Name = HabitEntry.Text });
        HabitEntry.Text = "";
    }

    void OnDeleteClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var habit = (Habit)button.BindingContext;
        habits.Remove(habit);
    }
}