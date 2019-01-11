#!/bin/bash
cd ..
docker build -t RV-web-img --no-cache .
docker rm RV-web
docker run --name  RV-web --network=RV-network -p 5000:5000 -d RV-web-img
