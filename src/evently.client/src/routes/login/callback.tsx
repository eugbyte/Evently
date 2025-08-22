import { createFileRoute, useNavigate } from "@tanstack/react-router";
import { useEffect } from "react";
import {getAccount} from "~/lib/services";

export const Route = createFileRoute("/login/callback")({
	component: LoginCallbackPage,
	beforeLoad: async () => {
		const account = await getAccount();
		return { account };
	}
});
export function LoginCallbackPage() {
	const navigate = useNavigate();
	useEffect(() => {
		navigate({ to: "/bookings/attending" }).then();
	}, [navigate]);
}
