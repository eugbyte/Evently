import axios from "axios";

export async function getStatus(): Promise<Record<string, string>> {
	const response = await axios.get<Record<string, string>>("/api/v1/HealthChecks");
	return response.data;
}
