﻿namespace Evently.Server.Common.Domains.Models;

public sealed record BookingReqDto(
	string BookingId,
	string AttendeeId,
	long GatheringId,
	DateTimeOffset CreationDateTime,
	DateTimeOffset? CheckInDateTime,
	DateTimeOffset? CheckoutDateTime,
	DateTimeOffset? CancellationDateTime);