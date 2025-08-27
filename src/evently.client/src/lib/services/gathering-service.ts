import type { Gathering } from "~/lib/domains/entities";
import axios from "axios";
import { GatheringReqDto } from "~/lib/domains/models";
import type { PageResult } from "~/lib/domains/interfaces";

export interface GetGatheringsParams {
	attendeeId?: string;
	organiserId?: string;
	name?: string;
	startDateBefore?: Date;
	startDateAfter?: Date;
	endDateBefore?: Date;
	endDateAfter?: Date;
	isCancelled?: boolean;
	offset?: number;
	limit?: number;
}

export async function getGatherings(params: GetGatheringsParams): Promise<PageResult<Gathering[]>> {
	const response = await axios.get<Gathering[]>("/api/v1/Gatherings", { params });
	const gatherings: Gathering[] = response.data;
	for (const gathering of gatherings) {
		gathering.start = new Date(gathering.start);
		gathering.end = new Date(gathering.end);
	}

	const totalCount: number = parseInt(response.headers["x-total-count"]);
	return {
		totalCount,
		data: gatherings
	};
}

export async function getGathering(id: number): Promise<Gathering> {
	const response = await axios.get<Gathering>(`/api/v1/Gatherings/${id}`);
	const gathering: Gathering = response.data;
	gathering.start = new Date(gathering.start);
	gathering.end = new Date(gathering.end);
	return gathering;
}

export async function createGathering(
	gatheringDto: GatheringReqDto,
	coverImg?: File | null
): Promise<Gathering> {
	const formData = new FormData();
	for (const [key, value] of Object.entries(gatheringDto)) {
		if (value != null) {
			formData.set(key, value);
		}
	}
	if (coverImg != null) {
		formData.set("coverImg", coverImg, coverImg.name);
	}
	formData.set("start", gatheringDto.start.toISOString());
	formData.set("end", gatheringDto.end.toISOString());

	const response = await axios.post<Gathering>(`/api/v1/Gatherings`, formData, {
		headers: { "Content-Type": "multipart/form-data" }
	});
	return response.data;
}

export async function updateGathering(
	gatheringId: number,
	gatheringDto: GatheringReqDto,
	coverImg?: File | null
): Promise<Gathering> {
	const formData = new FormData();
	for (const [key, value] of Object.entries(gatheringDto)) {
		if (value != null) {
			formData.set(key, value);
		}
	}
	if (coverImg != null) {
		formData.set("coverImg", coverImg, coverImg.name);
	}
	formData.set("start", gatheringDto.start.toISOString());
	formData.set("end", gatheringDto.end.toISOString());
	if (gatheringDto.cancellationDateTime != null) {
		formData.set("cancellationDateTime", gatheringDto.cancellationDateTime.toISOString());
	}

	const response = await axios.put<Gathering>(`/api/v1/Gatherings/${gatheringId}`, formData, {
		headers: { "Content-Type": "multipart/form-data" }
	});
	return response.data;
}
