using Common.Application.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Common.Application.Behaviors;

public sealed class TransactionalPipelineBehavior<TRequest, TResponse> : BasePipelineBehavior<TRequest, TResponse> where TRequest : ITransactionalBaseCommand
{
	private readonly IUnitOfWork                                                 _unitOfWork;
	private readonly ILogger<TransactionalPipelineBehavior<TRequest, TResponse>> _logger;

	public TransactionalPipelineBehavior(IUnitOfWork unitOfWork, ILogger<TransactionalPipelineBehavior<TRequest, TResponse>> logger)
	{
		_unitOfWork = unitOfWork;
		_logger     = logger;
	}

	public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		await using var transaction = await _unitOfWork.BeginTransactionAsync();

		try
		{
			var result = await next();
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			await transaction.CommitAsync(cancellationToken);

			return result;
		}
		catch (Exception exception)
		{
			await transaction.RollbackAsync(cancellationToken);

			_logger.LogWarning("Transaction rolled back due to an error: {Message}", exception.Message);

			throw;
		}
	}
}
