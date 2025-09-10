import axios from "axios";

/**
 * 
 * @param href e.g.: https://saeventlydevsea.blob.core.windows.net/evently-dev-images/gatherings/20/cover-image.png
 * @param fileName e.g.: cover-image.png
 */
export async function fetchFile(href: string, fileName?: string): Promise<File> {
    const urlObj = new URL(href);
    const filePath: string = urlObj.href;
    
    if (fileName == null) {
        fileName = urlObj.pathname.split("/").pop() ?? "";
    }

    const response = await axios.get(`/api/v1/Files/object-storage`, { params: { filePath },  responseType: 'blob' });
    const dataBlob: Blob = await response.data;
    return new File([dataBlob], fileName, { type: dataBlob.type });
}
