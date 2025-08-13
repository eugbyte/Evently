import { createFileRoute } from "@tanstack/react-router";
import { type JSX, useState } from "react";
import { getGatherings, type GetGatheringsParams } from "./-services/gathering-service";
import { useQuery } from "@tanstack/react-query";
import { Gathering } from "~/lib/domains/entities";
import { Tabs } from "./-components/tabs";
import { Card } from "~/routes/gatherings/-components/card.tsx";

export const Route = createFileRoute("/gatherings/")({
	component: GatheringsPage
});

export function GatheringsPage(): JSX.Element {
	const [tab, setTab] = useState(0);
	const [queryParams, setQueryParams] = useState<GetGatheringsParams>({});
	const { data: _gatherings, isLoading } = useQuery({
		queryKey: ["getGatherings", queryParams],
		queryFn: (): Promise<Gathering[]> => getGatherings(queryParams)
	});
	const gatherings: Gathering[] = _gatherings ?? [];
	const guestUserId = 2;
	const handleTabChange = (_tab: number) => {
		setTab(_tab);

		switch (_tab) {
			case 0: {
				setQueryParams({});
				break;
			}
			case 1: {
				setQueryParams({ guestUserId, start: new Date() });
				break;
			}
			case 2: {
				setQueryParams({ guestUserId, end: new Date() });
				break;
			}
		}
	};

	return (
		<div className="border border-white p-1 sm:p-4">
			<Tabs tab={tab} handleTabChange={handleTabChange} />
			{isLoading ? (
				<progress className="progress w-full"></progress>
			) : (
				<div className="my-4">
					{gatherings.map((gathering) => (
						<Card
							key={gathering.gatheringId}
							title={gathering.name}
							description={gathering.description}
							imgSrc={gathering.coverSrc}
						/>
					))}
				</div>
			)}
		</div>
	);
}
