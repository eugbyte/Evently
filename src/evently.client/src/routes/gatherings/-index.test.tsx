import { render, screen, waitFor } from "@testing-library/react";
import { getMockGatherings } from "~/lib/services/gathering-service.mock";
import type { GetGatheringsParams } from "~/lib/services";
import * as GatheringService from "~/lib/services";
import userEvent from "@testing-library/user-event";
import * as CategoryService from "~/lib/services/category-service";
import { Route as GatheringsRoute } from "./index.tsx";
import { TestRouteWrapper } from "~/lib/components";
import { wrappedRouteId } from "~/lib/components/test-component-wrapper.tsx";

describe("test gatherings page", () => {
	beforeAll(() => {
		Object.defineProperty(global.window, "setState", {
			value: vi.fn(),
			writable: true // Allow modification if needed
		});
	});

	it("renders GatheringPage", async () => {
		const gatheringSpy = vi.spyOn(GatheringService, "getGatherings");
		gatheringSpy.mockImplementation(
			async (params: GetGatheringsParams) => await getMockGatherings(params)
		);

		const categorySpy = vi.spyOn(CategoryService, "getCategories");
		categorySpy.mockResolvedValue([]);

		render(<TestRouteWrapper route={GatheringsRoute} />);
		await waitFor(() => screen.findByTestId(wrappedRouteId));
		expect(categorySpy).toHaveBeenCalledTimes(1);

		expect(gatheringSpy).toHaveBeenCalledTimes(1);
		let element = await screen.findByText("Tech Conference 2024");
		expect(element).toBeInTheDocument();

		element = await screen.findByText("Design Workshop");
		expect(element).toBeInTheDocument();

		element = await screen.findByText("Networking Event");
		expect(element).toBeInTheDocument();

		const filterBar: HTMLDivElement = await screen.findByTestId("filter-bar");
		await userEvent.click(filterBar);

		const input: HTMLInputElement = screen.getByPlaceholderText("Search gatherings...");
		await userEvent.type(input, "T");
		expect(gatheringSpy).toHaveBeenCalledTimes(2);
	});
});
