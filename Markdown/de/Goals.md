# SRAM was ist das?
SRAM steht für <a href="https://de.wikipedia.org/wiki/Static_random-access_memory" target=_> statischer Direkt-Zugriffsspeicher </a> und ist ein Puffer, in welchem das Spiel alle Informationen des Spiels speichert muss später von einem Savegame fortgesetzt werden.

Wenn Sie innerhalb des Spiels im Gasthaus speichern, wird der SRAM-Puffer gespeichert. Wenn Sie einen Emulator verwenden, wird das SRAM als * .srm-Datei auf der Festplatte gespeichert.
Jeder Fortschritt den du im Spiel gemacht hast, wird in dieser Datei gespeichert. So weit, so gut.
Daher kann dieser Fortschritt auch gelesen und manipuliert werden.

Das einzige Problem dabei ist, dass ca. 54% des SRAM-Speicherslot-Formates von SoE gelten immer noch als unbekannt, was bedeutet, dass wir nicht wirklich wissen, was diese Teile vom SRAM tatsächlich repräsentieren.

## Was sind die Ziele?
Sobald das SRAM-Format vollständig bekannt ist, bietet es die Möglichkeit einen leistungsstarken Spielstand-Editor zu entwickeln, der alles aktiviert oder deaktiviert, was von einem Spielstand aus gesteuert werden kann.

"Das klingt cool!" denkst du vielleicht. Absolut, das tut es!

## Exklusive Features eines zukünftigen Spielstand-Editors
* Zurücksetzen von bereits durchgeführten Boss-Kämpfen
* Festlegen des Spiel-Speicherortes an dem nach dem Laden weitergespielt werden soll
* Zurücksetzen der aufhobenen Zutaten je Map, Kapitel oder des ganzen Spiels
* Zurücksetzen der geöffneten Kisten, Körbe, Kürbisse oder Wände je Map, Kapitel oder des ganzen Spiels

### Was ist noch unbekannt?
Die meisten unbekannten Bereiche sind für Zutaten-Schnüffelstellen, geöffnete Mauern, Türen, Truhen, Kürbisse, Töpfe und zweimal angesprochenen Personen reserviert. Abseits dieser Dinge gibt es auch spielrelevante Fortschrittsmerkmale wie z. B. welche Endgegner bereits besiegt wurden oder welche sonstigen Ereignisse bereits in der Vergangenheit liegen. All diese Dinge müssen bekannt sein um einen leistungsstarken Spielstand-Editor zu entwickeln.

Eine unvollständige Liste findest du unter: <a href=unknowns>das Unbekannte</a>.

### Wann wird es verfügbar sein?

Nun, das hängt hauptsächlich von der Menge und Qualität der Zuarbeit ab.
Ich verbringe meine Freizeit damit daran zu arbeiten, aber das wird nicht ausreichen um schnell voranzukommen.

Wenn du der "Exploration Army" beitreten möchtest, lies <a href=contribute>hier</a>, wie es funktioniert.