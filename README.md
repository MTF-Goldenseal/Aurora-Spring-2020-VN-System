# Project - Paratoxic
-------
A ten minute animated visual novel demo. Supports reading text off of a text file and into dialogue bubbles, and running commands within a text file to manipulate the playing field (moving characters, playing visual effects and sound effects, applyng camera shake, etc.).

Chen - Base Visual Novel Engine
Maren McLean, SJCRPV - Phone Dialogue system, most of the demo specific features

Notes:

- EventManager.cs contains all functions that can be called from within the text. When writing a command into a text file, enter an open bracket, the method name, the argument type (list of acceptable argument type names inside TextCommands.cs ConvertParams()), an equal sign, the value being passed along, and a space to separate further argument types, finally ending with a closing bracket. Failure to follow this format generally results in an invocation error.
Examples:
[MethodName argumentType=argumentValue argumentTypeWithMultipleValues=Value1,Value2,Value3,ValueN]
[CharMove i=0 v3=4.20,6.9,0]
