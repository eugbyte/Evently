import { createFileRoute } from "@tanstack/react-router";
import { Scanner } from "./-components";
import { checkInBooking } from "~/lib/services";
import { Account, Booking } from "~/lib/domains/entities";
import { useState } from "react";
import { ToastContent, ToastStatus, toastStyles } from "~/lib/domains/models";

export const Route = createFileRoute("/bookings/hosting/$gatheringId/dashboard/scan")({
	component: RouteComponent
});

function RouteComponent() {
	const [toast, setToast] = useState(new ToastContent(false));
	const onSuccess = async (data: string) => {
		const url = new URL(data);
		const bookingId: string = url.searchParams.get("bookingId") ?? "";
		console.log({ bookingId });

		try {
			const booking: Booking = await checkInBooking(bookingId);
			const account: Account = booking.accountDto;
			setToast(new ToastContent(true, `Registered ${account.email}`, ToastStatus.Success));
		} catch (error) {
			console.error(error);
			setToast(new ToastContent(true, "Invalid QR code", ToastStatus.Error));
		}
		setTimeout(() => {
			setToast(new ToastContent(false));
		}, 1000);
	};
	return (
		<div>
			<Scanner onSuccess={onSuccess} onDecodeError={() => {}} />
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
