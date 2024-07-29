namespace Worker.Persistance;

public static class WokerBase
{
    public static async Task RunJobAsync(Func<Task> func, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await func.Invoke();
            await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
        }
    }
}