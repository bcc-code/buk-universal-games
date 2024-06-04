// Service worker file

// Define the cache name and files to cache
const CACHE_NAME = 'my-site-cache-v1';
const urlsToCache = [
  'favicon.ico',
  'favicon.png',
  'icon/192.png',
  'icon/buk-icon.svg',
  'icon/circle-info.svg',
  'icon/family-icon.svg',
  'icon/game-humanshuffleboard.svg',
  'icon/game-irongrip.svg',
  'icon/game-labyrinth.svg',
  'icon/game-landwaterbeach.svg',
  'icon/game-mastermind.svg',
  'icon/leaderlist.png',
  'icon/match.png',
  'icon/place.svg',
  'image/1_place.png',
  'image/2_place.png',
  'image/3_place.png',
  'image/logo_icon.svg',
  'image/ubg-arena-small.png',
  'image/ubg-b-liga-icons.svg',
  'image/ubg-beach-small.png',
  'image/ubg-k-liga-icons.svg',
  'image/ubg-logo.svg',
  'image/ubg-u-liga-icons.svg',
  'image/watermark.svg',
  'splashscreen/640x1136.png',
  'splashscreen/750x1334.png',
  'splashscreen/1125x2436.png',
  'splashscreen/1242x2208.png',
  'splashscreen/1536x2048.png',
  'splashscreen/1668x2224.png',
  'splashscreen/2048x2732.png',
];

// Install service worker
self.addEventListener('install', function (event) {
  // Perform install steps
  event.waitUntil(
    caches.open(CACHE_NAME).then(function (cache) {
      console.log('Opened cache');
      return cache.addAll(urlsToCache);
    }),
  );
});

// Fetch resources from cache or network
self.addEventListener('fetch', function (event) {
  event.respondWith(
    caches.match(event.request).then(function (response) {
      // Cache hit - return response
      if (response) {
        return response;
      }

      // Clone the request
      var fetchRequest = event.request.clone();

      // Fetch request from network
      return fetch(fetchRequest).then(function (response) {
        // Check if we received a valid response
        if (!response || response.status !== 200 || response.type !== 'basic') {
          return response;
        }

        // Clone the response
        var responseToCache = response.clone();

        // Cache the fetched response
        caches.open(CACHE_NAME).then(function (cache) {
          cache.put(event.request, responseToCache);
        });

        return response;
      });
    }),
  );
});

// Remove old caches
self.addEventListener('activate', function (event) {
  var cacheWhitelist = [CACHE_NAME];

  event.waitUntil(
    caches.keys().then(function (cacheNames) {
      return Promise.all(
        cacheNames.map(function (cacheName) {
          if (cacheWhitelist.indexOf(cacheName) === -1) {
            return caches.delete(cacheName);
          }
        }),
      );
    }),
  );
});
