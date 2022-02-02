// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

Console.Clear();
var tests = new List<(int, int)>()
{
    (0, 1), 
    (1, 2), 
    (2, 2), 
    (3, 3), 
    (4, 4), 
    (5, 6), 
    (6, 9), 
    (7, 14), 
    (8, 22), 
    (9, 35), 
    (10, 56), 
    (11, 90), 
    (12, 145), 
    (13, 234)
};

void Run(Func<int, int> f, int n)
{
    for (int i = 0; i < n; i++)
    {
        var sw = new Stopwatch();
        sw.Start();
        Console.WriteLine($"{i}: {f.Invoke(i)} \t - {sw.ElapsedTicks}");
        sw.Stop();
    }
}
//
// int Fib1(int n)
// {
//     return n switch
//     {
//         0 => 0,
//         1 => 1,
//         _ => Fib1(n - 1) + Fib1(n - 2)
//     };
// }
//
// int Fib2(int n)
// {
//     int Inner(int a, int b, int fib)
//     {
//         return fib switch
//         {
//             0 => a,
//             1 => b,
//             _ => Inner(b, a + b, fib - 1)
//         };
//     }
//
//     return Inner(0, 1, n);
// }
static int Fib1(int n)
{
    return n switch
    {
        0 => 0,
        1 => 1,
        _ => Fib1(n - 1) + Fib1(n - 2)
    };
}
static int Fib2(int n)
{
    int Inner(int a, int b, int fib)
    {
        return fib switch
        {
            0 => a,
            1 => b,
            _ => Inner(b, a + b, fib-1)
        };
    }

    return Inner(0, 1, n);
}
Run(Fib1, 30);
Run(Fib2, 30);
