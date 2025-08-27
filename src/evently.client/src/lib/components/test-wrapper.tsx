import type { ReactNode } from "react";
import { Account } from "~/lib/domains/entities";
import {
	createRootRouteWithContext,
	createRoute,
	createRouter,
	Outlet,
	RouterProvider
} from "@tanstack/react-router";
import type { RouteContext } from "~/lib/domains/interfaces";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

interface Props {
	children: ReactNode;
}

export function TestWrapper({ children }: Props) {
	const rootRoute = createRootRouteWithContext<RouteContext>()({
		beforeLoad: async () => {
			const account: Account | null = new Account();
			return { account };
		},
		component: () => <div data-testid="root-layout">
            <Outlet />
        </div>
	});

	const indexRoute = createRoute({
		getParentRoute: () => rootRoute,
		path: "/",
		component: () => <>{children}</>
	});

	const router = createRouter({
		routeTree: rootRoute.addChildren([indexRoute]),
		defaultPendingMinMs: 0,
		context: {
			account: undefined!
		}
	});
	const queryClient = new QueryClient();

	return (
		<QueryClientProvider client={queryClient}>
			<RouterProvider router={router} />
		</QueryClientProvider>
	);
}
