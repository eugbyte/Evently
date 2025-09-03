import { createRootRouteWithContext, Outlet } from "@tanstack/react-router";
import { TanStackRouterDevtools } from "@tanstack/react-router-devtools";
import { Navbar } from "~/lib/components";
import { type JSX } from "react";
import { getAccount } from "~/lib/services";
import { Account } from "~/lib/domains/entities";
import type { RouteContext } from "~/lib/domains/interfaces/route-context.ts";

export const Route = createRootRouteWithContext<RouteContext>()({
	beforeLoad: async () => {
		const account: Account | null = await getAccount();
		return { account };
	},
	component: App
});

export function App(): JSX.Element {
	return (
		<div className="h-screen">
			<Navbar />
			<div className="h-full py-18">
				<Outlet />
				<div className="h-10"></div>
			</div>
			<TanStackRouterDevtools />
		</div>
	);
}
