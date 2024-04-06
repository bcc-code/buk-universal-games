# BUK Universal Games

## Notes - if project ever re-used

1. Make it possible to clear a match result and to set a match to a draw (splitting winner points)
2. Extra safety on point-and-match clearing function - not allowed after game started
3. Hide highscore to keep things exciting failed because of missing check for timezone when parsing dates from settings (missed by some hours)
4. Recommended: When signing in as systemadmin - everything should be similar as for an admin but with an additional screen for system admin features (clear cache, pre-cache and clear points and matches)
5. Better info: If start links used through Telegram and if stickers scanned directly from phones camera and not camera button i app - user is not signed in because Telegram browser does not have same local storage as the browser used outside Telegram

## Running Locally

Run `docker-compose up --build -d` for the backend. Visit http://localhost:5127/ for the Directus UI, credentials are admin@admin.com:password
Run `cd frontend; npm install; npm run serve` for the front-end. Visit https://localhost:8080 to view it.

Note: To allow a PWA to work in development in Chromium browsers you must start it with an additional command line flag `--ignore-certificate-errors`:

```sh
chromium --ignore-certificate-errors http://localhost:8080
```
Or in PowerShell:
```ps
Start-Process "C:\Program Files\Google\Chrome\Application\chrome.exe" -ArgumentList "--ignore-certificate-errors", "http://localhost:8080"
```

## Test data

You can import the test data files in Directus in this order:

1. games…json
2. leagues…json
3. teams…json
4. matches…json
5. settings…json

### Accessing Database

1. Visit: http://localhost:5126/
2. Log in with: admin@admin.com / password (Note: You may get stuck in a redirect loop on Firefox, use Chrome instead)
3. Right click "Servers"->Register->Server...
4. Add following parameters
   1. Name: buk-universal-games
   2. (Under connection)
      - Host name: `host.docker.internal` on Windows / Mac (otherwise, see below)
      - Port: `5432`
      - Username: `admin`
      - Password: `password`
5. Database tables can be found under `buk-universal-games->Schemas->public`

#### If `host.docker.internal` doesn't work

On Windows and MacOS, these DNS entries are auto-added. On Linux systems, this doesn't seem to be the case.

Instead run `ip route` to get a list of adapter routes:
  
```sh
default via 192.168.2.254 dev eth0 proto dhcp metric 100
172.17.0.0/16 dev docker0 proto kernel scope link src 172.17.0.1 linkdown
172.18.0.0/16 dev br-9a750de8dfd7 proto kernel scope link src 172.18.0.1
192.168.2.0/24 dev eth0 proto kernel scope link src 192.168.2.51 metric 100
```

In this case you can see three routing configurations:

- `docker0` the default network adapter for docker
- `br-9a750de8dfd7` a bridge adapter created by docker-compose
- `eth0` the `default` LAN connection of the machine.

To connect to the database, use the gateway address for the docker-compose bridge adapter. This represents access to the `buk-universal-games` network defined in the docker compose file. The IP to use is therefore `172.18.0.1`.

Note: If you're running multiple docker-compose configurations with custom networks at the same time, you'll see multiple bridge adapters and you'll have to guess which one is correct. There are of course more deterministic ways to find which one is correct, please expand this document with such methods if you find you need them.

## Adding Database Migrations

In backend/Buk.UniversalGames.Api:

`dotnet tool install --global dotnet-ef`  
`dotnet ef migrations add [Migration Name]`

Migrations are automatically applied when the a new version of the application is deployed.

## Connect to the Remote Postgres Instance

1. Generate a credentials key for the "remote-admin" user in Google Cloud Console
2. Paste this key into 'credentials/gcp-remote-admin.json'
3. Run `docker-compose -f docker-compose.yml -f docker-compose.proxy.yml up`
4. Open pgAdmin: http://localhost:5126/
5. Right click "Servers"->Register->Server...
6. Add following parameters
   1. Name: `buk-universal-games - PROD` (or similar)
   2. (Under connection)
      - Host name: `host.docker.internal` (on windows, can also try localhost on other platforms)
      - Port: `5434`
      - Username: `remote-admin`
      - Password: `{***remote admin password***}`

## Avoiding refresh loops in the frontend

When having refresh loops in the frontend, go to devtools > Application > (in the sidebar) Service workers, and enable "Update on reload" and "Bypass for network".

To log out or clear the cache because of changes to the source code, go to devtools > Application > (in the sidebar) Storage > Clear site data.

## To update the leaderboard

Go to https://universalgames.buk.no/api/OVERLORDS/cache/warmup
