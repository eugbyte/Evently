import { createFileRoute } from "@tanstack/react-router";
import {HealthcheckPage} from "./page.tsx";

export const Route = createFileRoute("/healthcheck/")({
	component: HealthcheckPage
});

