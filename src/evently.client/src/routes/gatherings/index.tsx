import { createFileRoute } from "@tanstack/react-router";
import { type JSX, useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { Account, Gathering } from "~/lib/domains/entities";
import { getAccount, getGatherings, type GetGatheringsParams } from "~/lib/services";
import { Card } from "~/lib/components";

export const Route = createFileRoute("/gatherings/")({
	component: GatheringsPage,
	loader: async () => {
		return getAccount();
	},
	pendingComponent: () => (
		<div className="h-full">
			<progress className="progress w-full"></progress>
		</div>
	)
});

export function GatheringsPage(): JSX.Element {
	const account: Account | null = Route.useLoaderData();
	const [queryParams] = useState<GetGatheringsParams>({ startDateAfter: new Date() });
	const { data: _gatherings, isLoading } = useQuery({
		queryKey: ["getGatherings", queryParams],
		queryFn: (): Promise<Gathering[]> => getGatherings(queryParams)
	});
	const gatherings: Gathering[] = _gatherings ?? [];

	return (
		<div className="h-full">
			<div className="flex flex-row justify-center mt-1">
				<label className="input w-40">
					<svg
						className="h-[1em] opacity-50"
						xmlns="http://www.w3.org/2000/svg"
						viewBox="0 0 24 24"
					>
						<g
							strokeLinejoin="round"
							strokeLinecap="round"
							strokeWidth="2.5"
							fill="none"
							stroke="currentColor"
						>
							<circle cx="11" cy="11" r="8"></circle>
							<path d="m21 21-4.3-4.3"></path>
						</g>
					</svg>
					<input type="search" className="grow" placeholder="Search" />
				</label>
			</div>
			{isLoading ? (
				<progress className="progress w-full"></progress>
			) : (
				<div className="my-4 grid grid-cols-1 content-evenly justify-items-center gap-4 lg:grid-cols-2 xl:grid-cols-3">
					{gatherings.map((gathering, index) => (
						<Card
							key={gathering.gatheringId + "-" + index}
							gathering={gathering}
							accountId={account?.id}
						/>
					))}					
				</div>
			)}
			<div id="pagination" className="fixed left-1/2 bottom-20 -translate-x-1/2">
				<div className=" join border border-primary rounded-sm">
					<button className="join-item btn bg-base-100">«</button>
					<button className="join-item btn bg-base-100">Page 22</button>
					<button className="join-item btn bg-base-100">»</button>
				</div>
			</div>
			<div className="h-40"></div>
		</div>
	);
}
