import { Booking, Gathering, Category } from "~/lib/domains/entities";
import { GatheringReqDto } from "~/lib/domains/models";
import type { GetGatheringsParams } from "./gathering-service";
import type { PageResult } from "~/lib/domains/interfaces";

// Mock data for categories
const mockCategories: Category[] = [
	new Category({
		categoryId: 1,
		categoryName: "Technology"
	}),
	new Category({
		categoryId: 2,
		categoryName: "Networking"
	})
];

// Mock data for gatherings
const mockGatherings: Gathering[] = [
	{
		gatheringId: 1,
		name: "Tech Conference 2024",
		description: "Annual technology conference with industry leaders",
		start: new Date("2024-12-15T09:00:00Z"),
		end: new Date("2024-12-15T17:00:00Z"),
		location: "Convention Center",
		organiserId: "org-123",
		cancellationDateTime: null,
		coverSrc: "/images/tech-conference.jpg",
		bookings: [{ ...new Booking(), attendeeId: "abc" }],
		gatheringCategoryDetails: [
			{
				gatheringId: 1,
				categoryId: 1,
				category: mockCategories[0],
				gathering: new Gathering()
			},
			{
				gatheringId: 2,
				categoryId: 2,
				category: mockCategories[1],
				gathering: new Gathering()
			}
		]
	},
	{
		gatheringId: 2,
		name: "Design Workshop",
		description: "Interactive design thinking workshop",
		start: new Date("2024-12-20T14:00:00Z"),
		end: new Date("2024-12-20T18:00:00Z"),
		location: "Creative Studio",
		organiserId: "org-456",
		cancellationDateTime: null,
		coverSrc: "/images/design-workshop.jpg",
		bookings: [],
		gatheringCategoryDetails: []
	},
	{
		gatheringId: 3,
		name: "Networking Event",
		description: "Professional networking event for developers",
		start: new Date("2025-01-10T18:00:00Z"),
		end: new Date("2025-01-10T21:00:00Z"),
		location: "Downtown Hub",
		organiserId: "org-789",
		cancellationDateTime: null,
		coverSrc: "/images/networking.jpg",
		bookings: [],
		gatheringCategoryDetails: [
			{
				gatheringId: 2,
				categoryId: 3,
				category: mockCategories[1],
				gathering: new Gathering()
			}
		]
	}
];

export async function getMockGathering(id: number): Promise<Gathering> {
	return mockGatherings.find((g) => g.gatheringId === id)!;
}

export async function getMockGatherings(
	params: GetGatheringsParams = {}
): Promise<PageResult<Gathering[]>> {
	// Simulate network delay
	await new Promise((resolve) => setTimeout(resolve, 300));

	let filteredGatherings: Gathering[] = [...mockGatherings];

	// Apply filters based on params
	if (params.attendeeId != null) {
		filteredGatherings = filteredGatherings.filter((g) =>
			g.bookings.map((b) => b.attendeeId).includes(params.attendeeId!)
		);
	}

	if (params.organiserId) {
		filteredGatherings = filteredGatherings.filter((g) => g.organiserId === params.organiserId);
	}

	if (params.name) {
		filteredGatherings = filteredGatherings.filter((g) =>
			g.name.toLowerCase().includes(params.name!.toLowerCase())
		);
	}

	if (params.startDateBefore) {
		filteredGatherings = filteredGatherings.filter((g) => g.start <= params.startDateBefore!);
	}

	if (params.startDateAfter) {
		filteredGatherings = filteredGatherings.filter((g) => g.start >= params.startDateAfter!);
	}

	if (params.endDateBefore) {
		filteredGatherings = filteredGatherings.filter((g) => g.end <= params.endDateBefore!);
	}

	if (params.endDateAfter) {
		filteredGatherings = filteredGatherings.filter((g) => g.end >= params.endDateAfter!);
	}

	if (params.isCancelled !== undefined) {
		filteredGatherings = filteredGatherings.filter(
			(g) => (g.cancellationDateTime != null) === params.isCancelled
		);
	}

	const totalCount = filteredGatherings.length;

	// Apply pagination
	const offset = params.offset || 0;
	const limit = params.limit || 10;
	const paginatedGatherings = filteredGatherings.slice(offset, offset + limit);

	return {
		totalCount,
		data: paginatedGatherings
	};
}

export async function createMockGathering(
	gatheringDto: GatheringReqDto,
	_coverImg?: File | null
): Promise<Gathering> {
	return {
		...new Gathering(),
		...gatheringDto
	};
}

export async function updateMockGathering(
	gatheringId: number,
	gatheringDto: GatheringReqDto,
	_coverImg?: File | null
): Promise<Gathering> {
	return {
		...new Gathering(),
		...gatheringDto,
		gatheringId: gatheringId
	};
}
