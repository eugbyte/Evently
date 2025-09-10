import {DateTime} from "luxon";

export function camelCase(str: string): string {
	const letters: string[] = str.split("");
	letters[0] = letters[0].toUpperCase();
	return letters.join("");
}

export function sleep(ms: number): Promise<void> {
	return new Promise((resolve) => {
		setTimeout(() => resolve(), ms);
	});
}

export function hashString(text: string): number {
	let hash = 0;
	// A prime number commonly used for polynomial hashing.
	const P = 31;
	// A large prime modulus to prevent integer overflow and reduce collisions.
	const M = 1e9 + 7;

	// Iterate over each character of the string.
	for (let i = 0; i < text.length; i++) {
		// Get the character's ASCII value. Adding 1 helps to avoid a hash of 0 for empty or null strings.
		const charValue = text.charCodeAt(i);

		// Update the hash using the polynomial rolling hash formula.
		hash = (hash * P + charValue) % M;
	}
	return hash;
}

export function downloadFile(blob: Blob, fileName = ""): void {
	const objUrl: string = URL.createObjectURL(blob);
	const link = document.createElement("a");
	link.setAttribute("download", fileName);
	link.href = objUrl;
	document.body.appendChild(link);
	link.click();
	link.remove();
}

export function toIsoDateTimeString(date: Date | null): string {
	if (date == null) {
		return "";
	}
	const dateTime: DateTime = DateTime.fromJSDate(date);
	return dateTime.toFormat("yyyy-MM-dd'T'HH:mm");
}

export function toIsoDateString(date: Date | null): string {
	if (date == null) {
		return "";
	}
	const dateTime: DateTime = DateTime.fromJSDate(date);
	return dateTime.toFormat("yyyy-MM-dd");
}

