using System.Collections.Concurrent;
using System.Diagnostics;

TimeSpan delay = TimeSpan.FromSeconds(1);
IEnumerable<int> taskIds = Enumerable.Range(1, 100);

Stopwatch swFast = new();
swFast.Start();
// Run as fast as possible
await Parallel.ForEachAsync(taskIds, async (taskId, cancellationToken) =>
{
    await Console.Out.WriteLineAsync($"FAST Starting task {taskId}.");
    await Task.Delay(delay);
    await Console.Out.WriteLineAsync($"FAST Completed task {taskId}.");
});
swFast.Stop();



// Run with limited throughput
ParallelOptions options = new()
{
    MaxDegreeOfParallelism = 3
};
Stopwatch swLimited = new();
swLimited.Start();
await Parallel.ForEachAsync(taskIds, options, async (taskId, cancellationToken) =>
{
    await Console.Out.WriteLineAsync($"LIMITED Starting task {taskId}.");
    await Task.Delay(delay);
    await Console.Out.WriteLineAsync($"LIMITED Completed task {taskId}.");
});
swLimited.Stop();



Console.WriteLine();
Console.WriteLine();
Console.WriteLine($"Linear time: {(taskIds.Count() * delay)}");
Console.WriteLine($"FAST time: {swFast.Elapsed}");
Console.WriteLine($"LIMITED time: {swLimited.Elapsed}");