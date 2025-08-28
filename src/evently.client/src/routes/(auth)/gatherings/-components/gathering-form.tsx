import { useEffect, useState, type JSX } from "react";
import { compressImage, type GatheringForm as IGatheringForm } from "../-services";
import { FieldErrMsg as FieldInfo } from "~/lib/components";
import { Icon } from "@iconify/react";
import { DateTime } from "luxon";
import { GatheringReqDto, ToastContent } from "~/lib/domains/models";
import { useRouter } from "@tanstack/react-router";
import { toIsoString } from "~/lib/services";
import {Category} from "~/lib/domains/entities";
interface GatheringFormProps {
	file: File | null;
	setFile: (file: File | null) => void;
	form: IGatheringForm;
	categories: Category[];
}

export function GatheringForm({ file, setFile, form, categories }: GatheringFormProps): JSX.Element {
	const router = useRouter();
	const fileName: string = file?.name ?? "";
	const coverSrc: string =
		file != null ? URL.createObjectURL(file) : (form.state.values.coverSrc ?? "");
	const gathering: GatheringReqDto = form.state.values;
	const [toastMsg, setToastMsg] = useState(new ToastContent(false));

	useEffect(() => {
		// prevent memory leak
		// https://developer.mozilla.org/en-US/docs/Web/API/URL/createObjectURL_static#memory_management
		return () => URL.revokeObjectURL(coverSrc);
	}, [coverSrc]);

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
										validators={{
											onBlur: ({ value }) => {
												return value.trim() === "" ? "Event name is required" : null;
											}
										}}
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
										validators={{
											onBlur: ({ value }) => {
												return value.trim() === "" ? "Location is required" : null;
											}
										}}
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
										validators={{
											onBlur: ({ value }) => {
												return value == null ? "Start Date is required" : null;
											}
										}}
										children={(field) => (
											<div className="form-control">
												<label className="label">
													<span className="label-text font-semibold">Start Date & Time</span>
													<span className="label-text-alt text-error">*</span>
												</label>
												<input
													type="datetime-local"
													className="input input-bordered focus:input-primary w-full"
													value={field.state.value == null ? "" : toIsoString(field.state.value)}
													onBlur={field.handleBlur}
													onChange={(e) => {
														const value: DateTime<boolean> = DateTime.fromISO(e.target.value);
														const date: Date | null = !value.isValid ? null : value.toJSDate();
														field.handleChange(date as any);
													}}
												/>
												<FieldInfo field={field} />
											</div>
										)}
									/>

									<form.Field
										name="end"
										validators={{
											onBlur: ({ value }) => {
												return value == null ? "End Date is required" : null;
											}
										}}
										children={(field) => (
											<div className="form-control">
												<label className="label">
													<span className="label-text font-semibold">End Date & Time</span>
													<span className="label-text-alt text-error">*</span>
												</label>
												<input
													type="datetime-local"
													className="input input-bordered focus:input-primary w-full"
													value={field.state.value == null ? "" : toIsoString(field.state.value)}
													onBlur={field.handleBlur}
													onChange={(e) => {
														const value: DateTime<boolean> = DateTime.fromISO(e.target.value);
														const date: Date | null = !value.isValid ? null : value.toJSDate();
														field.handleChange(date as any);
													}}
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
										validators={{
											onBlur: ({ value }) => {
												return value.trim() === "" ? "Description is required" : null;
											}
										}}
										children={(field) => (
											<div>
												<label className="label block">
													<span className="label-text font-semibold">Description</span>
												</label>
												<textarea
													className="textarea textarea-bordered focus:textarea-primary h-32 resize-none sm:w-[350px]"
													placeholder="Describe your event..."
													value={field.state.value}
													onBlur={field.handleBlur}
													onChange={(e) => field.handleChange(e.target.value)}
												/>
												<FieldInfo field={field} />
											</div>
										)}
									/>

									<div>
										<label className="btn mb-2">
											<Icon height="24" icon="material-symbols:cloud-upload" width="24" />
											<span>Upload Cover Image</span>
											<input
												accept="image/*"
												type="file"
												className="input input-bordered focus:input-primary hidden w-full cursor-pointer"
												onChange={async (e) => {
													let file: File | null = e.target.files ? e.target.files[0] : null;
													if (file != null) {
														console.log("file is not null, compressing file");
														setToastMsg(new ToastContent(true, "Compressing image..."));
														file = await compressImage(file);
														setToastMsg(new ToastContent(false));
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
								</div>
							</div>

							{/* Action Buttons */}
							<div className="divider"></div>
							<div className="card-actions justify-end gap-4">
								<button
									type="button"
									className="btn btn-ghost"
									onClick={() => router.history.back()}
								>
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
			{toastMsg.show && (
				<div className="toast toast-center">
					<div className="alert alert-info">
						<span>{toastMsg.message}</span>
					</div>
				</div>
			)}
		</div>
	);
}
