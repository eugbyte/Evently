import { type JSX } from "react";
import { Link } from "@tanstack/react-router";

export function Dock(): JSX.Element {
	return (
		<div className="dock sm:hidden">
			<Link to="/gatherings">Gatherings</Link>
			<Link to="/gatherings">Profile</Link>
		</div>
	);
}
