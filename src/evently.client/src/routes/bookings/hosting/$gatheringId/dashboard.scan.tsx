import { createFileRoute } from "@tanstack/react-router";
import { Scanner } from "./-components";
import { checkInBooking, sleep } from "~/lib/services";
import { Account, Booking } from "~/lib/domains/entities";
import { useState, useCallback } from "react";
import { ToastContent, ToastStatus, toastStyles } from "~/lib/domains/models";

export const Route = createFileRoute("/bookings/hosting/$gatheringId/dashboard/scan")({
	component: RouteComponent
});

function RouteComponent() {
	const { gatheringId } = Route.useParams();
	const [toast, setToast] = useState(new ToastContent(false));
	const [showCamera, setShowCamera] = useState(true);
	// throttle the scan
	const [isPending, setPending] = useState(false);

	const onSuccess = async (data: string) => {
		if (isPending) {
			return;
		}

		setPending(true);

		try {
			// https://localhost:50071/gatherings/1/?bookingId=book_TEiqUReukK
			const url = new URL(data);
			const bookingId: string = url.searchParams.get("bookingId") ?? "";

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
	// eslint-disable-next-line react-hooks/exhaustive-deps
	const memoOnSuccess = useCallback(onSuccess, [gatheringId]);
	const memoOnError = useCallback(() => {}, []);

	const toggleCamera = () => {
		setShowCamera(!showCamera);
	};

	return (
		<div>
			{isPending ? (
				<progress className="progress w-full"></progress>
			) : (
				<div className="flex flex-col items-center justify-center gap-4">
					<Scanner
						memoizedOnSuccess={memoOnSuccess}
						memoizedOnDecodeError={memoOnError}
						showCamera={showCamera}
					/>
					<button
						className="variant-filled btn btn-primary btn-lg mx-auto mt-4 block"
						onClick={toggleCamera}
						type="button"
					>
						{!showCamera ? <p>Scan</p> : <p>Stop Scan</p>}
					</button>

					<div className="divider my-10">OR</div>

					<h3 className="card-title mb-4 text-center text-lg font-semibold">Manual Check-In</h3>

					<div className="flex flex-row items-end">
						<label className="floating-label">
							<span>Booking Id</span>
							<input type="text" className="input input-primary" placeholder="Booking Id" />
						</label>
						<button className="btn btn-primary" type="button">
							Check In
						</button>
					</div>
				</div>
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
