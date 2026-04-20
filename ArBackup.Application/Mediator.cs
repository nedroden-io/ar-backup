using Microsoft.Extensions.DependencyInjection;

namespace ArBackup.Application;

public interface ICommandHandler<in TCommand>
{
    Task Handle(TCommand command, CancellationToken cancellationToken);
}

public interface IMediator
{
    Task Publish<TCommand>(TCommand command, CancellationToken cancellationToken = default);
}

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public async Task Publish<TCommand>(TCommand command, CancellationToken cancellationToken = default)
    {
        var handler = serviceProvider.GetService<ICommandHandler<TCommand>>();
        if (handler == null) throw new InvalidOperationException($"No handler registered for {typeof(TCommand).Name}");
        await handler.Handle(command, cancellationToken);
    }
}

