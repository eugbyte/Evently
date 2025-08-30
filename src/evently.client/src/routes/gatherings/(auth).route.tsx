import { createFileRoute, Outlet } from "@tanstack/react-router";
import { guardRoute } from "~/lib/services";

export const Route = createFileRoute("/gatherings/(auth)")({
	beforeLoad: ({ context }) => guardRoute(context.account),
	component: AuthLayout
});

function AuthLayout() {
	return (
		<>
			<Outlet />
		</>
	);
}
