# Name Generation Library
## Description
This library strategically and logically generates names which are both readable and pronounceable. There is a library inside, and also an
example program that utilises the code and creates a UI.
## How does it work?
The program takes a list of ~26,000 pieces of data, taken from a census - it shows how many babies were given that name in a certain year.
Each piece of data contains a name and a number (referring to how many times it was used). The program iterates through each name and checks
character by character and builds a 'knowledge' on which lettering patterns can occur. Each name is repeated the amount of times it was given 
to someone. IE, the name 'Emily' appears 25,149 times, while 'Janet' only 825 times. The program would note that 'm' follows 'e' 25,149 times,
and 't' follows 'e' 825 times.

The program calculates a name by choosing a random (but biased towards a more common) character and repeating this until a nice string is made.
The help improve names, the first 2 characters of a name cannot both be consonants, and you can never have more than 2 identical characters in a row. The program then has the capability to 'critique' each name, and gives a rating on how good of a name it thinks it is.

![Screenshot](http://i.imgur.com/EzvTunN.png)
## Code documentation
Attach the library to your project and add references as you would with any library.
### void NameGenerator.Initialise()
This function initialises the whole generator, and MUST be called before any other functions in the library. This will take several seconds to
complete, as it is processing more than 26,000 names roughly 3.5 million times.
### string NameGenerator.GenerateName(int length)
This function will generate a name and return it as a string. You must pass the length in as an integer.
### string NameGenerator.Analysis()
This function returns a string which contains an analysis of all the names in the program.
### double NameGenerator.Analyse(string name)
This function analyses the commonness of the name, and generally creates a double between 0 and 1, however some names rate higher slightly.
The double refers to how confident the program is that the name is a 'good' one. A value of 0 means the name can't be generated by the program.
**NOTE:** In the example, this is times by 100 and rounded to whole numbers.

**Visit my portfolio at http://jamorris.co.uk/**
