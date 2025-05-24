namespace Common.Application.Abstractions.Command;

public interface ITransactionalCommand : ICommand, ITransactionalBaseCommand;

public interface ITransactionalCommand<TResponse> : ICommand<TResponse>, ITransactionalBaseCommand;
