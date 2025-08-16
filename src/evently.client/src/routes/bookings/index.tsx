import { createFileRoute } from "@tanstack/react-router";
import { type JSX, useState } from "react";
import { Account, Booking } from "~/lib/domains/entities";
import { Tabs } from "./-components";
import { getAccount, getBookings } from "~/lib/services";
import { Card } from "~/lib/components";
import cloneDeep from "lodash.clonedeep";

export const Route = createFileRoute("/bookings/")({
	component: GetBookingsPage,
	loader: async () => {
		const account: Account | null = await getAccount();
		const bookings: Booking[] = await getBookings({ attendeeId: account?.id ?? "" });
		return {
			account,
			bookings
		};
	},
	pendingComponent: () => (
		<div className="h-full">
			<progress className="progress w-full"></progress>
		</div>
	)
});

export function GetBookingsPage(): JSX.Element {
	const { account, ...rest } = Route.useLoaderData();
	const [tab, setTab] = useState(0);
	const _bookings: Booking[] = cloneDeep(rest.bookings).sort(
		(a, b) => Number(b.isOrganiser) - Number(a.isOrganiser)
	);

	const [bookings, setBookings] = useState<Booking[]>(cloneDeep(_bookings));

	const handleTabChange = (_tab: number) => {
		setTab(_tab);

		switch (_tab) {
			case 0: {
				setBookings(_bookings.filter((booking) => new Date() <= booking.gathering.end));
				break;
			}
			case 1: {
				setBookings(_bookings.filter((booking) => new Date() > booking.gathering.end));
				break;
			}
		}
	};

	return (
		<div className="mb-20 p-1 sm:mb-0 sm:p-4">
			<Tabs tab={tab} handleTabChange={handleTabChange} />
			<div className="my-4 grid grid-cols-1 content-evenly justify-items-center gap-4 lg:grid-cols-2 xl:grid-cols-3">
				{bookings.map((booking) => (
					<Card
						key={booking.gathering.gatheringId}
						gathering={booking.gathering}
						accountId={account?.id}
						bookingId={booking.bookingId}
					/>
				))}
			</div>
		</div>
	);
}
