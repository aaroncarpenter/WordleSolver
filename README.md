# WordleSolver

## Introduction
The purpose of this program assist in solving the daily Wordle puzzle.  It allows you to enter your guesses, along with the information about whether a guess's letter is in the right spot, in a different spot or not in the answer at all and presents a list of possible solutions to the Wordle using their standard and extended word lists.  In most cases, this program can narrow down the list of solutions to just a couple possible solutions after 2-3 guesses.  Then, it's up to you to decide which to try next.

## Usage

Enter the number coresponding to the options below at the text prompt.

### (1) Match String
The match string should be entered as each latter in the guess followed by an indicator of whether it's not in the word (!), in the word but in a different position (+) or in the correct position (*). 

For example, if the guess is frogs and the "f" is in the correct position, s is in the word at different position and the rest of the letters aren't found, then the match string would be "f*r!o!g!s+"

WARNING - In almost all cases you can use the color of the letter to determine which symbol to use in the match string.  However, if your guess includes a letter in the correct position and that letter appears again in the word, the Wordle site will flag that second instance with the color that indicates it does not appear in the word.  In this case, you must use the (+) option.  If you use the (!), the program will not work properly.

### (2) Excluded Letters
This option allows you to enter the excluded letters manually.  Enter the letter to exclude from the guess and press enter.

### (3) Included Letters
This option allows you to enter the included letters that are in the wrong position manually.  Enter the letter followed by it's position in the guessed word.  If the Wordle site says the "f" in the first position is somewhere else in the word, you would enter "f1".

### (4) Matched Letters
This option allows you to enter the letters that are in the right position manually.  Enter the letter followed by it's position in the guessed word.  If the Wordle site says the "f" in the first position is , you would enter "f1".

### (5) Show Words
This option shows a list of words from the standard and extended word lists that match the criteria entered so far.  The standard word list includes the 2,300+ 5 letter words that make up the standard Wordle dictionary.  The extended word list is another 10,000 5 letter words that are less common, but could potentially by used as answers.  If the criteria entered matches to more than 40 standard words, only the first 40 matched words in alphabetical order are displayed.  Once the matched standard words gets below 10, then the extended word matches will be displayed in case they are needed.

The order of the words shown are strictly alphabetically.

