using HKA.MediatRExtras.Attributes;
using HKA.MediatRExtras.Converters;
using HKA.MediatRExtras.DataResolvers;
using MediatR;
using Newtonsoft.Json;
using System.Reflection;

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
        var attr = reqType.GetCustomAttributes().OfType<MediatorRequestAttribute>().SingleOrDefault();
        try
        {
            if (attr == null || !attr.IgnoreRequestLogging)
                _logger.Trace("Request : {reqType} : {@request}", reqType.Name, serializedRequest);
            else
                _logger.Trace("Request : {reqType}", reqType.Name);
            var result = await next(cancellationToken);
            var serializedResult = JsonConvert.SerializeObject(result, _settings);
            if (attr == null || !attr.IgnoreResponseLogging)
                _logger.Trace("Result for : {reqType} \n{@result}", reqType.Name, serializedResult);
            else
                _logger.Trace("Result for : {reqType} received", reqType.Name);
            return result;
        }
        catch (Exception e)
        {
            if (attr == null || !attr.IgnoreRequestLogging)
                _logger.Error(e, "Error on request \n- Request : {reqType} \n- Request Data : {@request}", reqType.Name, serializedRequest);
            else
                _logger.Error(e, "Error on request \n- Request : {reqType}", reqType.Name);
            throw;
        }
    }

    #endregion
}
