const putInCache = async (request, response) => {
  const cache = await caches.open("v1");
  await cache.put(request, response);
};

const cacheFirst = async ({ request, preloadResponsePromise }) => {
  // First try to get the resource from the cache
  const responseFromCache = await caches.match(request);
  if (responseFromCache) {
    return responseFromCache;
  }

  // Next try to use the preloaded response, if it's there
  const preloadResponse = await preloadResponsePromise;
  if (preloadResponse) {
    console.info("using preload response", preloadResponse);
    putInCache(request, preloadResponse.clone());
    return preloadResponse;
  }

  // Next try to get the resource from the network
  try {
    const responseFromNetwork = await fetch(request);
    // response may be used only once
    // we need to save clone to put one copy in cache
    // and serve second one
    putInCache(request, responseFromNetwork.clone());
    return responseFromNetwork;
  } catch (error) {
    // There is nothing we can do, but we must always
    // return a Response object
    return new Response("Network error happened", {
      status: 408,
      headers: { "Content-Type": "text/plain" },
    });
  }
};

const networkFirst = async (request) => {
  // First try to get the resource from the network
  try {
    const responseFromNetwork = await fetch(request);
    // response may be used only once
    // we need to save clone to put one copy in cache
    // and serve second one
    putInCache(request, responseFromNetwork.clone());
    return responseFromNetwork;
  } catch (error) {
    // Fallback to the cache
    const responseFromCache = await caches.match(request);
    if (responseFromCache) {
      return responseFromCache;
    }
    // when even the cached response is not available,
    // there is nothing we can do, but we must always
    // return a Response object
    return new Response("Network error happened", {
      status: 408,
      headers: { "Content-Type": "text/plain" },
    });
  }
};

self.addEventListener("activate", (event) => {
  event.waitUntil(async () => {
    if (self.registration.navigationPreload) {
      // Enable navigation preloads!
      await self.registration.navigationPreload.enable();
    }
  });
});

self.addEventListener("install", (event) => {
  event.waitUntil(caches.open("v1").then(cache => cache.addAll(preCacheUris)));
});

self.addEventListener("fetch", (event) => {
  console.log(event.request.destination, event.request.url, event.request.mode);
  if (event.request.method === "GET") {
    if (cacheableDestinations.includes(event.request.destination)) {
      event.respondWith(
        cacheFirst({
          request: event.request,
          preloadResponsePromise: event.preloadResponse,
        }).catch((x) => console.log(x))
      );
    } else {
      event.respondWith(
        networkFirst(event.request)
      )
    }
  }
});

const cacheableDestinations = [
  "audio",
  "embed",
  "font",
  "image",
  "manifest",
  "report",
  "script",
  "style",
  "track",
  "video",
  "worker",
];

const preCacheUris = [
  'index.html',
  'js/app.js',
  'js/chunk-vendors.js',
  'icon/ball.svg',
  'icon/buk-icon.svg',
  'icon/calendar.svg',
  'icon/circle-info.svg',
  'icon/game-minefield.svg',
  'icon/game-monkeybars.svg',
  'icon/game-nervespiral.svg',
  'icon/game-tablesurfing.svg',
  'icon/game-tickettwist.svg',
  'icon/group.svg',
  'icon/home.svg',
  'icon/place.svg',
  'icon/refresh.svg',
  'icon/sidequest.svg',
  'icon/sq-guess.svg',
  'icon/sq-insight.svg',
  'icon/sq-knowledge.svg',
  'icon/sq-math.svg',
  'icon/sq-remember.svg',
  'image/4.jpg',
  'image/5.jpg',
  'image/6.jpg',
  'image/7.jpg',
  'image/8.jpg',
  'image/9.jpg',
  'image/10.jpg',
  'image/ubg-arena-small.png',
  'image/ubg-b-liga-icons.svg',
  'image/ubg-beach-small.png',
  'image/ubg-k-liga-icons.svg',
  'image/ubg-logo.png',
  'image/ubg-u-liga-icons.svg',
  'video/crowdsurfing.mp4',
  'video/monkeybars.mp4',
  'video/minefield.mp4',
  'video/nervespiral.mp4',
  'video/tickettwist.mp4',
]
