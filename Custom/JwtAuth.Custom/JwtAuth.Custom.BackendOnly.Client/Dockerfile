# Step 1: Use a lightweight node image
FROM node:16-alpine 

# Set the working directory inside the container
WORKDIR /app

# Copy the entire project into the container
COPY . .

# Install dependencies (faster and more reproducible with npm ci)
RUN npm ci

# Expose the port for the React app (default is 3000)
EXPOSE 3000

# Use npm start to run the React development server with hot reloading
CMD ["npm", "start"]
