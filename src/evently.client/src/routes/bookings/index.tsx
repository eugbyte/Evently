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
		return getAccount();
	}
});

export function GatheringsPage(): JSX.Element {
	const account: Account | null = Route.useLoaderData();
	const attendeeId: string = account?.id ?? "-1";

	const [tab, setTab] = useState(0);
	const [bookings, setBookings] = useState<Booking[]>(cloneDeep(_bookings));

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

	const gatherings: Gathering[] = bookings.map((booking) => booking.gathering);
	return (
		<div className="mb-20 p-1 sm:mb-0 sm:p-4">
			<Tabs tab={tab} handleTabChange={handleTabChange} />
			<div className="my-4 grid grid-cols-1 content-evenly justify-items-center gap-4 lg:grid-cols-2 xl:grid-cols-3">
				{gatherings.map((gathering) => (
					<Card key={gathering.gatheringId} gathering={gathering} accountId={account?.id} />
				))}
			</div>
		</div>
	);
}
