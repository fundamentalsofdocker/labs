FROM alpine:latest
RUN apk update && apk add docker
COPY pipeline.sh /scripts/pipeline.sh
WORKDIR /usr/src/app
CMD /scripts/pipeline.sh