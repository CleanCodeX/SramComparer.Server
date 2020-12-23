# SRAM format completion project

#### SRAM - what's that?
SRAM is a term for [Static Random Access Memory](https://de.wikipedia.org/wiki/Static_random-access_memory){target="_"} and is a buffer where the game stores all information which needs to be saved to continue a game later on.

When you save in-game at the inn the sram SRAM buffer saved. If you're using an emulator the SRAM gets saved to disk as *.srm file.
Every progress you made in the game gets stored in this file. So far so good. 
This progress can be read and also edited. 

The only problem is a little less than 50% (about aprox. 400 bytes) of SoE's SRAM game slot format is still considered as 'unknown', meaning that we don't really know what these parts of SRAM actually represent.

#### What's still unknown?
Most of unknown parts are reserved for pick-up information of various ingredient sniff spots, opened chests, gourds, pots and which persons were spoken to twice. Despite these there are also game relevant progress flags which we want to know to write more capable SRAM editors.

A non exhaustive list can be found under section [Unknowns](_?p=Unknowns).

#### What's the Goal?
Once the SRAM format is completely known, it offers the possibility to write a full capable SRAM editor, enabling/disabling everything what can be controlled by a savegame.

"That sounds nice!" you probably say. (obviously we also think that)

##### When will it be available?

Well, that's mostly up to the amount of contributions by the community.
We're spending our free time working on that, but that won't be enough to make it happen fast.

At this point you might ask yourself how you can make it happen faster. (likely I guess, due to the fact that you clicked on this page)

If you want to join the SoE army, see [here](_?p=HowCanIHelp) how it works.