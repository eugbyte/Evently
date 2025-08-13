import { createRootRoute, Outlet } from "@tanstack/react-router";
import { TanStackRouterDevtools } from "@tanstack/react-router-devtools";
import { Navbar, Dock } from "~/lib/components";

export const Route = createRootRoute({
	component: () => (
		<>
			<Navbar />
			<Outlet />
			<Dock />
			<TanStackRouterDevtools />
		</>
	)
});
