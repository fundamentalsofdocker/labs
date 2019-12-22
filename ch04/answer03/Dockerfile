FROM golang:alpine as builder
WORKDIR /src
ADD . /src
RUN go build -o goapp

FROM alpine
WORKDIR /app
COPY --from=builder /src/goapp /app/
ENTRYPOINT ./goapp