namespace SharedKernel;

public sealed record InternalError(ErrorCode Code) : Error(Code);
