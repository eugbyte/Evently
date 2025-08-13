import type { Gathering } from "~/lib/domains/entities";
import axios from "axios";

export interface GetGatheringsParams {
	guestUserId?: number;
	hostUserId?: number;
	exhibitionName?: string;
	start?: Date;
	end?: Date;
	offset?: number;
	limit?: number;
}
export async function getGatherings(params: GetGatheringsParams): Promise<Gathering[]> {
	const response = await axios.get<Gathering[]>("/api/v1/Gatherings", { params });
	return response.data;
}
