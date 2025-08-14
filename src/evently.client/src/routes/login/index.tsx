import {login} from "~/lib/services/auth-service.ts";
import GoogleIcon from "~/lib/assets/google.svg";
import {createFileRoute} from "@tanstack/react-router";

export const Route = createFileRoute("/login/")({
    component: LoginPage
});

export function LoginPage() {
    const redirectUrl: URL = new URL("/login/callback", window.location.href);
    
    return (
        <div className="flex h-full w-full flex-row items-center justify-center">
            <div className="card flex w-full max-w-7xl flex-row p-2 shadow-lg">
                <div className="space-y-4 p-1 sm:w-1/2 sm:p-8">
                    <p>
                        Login / Sign up to access your dashboard and connect with exhibitors, participants, and more
                    </p>
                    <div className="flex flex-row flex-wrap justify-center space-x-2">
                        <p className="m-1 w-full text-center text-xs">Continue with:</p>
                        <button
                            aria-label="login"
                            className="h-[50px] w-[50px] rounded-full shadow-xl"
                            onClick={() => login(redirectUrl.href)}
                            type="button"
                        >
                            <GoogleIcon />
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
};
