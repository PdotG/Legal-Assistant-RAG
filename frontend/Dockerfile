# We use Node LTS version
FROM node:22-alpine as build-angular

# We set the working directory
WORKDIR /build

# We copy the package.json and package-lock.json files to build them with npm later
COPY package*.json ./

# Install the dependencies
RUN npm install

# We copy the src/ code to the container
COPY . .

# We verify dependencies and build the Angular app
RUN npm install
RUN npm run build

# We use Caddy Reverse Proxy to launch Angular to production
FROM caddy:latest


WORKDIR /app

# We copy our project to the ./www directory of Caddy
COPY --from=build-angular /build/dist/frontend/browser ./www

# Copy the Caddyfile configuration to the container
COPY Caddyfile /etc/caddy/Caddyfile


EXPOSE 80

# We start Caddy with this configuration
CMD ["caddy", "run", "--config", "/etc/caddy/Caddyfile"]