Repozytorium Signals - No Unit Tests
===========================================

Metodyka
--------------
Pracując w metodyce NUT pisze się jedynie kod produkcyjny, bez testów jednostkowych.

Kodu produkcyjnego pisze się tyle ile programista uważa,
że jest konieczne, by rozszerzyć funkcjonalność programu o wybraną funckję.

Przypominamy, że aby eksperyment się udał, prosimy o trzymanie się skrupulatnie
tej metodyki w tym repozytorium.

Schemat pracy
--------------
Prosimy o standardowy schemat komitowania:

 1. Wybierz zadanie do implementacji.
 2. Napisz kod produkcyjny realizujący to zadanie
        (zawierający ew. poprawki w istniejącym kodzie, wprowadzone 
         na potrzeby zaimplementowania nowej funkcji).
 3. Zakomituj kod produkcyjny (opis wynikający z nazwy zadania).
 4. Sugerujemy wypchnięcie do repozytorium w tym miejscu.
 5. Powrót do punktu 1.
 
 Wypychać kod do repozytorium możecie w dowolnym momencie, 
 pod warunkiem, że się **kompiluje**.
 
 Drobne odstępstwa od powyższego schematu są jak najbardziej dopuszczalne
 (np. komit usuwający nieudaną implementację, czy zawierający pojedynczą 
 drobną poprawkę). Celem jest weryfikowalność tego, że kod w repozytorium powstał
 z wykorzystaniem wybranej metodyki.
 
 Osoby obeznane z git'em, w szczególności z operacjami takimi jak _rebase_ czy 
 _amend_, prosimy o takie z nich korzystanie, by nie zaburzyć wspomnianej wcześniej weryfikowalności
 (kod i ewentualny refaktoring powinny być widoczne w takiej kolejności w jakiej powstawały).
 
Zadanie
--------------
Dokładny opis zadań do wykonania znajduję się w zakładce _Issues_.

Dla przypomnienia - zaimplementowanie tych zadań wymaga modyfikacji 
w projektach `WebService` i `Domain`.

Instrukcje
--------------
Przypomnienie informacji ze szkoleń znajduje się na stronie
[Manuals](https://gitlab.tt.com.pl/TDDEvaluation/Manuals/wikis/home).
