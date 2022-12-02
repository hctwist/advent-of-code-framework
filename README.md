# Advent of Code Framework

Simple framework to run solutions in without having to worry about boilderplate.

## Setup
To kick of the process, `SolutionRunner.Run` needs to be called.
```csharp
new SolutionRunner().Run(args);
```
This takes an optional argument which specifies a subdirectory which contains the input files. If specified, relative paths from a `SolutionInput` attribute will be resolved relative to this directory.

This can be run in two modes:
#### "run &lt;day&gt; [problem]"
Runs the solution associated with a specific day. If a problem is specified then only that problem will run. Otherwise both problems will be run.

#### "benchmark [day] [problem]"
Runs a benchmark for the specified day. If no day is provided, then a benchmark will be run for all solutions. If a problem is specified then only that problem will run. Otherwise both problems will be run.

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

### Input

Input is read by the framework and can be accessed in the solution via the `Input` property (or directly in the constructor).

### Process

Each *problem* run instantiates a new solution, so state can't be shared between runs.

Benchmarking includes the runtime of the constructor as well as the specific problem, however it doesn't include reading input files.

### Disabling Solutions, Problems and Inputs

Solutions can be disabled by passing in `false` to the `Solution` attribute, which means it will be ignored for runs and benchmarks. This is also the case with inputs which can be disabled via the `SolutionInput` attribute.

A problem that throws a `NotImplementedException` is not considered as having failed and will not contribute to the benchmarks.
