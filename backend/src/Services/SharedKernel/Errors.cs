namespace SharedKernel;

public static class Errors
{
	public static readonly Error None                               = new InternalError(ErrorCodes.None);
	public static readonly Error Unknown                            = new InternalError(ErrorCodes.Unknown);
	public static readonly Error EntityNotFound                     = new InternalError(ErrorCodes.EntityNotFound);
	public static readonly Error ObjectStorageRequestFailed         = new InternalError(ErrorCodes.ObjectStorageRequestFailed);
	public static readonly Error ObjectStorageFailed                = new InternalError(ErrorCodes.ObjectStorageFailed);
	public static readonly Error ObjectRetrievalFailed              = new InternalError(ErrorCodes.ObjectRetrievalFailed);
	public static readonly Error ObjectExistFailed                  = new InternalError(ErrorCodes.ObjectExistFailed);
	public static readonly Error ObjectListFailed                   = new InternalError(ErrorCodes.ObjectListFailed);
	public static readonly Error StorageBucketCreationRequestFailed = new InternalError(ErrorCodes.StorageBucketCreationRequestFailed);
	public static readonly Error StorageBucketCreationFailed        = new InternalError(ErrorCodes.StorageBucketCreationFailed);
}
