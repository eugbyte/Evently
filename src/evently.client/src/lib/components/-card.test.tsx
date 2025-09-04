import { render, screen, waitFor } from "@testing-library/react";
import { Card } from "~/lib/components/card.tsx";
import { Gathering } from "~/lib/domains/entities";
import { getMockGathering } from "~/lib/services/gathering-service.mock";
import { TestWrappers, wrappedComponentId } from "~/lib/components/test-wrappers.tsx";

describe("Card Component", () => {
	let mockGathering: Gathering;
	beforeEach(async () => {
		mockGathering = await getMockGathering(1);
	});

	it("renders gathering name", async () => {
		render(
			<TestWrappers>
				<Card gathering={mockGathering} />
			</TestWrappers>
		);
		await waitFor(() => screen.findByTestId(wrappedComponentId));
		expect(screen.getByText("Tech Conference 2024")).toBeInTheDocument();
	});

	it("renders gathering description", async () => {
		render(
			<TestWrappers>
				<Card gathering={mockGathering} />
			</TestWrappers>
		);
		await waitFor(() => screen.findByTestId(wrappedComponentId));

		expect(
			screen.getByText("Annual technology conference with industry leaders")
		).toBeInTheDocument();
	});

	it("renders gathering location", async () => {
		render(
			<TestWrappers>
				<Card gathering={mockGathering} />
			</TestWrappers>
		);
		await waitFor(() => screen.findByTestId(wrappedComponentId));

		expect(screen.getByText("Convention Center")).toBeInTheDocument();
	});

	it("renders cover image when coverSrc is provided", async () => {
		render(
			<TestWrappers>
				<Card gathering={mockGathering} />
			</TestWrappers>
		);
		await waitFor(() => screen.findByTestId(wrappedComponentId));

		const image = screen.getByRole("img");
		expect(image).toBeInTheDocument();
		expect(image).toHaveAttribute("src", "/images/tech-conference.jpg");
	});

	it("displays cancelled status when gathering is cancelled", async () => {
		const cancelledGathering = {
			...mockGathering,
			cancellationDateTime: new Date("2024-12-10T10:00:00Z")
		};

		render(
			<TestWrappers>
				<Card gathering={cancelledGathering} />
			</TestWrappers>
		);
		await waitFor(() => screen.findByTestId(wrappedComponentId));
		expect(screen.getByText(/cancelled/i)).toBeInTheDocument();
	});

	it("handles gathering with multiple categories", async () => {
		const gatheringWithMultipleCategories: Gathering = await getMockGathering(1);

		render(
			<TestWrappers>
				<Card gathering={gatheringWithMultipleCategories} />
			</TestWrappers>
		);
		await waitFor(() => screen.findByTestId(wrappedComponentId));

		expect(screen.getByText("Technology")).toBeInTheDocument();
		expect(screen.getByText("Networking")).toBeInTheDocument();
	});
});
