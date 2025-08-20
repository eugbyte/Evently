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
	const gathering = new Gathering();
	const defaultGathering: GatheringReqDto = {
		...gathering
	};
	// need to separate file field as Tanstack Form does not support file upload
	const [file, setFile] = useState<File | null>(null);

	const onSubmit = async (values: GatheringReqDto): Promise<void> => {
		await createGathering(values, file);
	};
	const form: IGatheringForm = useGatheringForm(defaultGathering, onSubmit);
	return <GatheringForm file={file} setFile={setFile} form={form} />;
}
