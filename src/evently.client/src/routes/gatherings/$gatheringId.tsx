import { createFileRoute } from "@tanstack/react-router";
import { type JSX } from "react";
import { Account, Booking, Gathering } from "~/lib/domains/entities";
import Placeholder from "~/lib/assets/event_placeholder.webp";
import { getAccount, getBookings, getGathering } from "~/lib/services";

export const Route = createFileRoute("/gatherings/$gatheringId")({
	loader: async ({ params }) => {
		const account: Account | null = await getAccount();
		const gatheringId: number = parseInt(params.gatheringId);
		const gathering: Gathering | null = await getGathering(gatheringId);
		const bookings: Booking[] = await getBookings({
			accountId: account?.id,
			gatheringId,
			isCancelled: false
		});

		return {
			gathering,
			booking: bookings.length > 0 ? bookings[0] : null,
			account
		};
	},
	component: GatheringPage,
	pendingComponent: () => (
		<div className="h-full">
			<progress className="progress w-full"></progress>
		</div>
	)
});

export function GatheringPage(): JSX.Element {
	const { gathering } = Route.useLoaderData();
	const { name: title, description, coverSrc } = gathering;
	const imgSrc: string = coverSrc == null || coverSrc.length === 0 ? Placeholder : coverSrc;

	return (
		<div className="mb-20 sm:mb-0">
			<div className="card bg-base-200 card-lg container mx-auto w-full text-white shadow-sm md:w-2/3">
				<figure className="mt-5">
					<img src={imgSrc} alt="Event Image" className="h-60 w-120 rounded-lg" />
				</figure>
				<div className="card-body">
					<h2 className="card-title">{title}</h2>
					<p>{description}</p>
					<div className="card-actions justify-between">
						<button className="btn btn-primary">Register</button>
						<button className="btn btn-primary">View QR</button>
					</div>
				</div>
			</div>
		</div>
	);
}
