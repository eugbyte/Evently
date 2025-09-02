import "@testing-library/jest-dom/vitest";

declare global {
	interface Window {
		setState: (changes: any) => void;
	}
}
