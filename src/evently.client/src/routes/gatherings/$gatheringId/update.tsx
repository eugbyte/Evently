import { createFileRoute } from "@tanstack/react-router";
import { Gathering } from "~/lib/domains/entities";
import { useEffect, useState, type JSX } from "react";
import { FieldErrMsg as FieldInfo } from "~/routes/gatherings/-components";
import { getGathering } from "~/lib/services";
import {
	type IGathering,
	type GatheringForm,
	useGatheringForm,
	compressImage
} from "~/routes/gatherings/-services";
import { Icon } from "@iconify/react";

export const Route = createFileRoute("/gatherings/$gatheringId/update")({
	loader: async ({ params }) => {
		const gatheringId: number = parseInt(params.gatheringId);
		const gathering: Gathering | null = await getGathering(gatheringId);
		return gathering ?? new Gathering();
	},
	component: GatheringForm
});

function GatheringForm(): JSX.Element {
	// need to separate file field as Tanstack Form does not support file upload
	const [file, setFile] = useState<File | null>(null);
	const coverSrc: string = file === null ? "" : URL.createObjectURL(file);

	const gathering: Gathering = Route.useLoaderData();
	const defaultGathering: IGathering = {
		...gathering,
		coverImage: null
	};
	const onSubmit = async (values: IGathering): Promise<void> => {
		const formData = new FormData();
		for (const [key, value] of Object.entries(values)) {
			formData.set(key, value);
		}
		if (file != null) {
			formData.set("coverImg", file);
		}
		console.log(formData);
	};
	const form: GatheringForm = useGatheringForm(defaultGathering, onSubmit);
	const fileName: string = form.state.values.coverImage?.name ?? "";

	useEffect(() => {
		// prevent memory leak
		// https://developer.mozilla.org/en-US/docs/Web/API/URL/createObjectURL_static#memory_management
		return () => URL.revokeObjectURL(coverSrc);
	}, []);

	return (
		<div className="bg-base-200 mb-32 p-2 sm:mb-0 sm:h-full">
			<div className="mx-auto max-w-4xl">
				<div className="card bg-base-100 shadow-xl">
					<div className="card-body">
						<h2 className="card-title mb-8 text-center text-3xl font-bold">
							{gathering.gatheringId > 0 ? "Edit Event" : "Create New Event"}
						</h2>

						<form
							onSubmit={async (e) => {
								e.preventDefault();
								e.stopPropagation();
								await form.handleSubmit();
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
													className="textarea textarea-bordered focus:textarea-primary h-32 w-[350px] resize-none"
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
										name="coverImage"
										children={(field) => (
											<div>
												<label className="btn mb-2">
													<Icon height="24" icon="material-symbols:cloud-upload" width="24" />
													<span>Upload Cover Image</span>
													<input
														accept="image/*"
														type="file"
														className="input input-bordered focus:input-primary hidden w-full cursor-pointer"
														value={field.state.value?.name || ""}
														onBlur={field.handleBlur}
														onChange={async (e) => {
															let file: File | null = e.target.files ? e.target.files[0] : null;
															if (file != null) {
																console.log("file is not null, compressing file");
																file = await compressImage(file);
																setFile(file);
															}
														}}
													/>
												</label>
												<p className="">{fileName}</p>
												{coverSrc != null && coverSrc.trim() !== "" && (
													<img
														src={coverSrc}
														alt="Floor Plan"
														width="350px"
														height="350px"
														className="block"
													/>
												)}
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
