# Todddo Dotnet

Introduction
---

Simple DDD style API application to explore the usage of Asp.Net Core.

Domain model uses the Language-ext library which provides functional programming 
primitives (Option, Either, etc).

xUnit is used for testing.


Docker Compose
---

To run the application using docker-compose, first run the `generate-cert.sh` script 
to create a new development SSL certificate:

```
chmod +x generate-cert.sh
./generate-cert.sh
```

Then run `docker-compose up`, which will build the application and start running on port `44396`:

```
docker-compose up --build
```

You should now be able to connect to the service:

```
curl -k https://localhost:44396/api/todo
```

You should also be able to successfully execute the tests in `Todddo.IntegrationTests`.


Swagger
---

To generate the Swagger client code in `Todddo.ApiClient` you will need to `npm install`
and then run the code generator (make sure the application is running first via `docker-compose`):

```
cd Todddo.ApiClient
npm install
./node_modules/nswag/bin/nswag.js openapi2csclient \
    /input:https://localhost:44396/swagger/v1/swagger.json \
    /output:TodoApiClient.cs \
    /classname:TodoApiClient \
    /namespace:Todddo.ApiClient
```