#!/bin/bash
docker cp ./sql/createTopology.sql rv-database:/docker-entrypoint-initdb.d/createTopology.sql
docker exec -it rv-database psql -U postgres -f docker-entrypoint-initdb.d/createTopology.sql
