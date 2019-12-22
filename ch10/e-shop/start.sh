docker run -d \
    --name traefik \
    -p 8080:8080 \
    -p 80:80 \
    -v /var/run/docker.sock:/var/run/docker.sock \
    traefik:v2.0 --api.insecure=true --providers.docker

docker container run --rm -d \
    --name eshop \
    --label traefik.enable=true \
    --label traefik.port=5000 \
    --label traefik.priority=1 \
    --label traefik.http.routers.eshop.rule="Host(\"acme.com\")" \
    acme/eshop:1.0

docker run --rm -d \
    --name catalog \
    --label traefik.enable=true \
    --label traefik.port=3000 \
    --label traefik.priority=10 \
    --label traefik.http.routers.catalog.rule="Host(\"acme.com\") && PathPrefix(\"/catalog\")" \
    acme/catalog:1.0
