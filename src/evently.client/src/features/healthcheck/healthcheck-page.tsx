import { type JSX } from "react";
import { useQuery } from "@tanstack/react-query";
import { getStatus } from "./_services";

export function HealthcheckPage(): JSX.Element {
	const { data: statuses, isLoading } = useQuery({
		queryKey: ["getStatus"],
		queryFn: (): Promise<string[]> => getStatus()
	});
	return <div>{isLoading ? <p>Loading...</p> : <p>{statuses?.join(", ")}</p>}</div>;
}
