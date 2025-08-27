import { render, screen, waitFor } from "@testing-library/react";
import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";
import * as healthCheckService from "./-services/health-check-service";
import { HealthcheckPage } from "./index.tsx";
import { TestWrapper, WrapperDataTestId } from "~/lib/components";

// Mock the health check service
vi.mock("./-services/health-check-service", () => ({
	getStatus: vi.fn()
}));

const mockGetStatus = vi.mocked(healthCheckService.getStatus);

describe("HealthcheckPage", () => {
	beforeEach(() => {
		vi.clearAllMocks();
	});

	afterEach(() => {
		vi.resetAllMocks();
	});

	it("should render loading state initially", async () => {
		mockGetStatus.mockReturnValue(new Promise(() => {})); // Never resolving with a promise
		render(
			<TestWrapper>
				<HealthcheckPage />
			</TestWrapper>
		);
		await waitFor(() => screen.findByTestId(WrapperDataTestId));

		expect(screen.getByText("Loading...")).toBeInTheDocument();
	});

	it("should render health statuses when data is loaded successfully", async () => {
		const mockStatuses = {
			Database: "Healthy",
			API: "Healthy"
		};

		mockGetStatus.mockResolvedValue(mockStatuses);

		render(
			<TestWrapper>
				<HealthcheckPage />
			</TestWrapper>
		);
		await screen.findByTestId("root-layout");

		// Check that all status entries are rendered
		// await router.navigate({ to: "/healthcheck" });
		expect(screen.getByText("Database: Healthy")).toBeInTheDocument();
		expect(screen.getByText("API: Healthy")).toBeInTheDocument();
	});
});
