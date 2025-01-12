namespace SharedKernel;

public sealed record ValidationError(ErrorCode Code, IDictionary<string, string[]> Errors) : Error(Code);
