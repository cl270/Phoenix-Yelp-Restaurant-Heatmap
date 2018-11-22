# Phoenix, Arizone Yelp dataset Restaurant Heatmap
The final project for CompSci 216 at Duke University, this is a C# based heatmap creator of different metrics based on the Yelp dataset in SQL.

![Heatmap 1](heatmap1.PNG?raw=true "Intial GUI")
![Heatmap 2](heatmap2.PNG?raw=true "After data is processed")

## Rationale 
Opening a restaurant can be a risky enterprise, from the amount of initial capital needed, to competition.

This project was an attempt to quantify the ideal location to place a restaurant in a US city (in the current case Phoenix, AZ) based on both the density of restaurants in an area (competition), and the number of Yelp reviews for restaurants in the area (potential customer base). 

Finally, the average number of stars given by reviews of other restaurants in the area was also taken into account, representing consumer satisfaction with their choices in the area - the lower the average review, the more likely consumers would be willing to try a new option.

The data were queried from a locally run SQL Server (via SQL Server Management Studio) after the data was imported directly from the Yelp Dataset and filtered by latitude and longitude with a SQL query similar to 
"SELECT * from yelpData WHERE (latitude BETWEEN phoenixwestlatitude AND phoenixeastlatitude) AND (longitude BETWEEN phoenixnorthsouth);"

## Use

The program gives the user the option to weight these three influencing factors. The restaurant's location is entered via a latitude longitude coordinate pair.

Depending on the power of the user's CPU and time available, the user can also increase the resolution of the final heatmap, for more detailed results.
