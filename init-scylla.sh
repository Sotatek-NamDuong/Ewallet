#!/bin/bash
until cqlsh -e "describe keyspaces"; do
  echo "ScyllaDB is unavailable - sleeping"
  sleep 2
done

echo "Creating superuser..."
cqlsh -e "CREATE USER IF NOT EXISTS admin WITH PASSWORD 'admin_password' SUPERUSER;"

echo "Creating normal user..."
cqlsh -e "CREATE USER IF NOT EXISTS normal_user WITH PASSWORD 'user_password';"

echo "Creating keyspace..."
cqlsh -e "CREATE KEYSPACE IF NOT EXISTS my_keyspace WITH REPLICATION = { 'class' : 'SimpleStrategy', 'replication_factor' : 1 };"

echo "Granting permissions..."
cqlsh -e "GRANT ALL PERMISSIONS ON KEYSPACE my_keyspace TO normal_user;"

echo "ScyllaDB initialized."