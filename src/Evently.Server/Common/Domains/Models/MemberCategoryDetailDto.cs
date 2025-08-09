using System.Diagnostics.CodeAnalysis;

namespace Evently.Server.Common.Domains.Models;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record MemberCategoryDetailDto(long MemberId, long CategoryId);