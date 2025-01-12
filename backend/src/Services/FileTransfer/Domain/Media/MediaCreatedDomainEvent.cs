using SharedKernel;

namespace Domain.Media;

public sealed record MediaCreatedDomainEvent(MediaFolderId MediaFolderId, MediaItemId MediaItemId, MediaKind MediaKind) : IDomainEvent;
