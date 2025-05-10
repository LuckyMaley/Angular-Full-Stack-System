# Angular-Full-Stack-System



## Description
Instant Order is an e-commerce system that offers customers quality clothes at low cost. Customers can navigate through various categories in the system to view different products catered to by Instant Order. Admins can manage products and customer orders.
The system will have a clear navigation menu for easy browsing through product categories. The system will have a fast and accurate search functionality, allowing customers to find products quickly. Moreover, the system will have filters and sorting options to refine search results based on price, brand, or category criteria.
The system will have well-organised product listings displaying key information such as product name, price, and availability. The system will include product ratings and reviews to help users make informed purchase decisions. Customers will be able to add items to their cart without navigating away from the product page, and they can add products to their Wishlist.
The system will integrate user accounts to store payment and shipping details for seamless checkout. Moreover, it will consist of a streamlined checkout process with minimal steps to complete an order. Customers and Admins will be able to view orders and manage shipping addresses from their account dashboard.


Refer to the [Requirements Documentation](/LLM_eCommerce_Requirement_Specification.docx) for a detailed explanation 

## Tools
![Static Badge](https://img.shields.io/badge/Visual%20Studio-2022%20or%20later-green) ![Static Badge](https://img.shields.io/badge/.Net%20Framework-6.0-blue) ![Static Badge](https://img.shields.io/badge/MSSQL-v18%20or%20later-red) ![Static Badge](https://img.shields.io/badge/VScode-1.97.0%20or%20later-yellow)

## Installation
The Angular Web App project depends on the Entity Framework (EF) Code 1st and Rest API projects to run effectively. Hence, the EF Code 1st project should be run first, and then add migrations and then update the database. This will create the database for the entities involved in the system. After the initial process the Rest API Project needs to be opened and run the migration commands in package manager console making sure to state the context to add migration to and making sure to database is updated with the migrations so that the identity database tables are created in order to enable registration and login functionality and to be able to access critical data. The Angular also needs to update its packages before running properly, make sure to run npm install on the terminal, so all the node modules are added to the project. Make sure to keep the Rest API running and open the Angular Web app now to use it.


### Steps

1. Go to the documents folder and then clone the project there (you can use git bash or any cmd):
```
cd Documents
```
```
git clone https://github.com/LuckyMaley/Angular-Full-Stack-System.git
```

2. Open [EF Code 1st project](/LLM_eCommerce_EFCODE1ST) on Visual Studio, and navigate to app.config in order to update the connection string
```
<connectionStrings>
  <add name="Model1" connectionString="data source="Enter yor db source";initial catalog=eCommerce_EFDB;integrated security=True;encrypt=True;trustservercertificate=True;MultipleActiveResultSets=True;App=EntityFramework" providerName="System.Data.SqlClient" />
</connectionStrings>
```
**Note**: Your Connection String might differ depending on whether you have a password or using a local version without a password.

3. Once the connection string is updated, go to the toolbar and look for tools >> Nuget Package Manager >> Package Manager Console and type below.
```
update-database
```
**Note**: This will populate the MSSQL database with the models. 

> In the case you update any of the models and you want to update the database again just do as follows on the Package Manager Console:
> ```
> add-migration 'Give the migration a name'
> ```
> Then press enter and once it has been updated, update the database:
> ```
> update-database
> ```

4. Now that the database is sorted, you can close this project.

5. Open up the [Rest Api project](/LLM_eCommerce_RESTAPI) on Visual Studio to configure it.

6. Go to appsettings.json file and configure the connection string for the two databases, one is for authorization and the other is for crud operations, see below.
```
"ConnectionStrings": {
  "IdentityConnection": "Server='your db source'; Database=eCommerce_IDENTITYDB; Trusted_Connection=True; MultipleActiveResultSets=True;",
  "CRUDConnection": "Server='your db source'; Database=eCommerce_EFDB; Trusted_Connection=True; MultipleActiveResultSets=True;"
}
```

7. Once the connection string is updated, go to the toolbar and look for tools >> Nuget Package Manager >> Package Manager Console and type below.
```
add-migration -Context AuthenticationContext 'initialDBCreate'
```

Then press enter and once it has been updated, update the database:

```
update-database -Context AuthenticationContext
```

Then press enter and once it has been updated, update the database:

```
update-database -Context LLM_eCommerce_EFDBContext
```
**Note**: This will populate the MSSQL database with the models from both contexts. 

> In the case you update any of the models and you want to update the database again just do as follows on the Package Manager Console:
> ```
> add-migration -Context 'Contextname' 'Give the migration a name'
> ```
> Then press enter and once it has been updated, update the database:
> ```
> update-database -Context 'name context you updated'
> ```
8. Run the application by clicking the green play button, if you are having an issue running just click the dropdown button next to the play button and run the api using IIS express.

9. Open the [Angular front end web application](/LLM-eCommerce-Ang) using VSCode. You can use the cmd if you like. Just right click the project folder and open in terminal and type the following:
```
code .
```

10. Once opened, go to the toolbar View >> Terminal >> New Terminal and type this:
```
npm install
```

11. After packages have installed, run the application through the terminal by typing:
```
ng serve -o
``` 
**Note**: The Rest API must always be running inorder to be able to properly use the front-end application.

**Note**: The Rest API link address might differ from what is in the [Global Constants typescript file](https://github.com/LuckyMaley/Angular-Full-Stack-System/blob/main/LLM-eCommerce-Ang/src/app/global-constants.ts) so you need to change that link address to make the Rest API's link address so that the front end can communicate with the api correctly

## Branching Strategy

We follow a structured branching strategy to keep the codebase organized:

- **main**: Stable, production-ready code.
- **dev**: Ongoing development. Feature branches are merged here first

- **feature/**: New features.
  - Example: `feature/add-user-authentication` or `feature/neo4j-implementation`
- **bugfix/**: Bug fixes.
  - Example: `bugfix/fix-chatbot-response`
- **chore/**: Maintenance tasks, documentation,or configurations.
  - Example: `chore/add-read-me`
- **Hotfix branches**: Urgent fixes to `main`.
   - Example: `hotfix/critical-bug-in-production`

### Notes:
- Use **kebab-case** (lowercase with hyphens) for branch names (e.g., `feature/angular-make-payment`).
- Branch names should be **descriptive** but concise.
- Avoid spaces, uppercase letters, or special characters.

## Development Workflow

1. **Create a New Branch**:
   ```bash
   git checkout -b feature/add-user-authentication
   ```

2. **Make Changes**:
   ```bash
   git add .
   git commit -m "Implement user authentication feature"
   ```

3. **Push Your Branch**:
   ```bash
   git push origin feature/add-user-authentication
   ```

4. **Sync with the `dev` Branch**:
   Before creating a merge request, ensure your branch is up-to-date with the latest changes from the `dev` branch:
   ```bash
   git pull origin dev
   ```
   If there are any merge conflicts, resolve them in your branch locally. Once resolved, commit the changes and push them back to your branch:
   
   ```bash
   git push origin feature/add-user-authentication
   ```

5. **Create a Merge Request**:
   - Open a pull request on GitHub targeting `dev` for code review.



## Sample data for User Login	
```	
	             Username	        Password	     Role
Customer	     Efronz	        Efron@123456	     Customer 
Seller	             cfarquarson0       corlissF@123	     Seller
Administrator	     ZzimelaAdmin	zimelaZ@1234	     Administrator
```

## Visuals
![Screenshot 2025-02-28 124743](https://github.com/user-attachments/assets/7d503b67-e80a-4e71-a000-c889b723db58)

![Screenshot 2025-02-28 132308](https://github.com/user-attachments/assets/7280b35d-e351-485b-b8ab-bf2eefc5bec1)

![Screenshot 2025-02-28 132332](https://github.com/user-attachments/assets/44023e96-8fa7-4449-a68d-bc6ce8683004)

![Screenshot 2025-02-28 132352](https://github.com/user-attachments/assets/7752da04-e91c-488e-baea-be2081824c79)

![Screenshot 2025-02-28 132434](https://github.com/user-attachments/assets/616e1343-4fc6-4d5f-a604-56fc5a59cab9)

![Screenshot 2025-02-28 132504](https://github.com/user-attachments/assets/86d89be8-390c-43d9-82e5-81162c51ae29)

![Screenshot 2025-02-28 132524](https://github.com/user-attachments/assets/7b10799f-1ae0-43ff-84ba-636861137f7e)

![Screenshot 2025-02-28 132622](https://github.com/user-attachments/assets/e76e55ab-55b5-44ed-9dbe-faea56e9c41f)

![Screenshot 2025-02-28 132644](https://github.com/user-attachments/assets/a74b20f0-a08b-4f6e-82fe-4eb4725318e7)

![Screenshot 2025-02-28 141606](https://github.com/user-attachments/assets/71167183-d2e1-4263-92fa-505fabb3fdf3)

![Screenshot 2025-02-28 141645](https://github.com/user-attachments/assets/4a39050b-c394-4f9b-a250-f21dec3ffbd3)

![Screenshot 2025-02-28 141708](https://github.com/user-attachments/assets/031134e1-d6c4-466f-a31c-8ffe6540a23a)






