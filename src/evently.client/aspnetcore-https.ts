import { spawnSync } from "child_process";
import { env } from "node:process";
import path from "node:path";
import fs from "node:fs";

export interface KeyCertPair {
	key: string;
	cert: string;
}

export function generatePem(): KeyCertPair {
	const baseFolder: string =
		process.env.APPDATA !== undefined && process.env.APPDATA !== ""
			? `${process.env.APPDATA}/ASP.NET/https`
			: `${process.env.HOME}/.aspnet/https`;

	const certificateName = "evently.client";

	const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
	const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

	if (!fs.existsSync(baseFolder)) {
		fs.mkdirSync(baseFolder, { recursive: true });
	}

	if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
		const child = spawnSync(
			"dotnet",
			["dev-certs", "https", "--export-path", certFilePath, "--format", "Pem", "--no-password"],
			{ stdio: "inherit" }
		);

		if (child.status != 0) {
			throw new Error("Could not create certificate");
		}
	}

	return {
		key: fs.readFileSync(keyFilePath).toString(),
		cert: fs.readFileSync(certFilePath).toString()
	};
}

export function getBackendUrl(): string {
	let target = "https://localhost:7201";
	if (env.ASPNETCORE_HTTPS_PORT) {
		target = `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`;
	} else if (env.ASPNETCORE_URLS) {
		target = env.ASPNETCORE_URLS.split(";")[0];
	}
	console.log({
		ASPNETCORE_HTTPS_PORT: env.ASPNETCORE_HTTPS_PORT,
		ASPNETCORE_URLS: env.ASPNETCORE_URLS
	});
	return target;
}
