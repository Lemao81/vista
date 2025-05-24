using Common.Application.Abstractions.Command;
using MediatR;
using SharedKernel;

namespace Common.Application.Abstractions;

public interface ICommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
	where TRequest : IRequest<TResponse>, IBaseCommand
	where TResponse : Result;
