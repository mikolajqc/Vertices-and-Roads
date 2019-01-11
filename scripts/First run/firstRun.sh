#!/bin/bash
echo "network"
./shell/createRVNetwork.sh
echo "postgis"
./shell/runNewPostGisContainer.sh
sleep 10
echo "database in container"
./shell/createDatabaseInContainer.sh
echo "init database"
./shell/initDatabase.sh
sleep 5
echo "create topology"
./shell/createTopology.sh
echo "run web"
./shell/runRVWeb.sh
