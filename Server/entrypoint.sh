#!/bin/bash

set -e
run_cmd="dotnet run --server.urls http://*:80"

until dotnet ef database update; do
>&2 echo "SQL Server is starting up"
sleep 1
done

>&2 echo "SQL Server is up - executing command"
$run_cmd

sleep 90s
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Your_password123 -d master -i DBinit.sql