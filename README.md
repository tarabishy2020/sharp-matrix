# Sharp Matrix

This is a 2D Matrix C# class library.

Used in a lecture for a deep dive into machine learning matrix operations.

Structure is inspired from this repo https://github.com/rwl/ParallelColt and from here too https://github.com/CodingTrain/Toy-Neural-Network-JS/blob/master/lib/matrix.js

The implementation uses a List of doubles to store the data. 

## Features
- Indexer to get and set matrix cells.

```
Matrix2d m = new Matrix2d(3,3,1); // Create a 3x3 matrix of ones.
Console.Write(m[1, 1]); // Prints: 1
m[2,1] = 2;
Console.Write(m.ToString());
// Prints
// 1 1 1
// 1 1 1
// 1 2 1
```

- Indexer to quickly retrieve rows.

```
Matrix2d m = new Matrix2d(3, 3, new List<double>() { 20, 21, 22, 30, 31, 31, 40, 41, 42 });
m[1].ForEach(Console.WriteLine);
// Prints
// 30
// 31
// 31
```