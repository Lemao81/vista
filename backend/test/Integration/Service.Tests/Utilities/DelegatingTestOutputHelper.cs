namespace Service.Tests.Utilities;

public class DelegatingTestOutputHelper : ITestOutputHelper
{
	private readonly Func<ITestOutputHelper?> _provider;

	public DelegatingTestOutputHelper(Func<ITestOutputHelper?> provider)
	{
		_provider = provider;
	}

	public void Write(string message) => _provider()?.Write(message);

	public void Write(string format, params object[] args) => _provider()?.Write(format, args);

	public void WriteLine(string message) => _provider()?.WriteLine(message);

	public void WriteLine(string format, params object[] args) => _provider()?.WriteLine(format, args);

	public string Output => _provider()?.Output ?? "";
}
