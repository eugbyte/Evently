import {createFileRoute, useNavigate} from "@tanstack/react-router";
import {getAccount} from "~/lib/services/auth-service.ts";
import {Account} from "~/lib/domains/entities";
import {getMembers} from "~/lib/services";

export const Route = createFileRoute("/login/callback")({
    component: LoginCallbackPage,
    loader: async () => {
        const account: Account = await getAccount() ?? new Account();
        return await getMembers(account.id);
    }
});
export function LoginCallbackPage() {
    const member: Account = Route.useLoaderData();
    sessionStorage.setItem("identityUserId", member.id);
    sessionStorage.setItem("userName", member.userName);
    sessionStorage.setItem("email", member.email);

    const navigate = useNavigate();
    navigate({to: "/gatherings"}).then();
}