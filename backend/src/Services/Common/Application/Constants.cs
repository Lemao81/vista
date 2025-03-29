namespace Common.Application;

public static class Constants
{
}

public static class ServiceNames
{
	public const string FileTransfer = nameof(FileTransfer);
}

public static class MeterNames
{
	public const string FileTransfer = "Vista.FileTransfer";
}

public static class CounterNames
{
	public const string PictureUpload = "picture.uploads";
}

public static class CounterTagNames
{
	public const string PictureMediaType = "picture.media_type";
}

public static class ConfigurationKeys
{
	public const string DatabaseHost         = "Database:Host";
	public const string DatabaseUsername     = "Database:Username";
	public const string DatabasePassword     = "Database:Password";
	public const string DatabasePasswordFile = "Database:PasswordFile";
	public const string MediaUpload          = "Media:Upload";
	public const string MinioEndpoint        = "Minio:Endpoint";
	public const string MinioAccessKey       = "Minio:AccessKey";
	public const string MinioSecretKey       = "Minio:SecretKey";
	public const string MinioKeysFile        = "Minio:KeysFile";
	public const string AzureStorageBlob     = "Azure:Storage:Blob";
}
