#! /bin/bash

docker container rm -f api
docker container rm -f postgres
docker volume rm pg-data
