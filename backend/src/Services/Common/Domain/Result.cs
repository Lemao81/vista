﻿using Domain.ValueObjects;

namespace Domain;

public record Result
{
	protected Result(bool isSuccess, Error error)
	{
		if (isSuccess && error != Errors.None || !isSuccess && error == Errors.None)
		{
			throw new ArgumentException("Invalid argument combination");
		}

		IsSuccess = isSuccess;
		Error     = error;
	}

	public Error Error     { get; }
	public bool  IsSuccess { get; }
	public bool  IsFailure => !IsSuccess;

	public static Result Failure(Error error) => new(error);

	public static implicit operator Result(Error error) => Failure(error);
}

public sealed record Result<T> : Result
{
	private Result(T value) : base(true, Errors.None)
	{
		if (value is null)
		{
			throw new ArgumentNullException(nameof(value));
		}

		Value = value;
	}

	private Result(Error error) : base(false, error)
	{
	}

	public T? Value { get; }

	public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure)
	{
		if (!IsSuccess) return onFailure(Error);

		ArgumentNullException.ThrowIfNull(Value);

		return onSuccess(Value);
	}

	public static     Result<T> Success(T     value) => new(value);
	public new static Result<T> Failure(Error error) => new(error);

	public static implicit operator Result<T>(T     value) => Success(value);
	public static implicit operator Result<T>(Error error) => Failure(error);
}
