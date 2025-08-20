import { type JSX } from "react";
import { TabState } from "./tab-state";

export interface TabsProps {
	tab: number;
	handleTabChange: (tab: number) => void;
}

export function Tabs({ tab, handleTabChange }: TabsProps): JSX.Element {
	const tabStates: Record<TabState, string> = {
		[TabState.Upcoming]: "Upcoming",
		[TabState.Past]: "Past"
	};

	return (
		<div role="tablist" className="tabs tabs-border">
			<button
				role="tab"
				onClick={() => handleTabChange(0)}
				className={`tab ${tab === 0 ? "tab-active" : ""}`}
			>
				{tabStates[TabState.Upcoming]}
			</button>
			<button
				role="tab"
				onClick={() => handleTabChange(1)}
				className={`tab ${tab === 1 ? "tab-active" : ""}`}
			>
				{tabStates[TabState.Past]}
			</button>
		</div>
	);
}
