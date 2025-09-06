import { Link } from "@tanstack/react-router";
import { type JSX } from "react";

export function NotFound(): JSX.Element {
	return (
		<div className="bg-base-100 flex h-full items-center justify-center">
			<div className="text-center">
				<div className="mb-8">
					<h1 className="text-primary text-9xl font-bold">404</h1>
					<div className="text-base-content/20 text-6xl font-bold">Page Not Found</div>
				</div>

				<div className="mb-8 max-w-md">
					<p className="text-base-content/70 mb-4 text-lg">
						Oops! The page you're looking for doesn't exist.
					</p>
					<p className="text-base-content/60">
						It might have been moved, deleted, or you entered the wrong URL.
					</p>
				</div>

				<div className="flex flex-col justify-center gap-4 sm:flex-row">
					<Link to="/" className="btn btn-primary">
						Go Home
					</Link>
					<button onClick={() => window.history.back()} className="btn btn-outline">
						Go Back
					</button>
				</div>
			</div>
		</div>
	);
}
