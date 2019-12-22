# Docker for Desktop on Windows

If you are running Docker for Desktop on Windows you need to define the following environment variable to be able to mount the Docker socket into the Azure CLI container:

    export COMPOSE_CONVERT_WINDOWS_PATHS=1

Now if you run the CLI with Docker Compose you should be successful:

    docker-compose up -d

now exec into the `az` container:

    docker-compose exec az /bin/bash