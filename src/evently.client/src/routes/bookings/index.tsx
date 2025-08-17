import { createFileRoute } from "@tanstack/react-router";
import { type JSX, useState } from "react";
import { Account, Gathering } from "~/lib/domains/entities";
import { Tabs } from "./-components";
import { getAccount, getGatherings, type GetGatheringsParams } from "~/lib/services";
import { Card } from "~/lib/components";
import cloneDeep from "lodash.clonedeep";
import { useQuery } from "@tanstack/react-query";

export const Route = createFileRoute("/bookings/")({
	component: GetBookingsPage,
	loader: async () => {
		return getAccount();
	},
	pendingComponent: () => (
		<div className="h-full">
			<progress className="progress w-full"></progress>
		</div>
	)
});

export function GetBookingsPage(): JSX.Element {
	const account: Account | null = Route.useLoaderData();
	const [tab, setTab] = useState(0);

	const [queryParams, setQueryParams] = useState<GetGatheringsParams>({
		attendeeId: account?.id ?? "",
		endDateAfter: new Date()
	});

	const { data: _gatherings, isLoading } = useQuery({
		queryKey: ["getGatherings", queryParams],
		queryFn: (): Promise<Gathering[]> => getGatherings(queryParams)
	});

	const handleTabChange = (_tab: number) => {
		setTab(_tab);

		switch (_tab) {
			case 0: {
				setQueryParams({ attendeeId: account?.id ?? "", endDateAfter: new Date() });
				break;
			}
			case 1: {
				setQueryParams({ attendeeId: account?.id ?? "", startDateBefore: new Date() });
				break;
			}
			case 2: {
				setQueryParams({ organiserId: account?.id ?? "" });
				break;
			}
		}
	};

	const gatherings: Gathering[] = cloneDeep(_gatherings ?? []).sort(
		(a, b) => b.end.getTime() - a.end.getTime()
	);

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
