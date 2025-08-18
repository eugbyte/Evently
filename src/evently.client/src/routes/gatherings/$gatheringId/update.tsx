import { createFileRoute } from "@tanstack/react-router";
import { Gathering } from "~/lib/domains/entities";
import { useEffect, useState, type JSX } from "react";
import { getGathering, updateGathering } from "~/lib/services";
import {
	useGatheringForm,
	type GatheringForm as IGatheringForm
} from "~/routes/gatherings/-services";
import { GatheringReqDto } from "~/lib/domains/models";
import { GatheringForm } from "~/routes/gatherings/-components";

export const Route = createFileRoute("/gatherings/$gatheringId/update")({
	loader: async ({ params }) => {
		const gatheringId: number = parseInt(params.gatheringId);
		const gathering: Gathering | null = await getGathering(gatheringId);
		return gathering ?? new Gathering();
	},
	component: UpdateGathering
});

function UpdateGathering(): JSX.Element {
	// need to separate file field as Tanstack Form does not support file upload
	const [file, setFile] = useState<File | null>(null);
	const coverSrc: string = file === null ? "" : URL.createObjectURL(file);

	const gathering: Gathering = Route.useLoaderData();
	let defaultGathering: GatheringReqDto = {
		...gathering
	};
	const onSubmit = async (values: GatheringReqDto): Promise<void> => {
		await updateGathering(values.gatheringId, values, file);
	};
	const form: IGatheringForm = useGatheringForm(defaultGathering, onSubmit);

	useEffect(() => {
		// prevent memory leak
		// https://developer.mozilla.org/en-US/docs/Web/API/URL/createObjectURL_static#memory_management
		return () => URL.revokeObjectURL(coverSrc);
	}, []);

	return <GatheringForm file={file} setFile={setFile} form={form} />;
}
