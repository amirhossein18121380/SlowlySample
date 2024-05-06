using SlowlySimulate.Application.Common.Commands;

namespace SlowlySimulate.Application.Decorators.DatabaseRetry;

[Mapping(Type = typeof(DatabaseRetryAttribute))]
public class DatabaseRetryCommandDecorator<TCommand, TResult> : DatabaseRetryDecoratorBase, ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _handler;

    public DatabaseRetryCommandDecorator(ICommandHandler<TCommand, TResult> handler, DatabaseRetryAttribute options)
    {
        DatabaseRetryOptions = options;
        _handler = handler;
    }

    public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        Task<TResult> result = default;
        await WrapExecutionAsync(() => result = _handler.HandleAsync(command, cancellationToken));
        return await result;
    }
}
