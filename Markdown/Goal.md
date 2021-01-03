# SRAM what's that?
SRAM stands for <a href="https://de.wikipedia.org/wiki/Static_random-access_memory" target="_">Static Random Access Memory</a> and is a buffer where the game stores all information the game needs to continue from a savegame later on.

When you save within the game at the inn the sram SRAM buffer will be saved. If you're using an emulator the SRAM gets saved to disk as *.srm file.
Every progress you made in the game gets stored in this file. So far so good. 
This progress can be read and therefore edited, too. 

The only problem is that aprox. 54% of SoE's SRAM game slot format is still considered as 'unknown', meaning that we don't really know what these parts of SRAM actually represent.

## What's the goal?
Once the SRAM format is completely known, it offers the possibility to write a full capable SRAM editor, enabling/disabling everything what can be controlled by a savegame.

"That sounds nice!" you might say. (obviously I think that, too)

### What's still unknown?
Most of unknown parts are reserved for pick-up information from various ingredient sniff spots, opened chests, gourds, pots and which people were spoken twice. Despite these things there are also game relevant progress flags which need to be known to write a more capable SRAM editor.

A non-exhaustive list can be found under section <a href=Unknowns>Unknowns</a>.

### When will it be available?

Well, that's mostly up to the amount of contributions by the community.
I am spending my free time working on that, but that won't be enough to make it happen fast.

If you want to join the SoE 'army', see <a href=Contribute>here</a> how it works.
