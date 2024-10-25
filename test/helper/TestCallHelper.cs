using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgejoApiClient.Tests.helper;


public static class TestCallHelper
{
    public static async ValueTask<TResult> Satisfy<TResult>(Func<CancellationToken, Task<TResult>> caller, Predicate<TResult>? condition = default, TimeSpan? timeout = default, TimeSpan? interval = default)
    {
        var timeoutSpan = timeout ?? TimeSpan.FromMilliseconds(5000);
        var intervalSpan = interval ?? TimeSpan.FromMilliseconds(500);
        var satisfyChecker = condition ?? ((_) => true);

        using (var breaker = new CancellationTokenSource(timeoutSpan))
        {
            while (true)
            {
                await Task.Delay(intervalSpan, breaker.Token);

                var result = await caller(breaker.Token);
                if (satisfyChecker(result)) return result;
            }
        }
    }

    public static async ValueTask<TResult> TrySatisfy<TResult>(Func<CancellationToken, Task<TResult>> caller, Predicate<TResult>? condition = default, TimeSpan? timeout = default, TimeSpan? interval = default)
    {
        var timeoutSpan = timeout ?? TimeSpan.FromMilliseconds(5000);
        var intervalSpan = interval ?? TimeSpan.FromMilliseconds(500);
        var satisfyChecker = condition ?? ((_) => true);

        using (var breaker = new CancellationTokenSource(timeoutSpan))
        {
            while (true)
            {
                await Task.Delay(intervalSpan, breaker.Token);

                try
                {
                    var result = await caller(breaker.Token);

                    var satisfied = false;
                    try { satisfied = satisfyChecker(result); } catch { }
                    if (satisfied) return result;
                }
                catch (OperationCanceledException) { throw; }
                catch { }
            }
        }
    }
}
