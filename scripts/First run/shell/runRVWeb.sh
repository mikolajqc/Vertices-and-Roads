#!/bin/bash
cd ../..
docker build -t rv-web-img --no-cache .
#docker rm rv-web
docker run --name  rv-web --network=rv-network --ip 172.22.0.23 -p 5000:5000 -d rv-web-img
