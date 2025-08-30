import { type JSX, useEffect, useRef, useState } from "react";
import QrScanner from "qr-scanner";

interface ScannerProps {
	showCamera: boolean;
	// rmb to wrap it in useCallBack()
	memoizedOnSuccess: (data: string) => void;
	// rmb to wrap it in useCallBack()
	memoizedOnDecodeError?: (e: string | Error) => void;
	className?: string;
}

export function Scanner({
	showCamera,
	memoizedOnSuccess: onSuccess,
	className,
	memoizedOnDecodeError: onError
}: ScannerProps): JSX.Element {
	const videoRef = useRef<HTMLVideoElement>(null);
	const [qrScanner, setQrScanner] = useState<QrScanner | null>(null);

	useEffect(() => {
		// initialize scanner
		console.log("creating scanner");

		(async () => {
			if (!videoRef.current) {
				return;
			}

			const scanner: QrScanner = new QrScanner(
				videoRef.current,
				async (result) => {
					qrScanner?.stop();

					const data: string = result.data;
					onSuccess(data);
				},
				{
					preferredCamera: "environment",
					highlightScanRegion: true,
					onDecodeError: (e: string | Error) => {
						if (onError != null) {
							onError(e);
							return;
						}
						const errMsg = e instanceof Error ? e.message : e;
						console.error(errMsg);
					}
				}
			);
			setQrScanner(scanner);

			return () => {
				qrScanner?.destroy();
			};
		})();
		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, [onSuccess, onError]);

	useEffect(() => {
		if (qrScanner == null) {
			return;
		}
		if (showCamera) {
			qrScanner.start();
		} else {
			qrScanner.stop();
			// hacky way to resolve the bug that a highlighted region still shows (https://github.com/nimiq/qr-scanner/issues/169)
			const elements = Array.from(
				document.getElementsByClassName("scan-region-highlight")
			) as HTMLElement[];
			for (const element of elements) {
				element.style.display = "none";
			}
		}
	}, [showCamera, qrScanner]);

	return (
		<div className={`${className}`}>
			<div className="flex h-64 justify-center sm:h-80 md:h-96">
				<video ref={videoRef}>
					Video stream not available.
					<track default kind="captions" />
				</video>
			</div>
		</div>
	);
}
