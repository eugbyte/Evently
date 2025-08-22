import { createRootRouteWithContext, Outlet } from "@tanstack/react-router";
import { TanStackRouterDevtools } from "@tanstack/react-router-devtools";
import { Navbar } from "~/lib/components";
import { type JSX } from "react";
import { getAccount } from "~/lib/services";
import { Account } from "~/lib/domains/entities";

interface RouteContext {
	// The ReturnType of your useAuth hook or the value of your AuthContext
	account: Account;
}

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
			<Outlet />
			<TanStackRouterDevtools />
		</div>
	);
}
