# [INLÄMNINGSUPPGIFT WebAPI Del 2](https://github.com/johan-fahlgren/WebAPI_inlamnignsuppgift1-2)


## Är APIn?

#### Beskriv hur du löste att olika GeoComment versioner behövde föras in i databasen. Hade man kunnat lösa det på något annat sätt?

* Löste det genom att jobba med DTO's både för response och för request. Detta eftersom att formen på datan som skulle sparas hade förändrats.
Genom att lägga till den nya "Title" parametern som nullable i orginal modellen för Comment.cs kunde gamla och nya kommentarer kopplas ihop utan
att krocka med varandra i databasen.

* Man skulle kunna gå åt andra hållet och anpassa Comment.cs modellen med det 

#### Beskriv ett annat sätt du hade kunnat versionera i stället för att använda en query parameter. På vilket sätt hade det varit bättre/sämre för detta projekt?

* Headers.
* URI path
* Query parameter 

#### Ge exempel och förklaring på när man vill ha behörighetskontroll för en webbapi och när man inte vill ha det.

*

<br/>

> Johan Fahlgren, 2022-05-12
