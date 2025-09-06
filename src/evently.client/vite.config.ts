import { env } from "process";
import { generatePem, getBackendUrl, type KeyCertPair } from "./aspnetcore-https.ts";
import tailwindcss from "@tailwindcss/vite";
import react from "@vitejs/plugin-react";
import { tanstackRouter } from "@tanstack/router-plugin/vite";
import { fileURLToPath, URL } from "node:url";
import { defineConfig } from "vitest/config";
import { VitePWA } from "vite-plugin-pwa";

const keyCert: KeyCertPair = generatePem();
const { key, cert } = keyCert;
const backendUrl: string = getBackendUrl();
console.log({ backendUrl });

// https://vitejs.dev/config/
export default defineConfig({
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
	},
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
		react(),
		VitePWA({
			registerType: "autoUpdate",
			workbox: {
				globPatterns: ["**/*.{js,css,html,ico,png,svg}"],
				maximumFileSizeToCacheInBytes: 10_000_000, // 10mb
				// Important to prevent service worker from caching api calls
				// The Service Worker intercepts all network requests.
				// It can serve cached files directly from the browser without ever contacting the server.
				// Also, to prevent a route clash between front end routes and backend routes. 
				// Otherwise, if the user types in /api/v1/Healthcheck in the browser, 
				// the Service Worker will not even let the request reach the backend, and instead intercept it and return index.html.
				navigateFallbackDenylist: [/^(\/api\/.*)$/], 
			},
			devOptions: {
				enabled: true
			},
			strategies: "generateSW",
			manifest: {
				name: "Evently",
				description: "Manage your events",
				display: "standalone",
				start_url: "/",
				scope: "/",
				icons: [
					{
						src: "./icon.192.png",
						sizes: "192x192",
						type: "image/png",
						purpose: "any maskable"
					}
				]
			}
		})
	]
});
