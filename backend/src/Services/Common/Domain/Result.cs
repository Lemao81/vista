using Domain.ValueObjects;

namespace Domain;

public readonly record struct Result<T>
{
	private Result(T value)
	{
		if (value is null)
		{
			throw new ArgumentNullException(nameof(value));
		}

		Value     = value;
		IsSuccess = true;
		Error     = Errors.None;
	}

	private Result(Error error)
	{
		Error = error;
	}

	public T?    Value     { get; }
	public Error Error     { get; }
	public bool  IsSuccess { get; }
	public bool  IsFailure => !IsSuccess;

	public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure)
	{
		if (!IsSuccess)
		{
			return onFailure(Error);
		}

		ArgumentNullException.ThrowIfNull(Value);

		return onSuccess(Value);
	}

	public static Result<T> Success(T     value) => new(value);
	public static Result<T> Failure(Error error) => new(error);

	public static implicit operator Result<T>(T     value) => Success(value);
	public static implicit operator Result<T>(Error error) => Failure(error);
}
