import { createFileRoute, redirect } from "@tanstack/react-router";
import { Gathering } from "~/lib/domains/entities";
import { useState, type JSX } from "react";
import { getGathering, sleep, updateGathering } from "~/lib/services";
import {
	useGatheringForm,
	type GatheringForm as IGatheringForm
} from "~/routes/gatherings/-services";
import { GatheringReqDto, ToastContent } from "~/lib/domains/models";
import { GatheringForm } from "~/routes/gatherings/-components";

export const Route = createFileRoute("/gatherings/$gatheringId/update")({
	beforeLoad: ({ context }) => {
		console.log("in AuthLayout");
		if (context.account == null) {
			throw redirect({
				to: "/login",
				replace: true,
				search: {
					redirect: location.href
				}
			});
		}
	},
	loader: async ({ params }) => {
		const gatheringId: number = parseInt(params.gatheringId);
		const gathering: Gathering | null = await getGathering(gatheringId);
		return gathering ?? new Gathering();
	},
	component: UpdateGatheringPage
});

function UpdateGatheringPage(): JSX.Element {
	const gathering: Gathering = Route.useLoaderData();
	const navigate = Route.useNavigate();
	const defaultGathering: GatheringReqDto = {
		...gathering
	};
	// need to separate file field as Tanstack Form does not support file upload
	const [file, setFile] = useState<File | null>(null);
	const [toastMsg, setToastMsg] = useState(new ToastContent(false));

	const onSubmit = async (values: GatheringReqDto): Promise<void> => {
		setToastMsg(new ToastContent(true, "Updating..."));
		await updateGathering(values.gatheringId, values, file);
		setToastMsg(new ToastContent(true, "Successfully updated. Redirecting..."));
		await sleep(1500);
		navigate({ to: `/gatherings/${gathering.gatheringId}` });
	};
	const form: IGatheringForm = useGatheringForm(defaultGathering, onSubmit);
	return (
		<>
			<GatheringForm file={file} setFile={setFile} form={form} />
			{toastMsg.show && (
				<div className="toast toast-center">
					<div className="alert alert-success">
						<span>{toastMsg.message}</span>
					</div>
				</div>
			)}
		</>
	);
}
