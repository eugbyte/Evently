import { render, screen, waitFor } from "@testing-library/react";
import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";
import * as healthCheckService from "./-services/health-check-service";
import { HealthcheckPage } from "./index.tsx";
import {TestWrapper} from "~/lib/components";

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

	it("should render loading state initially", async() => {
		mockGetStatus.mockReturnValue(new Promise(() => {})); // Never resolving promise
		render(
			<TestWrapper>
				<HealthcheckPage />
			</TestWrapper>
		);

		const rootLayout: HTMLElement = await screen.findByTestId('root-layout');
		expect(rootLayout).toBeInTheDocument();
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

		const rootLayout: HTMLElement = await screen.findByTestId('root-layout');
		expect(rootLayout).toBeInTheDocument();

		await waitFor(() => {
			expect(screen.queryByText("Loading...")).not.toBeInTheDocument();
		});

		// Check that all status entries are rendered
		expect(screen.getByText("Database: Healthy")).toBeInTheDocument();
		expect(screen.getByText("API: Healthy")).toBeInTheDocument();
	});
});
