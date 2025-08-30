import { createFileRoute, Outlet } from "@tanstack/react-router";
import { guardRoute } from "~/lib/services";

export const Route = createFileRoute("/bookings/(auth)")({
	beforeLoad: ({ context }) => guardRoute(context.account, window.location.href),
	component: AuthLayout
});

function AuthLayout() {
	return <Outlet />;
}
