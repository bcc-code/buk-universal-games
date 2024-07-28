# BUK Universal Games

## Notes - if project is re-used

See issue https://github.com/bcc-code/buk-universal-games/issues/127
## Running Locally

Install docker desktop and run `cd backend; docker-compose up --build -d` to start the backend. Visit http://localhost:5127/ for the Directus UI, credentials are admin@admin.com:password. Go to http://localhost:5125/ to view swagger. 
Run `cd frontend; npm install; npm run dev` for the front-end. This project also supports bun. Install bun and replace all usages of npm with bun.

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

1. families...json
2. games...json
3. leagues...json
4. teams...json
5. matches...json
6. settings...json

There is a points json file, and you can import this to see what the actual points were at the end of ubg, but the points do not have to be imported, because this table will be populated during the game when scores are registered. 

### Accessing Database in production
Go to Google Cloud Console > sidebar > SQL >  buk-universal-games-pgsql-prod > sidebar > cloud SQL Studio
1. Database: buk-universal-games-prod
2. User: buk-universal-games-prod-default
3. You can find the password by going to: Cloud run > buk-universal-games-api-prod > Revisions > Environment Variables > Press the link which says `buk-universal-games-api-prod-POSTGRES_PASSWORD` > Press 3 dots on the single row which shows up > View secret value. This is the password for the database user.

### Accessing Database locally

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

`cd .\backend\Buk.UniversalGames.Data\`
`dotnet tool install --global dotnet-ef`  
`dotnet ef migrations add [Migration Name]`

Migrations are automatically applied when the a new version of the application is deployed.

## Access directus in prod
https://buk-universal-games-directus-prod-lo75nlp2va-ez.a.run.app/

This link may change when a new server is made. Ask Reng for new url in this case.

## Acces swagger in prod

https://universalgames.buk.no/api
