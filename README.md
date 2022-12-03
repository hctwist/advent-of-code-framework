# Advent of Code Framework

Simple framework to bootstrap Advent of Code solutions.

## Modes
The framework supports two run modes:

### Solve Mode
This runs solutions and outputs the result from the problems.

### Benchmark Mode
This benchmarks solution problems. The benchmark time includes constructor and method time.

## Setup
The entry point to the framework is the `SolutionRunner`.
```csharp
SolutionRunner runner = new();
```
The `SolutionRunner` itself takes an optional argument which specifies a subdirectory which contains the input files. If specified, relative paths from a `SolutionInput` attribute will be resolved relative to this directory.

You can then run the solution runner directly in one of the two modes:
```csharp
runner.Solve(1);
runner.Benchmark();
```
Extra arguments can be passed to restrict the runs (ie. to specific days or problems).

There is also an option intended to work with the command line:

```csharp
runner.Run(args);
```

The arguments passed to `Run` can define the two modes as follows:
```
run <day> [problem]
benchmark [day] [problem]
```
for example:
```csharp
runner.Run(new string[] { "run", "1", "1" });
```

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

Solutions can be disabled by passing in `false` to the `Solution` attribute, which means it will be ignored for runs and benchmarks. This is also the case with inputs which can be disabled via the `SolutionInput` attribute:

```csharp
[Solution(1, Enabled = true)]
[SolutionInput("Input1.test.txt", Enabled = false)]
[SolutionInput("Input1.txt")]
public class MySolution : Solution
```

A problem that throws a `NotImplementedException` is not considered as having failed and therefore will not contribute to the benchmarks, or stop the current run.
