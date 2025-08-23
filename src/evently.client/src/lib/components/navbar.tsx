import { type JSX } from "react";
import { Link, useRouteContext, useRouter } from "@tanstack/react-router";
import { logout } from "~/lib/services/auth-service";
import { Icon } from "@iconify/react";

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

	const handleBack = () => {
		router.history.back();
	};

	// Check if we're on the home page
	const isHomePage = router.state.location.pathname === "/";

	return (
		<div className="bg-base-100 navbar shadow-sm fixed top-0 left-0 right-0 z-50">
			<div className="navbar-start">
				{/* Mobile: Hamburger menu and back button */}
				<div className="flex items-center gap-2 lg:hidden">
					{/* Hamburger menu */}
					<div className="dropdown">
						<div tabIndex={0} role="button" className="btn btn-ghost btn-circle">
							<Icon icon="material-symbols:menu-rounded" width="24" height="24" />
						</div>
						<ul
							tabIndex={0}
							className="menu menu-sm dropdown-content bg-base-100 rounded-box z-[1] mt-3 w-52 p-2 shadow"
						>
							<li>
								<Link to="/gatherings" activeProps={{ className: "underline" }}>
									Gatherings
								</Link>
							</li>
							{isAuth && (
								<>
									<li>
										<Link
											to="/bookings/attending"
											activeProps={{ className: "underline" }}
										>
											Attending
										</Link>
									</li>
									<li>
										<Link
											to="/bookings/hosting"
											activeProps={{ className: "underline" }}
										>
											Hosting
										</Link>
									</li>
								</>
							)}
						</ul>
					</div>

					{/* Back button (only show when not on home page) */}
					{!isHomePage && (
						<button onClick={handleBack} className="btn btn-ghost btn-circle">
							<Icon icon="material-symbols:arrow-back-ios" width="24" height="24" />
						</button>
					)}
				</div>

				{/* Brand/Logo */}
				<Link className="btn btn-ghost text-xl" to="/">
					Evently
				</Link>
			</div>

			{/* Desktop menu */}
			<div className="navbar-center hidden lg:flex">
				<ul className="menu menu-horizontal px-1">
					<li>
						<Link
							to="/gatherings"
							activeProps={{ className: "underline" }}
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
								>
									Attending
								</Link>
							</li>
							<li>
								<Link
									to="/bookings/hosting"
									activeProps={{ className: "underline" }}
								>
									Hosting
								</Link>
							</li>
						</>
					)}
				</ul>
			</div>

			{/* Auth buttons */}
			<div className="navbar-end">
				{!isAuth ? (
					<Link to="/login" className="btn btn-sm">
						Login
					</Link>
				) : (
					<button className="btn btn-sm" onClick={handleLogout}>
						Logout
					</button>
				)}
			</div>
		</div>
	);
}