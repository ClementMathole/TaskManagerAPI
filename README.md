# CloudBased Task Management API

## Overview
The CloudBased Task Management API is a RESTful service designed to manage tasks efficiently. It provides endpoints to create, read, update, and delete tasks, along with user authentication and authorization.

## Features
- User Registration and Authentication
- Create, Read, Update, and Delete Tasks
- Task Assignment to Users
- Task Status Tracking
- Due Date Management
- Priority Levels

## Technologies Used
- **Programming Language:** C#
- **Framework:** ASP.NET Core
- **Database:** SQL Server
- **Authentication:** JWT

## Getting Started

### Prerequisites
- .NET 6.0 SDK
- SQLServer

### Installation

1. **Clone the repository:**
   ```sh
   git clone https://github.com/ClementMathole/CloudBased_TaskManagementAPI.git
   cd CloudBased_TaskManagementAPI
   ```

2. **Restore dependencies:**
   ```sh
   dotnet restore
   ```

3. **Run database migrations:**
   ```sh
   dotnet ef database update
   ```

4. **Start the development server:**
   ```sh
   dotnet run
   ```

## API Endpoints

### Authentication
- **POST /register:** Register a new user
- **POST /login:** Authenticate a user and return a token

### Tasks
- **GET /tasks:** Retrieve all tasks
- **POST /tasks:** Create a new task
- **GET /tasks/{id}:** Retrieve a specific task
- **PUT /tasks/{id}:** Update a specific task
- **DELETE /tasks/{id}:** Delete a specific task

## Contact
For any questions, please contact (clementmathole003@gmail.com).
