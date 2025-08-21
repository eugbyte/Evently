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
