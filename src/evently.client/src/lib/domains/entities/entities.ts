export class Gathering {
	gatheringId = 0;
	name = "";
	description = "";
	start = new Date();
	end = new Date();
	location = "";
	coverSrc?: string = "";
	organiserId = "";
	bookings: Booking[] = []; // You may want to replace 'any[]' with a proper Booking[] type
	gatheringCategoryDetails: GatheringCategoryDetail[] = []; // You may want to replace 'any[]' with a proper GatheringCategoryDetail[] type

	constructor(data: Partial<Gathering> = {}) {
		Object.assign(this, data);
	}
}

export class Account {
	id = "";
	name = "";
	userName = "";
	email = "";
	logoSrc?: string = "";
	bookings: Booking[] = []; // You may want to replace 'any[]' with a proper Booking[] type

	constructor(data: Partial<Account> = {}) {
		Object.assign(this, data);
	}
}

export class Booking {
	public bookingId = "";
	public accountId = 0;
	public accountDto = new Account();
	public gatheringId = 0;
	public gathering = new Gathering();
	public isOrganiser = false;
	public creationDateTime = new Date();
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
