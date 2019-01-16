echo "running starefossen/pgrouting container...\n"
docker run --name rv-database --network=rv-network --ip 172.22.0.22 -p 5432:5432 -v /var/run/postgresql:/var/run/postgresql -e POSTGRES_PASSWORD=password -d starefossen/pgrouting
