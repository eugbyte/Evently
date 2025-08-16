import axios from "axios";
import { Booking } from "~/lib/domains/entities";
import { BookingReqDto } from "~/lib/domains/models";

export interface GetBookingsParams {
	attendeeId?: string;
	gatheringId?: number;
	checkInStart?: Date;
	checkInEnd?: Date;
	gatheringStart?: Date;
	gatheringEnd?: Date;
	isCancelled?: boolean;
	offset?: number;
	limit?: number;
}

export async function getBookings(params: GetBookingsParams): Promise<Booking[]> {
	const response = await axios.get<Booking[]>("/api/v1/Bookings", { params });
	const bookings: Booking[] = response.data;
	for (const booking of bookings) {
		booking.creationDateTime = new Date(booking.creationDateTime);
		booking.checkInDateTime = booking.checkInDateTime ? new Date(booking.checkInDateTime) : null;
		booking.checkoutDateTime = booking.checkoutDateTime ? new Date(booking.checkoutDateTime) : null;
		booking.cancellationDateTime = booking.cancellationDateTime
			? new Date(booking.cancellationDateTime)
			: null;

		if (booking.gathering != null) {
			booking.gathering.start = new Date(booking.gathering.start);
			booking.gathering.end = new Date(booking.gathering.end);
		}
	}
	return bookings;
}

export async function createBooking(bookingReqDto: BookingReqDto): Promise<Booking> {
	const response = await axios.post<Booking>("/api/v1/Bookings", bookingReqDto);
	return response.data;
}
