# ProjectBank

## How to run

To run the ProjectBank Application do the following:

---

### Ensure Docker is installed

You can install Docker from: [https://www.docker.com/get-started](https://www.docker.com/get-started)

---

### Ensure Certificate is present

First ensure that you have created a valid developer certificate. This is done by running the following script in PowerShell in the directory ``.\scripts`` :

>``create-certificate.ps1``

---

### Ensure .local folder contents

In order to run the program you will have to create some local files in the project directory.

Create a folder: ``.local``

Now create the following files in the ``.local`` directory:

1. > ``connection_string.txt``

2. > ``db_password.txt``

Remember to fill in the respective files with the content needed. (It will be provided in a seperate file XXX)

### Run Project

Run the project with the following script in PowerShell:

>``run.ps1``

This will run the project in developer mode and build the project in a Docker container.

To run the project in production mode write:

>``run.ps1 -production $true``

When the project is up will be the message:

> ``projectbank_db_1       docker-entrypoint.sh postgres   Up      0.0.0.0:5433->5432/tcp``

> ``projectbank_server_1   dotnet ProjectBank.Server.dll   Up      0.0.0.0:7207->443/tcp, 0.0.0.0:5077->80/tcp``

---

### Connect to ProjectBank

To connect to the ProjectBank open a webbrowser and write the following in the address field:

> ``localhost:7207``

If you are met with an error message saying e.g. the site can't be reached try writing:

> ``localhost:5077``

---
