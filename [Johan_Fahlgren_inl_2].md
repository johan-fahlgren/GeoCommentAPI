# [INL�MNINGSUPPGIFT WebAPI Del 2](https://github.com/johan-fahlgren/WebAPI_inlamnignsuppgift1-2)


## �r APIn?

#### Beskriv hur du l�ste att olika GeoComment versioner beh�vde f�ras in i databasen. Hade man kunnat l�sa det p� n�got annat s�tt?

* L�ste det genom att jobba med DTO's b�de f�r response och f�r request. Detta eftersom att formen p� datan som skulle sparas hade f�r�ndrats.
Genom att l�gga till den nya "Title" parametern som nullable i orginal modellen f�r Comment.cs kunde gamla och nya kommentarer kopplas ihop utan
att krocka med varandra i databasen.

* Man skulle kunna g� �t andra h�llet och anpassa Comment.cs modellen med det 

#### Beskriv ett annat s�tt du hade kunnat versionera i st�llet f�r att anv�nda en query parameter. P� vilket s�tt hade det varit b�ttre/s�mre f�r detta projekt?

* Headers.
* URI path
* Query parameter 

#### Ge exempel och f�rklaring p� n�r man vill ha beh�righetskontroll f�r en webbapi och n�r man inte vill ha det.

*

<br/>

> Johan Fahlgren, 2022-05-12
