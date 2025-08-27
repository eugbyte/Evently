import { render, screen } from "@testing-library/react";
import { DateTime } from "luxon";
import { Card } from "~/lib/components/card.tsx";
import { Gathering } from "~/lib/domains/entities";
import { getMockGathering } from "~/lib/services/gathering-service.mock";

describe("Card Component", () => {
	let mockGathering: Gathering;
	beforeEach(async () => {
		mockGathering = await getMockGathering(1);
	});

	it("renders gathering name", () => {
		render(<Card gathering={mockGathering} />);
		expect(screen.getByText("Tech Conference 2024")).toBeInTheDocument();
	});

	it("renders gathering description", () => {
		render(<Card gathering={mockGathering} />);
		expect(
			screen.getByText("Annual technology conference with industry leaders")
		).toBeInTheDocument();
	});

	it("renders gathering location", () => {
		render(<Card gathering={mockGathering} />);
		expect(screen.getByText("Convention Center")).toBeInTheDocument();
	});

	it("displays formatted start date", () => {
		render(<Card gathering={mockGathering} />);
		const startDate = DateTime.fromJSDate(mockGathering.start).toLocaleString(
			DateTime.DATETIME_MED
		);
		expect(screen.getByText(startDate)).toBeInTheDocument();
	});

	it("renders cover image when coverSrc is provided", () => {
		render(<Card gathering={mockGathering} />);

		const image = screen.getByRole("img");
		expect(image).toBeInTheDocument();
		expect(image).toHaveAttribute("src", "/images/tech-conference.jpg");
	});

	it("displays cancelled status when gathering is cancelled", () => {
		const cancelledGathering = {
			...mockGathering,
			cancellationDateTime: new Date("2024-12-10T10:00:00Z")
		};

		render(<Card gathering={cancelledGathering} />);
		expect(screen.getByText(/cancelled/i)).toBeInTheDocument();
	});

	it("handles gathering with multiple categories", async () => {
		const gatheringWithMultipleCategories: Gathering = await getMockGathering(1);

		render(<Card gathering={gatheringWithMultipleCategories} />);

		expect(screen.getByText("Technology")).toBeInTheDocument();
		expect(screen.getByText("Networking")).toBeInTheDocument();
	});
});
