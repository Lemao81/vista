namespace Service.Tests;

public static class TestConstants
{
	public const string JwtSecretKey = "wHzxc2ctfDgH6Y4EH9zi1G/COjODaLeKPtv99V0rIR4=";

	public const string Jwt =
		"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAiLCJleHAiOjIwODQ4MDA1NDIsInN1YiI6InRlc3QiLCJqdGkiOiI0YjM5ZTk5OS1hOGMwLTQ3NTktOGZkZi0zOGU4ODU3ZGIyNDkiLCJ1bmlxdWVfbmFtZSI6InRlc3QiLCJpYXQiOjE3NjkyNjc3NDIsIm5iZiI6MTc2OTI2Nzc0Mn0.fv8KTjB9sJ90uWZu1hbV9AlmwkhV13Gzw0_86GdhOUo";
}

public static class NetworkAliases
{
	public const string Postgres       = "postgres";
	public const string Minio          = "minio";
	public const string MaintenanceApi = "maintenance-api";
	public const string Azurite        = "azurite";
}
