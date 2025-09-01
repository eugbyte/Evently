import type { JSX, ReactNode } from "react";
import { Account } from "~/lib/domains/entities";
import {
	createMemoryHistory,
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

export const WrapperDataTestId = "root-layout";

/**
 * A HOC to wire up the components with TanStack configurations so that it can be tested.
 *
 * Remember to wait for the wrapped Component to be rendered by calling: `await waitFor(() => screen.findByTestId(WrapperDataTestId))`
 * @param children The React Component to be tested
 * @constructor
 */
export function TestWrapper({ children }: Props): JSX.Element {
	const rootRoute = createRootRouteWithContext<RouteContext>()({
		beforeLoad: async () => {
			const account: Account | null = new Account();
			return { account };
		},		
		component: () => (
			<div data-testid={WrapperDataTestId}>
				<Outlet />
			</div>
		)
	});

	const indexRoute = createRoute({
		getParentRoute: () => rootRoute,
		path: "/",
		component: () => <>{children}</>
	});

	const router = createRouter({
		routeTree: rootRoute.addChildren([indexRoute]),
		defaultPendingMinMs: 0,
		history: createMemoryHistory({ initialEntries: ["/"] }),
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
