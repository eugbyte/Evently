using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Evently.Server.Common.Domains.Entities;

[Index(nameof(Email), IsUnique = true)]
[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
public class Member {
	public long MemberId { get; set; }
	[StringLength(100)] public string Name { get; set; } = string.Empty;
	[StringLength(100)] public string Email { get; set; } = string.Empty;

	[StringLength(1000)] public string? LogoSrc { get; set; } = string.Empty;

	public List<Booking> Bookings { get; set; } = [];

	[JsonIgnore] public IdentityUser? IdentityUser { get; set; }

	[StringLength(100)] public string? IdentityUserId { get; set; }
}