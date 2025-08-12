import { createFileRoute } from "@tanstack/react-router";
import { HomePage } from "./page.tsx";

export const Route = createFileRoute("/")({
	component: HomePage
});
