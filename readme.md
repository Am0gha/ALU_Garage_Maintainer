# v1.0.1



Shifted the logic to fetch the cars belonging to a certain class from backend to frontend. Since the page receives the list of all vehicles stored in the database the moment it is loaded (page) no need to write an API just to query the database for a list of cars belonging to a specific class. Instead we keep a copy of the original and filter the data / response received to view the desirable data.



* Created a function in angular where in it would filter the car database based on the class.
* Implemented a search bar for users to search for a car in both the cases
- When the entire list of cars is shown
- When a list of cars belonging to a specific class is shown 



# v1.0.0



This version of the app contains 2 pages:



1. To add a car to the database
2. To list all the cars present in the database.



Previously I had generated an unhealthy amount of API calls that would query the database for each and every atomic needs, and now I'm trying to minimise the number of queries the DB has to execute and still be able to provide the user with information in any way or shape or form.



The current condition / status of the app would be considered as the base application, over which improvements shall be made.

