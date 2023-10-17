namespace ClientTpsLimiter;

internal sealed class TaskRateLimiter
{
    private readonly TimeSpan _timespan;
    private readonly SemaphoreSlim _semaphore;

    public TaskRateLimiter(int count, TimeSpan timespan)
    {
        _semaphore = new SemaphoreSlim(count, count);
        _timespan = timespan;
    }

    public async Task LimitAsync(Func<Task> taskFactory)
    {
        await _semaphore.WaitAsync().ConfigureAwait(false);
        var task = taskFactory();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        task.ContinueWith(async e =>
        {
            await Task.Delay(_timespan);
            _semaphore.Release(1);
        });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        await task;
    }

    public async Task<T> LimitAsync<T>(Func<Task<T>> taskFactory)
    {
        await _semaphore.WaitAsync().ConfigureAwait(false);
        var task = taskFactory();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        task.ContinueWith(async e =>
        {
            await Task.Delay(_timespan);
            _semaphore.Release(1);
        });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        return await task;
    }
}