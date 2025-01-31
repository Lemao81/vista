namespace SharedKernel;

public sealed record ValidationError(string Code, IDictionary<string, string[]> Errors) : Error(Code);
