# Glossary

## S-RAM
S-RAM stands for <a href="https://en.wikipedia.org/wiki/Static_random-access_memory" target=_>Static Random Access Memory</a> and is a buffer where the game stores all information the game needs to continue from a savegame later on.

When you save in-game at the inn the S-RAM buffer will be updated. With an emulator the S-RAM gets saved to disk as *.srm file. Every progress you made in the game gets stored in this file.

As of recently, (Snes9x) savestates can also be compared. These do not require in-game saving.

## Unknowns
The "Unknowns" are areas of S-RAM which have no known meaning assigned yet.

Most of unknown areas are reserved for ingredient sniffing spots, opened walls, doors, chests, gourds, pots and twice spoken people. Apart from these things there are also game-relevant progress flags such as which bosses have already been defeated or which other events already occurred in the past. All these things must be known in order to develop a powerful savegame editor.

A non-exhaustive list can be found under: <a href=unknowns>Unknowns</a>.