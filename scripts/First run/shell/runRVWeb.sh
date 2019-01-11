#!/bin/bash
cd ../..
docker build -t rv-web-img --no-cache .
#docker rm rv-web
docker run --name  rv-web --network=rv-network -p 80:5000 -d rv-web-img
