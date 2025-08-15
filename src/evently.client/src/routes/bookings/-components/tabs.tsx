import { type JSX } from "react";

export interface TabsProps {
	tab: number;
	handleTabChange: (tab: number) => void;
}

export function Tabs({ tab, handleTabChange }: TabsProps): JSX.Element {
	const tabStates: Record<number, string> = {
		0: "Upcoming",
		1: "Past"
	};

	return (
		<div role="tablist" className="tabs tabs-border">
			<button
				role="tab"
				onClick={() => handleTabChange(0)}
				className={`tab ${tab === 0 ? "tab-active" : ""}`}
			>
				{tabStates[0]}
			</button>
			<button
				role="tab"
				onClick={() => handleTabChange(1)}
				className={`tab ${tab === 1 ? "tab-active" : ""}`}
			>
				{tabStates[1]}
			</button>
		</div>
	);
}
