using Application.Abstractions;
using Domain.Shared;
using MediatR;

namespace Application.Abstractions;

public interface ICommandHandler<in TCommand>: IRequestHandler<TCommand, Result> where TCommand : ICommand
{
    Task<Result> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken);
}