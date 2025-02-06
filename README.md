# Store Backend - .NET Project

## Overview
This is a **.NET-based Store Backend** providing RESTful API endpoints for managing products, baskets, orders, payments, authentication, and caching. It includes key e-commerce functionalities such as product sorting, filtering, searching, pagination, and secure transactions.

## Features
### **Modules**
- **Products**: Sorting, Filtering, Searching, Pagination
- **Error Handling**: Centralized exception handling
- **Baskets**: Manage user shopping carts
- **Caching**: Redis caching for performance optimization
- **Accounts**: Identity-based authentication with JWT security
- **Orders**: Order management system
- **Payments**: Stripe integration for payment processing
  
### **Design Patterns Used**
- **Specifications Pattern**: Flexible querying and filtering
- **Unit of Work**
- **Generic Repository**

## Technologies Used
- **Backend:** .NET Web API
- **Database:** SQL Server, Entity Framework Core (EF Core)
- **Caching:** Redis
- **Security:** JWT Authentication (JSON Web Tokens)
- **Mapping:** AutoMapper
- **Payments:** Stripe API for handling transactions
- **Testing:** Postman collection with all API endpoints

## Installation and Setup
### **1. Clone the Repository**
```bash
git clone https://github.com/your-username/your-repo-name.git
```

### **2. Configure the Environment**
Update the Connection String in appsettings.json to match your SQL Server database.<br>
Configure Redis and Stripe API keys in appsettings.json.

### 3.Run the Application


## Usage Instructions
### Postman Collection (API Testing)
**A Postman collection is included with all API endpoints for easy testing:**

1. Import the PostmanCollection.json file into Postman.<br>
2. Set up the environment variables for JWT tokens and API URLs.<br>
3. Run requests directly to test the API functionality.<br>



