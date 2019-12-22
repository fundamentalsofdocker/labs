#! /bin/bash

docker container run -d \
    --name postgres \
    -v $(pwd)/../database:/docker-entrypoint-initdb.d \
    -v pg-data:/var/lib/postgresql/data \
    -p 5432:5432 \
    -e POSTGRES_USER=dbuser \
    -e POSTGRES_DB=sample-db \
    postgres:11.5-alpine

docker run -it \
    --name api \
    -p 3000:3000 \
    -e DB_HOST=postgres \
    sample-api