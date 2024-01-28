# Stage 1: Build the Angular app
FROM node:18.13.0 AS builder

WORKDIR /usr/src/app

COPY package*.json ./

# Clean dependencies
RUN npm clean
COPY . .

# Install dependencies
RUN npm install
COPY . .

RUN npm install -g @angular/cli@latest
COPY . .

# Run Angular build
RUN ng build  

# Stage 2: Create a minimal Nginx image with the built Angular app
FROM nginx:1.21-alpine

COPY --from=builder /usr/src/app/dist /usr/share/nginx/html

# Copy the custom Nginx configuration
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expose the port that Nginx will run on
EXPOSE 80

