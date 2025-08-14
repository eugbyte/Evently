export class Gathering {
	gatheringId: number = 0;
	name: string = "";
	description: string = "";
	start: Date = new Date();
	end: Date = new Date();
	location: string = "";
	coverSrc?: string = "";
	organiserId: number = 0;
	member = new Member(); // You may want to replace 'any' with a proper Member type
	bookings: Booking[] = []; // You may want to replace 'any[]' with a proper Booking[] type
	gatheringCategoryDetails: GatheringCategoryDetail[] = []; // You may want to replace 'any[]' with a proper GatheringCategoryDetail[] type

	constructor(data: Partial<Gathering> = {}) {
		Object.assign(this, data);
	}
}

export class Member {
	id = "";
	name: string = "";
	userName: string = "";
	email: string = "";
	logoSrc?: string = "";
	bookings: Booking[] = []; // You may want to replace 'any[]' with a proper Booking[] type

	constructor(data: Partial<Member> = {}) {
		Object.assign(this, data);
	}
}

export class Booking {
	public bookingId = "";
	public memberId = 0;
	public member = new Member();
	public gatheringId = 0;
	public gathering = new Gathering();
	public registrationDateTime = new Date();
	public checkInDateTime: Date | null = null;
	public checkoutDateTime: Date | null = null;
	public cancellationDateTime: Date | null = null;

	constructor(partial: Partial<Booking> = {}) {
		Object.assign(this, partial);
	}
}

export class Category {
	public categoryId = 0;
	public categoryName = "";

	constructor(partial: Partial<Category> = {}) {
		// Apply partial properties using Object.assign
		Object.assign(this, partial);
	}
}

export class GatheringCategoryDetail {
	public gatheringId = 0;
	public categoryId = 0;
	public gathering = new Gathering();
	public category = new Category();

	constructor(partial: Partial<GatheringCategoryDetail> = {}) {
		// Apply partial properties using Object.assign
		Object.assign(this, partial);
	}
}

export class Account {
	id: string = "";
	email: string = "";
	userName: string = "";
}
