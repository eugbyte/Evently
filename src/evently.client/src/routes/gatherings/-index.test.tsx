import { render, screen, waitFor } from "@testing-library/react";
import { getMockGatherings } from "~/lib/services/gathering-service.mock";
import type { GetGatheringsParams } from "~/lib/services";
import * as GatheringService from "~/lib/services";
import userEvent from "@testing-library/user-event";
import { WrapperDataTestId } from "~/lib/components";
import * as CategoryService from "~/lib/services/category-service";
import {
	createMemoryHistory,
	createRouter,
	RouterProvider
} from "@tanstack/react-router";
import { Route } from "./index.tsx";
import {QueryClient, QueryClientProvider } from "@tanstack/react-query";

it("renders GatheringPage", async () => {
	const gatheringSpy = vi.spyOn(GatheringService, "getGatherings");
	gatheringSpy.mockImplementation(
		async (params: GetGatheringsParams) => await getMockGatherings(params)
	);

	const categorySpy = vi.spyOn(CategoryService, "getCategories");
	categorySpy.mockResolvedValue([]);
	
	const router = createRouter({
		routeTree: Route,
		defaultPendingMinMs: 0,
		history: createMemoryHistory({ initialEntries: ["/"] }),
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
	await waitFor(() => screen.findByTestId(WrapperDataTestId));
	expect(categorySpy).toHaveBeenCalledTimes(1);

	expect(gatheringSpy).toHaveBeenCalledTimes(1);
	let element = await screen.findByText("Tech Conference 2024");
	expect(element).toBeInTheDocument();

	element = await screen.findByText("Design Workshop");
	expect(element).toBeInTheDocument();

	element = await screen.findByText("Networking Event");
	expect(element).toBeInTheDocument();

	const input: HTMLInputElement = screen.getByPlaceholderText("Search Gatherings");
	await userEvent.type(input, "T");
	expect(gatheringSpy).toHaveBeenCalledTimes(2);

	const button: HTMLButtonElement = screen.getByRole("button", { name: "»" });
	await userEvent.click(button);
	expect(gatheringSpy).toHaveBeenCalledTimes(3);
});
