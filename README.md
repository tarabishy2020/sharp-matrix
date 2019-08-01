# Sharp Matrix
This is a 2D Matrix C# class library.
Used in a lecture for a deep dive into machine learning matrix operations.

Structure is inspired from this repo https://github.com/rwl/ParallelColt and from here as well https://github.com/CodingTrain/Toy-Neural-Network-JS/blob/master/lib/matrix.js

The implementation uses a 1D List of doubles to store the data. 

## Features
- Indexer to get and set matrix cells.
```C#
Matrix2d m = new Matrix2d(3,3,1); // Create a 3x3 matrix of ones.
Console.Write(m[1, 1]); // 1
m[2,1] = 2;
Console.Write(m.ToString());
// 1 1 1
// 1 1 1
// 1 2 1
```
- Indexer to quickly retrieve rows.
```C#
Matrix2d m = new Matrix2d(3, 3, new List<double>() { 20, 21, 22, 30, 31, 31, 40, 41, 42 });
m[1].ForEach(Console.WriteLine);
// 30
// 31
// 31
```

- Random initialization of values, uniform and Gaussian using the [Box-Muller transformation](https://stackoverflow.com/questions/218060/random-gaussian-variables).
```C#
Matrix2d m_uniform = new Matrix2d(3, 3).Random();
Console.Write(m_uniform.ToString());
// 0.698004952956925 0.335880612645243 0.362230141350175
// 0.494230345121692 0.665076169495041 0.984987090800417
// 0.727741793136923 0.0698694698837909 0.0824739393230872
Matrix2d m_gaussian = new Matrix2d(3, 3).Randn();
Console.Write(m_gaussian.ToString());
// -0.223498728675324 1.6653655980595 -1.13907259452779
// -0.61656402728777 -0.370450094746215 -0.738776443648561
// -0.27551853829444 2.10379830910225 1.40016473372225
```

- Matrix transpose, returns a copy
```C#
Matrix2d m = new Matrix2d(3, 2, new List<double>() { 20, 21, 30, 31, 40, 41 });
Console.Write(m.ToString());
// 20 21
// 30 31
// 40 41
var mTranspose = m.Transpose();
Console.Write(mTranspose.ToString());
// 20 30 40
// 21 31 41
m.vDice(); //Transposes this matrix
```
- Quick assigning and editing of values
```C#
Matrix2d m = new Matrix2d(3, 3);
m.Assign(5.0);
Console.Write(m.ToString());
// 5 5 5
// 5 5 5
// 5 5 5
m.Assign(n => n*n);
// 25 25 25
// 25 25 25
// 25 25 25
```

- View row, view column and get shape
```C#
Matrix2d m = new Matrix2d(3,  3).Random();
// 0.05988795452746 0.594980841779607 0.715880787333418
// 0.284296918327127 0.220808626721058 0.29949953514128
// 0.767850642450084 0.848819678578908 0.6419840490641
m.ViewRow(2); // List<double>(3) { 0.76785064245008428, 0.8488196785789075, 0.64198404906409978 }
m.ViewColumn(0); // List<double>(3) { 0.05988795452746002, 0.28429691832712706, 0.76785064245008428 }
m.Shape(); // "3 x 3 matrix"
```
- Different operators support
```C#
Matrix2d m1 = new Matrix2d(1,3, new List<double>() { 3, 4, 2});
Matrix2d m2 = new Matrix2d(3, 3, new List<double>() {13, 9, 7, 8, 7, 4, 6, 4, 0});
m1.Dot(m2); // [83 63 37]
// Pairwise operations with a double or another matrix of the same shape *, /, +, -
m2 * 2.0; // [[26 18 14],[16 14 8],[12 8 0]]
m2 * m2; // [[676 324 196],[256 196 64],[144 64 0]]
```
