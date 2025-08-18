import React, { useState, type JSX } from "react";
import { Icon } from "@iconify/react";

export interface FileEvent extends React.ChangeEvent<HTMLInputElement> {}

interface FileUploadProps {
	onChange: (event: FileEvent) => void;
}

export function FileUploadButton({ onChange: _onChange }: FileUploadProps): JSX.Element {
	const [fileName, setFileName] = useState<string>("JPEG/PNG/SVG");

	const onChange = (event: FileEvent) => {
		if (event.currentTarget.files == null || event.currentTarget.files.length === 0) {
			return;
		}
		const files: FileList = event.currentTarget.files;
		const file: File = files[0];
		setFileName(file.name);
		_onChange(event);
	};

	return (
		<div className="flex w-fit flex-row items-center">
			<div className="btn btn-sm preset-filled cursor-pointer">
				<Icon height="24" icon="material-symbols:cloud-upload" width="24" />
				<input accept="image/*" className="hidden cursor-pointer" onChange={onChange} type="file" />
				Upload photo
			</div>
			<span className="ml-2 text-xs text-gray-600">{fileName}</span>
		</div>
	);
}
