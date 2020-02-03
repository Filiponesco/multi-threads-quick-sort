# multi-threads-quick-sort
 5th semester .NET
 
# Porównanie czasów sortowania jednowątkowego z wielowątkowym

## 1.	Program spełnia następujące funkcje:
 -	Ustalenie liczby losowanych liczb,
 -	Ustawienie zakresu losowanych liczb,
 -	Miara czasu dla sortowania jednowątkowego,
 -	Miara czasu dla sortowania wielowątkowego,
 -	Zapis czasów do listy,
 -	Wyświetlanie tablicy liczb w wizualnej formie
## 2.	Aplikacja wychwytuje następujące błędy użytkownika:
 - Wprowadzenie nieprawidłowych danych do formularzu
## 3.	Wygląd aplikacji: 

![alt text](https://github.com/Filiponesco/multiThreads-quick-sort/blob/master/screenshot.png)

## 4.	Wnioski:
 - Sortowanie wątkowe tworzy każde kolejne dwa wątki przy podzieleniu tablicy, zatem jest efektywne w urządzeniach z wielowątkowymi procesorami. Przy większej ilości wątków, których potrzebuje program, system Windows będzie dzielił zadania na dostępne wątki procesora. 
