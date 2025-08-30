namespace Evently.Server.Common.Domains.Models;

// [{"categoryId":1,"gatheringId":22},{"categoryId":2,"gatheringId":22}]
public sealed record GatheringCategoryDetailDto(long GatheringId, long CategoryId);