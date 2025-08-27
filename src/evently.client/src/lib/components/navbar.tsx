import { type JSX } from "react";
import { Link, useLocation, useRouteContext, useRouter } from "@tanstack/react-router";
import { logout } from "~/lib/services/auth-service";
import { Icon } from "@iconify/react";

export function Navbar(): JSX.Element {
	const location = useLocation();
	const router = useRouter();

	const identityUserId: string | undefined = useRouteContext({
		from: "__root__",
		select: (context) => context.account?.id
	});
	console.log({ identityUserId });

	const isHomePage = location.pathname === "/";

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

	return (
		<div className="bg-base-100 navbar fixed top-0 right-0 left-0 z-50 shadow-sm">
			<div className="navbar-start">
				{/* Back button (only show when not on home page) */}
				{!isHomePage && (
					<button onClick={handleBack} className="btn btn-ghost btn-circle btn-xs lg:hidden">
						<Icon
							icon="material-symbols:arrow-circle-left-outline-rounded"
							width="24"
							height="24"
						/>
					</button>
				)}

				{/* Brand/Logo */}
				<Link className="btn btn-ghost btn-lg hidden sm:flex items-center" to="/">
					<span className="hidden sm:inline">Evently</span>
				</Link>
			</div>

			{/* Desktop menu */}
			<div className="navbar-center w-fit">
				<ul className="menu menu-horizontal text-xs sm:text-sm">
					<li>
						<Link to="/gatherings" activeProps={{ className: "underline" }}>
							<span>Gatherings</span>
						</Link>
					</li>
					{isAuth && (
						<>
							<li>
								<Link to="/bookings/attending" activeProps={{ className: "underline" }}>
									<span>Attending</span>
								</Link>
							</li>
							<li>
								<Link to="/bookings/hosting" activeProps={{ className: "underline" }}>
									<span>Hosting</span>
								</Link>
							</li>
						</>
					)}
				</ul>
			</div>

			{/* Auth buttons */}
			<div className="navbar-end">
				{!isAuth ? (
					<Link to="/login" className="btn btn-xs sm:btn-sm">
						Login
					</Link>
				) : (
					<button className="btn btn-xs sm:btn-sm" onClick={handleLogout}>
						Logout
					</button>
				)}
			</div>
		</div>
	);
}
