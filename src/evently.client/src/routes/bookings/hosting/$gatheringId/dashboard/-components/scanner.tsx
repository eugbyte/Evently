import { useEffect, useRef, useState, useCallback, type JSX } from "react";
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
	const qrScannerRef = useRef<QrScanner | null>(null);
	const [isScanning, setIsScanning] = useState(false);
	const [isLoading, setIsLoading] = useState(false);

	useEffect(() => {
		const initializeScanner = async () => {
			if (!videoRef.current) return;

			qrScannerRef.current = new QrScanner(
				videoRef.current,
				async (result) => {
					if (isLoading) {
						return;
					}
					qrScannerRef.current?.stop();
					setIsLoading(true);

					const data: string = result.data;
					try {
						onSuccess(data);
					} catch {
						setIsLoading(false);
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

			try {
				await qrScannerRef.current.start();
				setIsScanning(true);
			} catch (error) {
				console.error("Failed to start scanner:", error);
			}
		};

		initializeScanner();

		// Cleanup function
		return () => {
			if (qrScannerRef.current) {
				qrScannerRef.current.destroy();
			}
		};
	}, [onSuccess, onError, isLoading]);

	const handleClick = useCallback(async () => {
		if (!qrScannerRef.current) return;

		if (!isScanning) {
			try {
				await qrScannerRef.current.start();
				setIsScanning(true);
			} catch (error) {
				console.error("Failed to start scanner:", error);
			}
		} else {
			qrScannerRef.current.stop();
			setIsScanning(false);
		}
	}, [isScanning]);

	return (
		<div className={className}>
			<video ref={videoRef} className="mx-auto mb-10">
				Video stream not available.
				<track default kind="captions" src="" />
			</video>

			<button
				className="variant-filled btn mx-auto mb-10 block w-3/4 sm:w-1/2"
				onClick={handleClick}
				type="button"
			>
				{!isScanning ? <p>Scan</p> : <p>Stop</p>}
			</button>
		</div>
	);
}

export default Scanner;
