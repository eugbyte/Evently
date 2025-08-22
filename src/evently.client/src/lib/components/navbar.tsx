import { type JSX } from "react";
import { Link, useRouteContext, useRouter } from "@tanstack/react-router";
import { logout } from "~/lib/services/auth-service";

export function Navbar(): JSX.Element {
	const router = useRouter();
	const identityUserId: string | undefined = useRouteContext({
		from: "__root__",
		select: (context) => context.account?.id
	});
	console.log({ identityUserId });

	let isAuth = false;
	if (identityUserId != null) {
		isAuth = identityUserId.trim().length > 0;
	}

	const handleLogout = async () => {
		await router.invalidate();
		await logout("/");
	};
	return (
		<div className="bg-base-100 navbar shadow-sm">
			<div className="navbar-start">
				<Link className="btn btn-ghost btn-sm sm:btn-md hidden text-xl sm:block" to="/">
					Evently
				</Link>
			</div>
			<div className="navbar-center lg:flex">
				<ul className="menu menu-horizontal px-1">
					<li>
						<Link
							to="/gatherings"
							activeProps={{ className: "underline" }}
							className="text-xs sm:text-sm"
						>
							Gatherings
						</Link>
					</li>
					{isAuth && (
						<>
							<li>
								<Link
									to="/bookings/attending"
									activeProps={{ className: "underline" }}
									className="text-xs sm:text-sm"
								>
									Attending
								</Link>
							</li>
							<li>
								<Link
									to="/bookings/hosting"
									activeProps={{ className: "underline" }}
									className="text-xs sm:text-sm"
								>
									Hosting
								</Link>
							</li>
						</>
					)}
				</ul>
			</div>
			<div className="navbar-end pr-5">
				{!isAuth ? (
					<Link to="/login" className="btn btn-xs sm:btn-md">
						Login
					</Link>
				) : (
					<button className="btn btn-xs sm:btn-md" onClick={handleLogout}>
						Logout
					</button>
				)}
			</div>
		</div>
	);
}
