// ## Instructions

// Given a moment, determine the moment that would be after a gigasecond
// has passed.

// A gigasecond is 10^9 (1,000,000,000) seconds.
using System;

public static class Gigasecond
{
    private const double GIGASECOND = 1E9;

    public static DateTime Add(DateTime moment) => moment.AddSeconds(GIGASECOND);
}
