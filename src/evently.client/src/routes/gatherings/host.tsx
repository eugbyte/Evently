import { createFileRoute } from "@tanstack/react-router";
import { type JSX, useState } from "react";
import { Account, Gathering } from "~/lib/domains/entities";
import { getAccount, getGatherings, type GetGatheringsParams } from "~/lib/services";
import { Card, Tabs, TabState } from "~/lib/components";
import { useQuery } from "@tanstack/react-query";
import cloneDeep from "lodash.clonedeep";

export const Route = createFileRoute("/gatherings/host")({
	component: GetHostedGatheringsPage,
	loader: async () => getAccount(),
	pendingComponent: () => (
		<div className="h-full">
			<progress className="progress w-full"></progress>
		</div>
	)
});

export function GetHostedGatheringsPage(): JSX.Element {
	const account: Account | null = Route.useLoaderData();
	const [tab, setTab] = useState(0);

	const [queryParams, setQueryParams] = useState<GetGatheringsParams>({
		organiserId: account?.id ?? "",
		endDateAfter: new Date(),
		isCancelled: false
	});
	const { data: _hostedGatherings, isLoading: isHostedGatheringLoading } = useQuery({
		queryKey: ["getHostedGatherings", queryParams],
		queryFn: (): Promise<Gathering[]> => getGatherings(queryParams)
	});
	const isLoading: boolean = isHostedGatheringLoading;

	let gatherings: Gathering[] = cloneDeep(_hostedGatherings ?? []);
	gatherings = gatherings.sort((a, b) => b.end.getTime() - a.end.getTime());

	const handleTabChange = (_tab: number) => {
		setTab(_tab);

		switch (_tab) {
			case TabState.Upcoming: {
				setQueryParams({
					organiserId: account?.id ?? "",
					endDateAfter: new Date(),
					isCancelled: false
				});
				break;
			}
			case TabState.Past: {
				setQueryParams({
					organiserId: account?.id ?? "",
					endDateBefore: new Date(),
					isCancelled: false
				});
				break;
			}
		}
	};

	return (
		<div className="h-full p-1">
			<Tabs tab={tab} handleTabChange={handleTabChange} />
			<div className="flex w-full flex-row justify-end px-4">
				<button className="btn btn-success">Host Event</button>
			</div>
			{gatherings.length === 0 && !isLoading && <div className="text-center">None Found</div>}
			{isLoading ? (
				<progress className="progress mx-2 w-full"></progress>
			) : (
				<div className="my-4 grid grid-cols-1 content-evenly justify-items-center gap-4 lg:grid-cols-2 xl:grid-cols-3">
					{gatherings.map((gathering) => (
						<Card key={gathering.gatheringId} gathering={gathering} accountId={account?.id} />
					))}
				</div>
			)}
		</div>
	);
}
