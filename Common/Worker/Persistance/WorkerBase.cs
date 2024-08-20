namespace Worker.Persistance;

public static class WokerBase
{
    public static async Task RunJobAsync(Func<Task> func, TimeSpan delayTime, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await func.Invoke();
            await Task.Delay(delayTime, cancellationToken);
        }
    }
}