#!/bin/bash
# Simple ArgoCD password reset using only host tools

NEW_PASSWORD="4BTAl-1tccPT2S63"

echo "=== Simple ArgoCD Password Reset ==="
echo ""

# Ensure htpasswd is installed
if ! command -v htpasswd &> /dev/null; then
    echo "Installing htpasswd..."
    sudo apt-get update -qq && sudo apt-get install -y apache2-utils
fi

echo "Password to set: $NEW_PASSWORD"
echo ""

# Generate bcrypt hash (ArgoCD uses $2a$ format)
echo "Generating bcrypt hash..."
RAW_HASH=$(htpasswd -nbBC 10 "" "$NEW_PASSWORD" | tr -d ':\n')
BCRYPT_HASH=${RAW_HASH/\$2y\$/\$2a\$}

echo "Generated hash: ${BCRYPT_HASH:0:40}..."
echo ""

# Base64 encode the hash for Kubernetes secret
ENCODED_HASH=$(echo -n "$BCRYPT_HASH" | base64 -w0)
echo "Base64 encoded: ${ENCODED_HASH:0:40}..."
echo ""

# Get current timestamp
MTIME=$(date -u +%FT%T%Z)
ENCODED_MTIME=$(echo -n "$MTIME" | base64 -w0)

echo "Updating argocd-secret..."
echo ""

# Update the secret using kubectl patch
sudo kubectl -n argocd patch secret argocd-secret --type='json' -p='[
  {"op": "replace", "path": "/data/admin.password", "value": "'$ENCODED_HASH'"},
  {"op": "add", "path": "/data/admin.passwordMtime", "value": "'$ENCODED_MTIME'"}
]'

if [ $? -eq 0 ]; then
    echo "✓ Secret updated successfully"
else
    echo "✗ Failed to update secret"
    exit 1
fi

echo ""
echo "Restarting ArgoCD server pod..."
sudo kubectl -n argocd delete pod -l app.kubernetes.io/name=argocd-server

echo "Waiting for new pod to be ready..."
sleep 3
sudo kubectl -n argocd wait --for=condition=ready pod -l app.kubernetes.io/name=argocd-server --timeout=90s

echo ""
echo "========================================="
echo "✓ Password reset complete!"
echo "========================================="
echo ""
echo "Login credentials:"
echo "  URL: https://localhost:8080"
echo "  Username: admin"
echo "  Password: $NEW_PASSWORD"
echo ""
echo "IMPORTANT: Clear your browser cache or use incognito mode!"
