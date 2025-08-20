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
