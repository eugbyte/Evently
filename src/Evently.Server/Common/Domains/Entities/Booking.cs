using Evently.Server.Common.Domains.Models;
using Evently.Server.Common.Extensions;
using NanoidDotNet;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Evently.Server.Common.Domains.Entities;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class Booking {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	[StringLength(50)]
	public string BookingId { get; set; } = $"book_{Nanoid.Generate(size: 10)}";

	[ForeignKey("Account")] public string AttendeeId { get; set; } = string.Empty;
	[JsonIgnore] public Account? Account { get; set; }
	[NotMapped] public AccountDto? AccountDto => Account?.ToAccountDto();

	public long GatheringId { get; set; }
	public Gathering? Gathering { get; set; }

	public DateTimeOffset CreationDateTime { get; set; } = DateTimeOffset.UtcNow;
	public DateTimeOffset? CheckInDateTime { get; set; }
	public DateTimeOffset? CheckoutDateTime { get; set; }
	public DateTimeOffset? CancellationDateTime { get; set; }
}