echo "copying roads.sql...\n"
docker cp ./sql/insertRoads.sql rv-database:/docker-entrypoint-initdb.d/roads.sql

echo "insertRoads1.sql executing...\n"
docker exec -it rv-database psql -U postgres -f docker-entrypoint-initdb.d/roads.sql

echo "copying createIndexes.sql...\n"
docker cp ./sql/createIndexes.sql rv-database:/docker-entrypoint-initdb.d/createIndexes.sql

echo "createIndexes.sql executing...\n"
docker exec -it rv-database psql -U postgres -f docker-entrypoint-initdb.d/createIndexes.sql

echo "copying createPointView.sql...\n"
docker cp ./sql/createPointView.sql rv-database:/docker-entrypoint-initdb.d/createPointView.sql

echo "createPointView.sql executing...\n"
docker exec -it rv-database psql -U postgres -f docker-entrypoint-initdb.d/createPointView.sql



