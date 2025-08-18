import imageCompression, { type Options } from "browser-image-compression";

export async function compressImage(file: File): Promise<File> {
	const options: Options = {
		maxSizeMB: 1,
		maxWidthOrHeight: 1920,
		useWebWorker: true
	};

	try {
		file = await imageCompression(file, options);
	} catch (e) {
		console.error(e);
	}

	return file;
}
