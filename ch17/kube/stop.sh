#!/bin/bash

kubectl delete service/prometheus-svc
kubectl delete service/grafana-svc
kubectl delete service/node-api-svc
kubectl delete service/dotnet-api-svc

kubectl delete deployment/prometheus-deployment
kubectl delete deployment/grafana-deployment
kubectl delete deployment/node-api-deployment
kubectl delete deployment/dotnet-api-deployment

kubectl delete configmap/prometheus-cm
