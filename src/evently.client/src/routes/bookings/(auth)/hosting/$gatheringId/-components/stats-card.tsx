import { Icon } from "@iconify/react";

interface StatsCardProps {
	checkInCount: number;
	registrationCount: number;
}

export function StatsCard({ checkInCount, registrationCount }: StatsCardProps) {
	return (
		<div className="card bg-base-200 shadow-xl">
			<div className="card-body">
				<h2 className="card-title flex items-center gap-2">
					<Icon icon="material-symbols:analytics" className="text-primary" />
					Check Ins
				</h2>

				<div className="my-4">
					<div className="mb-2 flex items-end">
						<span className="text-primary text-2xl font-bold">{checkInCount}</span>
						<span className="text-base-content/60 text-lg">/ {registrationCount} total</span>
					</div>

					<progress
						value={checkInCount}
						max={registrationCount}
						className="progress progress-primary h-4 w-full"
					></progress>
				</div>

				<div className="mt-4 grid grid-cols-2 gap-4">
					<div className="bg-primary/10 rounded-lg p-3 text-center">
						<div className="text-primary text-lg font-bold">{checkInCount}</div>
						<div className="text-base-content/70 text-xs">Checked In</div>
					</div>
					<div className="bg-warning/10 rounded-lg p-3 text-center">
						<div className="text-warning text-lg font-bold">{registrationCount - checkInCount}</div>
						<div className="text-base-content/70 text-xs">Pending</div>
					</div>
				</div>
			</div>
		</div>
	);
}
