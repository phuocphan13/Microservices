# Basic Ecommerce project is built based on Microservices architecture.

This project is my self-learning project. 
I based on [this course](https://www.udemy.com/course/microservices-architecture-and-implementation-on-dotnet/) in Udemy of [Mehmet Ozkaya](https://github.com/mehmetozkaya).
And I'm still enhancing it with:
- Implement new business.
- Apply new structural.
- Apply unit test.
- Apply integration test.

## Introduction

My idea of this project is about applying some technologies that I want to learn. So I'm using lot of technologies but they are reasonable to apply.

This project is running in .NET 6.

Here is the structure of project that I captured from the course (I mentioned above).
![image](https://github.com/phuocphan13/Microservices/assets/44283172/54edd605-8afc-44f5-a665-4db7d1ea1bf0)

I applied Unit Test with xUnit (Catalog.API) and NUnit (Will implement soon).

And I also apply Integration test with [testcontainers](https://testcontainers.com) framework.


## Getting Started

Firstly, you have to clone this project in your local environment.
And install the [Docker Desktop](https://www.docker.com/products/docker-desktop/).

Access in `./docker-compose.dcproj` by terminal and run the command

```bash
docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml up -d
```

After running the app, if you want to stop, you can use the command

```bash
docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml down
```

## Kubernetes (k8s) Deployment

This repository contains Kubernetes manifests in the `k8s/` folder. The cluster deployment uses the `dotnet-app` namespace and is managed via ArgoCD in your environment. The main manifest is `k8s/deployment.yaml` which defines Deployments and Services for all microservices and supporting infra (RabbitMQ, databases, Redis, etc.).

Key service DNS names (inside the `dotnet-app` namespace):

- `rabbitmq:5672` — RabbitMQ AMQP
- `catalogdb:27017` — MongoDB for Catalog
- `basketdb:6379` — Redis for Basket
- `catalog-cache:6379` — Redis for Catalog cache
- `discountdb:5432` — PostgreSQL for Discount
- `orderdb:1433` — SQL Server for Orders
- `authendb:1433` — SQL Server for Identity
- `pgadmin`, `portainer`, `elasticsearch` — infra UIs

Note: ArgoCD deploys the same manifests to your cluster — the `k8s/deployment.yaml` file is the canonical source used here.

## Local access via SSH tunnel (`shell/login.ps1`)

Use `shell/login.ps1` to create persistent SSH port-forwards from the remote environment to `localhost` so you can access remote services locally.

Prerequisites:
- Windows PowerShell
- OpenSSH client installed and available on `PATH` (the script calls `ssh`)

How it works:
- Run `.\
oot\of\repo\shell\login.ps1` in PowerShell; the script will prompt for your SSH username.
- The script lists the port mapping and starts an `ssh` process in the background.
- The script is a toggle: running it again will detect matching `ssh` processes (by command line) and stop them.

Quick usage:

```powershell
# from repository root
.\shell\login.ps1

# re-run the same command to stop the tunnel(s)
.\shell\login.ps1
```

Forwarded port mapping (local -> remote):

- IdentityServer  -> http://localhost:8081 (remote:80)
- PgAdmin         -> http://localhost:8082 (remote:80)
- Portainer       -> http://localhost:9000
- Catalog API     -> http://localhost:5001
- Basket API      -> http://localhost:5002
- Discount API    -> http://localhost:5003
- Discount GRPC   -> http://localhost:5004
- Ordering API    -> http://localhost:5005
- MongoDB         -> localhost:27017
- Redis (basket)  -> localhost:6379
- Redis (catalog) -> localhost:6380
- PostgreSQL      -> localhost:5432
- MSSQL           -> localhost:1433
- RabbitMQ AMQP   -> localhost:5672
- RabbitMQ UI     -> http://localhost:15672

Notes:
- The script matches and stops `ssh` processes connecting to `nextcloudsg.ddns.net` (so it only stops related tunnels).
- If `ssh` is not found or fails to start, verify OpenSSH is installed and callable from your shell.
- You can also use `kubectl port-forward` for in-cluster port forwarding if you prefer.

## Running infra locally with Docker Compose

If you cannot or do not want to use the remote cluster, start the local infra with Docker Compose (recommended for local dev):

```bash
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d

# check containers
docker-compose ps

# stop
docker-compose -f docker-compose.yml -f docker-compose.override.yml down
```

When running locally with Docker Compose you can:
- Use `amqp://guest:guest@localhost:5672` for RabbitMQ
- Connect to SQL Server on the ports published by the compose files (see `docker-compose.override.yml`)

---

If you'd like, I can:
- Add a short `shell/README.md` describing `login.ps1` usage and examples, or
- Run `docker-compose` locally and verify connectivity for one service (requires Docker on your machine).

## Support

If you are having problems, please let me know by contacting me in [Linkedin](https://www.linkedin.com/in/phuoc-phan-47a3ab138/).

## License

This project is licensed with the [MIT license](LICENSE.txt).

## Local credentials

- **Database password:** Your_password123
- **PostgreSQL:** admin / admin1234
- **RabbitMQ:** guest / guest
