import type { Ref } from "react";

interface QrDialogProps {
	handleCancel: () => void;
	cancellationDialogRef: Ref<HTMLDialogElement>;
}

export function CancellationDialog({ cancellationDialogRef, handleCancel }: QrDialogProps) {
	return (
		<dialog className="modal" ref={cancellationDialogRef}>
			<div className="modal-box">
				<h3 className="text-lg font-bold">Cancel Registration?</h3>
				<div className="modal-action justify-between">
					<button className="btn btn-error" onClick={handleCancel}>
						Cancel
					</button>
					<form method="dialog">
						{/* if there is a button in form, it will close the modal */}
						<button className="btn">Close</button>
					</form>
				</div>
			</div>
		</dialog>
	);
}
