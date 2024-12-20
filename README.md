# Legal Assistant RAG

Legal Assistant RAG is a comprehensive web application designed to assist legal professionals in managing and processing legal documents and information efficiently. This project integrates a robust backend developed in C# with a dynamic frontend implemented using Angular, ensuring seamless operation and user-friendly interfaces.  

## Table of Contents

- [Project Overview](#project-overview)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Installation Guide](#installation-guide)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Project Overview

The Legal Assistant RAG project aims to streamline legal workflows by providing tools for document processing, case monitoring, and client documentation. The application leverages modern web technologies like Angular or Tailwind to deliver a responsive and intuitive user experience, ensuring legal practitioners can focus on their core tasks without being bogged down by administrative overhead.

## Features

- **Document Management**: Efficiently upload, store, and retrieve legal documents.
- **Case Tracking**: Track the progress of legal cases with detailed status updates.
- **Client Monitoring**: Facilitate secure storing of legal clients.
- **User Authentication**: Secure login and user management.
- **Responsive Design**: Accessible from various devices, ensuring mobility and flexibility.

## Technologies Used

The project utilizes a diverse set of technologies to achieve its goals:

- **Frontend**: Angular 19, HTML, TailwindCSS, TypeScript
- **Backend**: .NET 8.0, C#
- **AI** : OpenAI-API with 4o-mini model for testing
- **Databases**: PostgreSQL, PgVector Plugin for vectorial DB
- **Containerization**: Docker and Docker-Compose

## Installation Guide

To set up the Legal Assistant RAG application locally, follow these steps:

### Prerequisites

Ensure you have the following installed on your system:

- Node.js (v22.12.0 LTS or later)
- Angular CLI (v19 or later)
- .NET Core SDK (v8.0 or later)
- Docker (v27 or later)
- Git

### Steps

1. **Clone the Repository**

   ```bash
   git clone https://github.com/PdotG/Legal-Assistant-RAG.git
   cd Legal-Assistant-RAG
   ```
2. **Backend Setup**

Navigate to the backend directory and restore the dependencies:

   ```bash
    cd backend
    dotnet build
   ```
Run the backend server:
   ```bash
    dotnet run
    Frontend Setup
   ```

3. **Frontend Setup**

Navigate to the frontend directory, install the dependencies, and start the Angular development server:

   ```bash
    cd ../frontend
    npm install
    ng serve
   ```
4. **Docker Setup (Optional)**

If you prefer to run the application in a Docker container first you need to change the environment variable to the URL of the backend:

```bash
API_URL=http://localhost:3000
   ```
Then you can build and run the Docker images using docker-compose:

```bash
docker-compose up --build
   ```

## Usage

Access the application by navigating to http://localhost:4200 in your web browser. You can log in using your credentials and start managing your legal documents and cases.

## Contributing
We welcome contributions from the community. To contribute, please fork the repository, create a new branch for your feature or bug fix, and submit a pull request for review.

## License
This project is licensed under the MIT License. See the LICENSE file for details.
