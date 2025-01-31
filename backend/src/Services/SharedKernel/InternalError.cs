namespace SharedKernel;

public sealed record InternalError(string Code) : Error(Code);
