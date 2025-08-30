import { redirect } from "@tanstack/react-router";
import axios from "axios";
import { Account } from "~/lib/domains/entities";

export async function login(redirectUrl: string) {
	// must redirect instead of making a REST request (https://stackoverflow.com/a/48925986/6514532)
	// cannot use goto()
	window.location.href = `/api/v1/Auth/external/google/login?originUrl=${redirectUrl}`;
}

export async function logout(redirectUrl: string): Promise<void> {
	await axios.post("/api/v1/Auth/external/logout", {}, { params: { redirectUrl } });

	window.location.href = redirectUrl;
}

export async function getAccount(): Promise<Account | null> {
	try {
		const response = await axios.get<Account>("/api/v1/Auth/external/account");
		return response.data;
	} catch (e) {
		const error = e as Error;
		console.warn(error.message);
	}
	return null;
}

/**
 * Validate against the user's `Account` in the route context.
 * Redirect to login page if authentication fails.
 * Tightly coupled with TanStack framework
 * @param account
 */
export async function guardRoute(account: Account | null): Promise<void> {
	if (account == null) {
		throw redirect({
			to: "/login",
			replace: true,
			search: {
				redirect: location.href
			}
		});
	}
}
