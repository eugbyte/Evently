import { type JSX } from "react";
import { Link } from "@tanstack/react-router";

export function Dock(): JSX.Element {
	return (
		<div className="dock sm:hidden">
			<Link to="/gatherings" activeProps={{ className: "underline" }}>
				Gatherings
			</Link>
			<Link to="/bookings/attending" activeProps={{ className: "underline" }}>
				Attending
			</Link>
			<Link to="/bookings/hosting" activeProps={{ className: "underline" }}>
				Hosting
			</Link>
		</div>
	);
}
