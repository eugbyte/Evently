export class BookingReqDto {
	public bookingId = "";
	public attendeeId = "";
	public gatheringId = 0;
	public creationDateTime = new Date();
	public checkInDateTime: Date | null = null;
	public checkoutDateTime: Date | null = null;
	public cancellationDateTime: Date | null = null;

	constructor(partial: Partial<BookingReqDto> = {}) {
		Object.assign(this, partial);
	}
}

export class GatheringReqDto {
	public gatheringId = 0;
	public name = "";
	public description = "";
	public start = new Date();
	public end = new Date();
	public location = "";
	public hostId = 0;
	public coverSrc = "";

	constructor(partial: Partial<GatheringReqDto> = {}) {
		Object.assign(this, partial);
	}
}
