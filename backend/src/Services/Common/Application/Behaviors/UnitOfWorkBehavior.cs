using Application.Abstractions;
using Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors;

public sealed class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse?> where TRequest : IBaseCommand
{
	private readonly IUnitOfWork                        _unitOfWork;
	private readonly ILogger<UnitOfWorkBehaviorLogging> _logger;

	public UnitOfWorkBehavior(IUnitOfWork unitOfWork, ILogger<UnitOfWorkBehaviorLogging> logger)
	{
		_unitOfWork = unitOfWork;
		_logger     = logger;
	}

	public async Task<TResponse?> Handle(TRequest request, RequestHandlerDelegate<TResponse?> next, CancellationToken cancellationToken)
	{
		await using var transaction = await _unitOfWork.BeginTransactionAsync();
		TResponse?      response    = default;
		try
		{
			response = await next();
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			await transaction.CommitAsync(cancellationToken);
		}
		catch (Exception exception)
		{
			await transaction.RollbackAsync(cancellationToken);

			_logger.LogError(exception, "{Message}", exception.Message);
		}

		return response;
	}
}

public sealed class UnitOfWorkBehaviorLogging;
