# CATCH THE ACE SIMULATOR

This is a simulator for the Catch the Ace game as described by [The Alcohol and Gaming Commission of Ontario](https://www.agco.ca/lottery-and-gaming/catch-ace-faqs).

The purpose of this simulator is to determine the probability of the Ace being drawn in the last week of the year \
over a given number of years.

## Usage
```.\bin\Debug\net6.0\CatchTheAce.exe [true] [full]```

```true``` is an optional parameter that will display diagnostics regarding the time it took to execute the program, \
 the number of simulations ran, and the number of times the Ace was drawn in the last week of the year.

```full``` is an optional parameter that will display logs for each year that is simulated.

All other added parameters will be ignored.