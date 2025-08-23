import {createFileRoute, Link} from "@tanstack/react-router";
import type { JSX } from "react";

export const Route = createFileRoute("/")({
	component: HomePage
});

export function HomePage(): JSX.Element {
	return (
		<div className="bg-base-100 min-h-screen">
			<div className="hero from-primary to-secondary min-h-screen bg-gradient-to-br">
				<div className="hero-content text-center">
					<div className="max-w-4xl">
						<h1 className="text-primary-content mb-6 text-5xl font-bold md:text-7xl">
							Welcome to <span className="text-accent">Evently</span>
						</h1>
						<p className="text-primary-content/80 mb-8 text-xl md:text-2xl">
							Create amazing events, manage registrations seamlessly, and engage your audience with
							QR code technology.
						</p>
						<div className="flex flex-col gap-4 sm:flex-row sm:justify-center">
							<Link to="/gatherings/create" className="btn btn-accent btn-lg">Create Your First Event</Link>
							<Link to="/gatherings" className="btn btn-outline btn-lg text-primary-content hover:bg-primary-content hover:text-primary">
								Explore Events
							</Link>
						</div>
					</div>
				</div>
			</div>

			<div className="py-20">
				<div className="container mx-auto px-4">
					<div className="mb-16 text-center">
						<h2 className="text-base-content mb-4 text-4xl font-bold">
							Everything you need to manage events
						</h2>
						<p className="text-base-content/70 mx-auto max-w-2xl text-lg">
							From creation to check-in, Evently provides all the tools you need to run successful
							events.
						</p>
					</div>

					<div className="grid grid-cols-1 gap-8 md:grid-cols-3">
						<div className="text-center">
							<div className="mb-6">
								<div className="bg-primary text-primary-content inline-flex h-20 w-20 items-center justify-center rounded-full text-2xl font-bold">
									1
								</div>
							</div>
							<h3 className="mb-3 text-xl font-bold">Create Your Event</h3>
							<p className="text-base-content/70">
								Set up your event details, customize your page, and configure registration settings.
							</p>
						</div>

						<div className="text-center">
							<div className="mb-6">
								<div className="bg-secondary text-secondary-content inline-flex h-20 w-20 items-center justify-center rounded-full text-2xl font-bold">
									2
								</div>
							</div>
							<h3 className="mb-3 text-xl font-bold">Share & Register</h3>
							<p className="text-base-content/70">
								Share your event link and let attendees register. They'll receive QR codes
								automatically.
							</p>
						</div>

						<div className="text-center">
							<div className="mb-6">
								<div className="bg-accent text-accent-content inline-flex h-20 w-20 items-center justify-center rounded-full text-2xl font-bold">
									3
								</div>
							</div>
							<h3 className="mb-3 text-xl font-bold">Scan & Welcome</h3>
							<p className="text-base-content/70">
								Use our mobile app to scan QR codes at your event for lightning-fast check-ins.
							</p>
						</div>
					</div>
				</div>
			</div>
		</div>
	);
}
