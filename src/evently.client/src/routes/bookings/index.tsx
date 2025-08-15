import { createFileRoute } from "@tanstack/react-router";
import { type JSX, useState } from "react";
import { Account, Booking, Gathering } from "~/lib/domains/entities";
import { Tabs } from "./-components";
import { getAccount, getBookings } from "~/lib/services";
import { Card } from "~/lib/components";
import cloneDeep from "lodash.clonedeep";

export const Route = createFileRoute("/bookings/")({
	component: GatheringsPage,
	loader: async () => {
		const account: Account | null = await getAccount();
		let bookings: Booking[] = [];
		if (account != null) {
			bookings = await getBookings({
				accountId: account.id
			});
		}
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

export function GatheringsPage(): JSX.Element {
	const { bookings: _bookings } = Route.useLoaderData();
	const [tab, setTab] = useState(0);
	const [bookings, setBookings] = useState<Booking[]>(cloneDeep(_bookings));

	const handleTabChange = (_tab: number) => {
		setTab(_tab);

		switch (_tab) {
			case 0: {
				setBookings(
					_bookings.filter(
						(booking) => booking.cancellationDateTime == null && new Date() <= booking.gathering.end
					)
				);
				break;
			}
			case 1: {
				setBookings(_bookings.filter((booking) => booking.gathering.start <= new Date()));
				break;
			}
		}
	};

	const gatherings: Gathering[] = bookings.map((booking) => booking.gathering);
	return (
		<div className="mb-20 p-1 sm:mb-0 sm:p-4">
			<Tabs tab={tab} handleTabChange={handleTabChange} />
			<div className="my-4 grid grid-cols-1 content-evenly justify-items-center gap-4 lg:grid-cols-2 xl:grid-cols-3">
				{gatherings.map((gathering) => (
					<Card key={gathering.gatheringId} gathering={gathering} />
				))}
			</div>
		</div>
	);
}
