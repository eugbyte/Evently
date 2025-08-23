import { createFileRoute, Outlet, redirect } from "@tanstack/react-router";

export const Route = createFileRoute("/(auth)/bookings")({
	beforeLoad: ({ context }) => {
		if (context.account == null) {
			throw redirect({
				to: "/login",
				replace: true,
				search: {
					redirect: location.href
				}
			});
		}
	},
	component: AuthLayout
});

function AuthLayout() {
	return (
		<>
			<Outlet />
		</>
	);
}
