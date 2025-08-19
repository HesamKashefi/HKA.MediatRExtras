namespace HKA.MediatRExtras.Attributes;


[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class MediatorRequestAttribute : Attribute
{
    /// <summary>
    /// If set to true, <see cref="RequestLogginBehavior"/> will not log request data
    /// </summary>
    public bool IgnoreRequestLogging { get; set; }

    /// <summary>
    /// If set to true, <see cref="RequestLogginBehavior"/> will not log response data
    /// </summary>
    public bool IgnoreResponseLogging { get; set; }
}