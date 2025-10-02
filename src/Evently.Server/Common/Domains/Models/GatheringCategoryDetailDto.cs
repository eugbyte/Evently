using JetBrains.Annotations;

namespace Evently.Server.Common.Domains.Models;

[UsedImplicitly]
public sealed record GatheringCategoryDetailDto(long GatheringId, long CategoryId);