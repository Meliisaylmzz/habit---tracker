using SQLite;

namespace HabitTracker;

public class HabitDatabase
{
    SQLiteAsyncConnection? database;

    async Task Init()
    {
        if (database is not null)
            return;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "habits.db3");
        database = new SQLiteAsyncConnection(dbPath);
        await database.CreateTableAsync<Habit>();
    }

    public async Task<List<Habit>> GetHabitsAsync()
    {
        await Init();
        return await database!.Table<Habit>().ToListAsync();
    }

    public async Task SaveHabitAsync(Habit habit)
    {
        await Init();

        if (habit.Id != 0)
            await database!.UpdateAsync(habit);
        else
            await database!.InsertAsync(habit);
    }

    public async Task DeleteHabitAsync(Habit habit)
    {
        await Init();
        await database!.DeleteAsync(habit);
    }
}