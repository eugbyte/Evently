import { type JSX } from "react";
import { Link } from "@tanstack/react-router";
import { useStore } from "@tanstack/react-store";
import { store } from "~/lib/services";
import { logout } from "~/lib/services/auth-service";

export function Navbar(): JSX.Element {
	const identityUserId = useStore(store, (s) => s.identityUserId);
	const isAuth: boolean = identityUserId.trim().length > 0;
	console.log({ identityUserId });

	const handleLogout = async () => {
		store.setState((state) => ({
			...state,
			identityUserId: ""
		}));
		await logout("/");
		// await navigate({ to: "/" });
	};
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
						<Link
							to="/gatherings"
							activeProps={{ className: "underline" }}
							activeOptions={{ exact: true }}
						>
							Explore
						</Link>
					</li>
					{isAuth && (
						<>
							<li>
								<Link to="/bookings" activeProps={{ className: "underline" }}>
									Attending
								</Link>
							</li>
							<li>
								<Link to="/gatherings/host" activeProps={{ className: "underline" }}>
									Hosting
								</Link>
							</li>
						</>
					)}
				</ul>
			</div>
			<div className="navbar-end pr-5">
				{!isAuth ? (
					<Link to="/login" className="btn">
						Login
					</Link>
				) : (
					<button className="btn" onClick={handleLogout}>
						Logout
					</button>
				)}
			</div>
		</div>
	);
}
