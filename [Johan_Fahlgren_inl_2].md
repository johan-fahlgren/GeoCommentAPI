# [INLÄMNINGSUPPGIFT WebAPI Del 2](https://github.com/johan-fahlgren/WebAPI_inlamnignsuppgift1-2)




### Beskriv hur du löste att olika GeoComment versioner behövde föras in i databasen. Hade man kunnat lösa det på något annat sätt?

* Löste det genom att skapa med DTO's både för response och för request. Detta eftersom att formen på den data som skulle sparas och retuneras hade förändrats.
Genom att lägga till den nya "Title" parametern i orginal modellen för Comment.cs kunde gamla och nya kommentarer kopplas ihop utan
att krocka med varandra i databasen och mellan versionerna.

* Man skulle kunna gå åt andra hållet och anpassa Comment.cs modellen med det nya formatet med en body class och sedan göra DTOs som gör om det till den gamla modellen,
men gillar tanken av att hålla kvar det så när sitt original utförande som möjligt och att all data hamnar i samma modell i databasen.  

<br/>

### Beskriv ett annat sätt du hade kunnat versionera i stället för att använda en query parameter. På vilket sätt hade det varit bättre/sämre för detta projekt?

* Headers kan användas istället för query parametrar när man vill använda versionering i sin API. En fördel är att det blir lite 
mindre rörigt in ens URI, medan en nackdel är kanske att du måste lägga till en specifik header i sin request. Vet inte om det gjort någon jätte stor skillnad 
för just detta projekt att byta till headers, det är inte särskilt krångligt att lägga till egna headers i Postman men det skulle helt klart göra URIerna mindre röriga.

<br/>

### Ge exempel och förklaring på när man vill ha behörighetskontroll för en webbapi och när man inte vill ha det.

* Ett tydligt exempel från denna API är i ``DeleteComment`` anropet. Där vill man säkerställa att inte fel person/user kan ta bort
 en kommentar från någon annan i databasen. Så fort det handlar om privat data eller att usern behöver vara inloggad är detta viktigt
 att implementerar så att inte fel person kan få tag på privata uppgifter eller kan persoifierar användaren.     

* Medan om anropet bara ska hämta okänslig information som en artikel, offentliga kommentarer eller liknande krävs inte någon behörighet.
Ett exempel från APIn skulle kunna vara ``FindAllGeoComments`` där det retuneras en lista på alla kommentarer inom ett område. Men detta är just 
ett exempel för denna applikation, en annan kanske skulle kräva att man är inloggad för att använda applikationen. 

<br/>

> Johan Fahlgren, 2022-05-12
