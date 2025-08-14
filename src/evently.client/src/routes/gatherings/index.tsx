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
	let gatherings: Gathering[] = _gatherings ?? [];
	gatherings = [
		...gatherings,
		...gatherings,
		...gatherings,
		...gatherings,
		...gatherings,
		...gatherings
	];
	const guestUserId = 2;
	const handleTabChange = (_tab: number) => {
		setTab(_tab);

		switch (_tab) {
			case 0: {
				setQueryParams({});
				break;
			}
			case 1: {
				setQueryParams({ guestId: guestUserId, start: new Date() });
				break;
			}
			case 2: {
				setQueryParams({ guestId: guestUserId, end: new Date() });
				break;
			}
		}
	};

	return (
		<div className="mb-20 border p-1 sm:mb-0 sm:p-4">
			<div className="flex flex-col justify-between space-y-5 sm:flex-row">
				<Tabs tab={tab} handleTabChange={handleTabChange} />
				<label className="input w-40">
					<svg
						className="h-[1em] opacity-50"
						xmlns="http://www.w3.org/2000/svg"
						viewBox="0 0 24 24"
					>
						<g
							strokeLinejoin="round"
							strokeLinecap="round"
							strokeWidth="2.5"
							fill="none"
							stroke="currentColor"
						>
							<circle cx="11" cy="11" r="8"></circle>
							<path d="m21 21-4.3-4.3"></path>
						</g>
					</svg>
					<input type="search" className="grow" placeholder="Search" />
				</label>
			</div>
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
