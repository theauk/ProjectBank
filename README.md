# ProjectBank

## About this project

### Scope

The scope of this project has been to implement a service where a University has the
ability to personalize the filtering experience of projects.
This service has three different roles, an "Admin", a "Supervisor", and a normal user (i.e a student).
The admin has the ability to manage the different TagGroups of the university and create projects.
The supervisor has the ability to create projects. Lastly, all users have the ability to filter projects.

### Out of scope

The business logic and our client interpretation is that this service will be sold to many universities - not just one. We have therefore implemented a fourth role named "SuperAdmin", which would be in charge of adding new universities. This role would belong to the client who owns Project Bank and sells the service.

This was implemented in order for us to ensure that e-mails of different universities would not be shown the same content, and that the project could be extended without the need of restructuring.

### Partially Implemented Features

#### 1. Taggroup tag limit

When creating a taggroup the "Admin" has the choice of selecting a limit to the number of tags a "Supervisor" can add to a project in that taggroup.
When creating a project this limit on taggroup tags is not enforced.

#### 2. Manage Projects

Due to time constraints, we decided not to include the ability to edit og delete projects in the front-end structure, even though these features have been implemented from ProjectController to the database.

This would entail having a Page called "Manage Projects" where projects matching the e-mail of the user would have been shown.
Here, the user would have the ability to edit and delete their projects.

#### 3. SuperAdmin interface

An interface for the SuperAdmin features would have needed been needed to fully utilize the role, even though we have from the Controller level down to database implemented features specifically dedicated to this role. For example the ability to create a new University.  

#### 4. Taggroup required - "Feature"

At the current state of the program, an Admin can create a required taggroup with no tags, which leads to a deadlock when creating Projects.
We should have implemented a check for this in the front-end and checked for it in the TagGroup repository.

### Your role

Paolo and Rasmus have with their ITU email been added to Azure with an "Admin" authorization.
Our program will check the ending of your email and show you seeded data corresponding to that email ending, as this is connected to a University.

You will have the rights to create, edit, and delete taggroups, along with creating and filtering projects.  

---

## Running the program

### 1. Before you run

Before you can run Project Bank ensure the following three things are setup on your local machine.

---

### 1.1 Ensure Docker is installed and running

You can install Docker from: [https://www.docker.com/get-started](https://www.docker.com/get-started)

When installed start "Docker Desktop" and wait for the Docker daemon to start.

Once succesfully started go to the next section.

---

### 1.2 Ensure Certificate is present

Ensure that you have created a valid developer certificate.

This can be done by either running the script:

>``create-certificate.ps1`` in the directory ``.\scripts``.

If this doesn't work, it can be done manually by opening a terminal and enter the following commands in PowerShell.

Remeber to accept all prompts!

>1 ``dotnet dev-certs https --clean``
>
>2 Windows: ``dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p localhost``
>
>2 Mac: ``dotnet dev-certs https -ep $env:HOME/.aspnet/https/aspnetapp.pfx -p localhost``
>
>3 ``dotnet dev-certs https --trust``

---

### 1.3 Ensure .local folder is present with contents

In order to run the program you will have to create some local files in the project directory.

Create a folder called ``.local``

Now create the following files in the ``.local`` directory:

1. > ``connection_string.txt``

2. > ``db_password.txt``

Remember to fill in the respective files with the content needed. (It will be provided in the exam submission on LearnIt)

---

## 2. Run Project

Now you should be able to run the project.
Running it can be done in different ways which will be listed in the following sections.

The program has a script called ``run.ps1``, which can handle it all for you. Read the section [Run wih script](#run-with-script).

However, if you want to do it manually open a terminal in the project root directory and enter the following command:

>``docker-compose -f docker-compose.prod.yml up``

When the program has succesfully booted up, open a webbrowser and connect to either:
>``localhost:5077``

Or

>``localhost:7207``

### Run with script

For an easy startup you can run the program with a PowerShell script. The script supports startup via "docker-compose up" and "dotnet run" in case of failure.

#### With Docker

To start the program with the script using Docker-compose open a terminal in the root directory of the project and enter:

>``./run.ps1 -docker $true``

When the program has succesfully booted up, open a webbrowser and connect to either:
>``localhost:5077``

Or

>``localhost:7207``

#### With dotnet run

To start the program with the script using dotnet run open a terminal in the root directory of the project and enter:

>``./run.ps1``

When the program has succesfully booted up, open a webbrowser and connect to either:
>``localhost:7061``

Or

>``localhost:5073``

---
