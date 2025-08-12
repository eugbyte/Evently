export async function getStatus(): Promise<Record<string, string>> {
	const response = await fetch("/api/v1/HealthChecks");
	if (!response.ok) {
		throw new Error(`Response status: ${response.status}`);
	}
	return await response.json();
}
