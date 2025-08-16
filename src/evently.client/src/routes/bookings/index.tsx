import { createFileRoute } from "@tanstack/react-router";
import { type JSX, useState } from "react";
import { Account, Booking, Gathering } from "~/lib/domains/entities";
import { Tabs } from "./-components";
import { getAccount, getBookings, type GetBookingsParams } from "~/lib/services";
import { Card } from "~/lib/components";
import { useQuery } from "@tanstack/react-query";
import cloneDeep from "lodash.clonedeep";

export const Route = createFileRoute("/bookings/")({
	component: GetBookingsPage,
	loader: async () => {
		return getAccount();
	}
});

export function GetBookingsPage(): JSX.Element {
	const account: Account | null = Route.useLoaderData();
	const [tab, setTab] = useState(0);

	const [queryParams, setQueryParams] = useState<GetBookingsParams>({
		attendeeId: account?.id ?? ""
	});
	const { data: _bookings, isLoading } = useQuery({
		queryKey: ["getBookings", queryParams],
		queryFn: (): Promise<Booking[]> => getBookings(queryParams)
	});

	const handleTabChange = (_tab: number) => {
		setTab(_tab);

		switch (_tab) {
			case 0: {
				setQueryParams({
					attendeeId: account?.id ?? "",
					gatheringStart: new Date(),
					isCancelled: false
				});
				break;
			}
			case 1: {
				setQueryParams({ attendeeId: account?.id ?? "", gatheringEnd: new Date() });
				break;
			}
		}
	};

	const bookings: Booking[] = cloneDeep(_bookings ?? []).sort(
		(a, b) => Number(b.isOrganiser) - Number(a.isOrganiser)
	);
	const gatherings: Gathering[] = bookings.map((booking) => booking.gathering);
	return (
		<div className="mb-20 p-1 sm:mb-0 sm:p-4">
			<Tabs tab={tab} handleTabChange={handleTabChange} />
			{isLoading ? (
				<progress className="progress w-full"></progress>
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
