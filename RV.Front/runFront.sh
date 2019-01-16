docker build -t rv-front-img --no-cache .
docker rm -f rv-front
docker run --name rv-front --network=rv-network --ip 172.22.0.24 -p 80:8080 -d rv-front-img
