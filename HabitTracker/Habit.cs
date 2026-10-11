using SQLite;

namespace HabitTracker;

public class Habit
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; } = "";
    public bool IsDone { get; set; }
}