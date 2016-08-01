Repozytorium Signals - Test-Driven Development
===========================================

Metodyka
--------------
Pracując w metodyce TDD kod produkcyjny powstaje jako efekt małych kroków,
z których każdy rozpoczyna się napisaniem testu.

Zasady TDD:
 * Nie wolno pisać kodu produkcyjnego, o ile nie naprawi on istniejącego i nieprzechodzącego testu.
 * Nie wolno pisać kodu testu więcej, niż jest to konieczne, by test nie przechodził (błąd kompilacji to także nieprzechodzący test).
 * Nie wolno pisać kodu produkcyjnego więcej, niż jest to konieczne, by naprawić nieprzechodzący test.
 
W efekcie cykl pracy w TDD wygląda następująco:
 1. Napisz _minimalny_ test, który nie będzie przechodził.
 2. Wprowadź _minimalną_ zmianę w kodzie produkcyjnym, tak by wszystkie testy przechodziły.
 3. Zrefaktoruj kod, korzystając z przechodzących testów.
 4. Wróć do punktu 1.

Przypominamy, że aby eksperyment się udał, prosimy o trzymanie się skrupulatnie
tej metodyki w tym repozytorium.

Schemat pracy
--------------
Na potrzeby eksperymentu prosimy o niestandardowy schemat komitowania:

 1. Wybierz zadanie do implementacji.
 2. Napisz minimalny fragment kodu testów, taki, by test nie przechodził.
 3. Zakomituj (opis komitu powinien wynikać z nazwy testu).
 4. Rozwiń kod produkcyjny poprzez minimalną zmianę powodującą przechodzenie testu.
 5. Zakomituj (opis komitu powinien wynikać z nazwy testu i faktu, że przechodzi).
 6. Zrefaktoruj kod (jeśli konieczne - np. usuń duplikację kodu).
 7. Zakomituj ewentualne zmiany (opis powinien wynikać z wprowadzonych zmian).
 8. Sugerujemy wypchnięcie do repozytorium w tym miejscu (ale można częściej).
 9. Zweryfikuj, czy zadanie zostało wykonane, jeśli tak, idź do punktu 1, jeśli nie, do punktu 2.
 
(w normalnym projekcie kod produkcyjny i testy mogłyby znajdować się w jednym komicie).

 Wypychać kod do repozytorium możecie w dowolnym momencie, 
 pod warunkiem, że się **kompiluje**.
 
 Drobne odstępstwa od powyższego schematu są jak najbardziej dopuszczalne
 (np. komit usuwający nieudaną implementację, czy zawierający pojedynczą 
 drobną poprawkę). Celem jest weryfikowalność tego, że kod w repozytorium powstał
 z wykorzystaniem wybranej metodyki.
 
 Osoby obeznane z git'em, w szczególności z operacjami takimi jak _rebase_ czy 
 _amend_, prosimy o takie z nich korzystanie, by nie zaburzyć wspomnianej wcześniej weryfikowalności
 (kod, testy i refaktoring powinny być w oddzielnych komitach,
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
