import { createFileRoute } from "@tanstack/react-router";
import { type JSX, useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { Account, Gathering } from "~/lib/domains/entities";
import { getAccount, getGatherings, type GetGatheringsParams } from "~/lib/services";
import { Card } from "~/lib/components";
import type { PageResult } from "~/lib/domains/models";

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
	const pageSize = 6;
	const [queryParams, setQueryParams] = useState<GetGatheringsParams>({
		startDateAfter: new Date(),
		offset: 0,
		limit: pageSize
	});
	const { data, isLoading } = useQuery({
		queryKey: ["getGatherings", queryParams],
		queryFn: (): Promise<PageResult<Gathering[]>> => getGatherings(queryParams)
	});
	const gatherings: Gathering[] = data == null ? [] : data.data;
	const totalCount: number = data == null ? 0 : data.totalCount;


	const [page, setPage] = useState(1);
	const maxPage = Math.ceil(totalCount / pageSize);
	const onPrevPage = () => {
		let prevPage = page - 1;
		prevPage = Math.max(1, prevPage);
		setPage(prevPage);

		setQueryParams({
			...queryParams,
			offset: (prevPage - 1) * pageSize,
			limit: pageSize
		});
	};

	const onNextPage = () => {
		let nextPage = page + 1;
		nextPage = Math.min(maxPage, nextPage);
		setPage(nextPage);

		setQueryParams({
			...queryParams,
			offset: (nextPage - 1) * pageSize,
			limit: pageSize
		});
	};

	return (
		<div className="h-full">
			<div className="mt-1 flex flex-row justify-center">
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
					<input
						type="search"
						className="grow"
						placeholder="Search"
						onChange={(e) => {
							const text: string = e.target.value;
							setQueryParams({
								...queryParams,
								name: text
							});
						}}
					/>
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
			<div id="pagination" className="fixed bottom-20 left-1/2 -translate-x-1/2 sm:bottom-10">
				<div className="join border-primary rounded-sm border">
					<button className="join-item btn bg-base-100" onClick={onPrevPage} disabled={page === 1}>
						«
					</button>
					<button className="join-item btn bg-base-100">Page {page}</button>
					<button
						className="join-item btn bg-base-100"
						onClick={onNextPage}
						disabled={page === maxPage}
					>
						»
					</button>
				</div>
			</div>
			<div className="h-40"></div>
		</div>
	);
}
