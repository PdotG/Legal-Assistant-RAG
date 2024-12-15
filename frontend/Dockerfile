# Stage 1: Build Angular app
FROM node:lts-alpine AS build

# Set working directory
WORKDIR /app

# Copy package files and install dependencies
COPY package.json package-lock.json ./
RUN npm install

# Copy source code
COPY . .

# Define build arguments and use them during the build
ARG API_KEY
ARG CONFIGURATION=production
ENV API_KEY=$API_KEY
ENV CONFIGURATION=$CONFIGURATION

RUN npm run build -- --configuration=$CONFIGURATION --define apiKey=\"$API_KEY\" --define configuration=\"$CONFIGURATION\"

# Stage 2: Serve the app using Nginx
FROM nginx:alpine
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80