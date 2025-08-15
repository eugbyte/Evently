import Placeholder from "~/lib/assets/event_placeholder.webp";
import { type JSX } from "react";
import { Link } from "@tanstack/react-router";
import { Gathering } from "~/lib/domains/entities";

export interface CardProps {
	gathering: Gathering;
}

export function Card({ gathering }: CardProps): JSX.Element {
	let { name: title, description, coverSrc: imgSrc } = gathering;
	imgSrc = imgSrc == null || imgSrc.length === 0 ? Placeholder : imgSrc;
	if (title.length > 30) {
		title = title.substring(0, 30) + "...";
	}
	if (description.length > 190) {
		description = description.substring(0, 100) + "...";
	}
	return (
		<div className="card bg-base-200 w-96 text-white shadow-sm">
			<figure>
				<img src={imgSrc} alt="Event Image" className="h-48 w-96" />
			</figure>
			<div className="card-body">
				<h2 className="card-title">{title}</h2>
				<p>{description}</p>
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
