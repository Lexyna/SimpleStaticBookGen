# Godot Project Guide Generator

This Project automatically generates a "book-like" website (inspired by [Crafting Interpreters](https://craftinginterpreters.com/)) for a godot project.

Run the program with:
`dotnet run --project .\GodotGuideGen "path"`

Where path is the path to the godot project to read. 

## Structure

The contents of the Book need to be in a top level `Book` folder.
Each Chapter is represented by a `.md` file and a `index.json` file containing the structuer of the website.

### `Index.json`

```
{
    "ProjectName": "Name of the Project",
    "Book"[
        [], #Represents The first Section of the Book 
        [], #Represents The second Section of the Book
        [], #Represents The third Section of the Book
        [], #and so on
    ]
}
```

`Section`: Each Section is a list of `strings` forming that section of the book. The `0th` element is the "Header" Chapter for that Section, all other elements form subsections.

### `{Chapter}.md`

This file contains the main content to be displayed on the generated `{Chapter}.html` file.

Markdown text is automatically converted to html using [Markdig](https://github.com/xoofx/markdig), with some extra features.

## Extras

## `aside`

Using the format
```
:::aside
{Markdown}
:::
```

where `Markdown` is normal markdown syntax, generates a right-floating container at the top of the next element. 

## `hint`

Using the format
```
:::hint
{Markdown}
:::
```

where `Markdown` is normal Markdown syntax, generates a small, cursive text box

## `code`

Using the format 
```
:::code
{blockId}:{before}:{after}:{text}?
:::
```

`blockId`: The blockId as specified in a `.gd` script
`before`: The number of lines to render before the main block
`after`: The number of lines to render after the main block
`text`: Optional text to be rendered next to main snippet.

A `block` can be declared inside a `.gd` script using the following syntax:

```
#>> block_id
# Member variables.
var a = 5
var s = "Hello"
var arr = [1, 2, 3]
var dict = {"key": "value", 2: 3}
var other_dict = {key = "value", other_key = 2}
var typed_var: int
var inferred_type := "String"
#<< block_id
```

Blocks can be nested.