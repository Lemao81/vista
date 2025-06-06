﻿using SharedKernel;

namespace FileTransfer.Domain.Media;

public sealed record MediaCreatedDomainEvent(MediaFolderId MediaFolderId, MediaItemId MediaItemId, MediaKind MediaKind, string MediaType) : IDomainEvent;
