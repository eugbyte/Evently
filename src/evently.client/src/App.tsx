import "./App.css";
import { createRouter, RouterProvider } from "@tanstack/react-router";
import { routeTree } from "./routeTree.gen.ts";
import type { JSX } from "react";

declare module "@tanstack/react-router" {
	interface Register {
		router: typeof router;
	}
}

const router = createRouter({ routeTree });

export function App(): JSX.Element {
	return <RouterProvider router={router} />;
}
