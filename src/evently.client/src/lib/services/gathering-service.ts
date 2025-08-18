import type { Gathering } from "~/lib/domains/entities";
import axios from "axios";

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
export async function getGatherings(params: GetGatheringsParams): Promise<Gathering[]> {
	const response = await axios.get<Gathering[]>("/api/v1/Gatherings", { params });
	const gatherings: Gathering[] = response.data;
	for (const gathering of gatherings) {
		gathering.start = new Date(gathering.start);
		gathering.end = new Date(gathering.end);
	}
	return gatherings;
}

export async function getGathering(id: number): Promise<Gathering> {
	const response = await axios.get<Gathering>(`/api/v1/Gatherings/${id}`);
	const gathering: Gathering = response.data;
	gathering.start = new Date(gathering.start);
	gathering.end = new Date(gathering.end);
	return gathering;
}

export async function uploadCoverImg(exhibitionId: number, formData: FormData): Promise<string> {
	const response = await axios.post<Record<string, string>>(
		`/api/v1/Exhibitions/${exhibitionId}/images`,
		formData,
		{
			headers: { "Content-Type": "multipart/form-data" }
		}
	);

	return response.data["coverUri"];
}
