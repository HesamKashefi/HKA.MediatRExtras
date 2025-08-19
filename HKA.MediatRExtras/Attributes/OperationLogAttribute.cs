namespace HKA.MediatRExtras.Attributes;

/// <summary>
/// A marker attribute for <see cref="MediatorOperationLogger{TRequest, TResponse}"/>
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class OperationLogAttribute : Attribute
{

}
