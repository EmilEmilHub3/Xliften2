Xliften | Video Streaming System

Xliften is a distributed video streaming application developed as part of a school assignment.
The purpose of the system is to demonstrate how modern streaming platforms handle authentication, media storage, binary streaming, and separation of concerns using a service-based architecture.


The system consists of three services:

A .NET Minimal API

A MongoDB database with GridFS

A static HTML/JavaScript frontend hosted on Nginx

All services run together using Docker Compose, allowing the entire project to start with a single command.




The project was designed to show:

How to store and stream large media files using MongoDB GridFS

How to secure an API using JWT authentication

How to build a simple client that communicates with a protected API

How Docker Compose can be used to orchestrate multiple dependent services

How to expose a clean and testable API with Swagger documentation



Features:

User login with JWT token generation

Secured API endpoints

Video metadata retrieval

Video streaming directly from GridFS

Frontend that allows login, listing videos, and streaming them

Postman test collection for validating endpoints and response times



Technologies Used:

.NET Minimal API

MongoDB with GridFS

Nginx for hosting static frontend and proxying API calls

Docker and Docker Compose

Vanilla JavaScript with HTML/CSS

Postman for API testing



Requirements to run the project:

Docker Desktop must be installed

No other installations (such as MongoDB, .NET SDK, or Node.js) are required.

How to Run the System

Open the project folder.

Run the following command in the root of the project:

docker compose up --build

This will start (Service/port/description):

MongoDB / 27017 / Database storage
API / 5000 / Swagger and backend API
Frontend/ 3000 / User interface

Port mappings correspond to those defined in docker-compose.yml.

Accessing the Application
Frontend

Open:

http://localhost:3000

The frontend allows the user to:

Log in

Load a list of available videos

Play videos through a Blob-based streaming solution

API and Swagger

Open:

http://localhost:5000/swagger

Endpoints include:

POST /login

GET /videos

GET /video/{id}



Default Login Credentials

The system seeds a default user automatically when the database is empty:

Username: admin
Password: admin




How Video Streaming Works

The client requests a video through the endpoint /api/video/{id} while including a valid JWT token.

The API reads the video from MongoDB GridFS as a stream.

The frontend converts the response into a Blob and generates a temporary URL.

This URL is inserted into the <video> element, allowing the browser to play the video.

This approach bypasses the limitation that the <video> tag cannot send authorization headers directly.




Postman Testing

The project includes a Postman collection for testing:

Login

Accessing protected endpoints

Retrieving metadata

Streaming video content

Validating status codes and performance

API KEY FOR POSTMAN: https://api.postman.com/collections/49455240-b332a679-bd02-454a-b6df-cc54055d0455?access_key=PMAT-01KAHM5DF5CGG6T37XN10EYZXE

