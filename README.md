# Angular-Full-Stack-System



## Description
Instant Order is an e-commerce system that offers customers quality clothes at low cost. Customers can navigate through various categories in the system to view different products catered to by Instant Order. Admins can manage products and customer orders.
The system will have a clear navigation menu for easy browsing through product categories. The system will have a fast and accurate search functionality, allowing customers to find products quickly. Moreover, the system will have filters and sorting options to refine search results based on price, brand, or category criteria.
The system will have well-organised product listings displaying key information such as product name, price, and availability. The system will include product ratings and reviews to help users make informed purchase decisions. Customers will be able to add items to their cart without navigating away from the product page, and they can add products to their Wishlist.
The system will integrate user accounts to store payment and shipping details for seamless checkout. Moreover, it will consist of a streamlined checkout process with minimal steps to complete an order. Customers and Admins will be able to view orders and manage shipping addresses from their account dashboard.

## Badges
![Static Badge](https://img.shields.io/badge/Visual%20Studio-2022%20or%20later-green) ![Static Badge](https://img.shields.io/badge/.Net%20Framework-6.0-blue)

## Installation
The Angular Web App project depends on the Entity Framework (EF) Code 1st and Rest API projects to run effectively. Hence, the EF Code 1st project should be run first, and then add migrations and then update the database. This will create the database for the entities involved in the system. After the initial process the Rest API Project needs to be opened and run the migration commands in package manager console making sure to state the context to add migration to and making sure to database is updated with the migrations so that the identity database tables are created in order to enable registration and login functionality and to be able to access critical data. The Angular also needs to update its packages before running properly, make sure to run npm install on the terminal, so all the node modules are added to the project. Make sure to keep the Rest API running and open the Angular Web app now to use it.
```
Sample data for User Login		
	             Username	      Password	     Role
Customer	     Efronz	        Efron@123456	 Customer 
Seller	       cfarquarson0	  corlissF@123	 Seller
Administrator	 ZzimelaAdmin	  zimelaZ@1234	 Administrator
```

## Visuals
Use examples liberally, and show the expected output if you can. It's helpful to have inline the smallest example of usage that you can demonstrate, while providing links to more sophisticated examples if they are too long to reasonably include in the README.


