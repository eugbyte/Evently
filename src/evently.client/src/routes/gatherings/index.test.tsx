import { render, screen } from "@testing-library/react";
import { GatheringPage } from "./$gatheringId";
import { getMockGatherings } from "~/lib/services/gathering-service.mock";
import * as GatheringService from "~/lib/services";
import userEvent from "@testing-library/user-event";
it("renders GatheringPage", async () => {
	const spy = vi.spyOn(GatheringService, "getGatherings");
	spy.mockImplementation(async (params) => await getMockGatherings(params));

	render(<GatheringPage />);
	expect(spy).toHaveBeenCalled();
	expect(screen.getByText("Tech Conference 2024")).toBeInTheDocument();
	expect(screen.getByText("Design Workshop")).toBeInTheDocument();
	expect(screen.getByText("Networking Event")).toBeInTheDocument();

	const input: HTMLInputElement = screen.getByPlaceholderText("Search Gatherings");
	await userEvent.type(input, "T");

	expect(spy).toHaveBeenCalledWith({ name: "T" });
});
