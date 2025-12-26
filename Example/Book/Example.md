# Guide Example

:::aside
[This](https://github.com/Lexyna/GodotGuideGen) being the project in question
:::

This hsould be a part snippet:

:::code
part_snippet:5:3:This is only small a part of the main file
:::

and this shiould be the full snippet

:::code
full_snippet:5:3:This is only small a part of the main file
:::

This file serves as a primary example for all the features included in this project.

One very neat feature is, the project can automatically insert and highligh code blocks, liks this:

:::code
example:0:0:This is the complete .gd example file for godot 4.5
:::


But we can, *of course*, also only add some small snippets like this:

:::code
init:6:1:This is only small a part of the main file
:::

## Tables and other cool stuff

It might not come as much of a surprise but tables are possible as well, just look at them!

Header I | Header II | Header III
-- | - | -
Value1|Value2|Value3
Text1|Text2|Text3

Besides that we have hints

:::hint
**Take a hint!**
:::

So yeah, that's already it. It can render anything in the base Markdig config + `PipeTable` and `CustomContaniners` for `aside`, `hint` and `code`.