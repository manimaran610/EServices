
git clone https://github.com/manimaran610/EServices.git

cd EServices
docker-compose build
docker compose up -d


To clear cache storage and free up storage space
--------------------------------------------------------
docker system prune --volumes -a  

-----------------
docker-compose build --no-cache
docker-compose up --force-recreate -d