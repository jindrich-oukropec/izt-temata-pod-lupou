# Inovace Zpravodajskéko Storytellingu - Témata pod Lupou

## Nástroje

- IDE, vyber jeden z:
  - Visual Studio 2019 Community Edition, behem instalace vybrat
    - ASP.NET and web development workload
  - JetBrains Rider
  - VS Code (nezkouseno, budou potreba nejake plugins)

## Instalace

1) Otevri slozku src\TemataPodLupou.Web\App_Data
1) Vytvor kopii souboru UmbracoSeed.sdf jmenem Umbraco.sdf
1) Otevri solution (src\TemataPodLupou.sln) ve svem oblibenem IDE
1) Spust projekt TemataPodLupou.Web
1) Otevre se prohlizec s URL webu, napr https://localhost:44367
1) Prihlas se do backoffice na `/umbraco`
    - Uzivatel: `admin@tematapodlupou.cz`
    - Heslo: `H78WFWRUdyFUVMb0IFBW`
1) V sekci Users si vytvor vlastniho uzivatele
1) Prihlas se pod vlastim uzivatelskym uctem
1) V sekci Settings klikni na `uSync` a pak na tlacitko `Import`

## Testovací obsah

1) V sekci Media vytvor slozku a pojmenuj ji Uploads
1) V sekci Content vytvor obsah typu "Media", ktery reprezentuje redakci
    1) Pro ulozeni je potreba kliknout na "Save and Publish"
1) Pod redakci vytvor obsah typu "Submission Form"
    1) Vypln vsechny pole, v poli "Media Store Folder" vyber predem vytvorenou slozku Uploads
    1) Klikni na Save and Publish
1) Zobraz formular v prohlizeci
    1) Po ulozeni se preklinki na zalozku "Info" a klikni na odkaz v boxu Links
1) Vypln formular a odesli
1) Odeslana data se zobrazi v sekci Content pod formularem, ktery byl odeslan
    1) Muze byt nutne zmacknout na formular pravym tlacitkem a vybrat "Reload" 
