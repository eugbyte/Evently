import { fileURLToPath, URL } from "node:url";

import { defineConfig } from "vite";
import { env } from "process";
import { generatePem, getBackendUrl, type KeyCertPair } from "./aspnetcore-https.ts";
import tailwindcss from "@tailwindcss/vite";
import react from "@vitejs/plugin-react";

const keyCert: KeyCertPair = generatePem();
const { key, cert } = keyCert;
const backendUrl: string = getBackendUrl();
console.log({ backendUrl });

// https://vitejs.dev/config/
export default defineConfig({
	plugins: [tailwindcss(), react()],
	resolve: {
		alias: {
			"~": fileURLToPath(new URL("./src", import.meta.url))
		}
	},
	server: {
		proxy: {
			"^/api": {
				target: backendUrl,
				secure: false,
				// required for js proxy server to play nice with AddGoogle() .NET OAuth middleware
				// https://tinyurl.com/yz92t325
				changeOrigin: true
			}
		},
		port: parseInt(env.DEV_SERVER_PORT || "50071"),
		https: {
			key,
			cert
		}
	}
});
