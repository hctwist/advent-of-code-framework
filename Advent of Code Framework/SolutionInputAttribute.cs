﻿namespace AdventOfCode.Framework;

/// <summary>
/// Metadata for solutions.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class SolutionInputAttribute : Attribute
{
    /// <summary>
    /// Gets or sets whether the input is enabled.
    /// </summary>
    public bool Enabled { get; set; }
    
    /// <summary>
    /// Gets or sets whether this input should be used for benchmarking.
    /// </summary>
    public bool Benchmark { get; set; }

    /// <summary>
    /// The problem that this input should be used for.
    /// </summary>
    public Problem Problem { get; set; }
    
    internal string Path { get; }
    
    /// <summary>
    /// Creates a new <see cref="SolutionAttribute"/>.
    /// </summary>
    /// <param name="path">The input filepath.</param>
    public SolutionInputAttribute(string path)
    {
        Path = path;
        Enabled = true;
        Benchmark = false;
    }
}