export interface CardProps {
	title: string;
	description: string;
	imgSrc?: string;
}

export function Card({ title, description, imgSrc }: CardProps) {
	imgSrc =
		imgSrc == null || imgSrc.length === 0
			? "https://img.daisyui.com/images/stock/photo-1606107557195-0e29a4b5b4aa.webp"
			: imgSrc;
	return (
		<div className="card bg-base-200 w-96 shadow-sm">
			<figure>
				<img src={imgSrc} alt="Event Image" />
			</figure>
			<div className="card-body">
				<h2 className="card-title">{title}</h2>
				<p>{description}</p>
				<div className="card-actions justify-end">
					<button className="btn btn-primary">View</button>
				</div>
			</div>
		</div>
	);
}
