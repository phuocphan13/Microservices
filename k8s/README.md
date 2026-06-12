Kubernetes manifests (from docker-compose.yml)

Files:
- 00-namespace.yaml
- 01-microservices.yaml
- 02-databases.yaml
- 03-infra.yaml

Apply in order:

  kubectl apply --validate=false -f 00-namespace.yaml
  kubectl apply --validate=false -f 02-databases.yaml
  kubectl apply --validate=false -f 03-infra.yaml
  kubectl apply --validate=false -f 01-microservices.yaml

