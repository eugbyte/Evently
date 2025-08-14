﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Evently.Server.Common.Domains.Entities;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
public class Account : IdentityUser {
	[StringLength(100)] public string Name { get; set; } = string.Empty;
	[StringLength(1000)] public string? LogoSrc { get; set; }

	public List<Booking> Bookings { get; set; } = [];
}