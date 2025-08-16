import Placeholder from "~/lib/assets/event_placeholder.webp";
import { type JSX } from "react";
import { Link } from "@tanstack/react-router";
import { Category, Gathering } from "~/lib/domains/entities";
import { Icon } from "@iconify/react";
import { DateTime } from "luxon";

export interface CardProps {
	gathering: Gathering;
	accountId?: string | null;
}

export function Card({ gathering, accountId }: CardProps): JSX.Element {
	let { name: title, description, coverSrc: imgSrc } = gathering;
	const categories: Category[] = gathering.gatheringCategoryDetails.map(
		(detail) => detail.category
	);

	imgSrc = imgSrc == null || imgSrc.length === 0 ? Placeholder : imgSrc;
	if (title.length > 30) {
		title = title.substring(0, 30) + "...";
	}
	if (description.length > 190) {
		description = description.substring(0, 100) + "...";
	}
	const isOrganiser: boolean = gathering.organiserId === accountId;
	const start: DateTime = DateTime.fromJSDate(gathering.start);
	const end: DateTime = DateTime.fromJSDate(gathering.end);
	return (
		<div className="card bg-base-200 w-96 text-white shadow-sm">
			<figure>
				<img src={imgSrc} alt="Event Image" className="h-48 w-96" />
			</figure>
			<div className="card-body">
				<div className="flex flex-row justify-between">
					<h2 className="card-title">{title}</h2>
					{isOrganiser && <div className="badge badge-outline badge-secondary">I'm the host</div>}
				</div>
				<p>{description}</p>
				<div className="flex flex-row items-center space-x-2 text-xs">
					<Icon icon="material-symbols:location-on" width="24" height="24" className="inline">
						Location
					</Icon>
					<span>{gathering.location}</span>
				</div>
				<div className="flex flex-row items-center space-x-2 text-xs">
					<Icon icon="mingcute:time-fill" width="24" height="24" />
					<span>{`${start.toLocaleString(DateTime.DATETIME_MED)} — ${end.toLocaleString(DateTime.DATETIME_MED)}`}</span>
				</div>
				<div className="flex flex-row flex-wrap space-x-2">
					{categories.map((category) => (
						<div className="badge badge-soft badge-info" key={category.categoryId}>
							{category.categoryName}
						</div>
					))}
				</div>
				<div className="card-actions justify-end">
					<Link
						className="btn btn-primary"
						to={`/gatherings/$gatheringId`}
						params={{ gatheringId: gathering.gatheringId.toString() }}
					>
						View
					</Link>
				</div>
			</div>
		</div>
	);
}
