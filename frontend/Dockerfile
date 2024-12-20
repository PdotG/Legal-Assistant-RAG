# Build stage
FROM node:22-alpine as build-angular

# Set working directory
WORKDIR /build

# Copy package files
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy source code
COPY . .

ARG API_URL
ARG CONFIGURATION=production
ENV API_URL=$API_URL

# Build the Angular app with the variables
RUN npm run build -- --configuration=$CONFIGURATION --define apiUrl=\"$API_URL\" --define configuration=\"$CONFIGURATION\"

# Production stage
FROM nginx:alpine

# Copy built Angular app
COPY --from=build-angular /build/dist/frontend/browser /usr/share/nginx/html

# Copy nginx configuration
COPY nginx.conf /etc/nginx/nginx.conf

# Expose port 80
EXPOSE 80

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]