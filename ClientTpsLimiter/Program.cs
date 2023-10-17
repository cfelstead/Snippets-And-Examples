using ClientTpsLimiter;

IEnumerable<int> taskIds = Enumerable.Range(1, 150);
List<Task> fireAndForGetTasks = new();
List<Task> waitTasks = new();
var limiter = new TaskRateLimiter(20, TimeSpan.FromSeconds(1));


// A maximum number of tasks (set by the limiter) will run at any moment.
// The limiter will ensure that should a task be completed before the limit is reached, the next thread is not started prematurely.
// The key is the lack of the async keyword on the line below
waitTasks.AddRange(taskIds.Select(tid => limiter.LimitAsync(() => RunItem("WAIT", tid))));
await Task.WhenAll(waitTasks);
Console.WriteLine("All tasks have been completed.");



// The limiter will ensure the number of tasks specifed are launched for every period of the limiter.
// These tasks are fire-and-forget so you cannot await their response.
// The key is the presence of the async keyword on the line below
fireAndForGetTasks.AddRange(taskIds.Select(tid => limiter.LimitAsync(async () => RunItem("FnF", tid))));
await Task.WhenAll(fireAndForGetTasks);
Console.WriteLine("NO! Fire and forget tasks will still be running.");





static async Task RunItem(string type, int taskId)
{
    await Console.Out.WriteLineAsync($"{type} Started Task {taskId}.");
    await Task.Delay(TimeSpan.FromSeconds(3));
    await Console.Out.WriteLineAsync($"{type} Completed Task {taskId}.");
}