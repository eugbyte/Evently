import axios from "axios";

/**
 *
 * @param href e.g.: gatherings/20/cover-image.png
 */
export async function fetchFile(href: string): Promise<File> {
	const urlObj = new URL(href);
	const filePath: string = urlObj.pathname;

	console.log(filePath.split("/"));
	const bucket = filePath.split("/")[1];
	const fileName = filePath.split("/").slice(2).join("/");

	const response = await axios.get(`/api/v1/Files/object-storage/${bucket}`, {
		params: { fileName },
		responseType: "blob"
	});
	const dataBlob: Blob = await response.data;
	return new File([dataBlob], fileName, { type: dataBlob.type });
}
