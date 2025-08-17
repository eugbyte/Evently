import { createFileRoute } from "@tanstack/react-router";
import { Gathering } from "~/lib/domains/entities";
import { useForm } from "@tanstack/react-form";
import { type JSX } from "react";
import { FieldErrMsg as FieldInfo } from "./-components";
import { getGathering } from "~/lib/services";

export const Route = createFileRoute("/gatherings/$gatheringId/upsert")({
	loader: async ({ params }) => {
		const gatheringId: number = parseInt(params.gatheringId);
		const gathering: Gathering | null = await getGathering(gatheringId);
		return gathering ?? new Gathering();
	},
	component: GatheringForm
});

type IGathering = Omit<Gathering, "bookings" | "gatheringCategoryDetails">;

function GatheringForm(): JSX.Element {
	const gathering: Gathering = Route.useLoaderData();
	const defaultGathering: IGathering = gathering;

	const form = useForm({
		defaultValues: defaultGathering,
		onSubmit: async ({ value }) => {
			// Do something with form data
			console.log(value);
		}
	});

	return (
		<div className="bg-base-200 mb-32 p-2 sm:mb-0 sm:h-full">
			<div className="mx-auto max-w-4xl">
				<div className="card bg-base-100 shadow-xl">
					<div className="card-body">
						<h2 className="card-title mb-8 text-center text-3xl font-bold">
							{gathering.gatheringId > 0 ? "Edit Event" : "Create New Event"}
						</h2>

						<form
							onSubmit={(e) => {
								e.preventDefault();
								e.stopPropagation();
								void form.handleSubmit();
							}}
						>
							<div className="grid grid-cols-1 gap-6 lg:grid-cols-2">
								{/* Left Column */}
								<div className="space-y-6">
									<form.Field
										name="name"
										children={(field) => (
											<div className="form-control">
												<label className="label">
													<span className="label-text font-semibold">Event Name</span>
													<span className="label-text-alt text-error">*</span>
												</label>
												<input
													type="text"
													placeholder="Enter event name"
													className="input input-bordered focus:input-primary w-full"
													value={field.state.value}
													onBlur={field.handleBlur}
													onChange={(e) => field.handleChange(e.target.value)}
												/>
												<FieldInfo field={field} />
											</div>
										)}
									/>

									<form.Field
										name="location"
										children={(field) => (
											<div className="form-control">
												<label className="label">
													<span className="label-text font-semibold">Location</span>
													<span className="label-text-alt text-error">*</span>
												</label>
												<input
													type="text"
													placeholder="Enter event location"
													className="input input-bordered focus:input-primary w-full"
													value={field.state.value}
													onBlur={field.handleBlur}
													onChange={(e) => field.handleChange(e.target.value)}
												/>
												<FieldInfo field={field} />
											</div>
										)}
									/>

									<form.Field
										name="start"
										children={(field) => (
											<div className="form-control">
												<label className="label">
													<span className="label-text font-semibold">Start Date & Time</span>
													<span className="label-text-alt text-error">*</span>
												</label>
												<input
													type="datetime-local"
													className="input input-bordered focus:input-primary w-full"
													value={
														field.state.value instanceof Date
															? field.state.value.toISOString().slice(0, -1)
															: field.state.value
													}
													onBlur={field.handleBlur}
													onChange={(e) => field.handleChange(new Date(e.target.value))}
												/>
												<FieldInfo field={field} />
											</div>
										)}
									/>

									<form.Field
										name="end"
										children={(field) => (
											<div className="form-control">
												<label className="label">
													<span className="label-text font-semibold">End Date & Time</span>
													<span className="label-text-alt text-error">*</span>
												</label>
												<input
													type="datetime-local"
													className="input input-bordered focus:input-primary w-full"
													value={
														field.state.value instanceof Date
															? field.state.value.toISOString().slice(0, -1)
															: field.state.value
													}
													onBlur={field.handleBlur}
													onChange={(e) => field.handleChange(new Date(e.target.value))}
												/>
												<FieldInfo field={field} />
											</div>
										)}
									/>
								</div>

								{/* Right Column */}
								<div className="space-y-6">
									<form.Field
										name="description"
										children={(field) => (
											<div>
												<label className="label block">
													<span className="label-text font-semibold">Description</span>
												</label>
												<textarea
													className="textarea textarea-bordered focus:textarea-primary h-32 resize-none"
													placeholder="Describe your event..."
													value={field.state.value}
													onBlur={field.handleBlur}
													onChange={(e) => field.handleChange(e.target.value)}
												/>
												<FieldInfo field={field} />
											</div>
										)}
									/>

									<form.Field
										name="coverSrc"
										children={(field) => (
											<div className="form-control">
												<label className="label">
													<span className="label-text font-semibold">Cover Image URL</span>
												</label>
												<input
													type="url"
													placeholder="https://example.com/image.jpg"
													className="input input-bordered focus:input-primary w-full"
													value={field.state.value || ""}
													onBlur={field.handleBlur}
													onChange={(e) => field.handleChange(e.target.value)}
												/>
												<label className="label">
													<span className="label-text-alt">
														Optional: Add a cover image for your event
													</span>
												</label>
												<FieldInfo field={field} />
											</div>
										)}
									/>
								</div>
							</div>

							{/* Action Buttons */}
							<div className="divider"></div>
							<div className="card-actions justify-end gap-4">
								<button type="button" className="btn btn-ghost">
									Cancel
								</button>
								<button type="submit" className="btn btn-primary">
									{gathering.gatheringId > 0 ? "Update Event" : "Create Event"}
								</button>
							</div>
						</form>
					</div>
				</div>
			</div>
			<div className="none mb-20 sm:block"></div>
		</div>
	);
}
