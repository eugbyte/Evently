import { createFileRoute } from "@tanstack/react-router";
import type { JSX } from "react";
import { useQuery } from "@tanstack/react-query";
import { getStatus } from "./-services/health-check-service";

export const Route = createFileRoute("/healthcheck/")({
	component: HealthcheckPage
});

export function HealthcheckPage(): JSX.Element {
	const { data: _statuses, isLoading } = useQuery({
		queryKey: ["getStatus"],
		queryFn: (): Promise<Record<string, string>> => getStatus()
	});
	const statuses: Record<string, string> = _statuses ?? {};

	return (
		<div className="h-full flex flex-col justify-center items-center space-y-4">
			{isLoading ? (
				<p>Loading...</p>
			) : (
				<>
					{Object.entries(statuses).map(([key, value]) => (
						<code>
							{key}: {value}
						</code>
					))}
				</>
			)}
		</div>
	);
}
