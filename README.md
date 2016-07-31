Repozytorium Signals - Iterative Test Last
===========================================

Metodyka
--------------
Pracując w metodyce ITL najpierw pisze się kod produkcyjny, 
a następnie weryfikuje go poprzez dopisanie testów jednostkowych.

Kodu produkcyjnego pisze się tyle ile programista uważa,
że jest konieczne, by rozszerzyć funkcjonalność programu o wybraną funckję.

Testy jednostkowe pisze się, by zweryfikować, czy w każdej sytuacji opisanej
w wymaganiach, kod produkcyjny zachowuje się zgodnie z tymi wymaganiami.

Przypominamy, że aby eksperyment się udał, prosimy o trzymanie się skrupulatnie
tej metodyki w tym repozytorium.

Schemat pracy
--------------
Na potrzeby eksperymentu prosimy o niestandardowy schemat komitowania:

 1. Wybierz zadanie do implementacji.
 2. Napisz kod produkcyjny realizujący to zadanie
        (zawierający ew. poprawki w istniejącym kodzie, wprowadzone 
         na potrzeby zaimplementowania nowej funkcji).
 3. Zakomituj kod produkcyjny (opis wynikający z nazwy zadania).
 4. Napisz kod testów jednostkowych
        (jeśli konieczne - popraw kod produkcyjny gdy testy wykryją błędy,
        bądź gdy kod produkcyjny wymaga zmian, by w ogóle mógł być
        testowany jednostkowo).
 5. Zakomituj kod testów (opis wynikający z nazwy zadania i tego, że to testy).
 6. Sugerujemy wypchnięcie do repozytorium w tym miejscu (ale można częściej).
 7. Powrót do punktu 1.

(w normalnym projekcie kod produkcyjny i testy mogłyby znajdować się w jednym komicie).
 
 Wypychać kod do repozytorium możecie w dowolnym momencie, 
 pod warunkiem, że się **kompiluje**.
 
 Drobne odstępstwa od powyższego schematu są jak najbardziej dopuszczalne
 (np. komit usuwający nieudaną implementację, czy zawierający pojedynczą 
 drobną poprawkę). Celem jest weryfikowalność tego, że kod w repozytorium powstał
 z wykorzystaniem wybranej metodyki.
 
 Osoby obeznane z git'em, w szczególności z operacjami takimi jak _rebase_ czy 
 _amend_, prosimy o takie z nich korzystanie, by nie zaburzyć wspomnianej wcześniej weryfikowalności
 (kod i testy powinny być w dwóch oddzielnych komitach,
 w takiej kolejności w jakiej powstawały).
 
Zadanie
--------------
Dokładny opis zadań do wykonania znajduję się w zakładce _Issues_.

Dla przypomnienia - zaimplementowanie tych zadań wymaga modyfikacji 
w projektach `WebService` i `Domain`. Wszystke testy jednostkowe prosimy
umieszczać w projekcie `WebService.Tests`.

Instrukcje
--------------
Przypomnienie informacji ze szkoleń znajduje się na stronie
[Manuals](https://gitlab.tt.com.pl/TDDEvaluation/Manuals/wikis/home).
