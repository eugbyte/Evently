import { createFileRoute } from "@tanstack/react-router";
import { type JSX, useState } from "react";
import { Account, Booking } from "~/lib/domains/entities";
import { Tabs } from "./-components";
import { getAccount, getBookings, type GetBookingsParams } from "~/lib/services";
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

	const [queryParams, setQueryParams] = useState<GetBookingsParams>({
		attendeeId: account?.id ?? "",
		gatheringEndAfter: new Date()
	});
	const { data: _bookings, isLoading } = useQuery({
		queryKey: ["getBookings", queryParams],
		queryFn: (): Promise<Booking[]> => getBookings(queryParams)
	});

	const handleTabChange = (_tab: number) => {
		setTab(_tab);

		switch (_tab) {
			case 0: {
				setQueryParams({ attendeeId: account?.id ?? "", gatheringEndAfter: new Date() });
				break;
			}
			case 1: {
				setQueryParams({ attendeeId: account?.id ?? "", gatheringEndBefore: new Date() });
				break;
			}
		}
	};

	const bookings: Booking[] = cloneDeep(_bookings ?? []).sort(
		(a, b) => Number(b.isOrganiser) - Number(a.isOrganiser)
	);

	return (
		<div className="mb-20 p-1 sm:mb-0 sm:p-4">
			<Tabs tab={tab} handleTabChange={handleTabChange} />
			<div className="flex w-full flex-row justify-end">
				<button className="btn btn-success">Create Event</button>
			</div>
			{isLoading ? (
				<progress className="progress w-full"></progress>
			) : (
				<div className="my-4 grid grid-cols-1 content-evenly justify-items-center gap-4 lg:grid-cols-2 xl:grid-cols-3">
					{bookings.map((booking) => (
						<Card
							key={booking.gathering.gatheringId}
							gathering={booking.gathering}
							accountId={account?.id}
						/>
					))}
				</div>
			)}
		</div>
	);
}
