FROM alpine:latest
RUN apk update && apk add jq
WORKDIR /app
COPY script.sh ./
CMD source script.sh
 