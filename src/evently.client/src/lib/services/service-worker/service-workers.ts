/// <reference lib="esnext" />
/// <reference lib="webworker" />

// Copied from https://svelte.dev/docs/kit/service-workers#Inside-the-service-worker

export function onInstall(filePaths: string[], version: string, event: ExtendableEvent) {
	// Create a new cache and add all files to it
	const addFilesToCache = async () => {
		const cache = await caches.open(version);
		await cache.addAll(filePaths);
	};

	event.waitUntil(addFilesToCache());
}

export function onActivate(version: string, event: ExtendableEvent) {
	const deleteOldCaches = async () => {
		for (const key of await caches.keys()) {
			if (key !== version) {
				await caches.delete(key);
			}
		}
	};

	event.waitUntil(deleteOldCaches());
}

export function onFetch(version: string, event: FetchEvent) {
	const { request } = event;
	const url = new URL(request.url);
	// ignore POST requests etc, // ignore requests made to backend server - will mess up the auth process
	if (request.method !== "GET" || url.pathname.startsWith("/api/v1")) {
		return;
	}

	event.respondWith(cacheFirst(version, request) as Promise<Response>);
}

async function cacheFirst(version: string, request: Request) {
	const url = new URL(request.url);
	const cache = await caches.open(version);

	// `build`/`files` can always be served from the cache
	const cacheResp: Response | undefined = await cache.match(url.pathname);
	if (cacheResp != null) {
		return cacheResp;
	}

	try {
		// note that fetch does not throw an error even if the status code is 3XX - 5XX.
		const response: Response = await fetch(request);

		if (response.ok) {
			await cache.put(request, response.clone());
		}
		return response;
	} catch {
		// error is thrown here only when network is unavailable
		return new Response("Network error happened", {
			status: 408,
			headers: { "Content-Type": "text/plain" }
		});
	}
}

interface Notification {
	title: string;
	body: string;
	icon: string;
}

export function onPush(event: PushEvent, onComplete = () => undefined) {
	if (event == null || event.data == null) {
		return;
	}
	const sw = self as unknown as ServiceWorkerGlobalScope;

	const data: Notification = event.data.json();
	const displayPromise: Promise<void> = sw.registration.showNotification(data.title, {
		body: data.body
	});
	const promiseChain = Promise.all([displayPromise])
		.then(() => onComplete())
		.catch((err) => console.error(err));
	event.waitUntil(promiseChain);
}
