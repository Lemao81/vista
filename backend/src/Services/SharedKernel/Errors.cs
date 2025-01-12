namespace SharedKernel;

public static class ErrorCodes
{
	private const string NoneValue                        = "None";
	private const string UnknownValue                     = "Unknown";
	private const string ValidationFailedValue            = "ValidationFailed";
	private const string EntityNotFoundValue              = "EntityNotFound";
	private const string ObjectStorageRequestFailedValue  = "ObjectStorageRequestFailed";
	private const string ObjectStorageFailedValue         = "ObjectStorageFailed";
	private const string ObjectRetrievalFailedValue       = "ObjectRetrievalFailed";
	private const string ObjectExistFailedValue           = "ObjectExistFailed";
	private const string ObjectListFailedValue            = "ObjectListFailed";
	private const string StorageBucketCreationFailedValue = "StorageBucketCreationFailed";

	public static readonly ErrorCode None                        = new((string)NoneValue);
	public static readonly ErrorCode Unknown                     = new((string)UnknownValue);
	public static readonly ErrorCode ValidationFailed            = new((string)ValidationFailedValue);
	public static readonly ErrorCode EntityNotFound              = new((string)EntityNotFoundValue);
	public static readonly ErrorCode ObjectStorageRequestFailed  = new((string)ObjectStorageRequestFailedValue);
	public static readonly ErrorCode ObjectStorageFailed         = new((string)ObjectStorageFailedValue);
	public static readonly ErrorCode ObjectRetrievalFailed       = new((string)ObjectRetrievalFailedValue);
	public static readonly ErrorCode ObjectExistFailed           = new((string)ObjectExistFailedValue);
	public static readonly ErrorCode ObjectListFailed            = new((string)ObjectListFailedValue);
	public static readonly ErrorCode StorageBucketCreationFailed = new((string)StorageBucketCreationFailedValue);
}

public static class Errors
{
	public static readonly Error None                        = new InternalError(ErrorCodes.None);
	public static readonly Error Unknown                     = new InternalError(ErrorCodes.Unknown);
	public static readonly Error EntityNotFound              = new InternalError(ErrorCodes.EntityNotFound);
	public static readonly Error ObjectStorageRequestFailed  = new InternalError(ErrorCodes.ObjectStorageRequestFailed);
	public static readonly Error ObjectStorageFailed         = new InternalError(ErrorCodes.ObjectStorageFailed);
	public static readonly Error ObjectRetrievalFailed       = new InternalError(ErrorCodes.ObjectRetrievalFailed);
	public static readonly Error ObjectExistFailed           = new InternalError(ErrorCodes.ObjectExistFailed);
	public static readonly Error ObjectListFailed            = new InternalError(ErrorCodes.ObjectListFailed);
	public static readonly Error StorageBucketCreationFailed = new InternalError(ErrorCodes.StorageBucketCreationFailed);
}
