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
		let state = {};

		window.setState = (changes: any) => {
			state = Object.assign({}, state, changes);
		};
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
		userEvent.click(filterBar);

		const input: HTMLInputElement = screen.getByPlaceholderText("Search gatherings...");
		await userEvent.type(input, "T");
		expect(gatheringSpy).toHaveBeenCalledTimes(2);

		const button: HTMLButtonElement = screen.getByRole("button", { name: "»" });
		await userEvent.click(button);
		expect(gatheringSpy).toHaveBeenCalledTimes(3);
	});
});
