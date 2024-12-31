using Application.Abstractions;
using Domain;
using Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors;

public sealed class TransactionalBehavior<TRequest, TResponse> : BasePipelineBehavior<TRequest, TResponse> where TRequest : ITransactionalBaseCommand
{
	private readonly IUnitOfWork                                         _unitOfWork;
	private readonly ILogger<TransactionalBehavior<TRequest, TResponse>> _logger;

	public TransactionalBehavior(IUnitOfWork unitOfWork, ILogger<TransactionalBehavior<TRequest, TResponse>> logger)
	{
		_unitOfWork = unitOfWork;
		_logger     = logger;
	}

	public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		await using var transaction = await _unitOfWork.BeginTransactionAsync();

		try
		{
			var response = await next();
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			await transaction.CommitAsync(cancellationToken);

			return response;
		}
		catch (Exception exception)
		{
			await transaction.RollbackAsync(cancellationToken);

			_logger.LogError(exception, "{Message}", exception.Message);

			return CreateErrorResult(Errors.Unknown);
		}
	}
}
