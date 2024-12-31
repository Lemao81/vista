using Domain.ValueObjects;

namespace Domain;

public static class ErrorCodes
{
	private const string NoneValue           = "None";
	private const string UnknownValue        = "Unknown";
	private const string EntityNotFoundValue = "EntityNotFound";

	public static readonly ErrorCode None           = new(NoneValue);
	public static readonly ErrorCode Unknown        = new(UnknownValue);
	public static readonly ErrorCode EntityNotFound = new(EntityNotFoundValue);
}

public static class Errors
{
	public static readonly Error None           = new InternalError(ErrorCodes.None);
	public static readonly Error Unknown        = new InternalError(ErrorCodes.Unknown);
	public static readonly Error EntityNotFound = new InternalError(ErrorCodes.EntityNotFound);
}
