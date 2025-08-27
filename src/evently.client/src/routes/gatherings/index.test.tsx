import { render, screen } from "@testing-library/react";
import { GatheringPage } from "./$gatheringId";
import { getMockGatherings } from "~/lib/services/gathering-service.mock";
import * as GatheringService from "~/lib/services";
import userEvent from "@testing-library/user-event";
import {
	createRootRouteWithContext,
	createRoute,
	createRouter,
	Outlet,
	RouterProvider
} from "@tanstack/react-router";
import { Account } from "~/lib/domains/entities";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

interface RouteContext {
	// The ReturnType of your useAuth hook or the value of your AuthContext
	account: Account;
}

it.only("renders GatheringPage", async () => {
	const spy = vi.spyOn(GatheringService, "getGatherings");
	spy.mockImplementation(async (params) => await getMockGatherings(params));

	const rootRoute = createRootRouteWithContext<RouteContext>()({
		beforeLoad: async () => {
			const account: Account | null = new Account();
			return { account };
		},
		component: () => <Outlet />
	});

	const indexRoute = createRoute({
		getParentRoute: () => rootRoute,
		path: "/",
		component: GatheringPage
	});

	const router = createRouter({
		routeTree: rootRoute.addChildren([indexRoute]),
		defaultPendingMinMs: 0,
		context: {
			account: undefined!
		}
	});

	const queryClient = new QueryClient();
	render(
		<QueryClientProvider client={queryClient}>
			<RouterProvider router={router} />
		</QueryClientProvider>
	);

	expect(spy).toHaveBeenCalled();
	expect(screen.getByText("Tech Conference 2024")).toBeInTheDocument();
	expect(screen.getByText("Design Workshop")).toBeInTheDocument();
	expect(screen.getByText("Networking Event")).toBeInTheDocument();

	const input: HTMLInputElement = screen.getByPlaceholderText("Search Gatherings");
	await userEvent.type(input, "T");

	expect(spy).toHaveBeenCalledWith({ name: "T" });
});
