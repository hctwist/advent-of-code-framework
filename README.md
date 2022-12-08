# Advent of Code Framework

Simple framework to bootstrap Advent of Code solutions.

## Modes
The framework supports two run modes:

### Solve Mode
Runs solutions and outputs the result from the problems.

### Benchmark Mode
Benchmarks solution problems.

## Setting Up Solutions

A solution needs to satisfy four constrains: inheriting from `Solution`, having a constructor which takes in a single parameter of type `Input`, having a single `Solution` attribute, and having at least one `SolutionInput` attribute.

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

    protected override string? Problem1()
    {
        return null;
    }

    protected override string? Problem2()
    {
        return null;
    }
}
```

The solutions to the problems should be returned from `Problem1` and `Problem2` as strings.

## Running
The entry point to the framework is the `SolutionRunner`.
```csharp
SolutionRunner runner = new();
```

You can then run the solution runner directly in one of the two modes and for a specific combination of days/problems.
```csharp
// Solve mode
runner.Solve(1, Problem.Problem2);
runner.Solve(1);
runner.SolveLatest();
runner.SolveAll();

// Benchmark mode
runner.Benchmark(1, Problem.Problem2);
runner.Benchmark(1);
runner.BenchmarkAll();
```

There is also an option intended to work with the command line:

```csharp
runner.Run(args);
```

The arguments passed to `Run` can define the two modes as follows:
```
run <day> [problem]
benchmark <day> <problem>
```
for example:
```csharp
runner.Run(new string[] { "run", "1", "1" });
```
*Note that `SolveLatest` is not supported with run arguments.*

## Input

Input is read by the framework and can be accessed in the solution via the `Input` property (or directly in the constructor). Either the input split by line, or a raw input string can be read.

```csharp
protected override string? Problem1()
{
    string[] lines = Input.Lines;
    string raw = Input.Raw;
}
```

## Solution Options

### Solution Attribute

The solution attribute takes in the day that the solution corresponds to, and optionally whether the solution is enabled.

```csharp
[Solution(1, Enabled = true)]
```

### Solution Input Attribute

The solution input attribute takes in a path to an input file to run the solution for. This is by default resolved from the base directory of the project but this can be changed via the `SolutionRunner` options. Optional properties can be set to enable/disable the input, specify that the input is only relevant to a single problem and enable benchmarking.

```csharp
[SolutionInput("Input.txt", Enabled = true, Benchmark = true, Problem = Problem.All)
```

*Note that in benchmark mode, only inputs marked with `Benchmark = true` will be considered. This is `false` by default.

## Notes

### Process

Each *problem* run instantiates a new solution, so state can't be shared between runs.
Benchmarking includes the runtime of the constructor as well as the specific problem, however it doesn't include reading input files.

### Disabling Problems

A problem that returns `null` is not considered as having failed and therefore will not contribute to the benchmarks, or stop the current run.

### Pitfalls

Input file paths should be specified relative to the output directory (or usually relative to the project root). For this to work the files need to be setup to copy over when building (https://social.technet.microsoft.com/wiki/contents/articles/53248.visual-studio-copying-files-to-debug-or-release-folder.aspx).
