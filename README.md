# BUK Universal Games

## Running Locally

Run `docker-compose up --build`
Visit http://localhost:5125/weatherforecast

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
