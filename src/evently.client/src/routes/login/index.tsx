import GoogleIcon from "~/lib/assets/GoogleIcon.svg";
import { createFileRoute, useSearch } from "@tanstack/react-router";
import { login } from "~/lib/services";

export const Route = createFileRoute("/login/")({
	component: LoginPage
});

export function LoginPage() {
	const defaultRedirect: URL = new URL("/bookings/attending", window.location.href);
	const searchParams: Record<string, string> = useSearch({ strict: false });
	const redirect: string = searchParams["redirect"] ?? defaultRedirect.href;
	console.log({ redirect });

	return (
		<div className="flex h-full items-center justify-center bg-gradient-to-br from-gray-900 to-gray-800 p-4">
			<div className="w-full max-w-md">
				<div className="rounded-2xl bg-white p-8 shadow-xl">
					{/* Header */}
					<div className="mb-8 text-center">
						<h1 className="mb-2 text-3xl font-bold text-gray-900">Welcome Back</h1>
						<p className="text-gray-600">Sign in to your account to continue</p>
					</div>

					{/* Google Login Button */}
					<div className="space-y-4">
						<button
							onClick={() => login(redirect)}
							className="flex w-full cursor-pointer items-center justify-center gap-3 rounded-lg border border-gray-300 bg-white px-6 py-3 shadow-sm transition-all duration-200 hover:bg-gray-50 focus:border-transparent focus:ring-2 focus:ring-blue-500 focus:outline-none"
							type="button"
							aria-label="Sign in with Google"
						>
							<img src={GoogleIcon} alt="Google" className="h-5 w-5" />
							<span className="font-medium text-gray-700">Continue with Google</span>
						</button>
					</div>
				</div>
			</div>
		</div>
	);
}
