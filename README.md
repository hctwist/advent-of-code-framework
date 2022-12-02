# Advent of Code Framework

Simple framework to run solutions in without having to worry about boilderplate.

## Setup
To kick of the process, run
```csharp
SolutionRunner.Run(args);
```

This can be run in two modes:
#### "run &lt;day number&gt;"
Runs the solution associated with a specific day. This will run both problems.

#### "benchmark [day number]"
Runs a benchmark for the specified day. If no day is provided, then a benchmark will be run for all solutions. This will run both problems.

## Solutions

A solution needs to satisfy four constrains: inheriting from `Solution`, having a constructor which takes in a single parameter of type `Input`, having a single `Solution` attribute, having at least one `SolutionInput` attribute.

A solution should then look something like this:

```csharp
using AdventOfCode.Framework;

[Solution(1)]
[SolutionInput("Input.txt")]
public class MySolution : Solution
{
    public MySolution(Input input) : base(input)
    {
    }

    protected override string Problem1()
    {
    }

    protected override string Problem2()
    {
    }
}
```

### Notes

Input is read by the framework and can be accessed in the solution via the `Input` property (or directly in the constructor).

Each *problem* run instantiates a solution, so state can't be shared between runs.

The runner will pick up the first solution that matches the day number, so multiple solutions for a single day will be ignored. To direct the runner to a specific solution you can disable solutions by passing in `false` to the `Solution` attribute.

Benchmarking includes the runtime of the constructor as well as the specific problem, however it doesn't include reading input files.
