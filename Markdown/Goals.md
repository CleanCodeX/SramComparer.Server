# SRAM what's that?
SRAM stands for <a href="https://de.wikipedia.org/wiki/Static_random-access_memory" target=_>Static Random Access Memory</a> and is a buffer where the game stores all information the game needs to continue from a savegame later on.

When you save within the game at the inn the SRAM buffer will be saved. If you're using an emulator the SRAM gets saved to disk as *.srm file.
Every progress you made in the game gets stored in this file. So far so good. 
Therefore this progress can be read and mnipulated, too. 

The only problem is that aprox. 54% of SoE's SRAM save slot format is still considered to be unknown, meaning that we don't really know what these parts of SRAM actually represent.

## What are the goals?
Once the SRAM format is completely known, it offers the possibility to write a full capable SRAM editor, enabling/disabling everything what can be controlled by a savegame.

Possible features:

"That sounds cool!" you might think. Indeed it does!

### What's still unknown?
Most of unknown areas are reserved for ingredient sniffing spots, opened walls, doors, chests, gourds, pots and twice spoken people. Apart from these things there are also game-relevant progress flags such as which bosses have already been defeated or which other events already occurred in the past. All these things must be known in order to develop a powerful savegame editor.

A non-exhaustive list can be found under: <a href=Unknowns>Unknowns</a>.

### Exclusive features of a future savegame editor
* Reset boss fights which have already been defeated
* Specify the in-game save location where you want to continue playing after loading
* Resetting the cancelled ingredients for each map, chapter or the whole game
* Resetting the opened boxes, pots and gourds or walls per map, chapter or the whole game

### When will it be available?

Well, that's mostly up to the amount and quality of contributions by the community.
I am spending my free time working on that, but that won't be enough to make it happen fast.

If you want to join the "exploration army", see <a href=Contribute>here</a> how it works.
