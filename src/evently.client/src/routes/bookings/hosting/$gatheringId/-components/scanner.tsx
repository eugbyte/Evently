import { useEffect, useRef, useState, type JSX } from "react";
import QrScanner from "qr-scanner";

interface ScannerProps {
	onSuccess: (data: string) => void;
	onDecodeError?: (e: string | Error) => void;
	className?: string;
}

export function Scanner({
	onSuccess,
	className,
	onDecodeError: onError
}: ScannerProps): JSX.Element {
	const videoRef = useRef<HTMLVideoElement>(null);
	const [qrScanner, setQrScanner] = useState<QrScanner | null>(null);
	const [isScanning, setIsScanning] = useState(false);

	useEffect(() => {
		// initialize scanner
		(async () => {
			if (!videoRef.current) return;

			const scanner: QrScanner = new QrScanner(
				videoRef.current,
				async (result) => {
					qrScanner?.stop();

					const data: string = result.data;
					try {
						onSuccess(data);
					} catch {
						setIsScanning(false);
					}
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

			try {
				await scanner.start();
				setIsScanning(true);
			} catch (error) {
				console.error("Failed to start scanner:", error);
			}
		})();
	}, [onSuccess, onError]);

	useEffect(() => {
		// Cleanup function
		return () => {
			if (qrScanner != null) {
				qrScanner.destroy();
			}
		};
	}, [qrScanner]);

	const handleClick = async () => {
		if (qrScanner == null) {
			return;
		}

		if (!isScanning) {
			try {
				await qrScanner.start();
				setIsScanning(true);
			} catch (error) {
				console.error("Failed to start scanner:", error);
			}
		} else {
			qrScanner.stop();
			setIsScanning(false);
		}
	};

	return (
		<div className={`${className} space-y-2`}>
			<video ref={videoRef} className="mx-auto">
				Video stream not available.
				<track default kind="captions" src="" />
			</video>

			<button
				className="variant-filled btn mx-auto block w-3/4 sm:w-1/2"
				onClick={handleClick}
				type="button"
			>
				{!isScanning ? <p>Scan</p> : <p>Stop Scan</p>}
			</button>
		</div>
	);
}
