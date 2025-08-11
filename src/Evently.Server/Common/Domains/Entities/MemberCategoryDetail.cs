using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Evently.Server.Common.Domains.Entities;

[PrimaryKey(propertyName: nameof(MemberId), nameof(CategoryId))]
[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class MemberCategoryDetail {
	[ForeignKey("Member")] public long MemberId { get; set; }
	public long CategoryId { get; set; }

	public Member? Member { get; set; }
	public Category? Category { get; set; }
}