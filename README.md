# Aurora Spring 2020 - VN System
-------
A Unity project with support for dialogue/visual novels. Supports reading commands and text from a text file.

Notes:

- EventManager.cs contains all functions that can be called from within the text. When writing a command into a text file, enter an open bracket, the method name, the argument type (list of acceptable argument type names inside TextCommands.cs ConvertParams()), an equal sign, the value being passed along, and a space to separate further argument types, finally ending with a closing bracket. Failure to follow this format generally results in an invocation error.
Examples:
[MethodName argumentType=argumentValue argumentTypeWithMultipleValues=Value1,Value2,Value3,ValueN]
[CharMove i=0 v3=4.20,6.9,0]
