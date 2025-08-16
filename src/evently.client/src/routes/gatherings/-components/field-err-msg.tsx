import type { AnyFieldApi } from "@tanstack/react-form";
import type { JSX } from "react";

export function FieldErrMsg({ field }: { field: AnyFieldApi }): JSX.Element {
	return (
		<>
			{field.state.meta.isTouched && !field.state.meta.isValid ? (
				<label className="label">
					<span className="label-text-alt text-error">{field.state.meta.errors.join(", ")}</span>
				</label>
			) : null}
			{field.state.meta.isValidating ? (
				<label className="label">
					<span className="label-text-alt">
						<span className="loading loading-spinner loading-xs"></span>
						Validating...
					</span>
				</label>
			) : null}
		</>
	);
}
