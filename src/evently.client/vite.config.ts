import { env } from "process";
import { generatePem, getBackendUrl, type KeyCertPair } from "./aspnetcore-https.ts";
import tailwindcss from "@tailwindcss/vite";
import react from "@vitejs/plugin-react";
import { tanstackRouter } from "@tanstack/router-plugin/vite";
import { fileURLToPath, URL } from "node:url";
import { defineConfig } from "vitest/config";

const keyCert: KeyCertPair = generatePem();
const { key, cert } = keyCert;
const backendUrl: string = getBackendUrl();
console.log({ backendUrl });

// https://vitejs.dev/config/
export default defineConfig({
	test: {
		include: ["src/**/*.{test,spec}.{js,ts,jsx,tsx}"],
		environment: "jsdom",
		setupFiles: "src/setup-tests.ts",
		globals: true
	},
	plugins: [
		tailwindcss(),
		// Please make sure that '@tanstack/router-plugin' is passed before '@vitejs/plugin-react'
		tanstackRouter({
			target: "react",
			autoCodeSplitting: true
		}),
		react()
	],
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
