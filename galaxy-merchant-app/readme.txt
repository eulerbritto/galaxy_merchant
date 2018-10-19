galaxy-merchant-app

This is the Angular project that is the front end receiving the user's instructions to compute conversions.
The app contains basicly 2 rows. The right sided row will register the hystoric of command typed.
The center row, contains an input box and a button: The command must be typed in this box and so you must click "DO".
- The program will keep the last value setted. For instance> you typed "GLOB IS I", and after you typed "GLOB IS V", 
the program will keep the value 5 for this word "GLOB".
- If the set or questions don't follow the pattern shown bellow [Test Input], it will return a message informming that
wasn't possible complete the operation. Message: "I have no idea what you are talking about".

Above, an example of a command sequence and the expected result.

Test Input:
glob is I
prok is V
pish is X
tegj is L
glob glob Silver is 34 Credits
glob prok Gold is 57800 Credits
pish pish Iron is 3910 Credits
how much is pish tegj glob glob ?
how many Credits is glob prok Silver ?
how many Credits is glob prok Gold ?
how many Credits is glob prok Iron ?
how much wood could a woodchuck chuck if a woodchuck could chuck wood ?

Test Output:
 pish tegj glob glob is 42
 glob prok Silver is 68 Credits
 glob prok Gold is 57800 Credits
 glob prok Iron is 782 Credits
 I have no idea what you are talking about


