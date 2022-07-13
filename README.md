# BUK Universal Games

## Running Locally

Run `docker-compose up --build`  
Visit http://localhost:5125/

### Accessing Database

1. Visit: http://localhost:5126/
2. Log in with: admin@admin.com / password
3. Right click "Servers"->Register->Server...
4. Add following parameters
   1. Name: buk-universal-games
   2. (Under connection)
     * Host name: `host.docker.internal` (on windows, can also try localhost on other platforms)
     * Port: `5432`
     * Username: `admin`
     * Password: `password`
5. Database tables can be found under `buk-universal-games->Schemas->public`

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
     * Host name: `host.docker.internal` (on windows, can also try localhost on other platforms)
     * Port: `5434`
     * Username: `remote-admin`
     * Password: `{***remote admin password***}`
