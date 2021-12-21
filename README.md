# ProjectBank
## About this project
### Scope
The scope of this project has been to implement a service where a University has the
ability to personalize the filtering experience for projects available to study. 
This service has three different roles, an "Admin", "Supervisor" and a normal user (i.e a student).
The Admin has the ability to manage the different taggroups of the university and create projects. 
The supervisor has the ability to create projects. Lastly all users have the ability to filter projects.

### Out of scope
The business logic and our client interpretation is that this service will be sold to many 
universities and not just one. We have therefore implemented a fourth role named "SuperAdmin", which would be in charge of 
adding new universities. This role would belong to the client who owns Project Bank, and sells the service. 
This was implemented in order for us to ensure that emails of different universities would not be shown the same content, and that the project could be extended without the need of restructuring. 

### Partially Implemented Features
##### 1. Taggroup tag limit
When creating a taggroup the "Admin" has the choice of choosing a limit of maximum tags a "Supervisor" can add to a project in that taggroup. 
When creating a project this limit on taggroup tags is not enforced.

##### 2. Manage Projects
Due to time constraints we decided not to include the ability to edit og delete projects in the front end structure even though these features have been implemented from ProjectController to the database. 
This would entail having a Page called "Manage Projects" where projects matching the email of the user would have been shown.
Here the user would have the ability to edit and delete their projects. 

##### 3. SuperAdmin interface
An interface for the SuperAdmin features would have needed been needed to fully utilize the role, even though we have from the Controller level down to database implemented features specifically dedicated to this role. For example the ability to create a new University.  

### Your role
Paolo and Rasmus have with their ITU email been added to Azure with an "Admin" authorization.
Our program will check the ending of your email and show you seeded data corresponding to that email ending, as this is connected to a University. 
You will have the rights to create, edit and delete taggroups, along with creating and filtering projects.  

# Running the program:
## 1. Before you run

Before you can run ProjectBank ensure the following three things are setup on your machine.

---

### 1.1 Ensure Docker is installed and running

You can install Docker from: [https://www.docker.com/get-started](https://www.docker.com/get-started)

When installed start "Docker Desktop".

---

### 1.2 Ensure Certificate is present

Ensure that you have created a valid developer certificate. This is done by running the following script in PowerShell in the directory ``.\scripts`` :

>``create-certificate.ps1``

---

### 1.3 Ensure .local folder with contents

In order to run the program you will have to create some local files in the project directory.

Create a folder called ``.local``

Now create the following files in the ``.local`` directory:

1. > ``connection_string.txt``

2. > ``db_password.txt``

Remember to fill in the respective files with the content needed. (It will be provided in a seperate file XXX)

---

## 2. Run Project

Run the project with the following script in PowerShell:

>``run.ps1``

This will run the project in developer mode and build the project in a Docker container.

To run the project in production mode write:

>``run.ps1 -production $true``

When the project is up you will be shown the message:

> ``projectbank_db_1       docker-entrypoint.sh postgres   Up      0.0.0.0:5433->5432/tcp``

> ``projectbank_server_1   dotnet ProjectBank.Server.dll   Up      0.0.0.0:7207->443/tcp, 0.0.0.0:5077->80/tcp``

---

### Connect to ProjectBank

To connect to the ProjectBank open a webbrowser and write the following in the address field:

> ``localhost:7207``

If you are met with an error message saying e.g. the site can't be reached try writing:

> ``localhost:5077``

---
