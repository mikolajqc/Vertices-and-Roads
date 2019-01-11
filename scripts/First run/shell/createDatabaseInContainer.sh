echo "copying createDatabase.sql...\n"
docker cp ./sql/createDatabase.sql rv-database:/docker-entrypoint-initdb.d/createDatabase.sql

echo "executing createDatabase.sql...\n"
docker exec -it rv-database psql -U postgres -f docker-entrypoint-initdb.d/createDatabase.sql
