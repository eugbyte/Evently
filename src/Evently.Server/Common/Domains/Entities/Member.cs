using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Evently.Server.Common.Domains.Entities;

[Index(nameof(Email), IsUnique = true)]
[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class Member : IdentityUser<long> {
	[StringLength(100)] public string Name { get; set; } = string.Empty;
	[StringLength(100)] public string Phone { get; set; } = string.Empty;
	[StringLength(100)] public new string Email { get; set; } = string.Empty;

	[StringLength(100)] public string Company { get; set; } = string.Empty;

	[StringLength(100)] public string Role { get; set; } = string.Empty;

	[StringLength(1_000)] public string Objective { get; set; } = string.Empty;

	[StringLength(100)] public string AdSource { get; set; } = string.Empty;

	[StringLength(1000)] public string? LogoSrc { get; set; } = string.Empty;

	public List<Booking> BookingEvents { get; set; } = [];
	public List<MemberCategoryDetail> MemberCategoryDetails { get; set; } = [];
}