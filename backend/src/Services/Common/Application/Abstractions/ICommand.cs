using MediatR;
using SharedKernel;

namespace Common.Application.Abstractions;

public interface IBaseCommand;

public interface ICommand : IRequest, IBaseCommand;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;

public interface ITransactionalBaseCommand;

public interface ITransactionalCommand : ICommand, ITransactionalBaseCommand;

public interface ITransactionalCommand<TResponse> : ICommand<TResponse>, ITransactionalBaseCommand;
