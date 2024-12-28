using MediatR;

namespace Application.Abstractions;

public interface ICommand : IBaseCommand, IRequest;

public interface ICommand<out T> : IBaseCommand, IRequest<T>;

public interface IBaseCommand;
