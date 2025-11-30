# Advent of Code Framework

C# framework to interactively bootstrap Advent of Code solutions.

# Getting Started

The entry point to the framework is the `AdventOfCodeRunner`. From here you can either run in a full interactive mode
via `AdventOfCodeRunner.Run` or specify fixed options via methods like `AdventOfCodeRunner.Solve`. Solutions setup as
in [Solutions](#solutions) will be picked up automatically, but the runner will guide through this.

![img.png](Images/InteractiveConsole.png)

# Solutions

A `Solution` represents the logic to solve both problems for a particular puzzle day. Solutions should both implement
`ISolution` and be attributed with `SolutionAttribute` as follows:

```csharp
[Solution(Day = 1)]
internal class SampleSolution : ISolution
{
    /// <inheritdoc />
    public string SolveProblem1(ProblemInput input, ISolutionLogger logger)
    {
        ...
    }

    /// <inheritdoc />
    public string SolveProblem2(ProblemInput input, ISolutionLogger logger)
    {
        ...
    }
}
```

# Inputs and Outputs

When a problem is run for the first time, input and output files need to be provided. These are prompted for via Notepad
by the runner, and the user is expected to save and exit Notepad to continue (similar to command line Git). Initially
required files are the input and output of the sample part of the problem, plus the input of the main problem (the file
title will indicate the data expected).

After running the solution to the main problem the option will be given to save the output.

Inputs and outputs will be persisted at `%LocalAppData%\Advent of Code` and can be edited there if they require
updating.

# Logging

Due to the interactive nature of the runner, writing to the console directly has undefined behaviour. To log in
solutions a logger is passed in which is compatible with the interactive console.