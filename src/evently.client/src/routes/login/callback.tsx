import { createFileRoute, useNavigate } from "@tanstack/react-router";
import { Account } from "~/lib/domains/entities";
import { useEffect } from "react";
import { store, getAccount, type StoreState } from "~/lib/services";

export const Route = createFileRoute("/login/callback")({
	component: LoginCallbackPage,
	loader: async () => {
		return (await getAccount()) ?? new Account();
	}
});
export function LoginCallbackPage() {
	const member: Account = Route.useLoaderData();
	store.setState((state: StoreState) => ({
		...state,
		identityUserId: member.id
	}));

	const navigate = useNavigate();
	useEffect(() => {
		navigate({ to: "/bookings" }).then();
	}, [navigate]);
}
