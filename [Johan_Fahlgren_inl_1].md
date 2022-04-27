# [INL�MNINGSUPPGIFT WebAPI](https://github.com/johan-fahlgren/WebAPI_inlamnignsuppgift1-2)


## �r APIn RESTful?


Denna API �r v�ldigt liten och enkel, skulle nog inte kalla den RESTful. Men den �r p�b�rjade 
med t�nket kring RESTful och har grunderna f�r att kunna bli. 

Den har ett _Uniform Interface_ d� den bara (i skrivande stund) anv�nder en URI 
``/api/geo-comments`` f�r v�ra GET som h�mtar data och POST som l�gger till data med sina anrop. 
Sedan utnyttjas Queryparametrar om ett GET anrop ska begr�nsas p� n�got s�tt. 

Klienten k�nner bara till olika URIs till APIn och d�r uppfylles kravet med _Client-Server_ f�rh�llandet. 

APIn �r ocks� _Statless_ d� den inte sparar ner n�gon information om klienten vid anrop. 

Den har lager, eller �r ett  _Layers system_. Nu �r detta en liten API s� det �r inte s� mycket, men 
exempelvis kan inte Comment ``Id`` p�verkas via en POST n�r man skapar en ny kommentar, 
det g�r via en DTO.

Den �r inte _Cacheable_ d� den inte implemnterar n�gra cache specifika metoder varken f�r servern
eller i headers f�r klienten att hantera caching.  

Denna API retunerar inte n�gon k�rbar kod i dagsl�get och uppfyller inte kravet i _Code On Demand_ 
som dock �r valfri. 

<br/>

> Johan Fahlgren, 2022-04-27
