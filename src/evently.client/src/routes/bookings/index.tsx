import { createFileRoute } from "@tanstack/react-router";
import { type JSX, useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { Account, Gathering } from "~/lib/domains/entities";
import { Tabs } from "./-components";
import { getAccount, getGatherings, type GetGatheringsParams } from "~/lib/services";
import { Card } from "~/lib/components";

export const Route = createFileRoute("/bookings/")({
	component: GatheringsPage,
	loader: async () => {
		return getAccount();
	}
});

export function GatheringsPage(): JSX.Element {
	const account: Account | null = Route.useLoaderData();
	const attendeeId: string = account?.id ?? "-1";

	const [tab, setTab] = useState(0);
	const [queryParams, setQueryParams] = useState<GetGatheringsParams>({
		attendeeId,
		start: new Date()
	});
	const { data: _gatherings, isLoading } = useQuery({
		queryKey: ["getGatherings", queryParams],
		queryFn: (): Promise<Gathering[]> => getGatherings(queryParams)
	});
	let gatherings: Gathering[] = _gatherings ?? [];
	gatherings = [
		...gatherings,
		...gatherings,
		...gatherings,
		...gatherings,
		...gatherings,
		...gatherings
	];
	const handleTabChange = (_tab: number) => {
		setTab(_tab);

		switch (_tab) {
			case 0: {
				setQueryParams({ attendeeId, start: new Date() });
				break;
			}
			case 1: {
				setQueryParams({ attendeeId, end: new Date() });
				break;
			}
		}
	};

	return (
		<div className="mb-20 p-1 sm:mb-0 sm:p-4">
			<Tabs tab={tab} handleTabChange={handleTabChange} />
			{isLoading ? (
				<progress className="progress w-full"></progress>
			) : (
				<div className="my-4 grid grid-cols-1 content-evenly justify-items-center gap-4 lg:grid-cols-2 xl:grid-cols-3">
					{gatherings.map((gathering, index) => (
						<Card key={gathering.gatheringId + "-" + index} gathering={gathering} />
					))}
				</div>
			)}
		</div>
	);
}
