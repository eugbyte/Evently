import { useForm } from "@tanstack/react-form";
import { Gathering } from "~/lib/domains/entities";

export interface IGathering extends Omit<Gathering, "bookings" | "gatheringCategoryDetails"> {
	coverImage: File | null;
}

export function useGatheringForm(
	defaultGathering: IGathering,
	onSubmit: (value: IGathering) => Promise<void>
) {
	return useForm({
		defaultValues: defaultGathering,
		onSubmit: async ({ value }) => {
			// Do something with form data
			await onSubmit(value);
		}
	});
}

export type GatheringForm = ReturnType<typeof useGatheringForm>;
