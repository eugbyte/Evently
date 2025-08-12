import { createRootRoute, Link, Outlet } from "@tanstack/react-router";
import { TanStackRouterDevtools } from "@tanstack/react-router-devtools";

export const Route = createRootRoute({
	component: () => (
		<>
			<div className="flex gap-2 p-2">
				<Link to="/" className="[&.active]:font-bold">
					Home
				</Link>{" "}
				<Link to="/healthcheck" className="[&.active]:font-bold">
					Health Check
				</Link>
			</div>
			<hr />
			<Outlet />
			<TanStackRouterDevtools />
		</>
	)
});
