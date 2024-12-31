namespace Domain.ValueObjects;

public sealed record InternalError(ErrorCode Code) : Error(Code);
