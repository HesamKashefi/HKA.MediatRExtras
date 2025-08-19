using HKA.MediatRExtras.Converters;
using HKA.MediatRExtras.DataResolvers;
using MediatR;
using Newtonsoft.Json;

namespace HKA.MediatRExtras;

/// <summary>
/// Intercepts every request and logs it
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class RequestLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    #region Fields
    private static readonly JsonSerializerSettings _settings = new()
    {
        Converters = [new ByteArrayJsonConverter(), new FormFileJsonConverter()],
        ContractResolver = new SensitiveDataResolver()
    };

    private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
    #endregion

    #region Implementation of IPipelineBehavior<in TRequest,TResponse>

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var reqType = typeof(TRequest);
        var serializedRequest = JsonConvert.SerializeObject(request, _settings);
        try
        {
            _logger.Trace("Request : {reqType} : {@request}", reqType.Name, serializedRequest);
            var result = await next(cancellationToken);
            var serializedResult = JsonConvert.SerializeObject(result, _settings);
            _logger.Trace("Result for request {reqType} {@request} : {@result}", reqType.Name, serializedRequest, serializedResult);
            return result;
        }
        catch (Exception e)
        {
            _logger.Error(e, "Error on request - Request : {reqType} - Request Data : {@request}", reqType.Name, serializedRequest);
            throw;
        }
    }

    #endregion
}
