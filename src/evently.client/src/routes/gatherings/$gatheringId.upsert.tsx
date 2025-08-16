import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/gatherings/$gatheringId/upsert")({
	component: GatheringForm
});

function GatheringForm() {
	return <div>gatherings/$gatheringId/upsert</div>;
}
