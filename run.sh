docker network create -d bridge app-network
docker compose down
docker builder prune
docker compose up -d --no-deps --build