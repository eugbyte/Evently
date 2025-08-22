import { createFileRoute } from "@tanstack/react-router";
import { Gathering } from "~/lib/domains/entities";
import { useState, type JSX } from "react";
import { createGathering } from "~/lib/services";
import {
	useGatheringForm,
	type GatheringForm as IGatheringForm
} from "~/routes/gatherings/-services";
import { GatheringReqDto } from "~/lib/domains/models";
import { GatheringForm } from "~/routes/gatherings/-components";

export const Route = createFileRoute("/gatherings/create")({
	component: CreateGatheringPage
});

function CreateGatheringPage(): JSX.Element {
	const { account } = Route.useRouteContext();
	const accountId: string | undefined = account?.id;

	const navigate = Route.useNavigate();

	const gathering = new Gathering();
	gathering.organiserId = accountId ?? "";
	const defaultGathering: GatheringReqDto = {
		...gathering
	};
	// need to separate file field as Tanstack Form does not support file upload
	const [file, setFile] = useState<File | null>(null);

	const onSubmit = async (values: GatheringReqDto): Promise<void> => {
		const { gatheringId } = await createGathering(values, file);
		navigate({ to: `/gatherings/${gatheringId}` });
	};
	const form: IGatheringForm = useGatheringForm(defaultGathering, onSubmit);
	return <GatheringForm file={file} setFile={setFile} form={form} />;
}
