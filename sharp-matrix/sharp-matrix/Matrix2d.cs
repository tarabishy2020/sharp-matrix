using System;
using System.Collections.Generic;
using System.Linq;

namespace sharp_matrix
{
    /// <summary>
    /// References:
    /// https://github.com/rwl/ParallelColt
    /// https://github.com/CodingTrain/Toy-Neural-Network-JS/blob/master/lib/matrix.js
    /// A matrix class implementation
    /// </summary>
    public class Matrix2d
    {
        protected int rows;
        protected int columns;
        protected int rowStride;
        protected int columnStride;
        protected List<double> data;
        public Gaussian g = new Gaussian();
        public Random rand = new Random();
        public Matrix2d(int rows, int columns)
        {
            SetUp(rows, columns);
        }
        public Matrix2d(int rows, int columns, double value)
        {
            SetUp(rows, columns);
            Assign(value);
        }
        public Matrix2d(int rows, int columns, List<double> values)
        {
            if (!(rows * columns == values.Count)) throw new ArgumentException("r*c == List.Count");
            SetUp(rows, columns);
            this.data = values;
        }
        public Matrix2d RandUniform()
        {
            this.data = Enumerable.Range(0, (Rows() * Columns())).Select(n => g.RandomGauss()).ToList();
            return this;
        }
        public Matrix2d Randn()
        {
            this.data = Enumerable.Range(0, (Rows() * Columns())).Select(n => rand.NextDouble()).ToList();
            return this;
        }
        public double this[int r, int c]
        {
            get
            {
                return Get(r, c);
            }
            set
            {
                Set(r, c, value);
            }
        }
        public List<double> this[int r]
        {
            get
            {
                return ViewRow(r);
            }
        }
        public double Get(int row, int column)
        {
            if (column < 0 || column >= columns || row < 0 || row >= rows)
                throw new IndexOutOfRangeException("row:" + row + ", column:" + column);
            return GetQuick(row, column);
        }
        /// <summary>
        /// Returns the matrix cell value at coordinate (row,column).
        /// This method may return invalid objects without throwing any exception.
        /// If you are not sure if your coordinates are within bound use Get instead!
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        private double GetQuick(int row, int column)
        {
            return data[row * rowStride + column * columnStride];
        }
        public void Set(int row, int column, double value)
        {
            if (column < 0 || column >= columns || row < 0 || row >= rows)
                throw new IndexOutOfRangeException("row:" + row + ", column:" + column);
            SetQuick(row, column, value);
        }
        /// <summary>
        /// Sets the matrix cell value at coordinate (row,column).
        /// Provided with invalid parameters this method may access illegal indexes without throwing any exception.
        /// If you are not sure if your coordinates are within bound use Set instead!
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private void SetQuick(int row, int column, double value)
        {
            data[row * rowStride + column * columnStride] = value;
        }
        private void SetUp(int rows, int columns)
        {
            SetUp(rows, columns, columns, 1);
        }
        private void SetUp(int rows, int columns, int rowStride, int columnStride)
        {
            if (rows < 0 || columns < 0)
                throw new ArgumentException("negative size");
            this.rows = rows;
            this.columns = columns;

            this.rowStride = rowStride;
            this.columnStride = columnStride;
        }
        public int Rows() { return this.rows; }
        public int Columns() { return this.columns; }
        public int Index(int row, int column)
        {
            return row * rowStride + column * columnStride;
        }
        public Matrix2d Transpose()
        {
            return this.Copy().vDice();
        }
        public Matrix2d Copy()
        {
            return new Matrix2d(rows, columns, this.data.Select(x => x).ToList());
        }
        public Matrix2d vDice()
        {
            int tmp;
            // swap;
            tmp = rows;
            rows = columns;
            columns = tmp;
            tmp = rowStride;
            rowStride = columnStride;
            columnStride = tmp;
            return this;
        }
        public Matrix2d Assign(double value)
        {
            this.data = Enumerable.Range(0, (Rows() * Columns())).Select(x => value).ToList();
            return this;
        }
        public Matrix2d Assign(Func<double, Int32, double> func)
        {
            this.data = data.Select(func).ToList();
            return this;
        }
        public Matrix2d Assign(Func<double, double> func)
        {
            this.data = data.Select(func).ToList();
            return this;
        }
        public List<double> ViewRow(int row)
        {
            var values = new List<double>();
            int viewSize = this.columns;
            int viewZero = Index(row, 0);
            int viewStride = this.columnStride;
            for (int i = 0; i < viewSize; i++)
            {
                values.Add(this.data[viewZero + i * viewStride]);
            }
            return values;
        }
        public List<double> ViewColumn(int column)
        {
            var values = new List<double>();
            int viewSize = this.rows;
            int viewZero = (int)Index(0, column);
            int viewStride = this.rowStride;
            for (int i = 0; i < viewSize; i++)
            {
                values.Add(this.data[viewZero + i * viewStride]);
            }
            return values;
        }
        public override String ToString()
        {
            var s = new List<string>();
            for (int i = 0; i < rows; i++)
            {
                s.Add(string.Join(" ", ViewRow(i).ToArray()));
            }
            return string.Join(System.Environment.NewLine, s.ToArray());
        }
        public double[][] ToArray()
        {
            double[][] values = new double[rows][];
            int idx = Index(0, 0);
            for (int r = 0; r < rows; r++)
            {
                values[r] = new double[columns];
                double[] currentRow = values[r];
                for (int i = idx, c = 0; c < columns; c++)
                {
                    currentRow[c] = this.data[i];
                    i += columnStride;
                }
                idx += rowStride;
            }
            return values;
        }
        public List<double> ToList()
        {
            return new List<double>(this.data);
        }
        public String Shape()
        {
            return Rows() + " x " + Columns() + " matrix";
        }
        public double Sum()
        {
            return this.data.Sum();
        }
        public Matrix2d Dot(Matrix2d m)
        {
            if (this.Columns() != m.Rows()) throw new ArgumentException("Columns of m1 must match rows of m2.");
            var mOut = new Matrix2d(this.Rows(), m.Columns(), 0);
            for (int i = 0; i < mOut.Rows(); i++)
            {
                for (int j = 0; j < mOut.Columns(); j++)
                {
                    double sum = 0;
                    for (int k = 0; k < this.Columns(); k++)
                    {
                        sum += this[i, k] * m[k, j];
                    }
                    mOut[i, j] = sum;
                }
            }
            return mOut;
        }
        public static Matrix2d operator *(Matrix2d m, double num)
        {
            return m.Assign(value => value * num);
        }
        public static Matrix2d operator *(double num, Matrix2d m)
        {
            return m.Assign(value => value * num);
        }
        public static Matrix2d operator *(Matrix2d m1, Matrix2d m2)
        {
            if (m1.Rows() != m2.Rows() || m1.Columns() != m2.Columns()) throw new ArgumentException("Columns and Rows of m1 must match Columns and Rows of m2.");
            return new Matrix2d(m1.Rows(), m1.Columns(), m1.data.Zip(m2.data, (data1, data2) => data1 * data2).ToList());
        }
        public static Matrix2d operator -(Matrix2d m, double num)
        {
            return m.Assign(value => value - num);
        }
        public static Matrix2d operator -(Matrix2d m1, Matrix2d m2)
        {
            if (m1.Rows() != m2.Rows() || m1.Columns() != m2.Columns()) throw new ArgumentException("Columns and Rows of m1 must match Columns and Rows of m2.");
            return new Matrix2d(m1.Rows(), m1.Columns(), m1.data.Zip(m2.data, (data1, data2) => data1 - data2).ToList());
        }
        public static Matrix2d operator +(Matrix2d m, double num)
        {
            return m.Assign(value => value + num);
        }
        public static Matrix2d operator +(Matrix2d m1, Matrix2d m2)
        {
            if (m1.Rows() != m2.Rows() || m1.Columns() != m2.Columns()) throw new ArgumentException("Columns and Rows of m1 must match Columns and Rows of m2.");
            return new Matrix2d(m1.Rows(), m1.Columns(), m1.data.Zip(m2.data, (data1, data2) => data1 + data2).ToList());
        }
        public static Matrix2d operator /(Matrix2d m, double num)
        {
            return m.Assign(value => value / num);
        }
        public static Matrix2d operator /(Matrix2d m1, Matrix2d m2)
        {
            if (m1.Rows() != m2.Rows() || m1.Columns() != m2.Columns()) throw new ArgumentException("Columns and Rows of m1 must match Columns and Rows of m2.");
            return new Matrix2d(m1.Rows(), m1.Columns(), m1.data.Zip(m2.data, (data1, data2) => data1 / data2).ToList());
        }
    }
}
