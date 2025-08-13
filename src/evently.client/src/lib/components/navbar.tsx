import { type JSX } from "react";
import { Link } from "@tanstack/react-router";

export function Navbar(): JSX.Element {
	return (
		<div className="bg-base-100 sm:navbar hidden shadow-sm">
			<div className="navbar-start">
				<Link className="btn btn-ghost text-xl" to="/">
					Evently
				</Link>
			</div>
			<div className="navbar-center lg:flex">
				<ul className="menu menu-horizontal px-1">
					<li>
						<Link to="/gatherings">Events</Link>
					</li>
				</ul>
			</div>
			<div className="navbar-end">
				<a className="btn">Login</a>
			</div>
		</div>
	);
}
