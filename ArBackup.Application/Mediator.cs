using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ArBackup.Application.Mediator;

public interface ICommandHandler<TCommand, TResponse>
{
    Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface IMediator
{
    Task<TResponse> Send<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default);
}

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public async Task<TResponse> Send<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default)
    {
        var handler = serviceProvider.GetService<ICommandHandler<TCommand, TResponse>>();
        if (handler == null) throw new InvalidOperationException($"No handler registered for {typeof(TCommand).Name}");

        return await handler.Handle(command, cancellationToken);
    }
}
