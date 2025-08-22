import { createFileRoute } from "@tanstack/react-router";
import { Gathering } from "~/lib/domains/entities";
import { useState, type JSX } from "react";
import { createGathering, store } from "~/lib/services";
import {
	useGatheringForm,
	type GatheringForm as IGatheringForm
} from "~/routes/gatherings/-services";
import { GatheringReqDto } from "~/lib/domains/models";
import { GatheringForm } from "~/routes/gatherings/-components";
import { useStore } from "@tanstack/react-store";

export const Route = createFileRoute("/gatherings/create")({
	component: CreateGatheringPage
});

function CreateGatheringPage(): JSX.Element {
	const accountId: string | undefined = useStore(store, (store) => store.account?.id);
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
		navigate({ to: `/gatherings/${gatheringId}`, reloadDocument: true });
	};
	const form: IGatheringForm = useGatheringForm(defaultGathering, onSubmit);
	return <GatheringForm file={file} setFile={setFile} form={form} />;
}
