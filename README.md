Xliften | Video Streaming System

Xliften is a distributed video streaming system developed as part of a school assignment. The purpose of the project is to demonstrate how modern streaming platforms handle authentication, media storage, binary streaming, and separation of concerns using a service-based architecture.

The system consists of three services:

A .NET Minimal API

A MongoDB database using GridFS

A static HTML/JavaScript frontend hosted on Nginx

All services run together using Docker Compose, allowing the entire system to start with a single command.

The project demonstrates:

How to store and stream large media files with MongoDB GridFS

How to secure an API using JWT authentication

How to build a simple client that communicates with a protected backend

How Docker Compose can orchestrate multiple dependent services

How to expose a clean and testable API using Swagger

Main features:

User login with JWT token generation

Secured API endpoints

Video metadata retrieval

Video streaming directly from GridFS

A frontend that supports login, video listing, and video playback

A Postman test collection for verifying endpoints and response times

Technologies used:

.NET Minimal API

MongoDB with GridFS

Nginx for static hosting

Docker and Docker Compose

Vanilla JavaScript with HTML and CSS

Postman for API testing

Requirements:

Docker Desktop must be installed

No other installations are required (no .NET SDK, MongoDB, or Node.js)

How to run the system:

Open the project folder.

Run the following command in the root directory:
docker compose up --build

This will start the following services (service / port / description):

MongoDB on port 27017 for database storage

API on port 5000 for Swagger and backend endpoints

Frontend on port 3000 for the user interface

The port mappings correspond to the configuration in docker-compose.yml.

Accessing the frontend:
Open the following URL in a browser:
http://localhost:3000

The frontend allows the user to:

Log in

Load a list of available videos

Play videos using a Blob-based streaming method

Accessing the API and Swagger:
Open the following URL:
http://localhost:5000/swagger

Available API endpoints include:

POST /login

GET /videos

GET /video/{id}

Default login credentials:
The system automatically seeds a default user when the database is empty.
Username: admin
Password: admin

How video streaming works:
The client sends a request to /api/video/{id} and includes a valid JWT token.
The API streams the video directly from MongoDB GridFS.
The frontend receives the stream as binary data, converts it into a Blob, and generates a temporary URL.
This URL is then inserted into the HTML video element.
This approach bypasses the limitation that the video tag cannot send Authorization headers.

Postman testing:
The project includes a Postman collection that allows testing of:

Login

Protected endpoints

Metadata retrieval

Video streaming

Status code validation and performance

Postman API collection link (with API key):
https://api.postman.com/collections/49455240-b332a679-bd02-454a-b6df-cc54055d0455?access_key=PMAT-01KAHM5DF5CGG6T37XN10EYZXE
