using System.Text.Json;
using Application.Common.Commands;

namespace Application.Decorators.AuditLog;

[Mapping(Type = typeof(AuditLogAttribute))]
public class AuditLogCommandDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _handler;

    public AuditLogCommandDecorator(ICommandHandler<TCommand, TResult> handler)
    {
        _handler = handler;
    }

    public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        LogCommand(command);
        return await _handler.HandleAsync(command, cancellationToken);
    }

    private void LogCommand(TCommand command)
    {
        var commandJson = JsonSerializer.Serialize(command);
        Console.WriteLine($"Command of type {command.GetType().Name}: {commandJson}");
    }

    //private void LogResult(TResult result)
    //{
    //    var resultJson = JsonSerializer.Serialize(result);
    //    Console.WriteLine($"Result of type {typeof(TResult).Name}: {resultJson}");
    //}
}


