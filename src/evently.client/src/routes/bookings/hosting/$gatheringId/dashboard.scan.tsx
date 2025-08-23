import { createFileRoute } from "@tanstack/react-router";
import { Scanner } from "./-components";
import { checkInBooking, sleep } from "~/lib/services";
import { Account, Booking } from "~/lib/domains/entities";
import { useState } from "react";
import { ToastContent, ToastStatus, toastStyles } from "~/lib/domains/models";

export const Route = createFileRoute("/bookings/hosting/$gatheringId/dashboard/scan")({
	component: RouteComponent
});

function RouteComponent() {
	const { gatheringId } = Route.useParams();
	const [toast, setToast] = useState(new ToastContent(false));
	// throttle the scan
	const [isPending, setPending] = useState(false);
	// https://gatherings/1/...
	const urlRegex: RegExp = /\/gatherings\/([^/]+)/;

	const onSuccess = async (data: string) => {
		if (isPending) {
			return;
		}

		setPending(true);
		const url = new URL(data);
		const bookingId: string = url.searchParams.get("bookingId") ?? "";
		const _gatheringId: string = url.pathname.match(urlRegex)?.[1] ?? "";

		if (gatheringId !== _gatheringId) {
			// silence for now
			console.warn("Scanner is not the organiser");
		}

		try {
			const booking: Booking = await checkInBooking(bookingId);
			const account: Account = booking.accountDto;
			setToast(new ToastContent(true, `Registered ${account.email}`, ToastStatus.Success));
			await sleep(1000);
		} catch (error) {
			console.error(error);
			setToast(new ToastContent(true, "Invalid QR code", ToastStatus.Error));
			await sleep(1000);
		} finally {
			setToast(new ToastContent(false));
			setPending(false);
		}
	};

	return (
		<div>
			{isPending ? (
				<progress className="progress w-full"></progress>
			) : (
				<Scanner onSuccess={onSuccess} onDecodeError={() => {}} />
			)}
			{toast.show && (
				<div className="toast toast-center">
					<div className={`${toastStyles[toast.toastStatus]}`}>
						<span>{toast.message}</span>
					</div>
				</div>
			)}
		</div>
	);
}
