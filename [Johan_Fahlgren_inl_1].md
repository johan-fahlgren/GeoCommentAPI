# [INLÄMNINGSUPPGIFT WebAPI](https://github.com/johan-fahlgren/WebAPI_inlamnignsuppgift1-2)


## Är APIn RESTful?


Denna API är väldigt liten och enkel, skulle nog inte kalla den RESTful. Men den är påbörjade 
med tänket kring RESTful och har grunderna för att kunna bli. 

Den har ett _Uniform Interface_ då den bara (i skrivande stund) använder en URI 
``/api/geo-comments`` för våra GET som hämtar data och POST som lägger till data med sina anrop. 
Sedan utnyttjas Queryparametrar om ett GET anrop ska begränsas på något sätt. 

Klienten känner bara till olika URIs till APIn och där uppfylles kravet med _Client-Server_ förhållandet. 

APIn är också _Statless_ då den inte sparar ner någon information om klienten vid anrop. 

Den har lager, eller är ett  _Layers system_. Nu är detta en liten API så det är inte så mycket, men 
exempelvis kan inte Comment ``Id`` påverkas via en POST när man skapar en ny kommentar, 
det går via en DTO.

Den är inte _Cacheable_ då den inte implemnterar några cache specifika metoder varken för servern
eller i headers för klienten att hantera caching.  

Denna API retunerar inte någon körbar kod i dagsläget och uppfyller inte kravet i _Code On Demand_ 
som dock är valfri. 

<br/>

> Johan Fahlgren, 2022-04-27
