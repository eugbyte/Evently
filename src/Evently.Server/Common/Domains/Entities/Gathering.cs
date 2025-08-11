using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Evently.Server.Common.Domains.Entities;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
[SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
public class Gathering {
	[Key] public long GatheringId { get; set; }

	[StringLength(100)] public string Name { get; set; } = string.Empty;

	[StringLength(10_000)] public string Description { get; set; } = string.Empty;

	public DateTimeOffset Start { get; set; }  = DateTimeOffset.UtcNow;
	public DateTimeOffset End { get; set; } = DateTimeOffset.UtcNow;

	[StringLength(100)] public string Location { get; set; } = string.Empty;

	[StringLength(1000)] public string? CoverSrc { get; set; } = string.Empty;

	[ForeignKey("Member")] public long MemberId { get; set; }
	public Member? Member { get; set; }

	public List<Booking> BookingEvents { get; set; } = [];
}