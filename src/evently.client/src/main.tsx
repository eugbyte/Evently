import { StrictMode } from "react";
import "reflect-metadata";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import ReactDOM from "react-dom/client";
import "./main.css";
import { createRouter, RouterProvider } from "@tanstack/react-router";
// Import the generated route tree
import { routeTree } from "~/routeTree.gen.ts";

declare module "@tanstack/react-router" {
	interface Register {
		router: typeof router;
	}
}

const queryClient = new QueryClient();

const router = createRouter({ routeTree });

const rootElement = document.getElementById("root")!;
if (!rootElement.innerHTML) {
	const root = ReactDOM.createRoot(rootElement);
	root.render(
		<StrictMode>
			<QueryClientProvider client={queryClient}>
				<RouterProvider router={router} />
			</QueryClientProvider>
		</StrictMode>
	);
}
