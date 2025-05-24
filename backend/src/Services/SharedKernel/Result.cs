namespace SharedKernel;

public record Result
{
	protected internal Result(bool isSuccess, Error error)
	{
		if ((isSuccess && error != Errors.None) || (!isSuccess && error == Errors.None))
		{
			throw new ArgumentException("Invalid argument combination");
		}

		IsSuccess = isSuccess;
		Error     = error;
	}

	public Error Error { get; }

	public bool IsSuccess { get; }

	public bool IsFailure => !IsSuccess;

	public static implicit operator Result(Error error) => Failure(error);

	public static Result Success() => new(true, Errors.None);

	public static Result Failure(Error error) => new(false, error);
}

#pragma warning disable SA1402
public sealed record Result<T> : Result
#pragma warning restore SA1402
{
	private readonly T? _value;

	private Result(T value) : base(true, Errors.None)
	{
		if (value is null)
		{
			throw new ArgumentNullException(nameof(value));
		}

		_value = value;
	}

	private Result(Error error) : base(false, error)
	{
	}

	public T Value => IsSuccess ? _value! : throw new InvalidOperationException("Value of a non-success result can't be accessed");

	public static implicit operator Result<T>(T value) => new(value);

	public static implicit operator Result<T>(Error error) => new(error);

	public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure) => IsSuccess ? onSuccess(Value) : onFailure(Error);
}
