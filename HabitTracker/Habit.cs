using System.ComponentModel;
using SQLite;

namespace HabitTracker;

public class Habit : INotifyPropertyChanged
{
    int streak;

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; } = "";
    public bool IsDone { get; set; }

    public int Streak
    {
        get => streak;
        set
        {
            if (streak == value)
                return;

            streak = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Streak)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StreakText)));
        }
    }

    public DateTime? LastCompletedDate { get; set; }

    [Ignore]
    public string StreakText => Streak > 0 ? $"🔥 {Streak} gün" : "";

    public event PropertyChangedEventHandler? PropertyChanged;
}