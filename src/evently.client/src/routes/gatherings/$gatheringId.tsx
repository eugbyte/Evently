import { createFileRoute } from "@tanstack/react-router";
import { type JSX, useEffect, useRef } from "react";
import { Account, Booking, Category, Gathering } from "~/lib/domains/entities";
import Placeholder from "~/lib/assets/event_placeholder.webp";
import { createBooking, getAccount, getBookings, getGathering } from "~/lib/services";
import { DateTime } from "luxon";
import { Icon } from "@iconify/react";
import { useMutation } from "@tanstack/react-query";
import { BookingReqDto } from "~/lib/domains/models";
import QRCode from "qrcode";

export const Route = createFileRoute("/gatherings/$gatheringId")({
	loader: async ({ params }) => {
		const account: Account | null = await getAccount();
		const gatheringId: number = parseInt(params.gatheringId);
		const gathering: Gathering | null = await getGathering(gatheringId);
		const bookings: Booking[] = await getBookings({
			attendeeId: account?.id ?? "",
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
		<div className="h-full px-2">
			<progress className="progress w-full"></progress>
		</div>
	)
});

export function GatheringPage(): JSX.Element {
	const { gathering, account, ...rest } = Route.useLoaderData();
	const { name: title, description, coverSrc, gatheringCategoryDetails } = gathering;
	const categories: Category[] = gatheringCategoryDetails.map((detail) => detail.category);

	const imgSrc: string = coverSrc == null || coverSrc.length === 0 ? Placeholder : coverSrc;
	const isOrganiser: boolean = gathering.organiserId === account?.id;
	const isAttendee: boolean = !isOrganiser;
	const start: DateTime = DateTime.fromJSDate(gathering.start);
	const end: DateTime = DateTime.fromJSDate(gathering.end);

	const { mutate: handleRegister, data } = useMutation({
		mutationFn: () => {
			const dto = new BookingReqDto();
			dto.attendeeId = account?.id ?? "";
			dto.gatheringId = gathering.gatheringId;
			return createBooking(dto);
		}
	});
	const booking: Booking | null = data ?? rest.booking;

	const dialogRef = useRef<HTMLDialogElement>(null);
	const canvasRef = useRef<HTMLCanvasElement>(null);
	useEffect(() => {
		const url = new URL(`bookings/${booking?.bookingId}`, window.location.href);
		QRCode.toCanvas(canvasRef.current, url.href, (error) => {
			if (error) {
				console.error(error);
			}
		});
	}, [canvasRef.current]);

	return (
		<div className="mb-20 sm:mb-0">
			<div className="card bg-base-200 card-lg container mx-auto w-full text-white shadow-sm md:w-2/3">
				<figure className="mt-0 sm:mt-5">
					<img src={imgSrc} alt="Event Image" className="h-60 w-120 rounded-lg" />
				</figure>
				<div className="card-body">
					<h2 className="card-title">{title}</h2>
					<p>{description}</p>
					<div className="flex flex-row items-center space-x-2 text-xs">
						<Icon icon="material-symbols:location-on" width="24" height="24" className="inline">
							Location
						</Icon>
						<span>{gathering.location}</span>
					</div>
					<div className="flex flex-row items-center space-x-2 text-xs">
						<Icon icon="mingcute:time-fill" width="24" height="24" />
						<span>{`${start.toLocaleString(DateTime.DATETIME_MED)} — ${end.toLocaleString(DateTime.DATETIME_MED)}`}</span>
					</div>
					<div className="flex flex-row flex-wrap space-x-2">
						{categories.map((category) => (
							<div className="badge badge-soft badge-info" key={category.categoryId}>
								{category.categoryName}
							</div>
						))}
					</div>
					<div className="divider">More</div>
					{isAttendee && booking?.accountId === account?.id && (
						<div className="card-actions justify-between">
							<button className="btn btn-info" onClick={() => dialogRef.current?.showModal()}>
								View QR
							</button>
							<button className="btn btn-error">Cancel</button>
						</div>
					)}
					{isAttendee && (booking == null || account == null) && (
						<div className="card-actions justify-between">
							<button className="btn btn-primary" onClick={() => handleRegister()}>
								Register
							</button>
						</div>
					)}
					{isOrganiser && (
						<>
							<div className="card-actions justify-between">
								<button className="btn btn-primary">Edit</button>
								<button className="btn btn-error">Cancel</button>
							</div>
							<div className="card-actions my-2 justify-between">
								<button className="btn btn-accent btn-sm">Manage Registrations</button>
							</div>
						</>
					)}
					<dialog className="modal" ref={dialogRef}>
						<div className="modal-box">
							<h3 className="text-lg font-bold">Event Ticket</h3>
							<canvas className="mx-auto block" id="qr-canvas" ref={canvasRef}></canvas>
							<div className="my-2 text-center">
								<code>{booking?.bookingId}</code>
							</div>
							<div className="modal-action">
								<form method="dialog">
									{/* if there is a button in form, it will close the modal */}
									<button className="btn">Close</button>
								</form>
							</div>
						</div>
					</dialog>
				</div>
			</div>
		</div>
	);
}
