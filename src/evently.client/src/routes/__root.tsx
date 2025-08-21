import { createRootRoute, Outlet } from "@tanstack/react-router";
import { TanStackRouterDevtools } from "@tanstack/react-router-devtools";
import { Navbar } from "~/lib/components";
import { type JSX, useEffect } from "react";
import { getAccount, store, type StoreState } from "~/lib/services";
import { Account } from "~/lib/domains/entities";

export const Route = createRootRoute({
	component: App
});

export function App(): JSX.Element {
	useEffect(() => {
		(async () => {
			const account: Account | null = await getAccount();
			store.setState((state: StoreState) => ({
				...state,
				account
			}));
		})();
	}, []);

	return (
		<div className="h-screen">
			<Navbar />
			<Outlet />
			<TanStackRouterDevtools />
		</div>
	);
}
