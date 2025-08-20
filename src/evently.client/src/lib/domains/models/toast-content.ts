export enum ToastStatus {
	Success = 0,
	Info,
	Warning,
	Error
}

export class ToastContent {
	constructor(
		public show: boolean,
		public message = "",
		public toastStatus = ToastStatus.Info
	) {}
}
