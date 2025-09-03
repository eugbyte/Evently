import { createFileRoute, Outlet } from "@tanstack/react-router";
import { authenticateRoute } from "~/lib/services";

export const Route = createFileRoute("/bookings/(auth)")({
	beforeLoad: ({ context }) => authenticateRoute(context.account, window.location.href),
	component: AuthLayout
});

function AuthLayout() {
	return (
		<>
			<Outlet />
		</>
	);
}
