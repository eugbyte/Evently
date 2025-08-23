import { createFileRoute } from "@tanstack/react-router";
import { Scanner } from "./-components";
import { checkInBooking, sleep } from "~/lib/services";
import { Booking } from "~/lib/domains/entities";
import { useCallback, useState } from "react";
import { ToastContent, ToastStatus, toastStyles } from "~/lib/domains/models";
import { useForm } from "@tanstack/react-form";
import { FieldErrMsg as FieldInfo } from "~/lib/components";

export const Route = createFileRoute("/bookings/hosting/$gatheringId/dashboard/scan")({
	component: RouteComponent
});

function RouteComponent() {
	const [toast, setToast] = useState(new ToastContent(false));
	const [showCamera, setShowCamera] = useState(true);
	// throttle the scan
	const [isPending, setPending] = useState(false);
	const [showAccordion, setShowAccordion] = useState(false);

	const form = useForm({
		defaultValues: {
			bookingId: ""
		},
		onSubmit: async ({ value }) => {
			// Do something with form data
			const { bookingId } = value;

			setPending(true);
			try {
				const booking: Booking = await checkInBooking(bookingId);
				setToast(
					new ToastContent(true, `Registered ${booking.accountDto.email}`, ToastStatus.Success)
				);
				await sleep(1000);
			} catch (e) {
				console.error(e);
				setToast(new ToastContent(true, "Invalid QR code", ToastStatus.Error));
				await sleep(1000);
			}
			setToast(new ToastContent(false));
			setPending(false);
		}
	});
	const { handleSubmit } = form;

	const onSuccess = async (data: string) => {
		if (isPending || data == null || data.trim().length === 0) {
			return;
		}

		setPending(true);

		try {
			// https://localhost:50071/gatherings/1/?bookingId=book_TEiqUReukK
			const url = new URL(data);
			const bookingId: string = url.searchParams.get("bookingId") ?? "";
			form.setFieldValue("bookingId", bookingId);
			await handleSubmit();
		} catch (error) {
			console.error(error);
		}
	};
	// eslint-disable-next-line react-hooks/exhaustive-deps
	const memoOnSuccess = useCallback(onSuccess, []);
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
					<section>
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
					</section>

					<div className="divider my-10">OR</div>

					<section>
						<div
							tabIndex={0}
							className="collapse-arrow bg-base-100 border-base-300 collapse border"
						>
							<input
								type="checkbox"
								checked={showAccordion}
								onChange={(e) => setShowAccordion(e.target.checked)}
							/>
							<div className="collapse-title font-semibold">Manual Check-In</div>
							<div className="collapse-content justify-center text-sm">
								<div className="flex flex-row items-end">
									<form
										onSubmit={(e) => {
											e.preventDefault();
											e.stopPropagation();
											form.handleSubmit();
										}}
									>
										<form.Field
											name="bookingId"
											validators={{
												onChange: ({ value }) => {
													return value.trim().length === 0
														? "First name must be at least 3 characters"
														: undefined;
												}
											}}
											children={(field) => (
												<>
													<label className="floating-label">
														<span>Booking Id</span>
														<input
															type="text"
															className="input input-primary"
															placeholder="Booking Id"
															id={field.name}
															name={field.name}
															value={field.state.value}
															onBlur={field.handleBlur}
															onChange={(e) => field.handleChange(e.target.value)}
														/>
													</label>
													<FieldInfo field={field} />
												</>
											)}
										/>
									</form>
									<button className="btn btn-primary" type="button" onClick={handleSubmit}>
										Check In
									</button>
								</div>
							</div>
						</div>
					</section>
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
