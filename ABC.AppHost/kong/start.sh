#!/bin/sh
set -e

echo "Running Kong migrations..."
kong migrations bootstrap 2>/dev/null || true
kong migrations up

CORS_ORIGIN="${ALLOWED_ORIGINS:-*}"

# Generate declarative config from environment variables
cat > /tmp/kong.yml <<EOF
_format_version: "3.0"
_transform: true

services:
  - name: management-api
    url: ${UPSTREAM_URL}
    routes:
      - name: api-route
        paths:
          - /
        strip_path: false
    plugins:
      - name: key-auth
        config:
          hide_credentials: true
      - name: cors
        config:
          origins:
            - "${CORS_ORIGIN}"
          methods:
            - GET
            - POST
            - OPTIONS
          headers:
            - Authorization
            - Content-Type
            - apikey
          credentials: true
          max_age: 3600

consumers:
  - username: abc-react
    keyauth_credentials:
      - key: ${REACT_API_KEY}
EOF

echo "Importing Kong configuration..."
kong config db_import /tmp/kong.yml

echo "Starting Kong..."
export KONG_NGINX_DAEMON=off
PREFIX=${KONG_PREFIX:=/usr/local/kong}
kong prepare -p "$PREFIX"
ln -sfn /dev/stdout $PREFIX/logs/access.log
ln -sfn /dev/stdout $PREFIX/logs/admin_access.log
ln -sfn /dev/stderr $PREFIX/logs/error.log
exec /usr/local/openresty/nginx/sbin/nginx -p "$PREFIX" -c nginx.conf
