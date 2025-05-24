using MediatR;
using SharedKernel;

namespace Common.Application.Abstractions.Command;

public interface ICommand : IRequest, IBaseCommand;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;
