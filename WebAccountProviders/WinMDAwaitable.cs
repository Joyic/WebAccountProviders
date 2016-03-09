using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace WebAccountProviders
{
    internal static class WinMDAwaitable
    {
        internal static Task<T> AsTask<T>(this IAsyncOperation<T> operation, CancellationToken cancellationToken = default(CancellationToken))
        {
            var registration = cancellationToken.Register(operation.Cancel);
            var tcs = new TaskCompletionSource<T>();
            AsyncOperationCompletedHandler<T> completionHandler = (op, status) =>
            {
                registration.Dispose();
                switch (status)
                {
                    case AsyncStatus.Canceled:
                        tcs.TrySetCanceled();
                        break;
                    case AsyncStatus.Completed:
                        tcs.TrySetResult(op.GetResults());
                        break;
                    case AsyncStatus.Error:
                        tcs.TrySetException(op.ErrorCode);
                        break;
                    default:
                        tcs.TrySetException(new InvalidOperationException("Unexpected async operation transition"));
                        break;
                }
            };

            var oldCompleted = operation.Completed;
            operation.Completed = new AsyncOperationCompletedHandler<T>((op, status) =>
            {
                completionHandler(op, status);
                oldCompleted?.Invoke(op, status);
            });

            if (operation.Status != AsyncStatus.Started)
            {
                completionHandler(operation, operation.Status);
            }

            return tcs.Task;
        }
    }
}
