# [INL�MNINGSUPPGIFT WebAPI Del 2](https://github.com/johan-fahlgren/WebAPI_inlamnignsuppgift1-2)




### Beskriv hur du l�ste att olika GeoComment versioner beh�vde f�ras in i databasen. Hade man kunnat l�sa det p� n�got annat s�tt?

* L�ste det genom att skapa med DTO's b�de f�r response och f�r request. Detta eftersom att formen p� den data som skulle sparas och retuneras hade f�r�ndrats.
Genom att l�gga till den nya "Title" parametern i orginal modellen f�r Comment.cs kunde gamla och nya kommentarer kopplas ihop utan
att krocka med varandra i databasen och mellan versionerna.

* Man skulle kunna g� �t andra h�llet och anpassa Comment.cs modellen med det nya formatet med en body class och sedan g�ra DTOs som g�r om det till den gamla modellen,
men gillar tanken av att h�lla kvar det s� n�r sitt original utf�rande som m�jligt och att all data hamnar i samma modell i databasen.  

<br/>

### Beskriv ett annat s�tt du hade kunnat versionera i st�llet f�r att anv�nda en query parameter. P� vilket s�tt hade det varit b�ttre/s�mre f�r detta projekt?

* Headers kan anv�ndas ist�llet f�r query parametrar n�r man vill anv�nda versionering i sin API. En f�rdel �r att det blir lite 
mindre r�rigt in ens URI, medan en nackdel �r kanske att du m�ste l�gga till en specifik header i sin request. Vet inte om det gjort n�gon j�tte stor skillnad 
f�r just detta projekt att byta till headers, det �r inte s�rskilt kr�ngligt att l�gga till egna headers i Postman men det skulle helt klart g�ra URIerna mindre r�riga.

<br/>

### Ge exempel och f�rklaring p� n�r man vill ha beh�righetskontroll f�r en webbapi och n�r man inte vill ha det.

* Ett tydligt exempel fr�n denna API �r i ``DeleteComment`` anropet. D�r vill man s�kerst�lla att inte fel person/user kan ta bort
 en kommentar fr�n n�gon annan i databasen. S� fort det handlar om privat data eller att usern beh�ver vara inloggad �r detta viktigt
 att implementerar s� att inte fel person kan f� tag p� privata uppgifter eller kan persoifierar anv�ndaren.     

* Medan om anropet bara ska h�mta ok�nslig information som en artikel, offentliga kommentarer eller liknande kr�vs inte n�gon beh�righet.
Ett exempel fr�n APIn skulle kunna vara ``FindAllGeoComments`` d�r det retuneras en lista p� alla kommentarer inom ett omr�de. Men detta �r just 
ett exempel f�r denna applikation, en annan kanske skulle kr�va att man �r inloggad f�r att anv�nda applikationen. 

<br/>

> Johan Fahlgren, 2022-05-12
