docker image build -t api-node api
docker image build -t tests-node tests

docker network create test-net

docker container run --rm -d \
    --name postgres \
    --net test-net \
    -v $(pwd)/database:/docker-entrypoint-initdb.d \
    -v pg-data:/var/lib/postgresql/data \
    -e POSTGRES_USER=dbuser \
    -e POSTGRES_DB=sample-db \
    postgres:11.5-alpine

docker container run --rm -d \
    --name api \
    --net test-net \
    -p 3000:3000 \
    api-node

echo "Sleeping for 5 sec..."
sleep 5

docker container run --rm -it \
    --name tests \
    --net test-net \
    -e BASE_URL="http://api:3000" \
    tests-node