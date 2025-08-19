using MediatR;
using System.Diagnostics;

namespace HKA.MediatRExtras;

/// <summary>
/// This will log the elapsed time of processing the request
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class RequestTimerBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    #region Dependencies

    private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

    #endregion

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var timer = Stopwatch.StartNew();
        try
        {
            return await next(cancellationToken);
        }
        finally
        {
            timer.Stop();
            _logger.Trace("request : {name} - Time Elapsed : {time} MS - Data : {@data}", typeof(TRequest).Name, timer.ElapsedMilliseconds, request);
        }
    }
}