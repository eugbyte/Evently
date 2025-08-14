using NanoidDotNet;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Evently.Server.Common.Domains.Entities;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class Booking {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	[StringLength(50)]
	public string BookingId { get; set; } = $"book_{Nanoid.Generate(size: 10)}";

	[StringLength(100)] [ForeignKey("Member")] public string MemberId { get; set; } = string.Empty;
	public Member? Member { get; set; }

	public long GatheringId { get; set; }
	public Gathering? Gathering { get; set; }

	public DateTimeOffset RegistrationDateTime { get; set; } = DateTimeOffset.UtcNow;
	public DateTimeOffset? CheckInDateTime { get; set; }
	public DateTimeOffset? CheckoutDateTime { get; set; }
	public DateTimeOffset? CancellationDateTime { get; set; }
}