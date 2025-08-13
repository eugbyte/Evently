export class BookingReqDto {
	public bookingId: string = "";
	public attendeeId: number = 0;
	public exhibitionId: number = 0;
	public registrationDateTime: Date = new Date();
	public checkInDateTime: Date | null = null;
	public checkoutDateTime: Date | null = null;
	public cancellationDateTime: Date | null = null;

	constructor(partial: Partial<BookingReqDto> = {}) {
		Object.assign(this, partial);
	}
}

export class GatheringReqDto {
	public gatheringId: number = 0;
	public name: string = "";
	public description: string = "";
	public start: Date = new Date();
	public end: Date = new Date();
	public location: string = "";
	public eventOrganiserId: number = 0;
	public coverSrc = "";

	constructor(partial: Partial<GatheringReqDto> = {}) {
		Object.assign(this, partial);
	}
}

export class MemberReqDto {
	public memberId: number = 0;
	public name: string = "";
	public email: string = "";
	public logoSrc: string | null = null;

	constructor(partial: Partial<MemberReqDto> = {}) {
		Object.assign(this, partial);
	}
}
