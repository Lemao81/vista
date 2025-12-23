namespace Common.Application.Constants;

public static class ConfigurationKeys
{
	public const string DatabaseHost                        = "Database:Host";
	public const string DatabaseUsername                    = "Database:Username";
	public const string DatabasePassword                    = "Database:Password";
	public const string DatabasePasswordFile                = "Database:PasswordFile";
	public const string MediaUpload                         = "Media:Upload";
	public const string MinioEndpoint                       = "Minio:Endpoint";
	public const string MinioAccessKey                      = "Minio:AccessKey";
	public const string MinioSecretKey                      = "Minio:SecretKey";
	public const string MinioKeysFile                       = "Minio:KeysFile";
	public const string AzureStorageBlob                    = "Azure:Storage:Blob";
	public const string AzureStorageBlobServiceUri          = "Azure:Storage:Blob:ServiceUri";
	public const string AzureStorageManagedIdentityClientId = "Azure:Storage:ManagedIdentityClientId";
	public const string ReverseProxy                        = "ReverseProxy";
	public const string Jwt                                 = "JWT";
	public const string JwtIssuer                           = "JWT:Issuer";
	public const string JwtAudience                         = "JWT:Audience";
	public const string JwtSecretKey                        = "JWT:SecretKey";
}
