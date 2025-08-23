export enum ToastStatus {
	Success = 0,
	Info,
	Warning,
	Error
}

export const toastStyles = {
	[ToastStatus.Success]: "alert-success",
	[ToastStatus.Info]: "alert-info",
	[ToastStatus.Warning]: "alert-warning",
	[ToastStatus.Error]: "alert-error"
};

export class ToastContent {
	constructor(
		public show: boolean,
		public message = "",
		public toastStatus = ToastStatus.Info
	) {}
}
