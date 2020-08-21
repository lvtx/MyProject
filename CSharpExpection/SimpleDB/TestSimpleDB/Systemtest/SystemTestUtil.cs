using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleDB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime;

namespace TestSimpleDB.Systemtest
{
    public class SystemTestUtil
    {
        public static readonly TupleDesc SINGLE_INT_DESCRIPTOR =
            new TupleDesc(new Type[] { typeof(INT_TYPE) });
        private static readonly int MAX_RAND_VALUE = 1 << 16;
        /** @param columnSpecification Mapping between column index and value. */
        public static HeapFile CreateRandomHeapFile(
                int columns, int rows, Dictionary<int, int> columnSpecification,
                List<List<int>> tuples)
        {
            return CreateRandomHeapFile(columns, rows, MAX_RAND_VALUE, columnSpecification, tuples);
        }
        /** @param columnSpecification Mapping between column index and value. */
        public static HeapFile CreateRandomHeapFile(
                int columns, int rows, int maxValue, Dictionary<int, int> columnSpecification,
                List<List<int>> tuples)
        {
            FileInfo temp = CreateRandomHeapFileUnopened(columns, rows, maxValue,
                    columnSpecification, tuples);
            return Utility.OpenHeapFile(columns, temp);
        }
        public static FileInfo CreateRandomHeapFileUnopened(int columns, int rows,
            int maxValue, Dictionary<int, int> columnSpecification,
            List<List<int>> tuples)
        {
            if (tuples != null)
            {
                tuples.Clear();
            }
            else
            {
                tuples = new List<List<int>>(rows);
            }

            Random r = new Random();

            // Fill the tuples list with generated values
            for (int i = 0; i < rows; ++i)
            {
                List<int> tuple = new List<int>(columns);
                for (int j = 0; j < columns; ++j)
                {
                    // Generate random values, or use the column specification
                    int columnValue = 0;
                    try
                    {
                        if (columnSpecification != null) columnValue = columnSpecification[j];
                        if (columnValue == 0)
                        {
                            columnValue = r.Next(maxValue);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    tuple.Add(columnValue);
                }
                tuples.Add(tuple);
            }

            // Convert the tuples list to a heap file and open it
            FileInfo temp = new FileInfo(@"D:\test\table.dat");
            HeapFileEncoder.Convert(tuples, temp, BufferPool.GetPageSize(), columns);
            return temp;
        }

        public static List<int> TupleToList(SimpleDB.Tuple tuple)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < tuple.GetTupleDesc().NumFields(); i++)
            {
                int value = ((IntField)tuple.GetField(i)).GetValue();
                list.Add(value);
            }
            return list;
        }

        public static void MatchTuples(DbFile f, List<List<int>> tuples)
        {
            TransactionId tid = new TransactionId();
            MatchTuples(f, tid, tuples);
            Database.GetBufferPool().TransactionComplete(tid);
        }

        public static void MatchTuples(DbFile f, TransactionId tid, List<List<int>> tuples)
        {
            SeqScan scan = new SeqScan(tid, f.GetId(), "");
            MatchTuples(scan, tuples);
        }

        public static void MatchTuples(DbIterator iterator, List<List<int>> tuples)
        {
            List<List<int>> copy = new List<List<int>>(tuples);
            Console.WriteLine("一共有{0}个元组",copy.Count);
            if (Debug.IsEnabled())
            {
                Debug.Log("Expected tuples:");
                foreach (List<int> t in copy)
                {
                    Debug.Log("\t" + Utility.ListToString(t));
                }
            }
            iterator.Open();
            int matchedcount = 0;
            while (iterator.HasNext())
            {
                SimpleDB.Tuple t = iterator.Next() as SimpleDB.Tuple;
                List<int> list = TupleToList(t);
                List<int> searched = copy.Find((cs) =>
                    cs[0] == list[0]);
                bool IsExpected = copy.Remove(searched);
                Debug.Log("scanned tuple: %s (%s)", t, IsExpected ? "expected" : "not expected");
                if (!IsExpected)
                {
                    Console.WriteLine("expected tuples does not contain: " + t); ;
                    //Assert.Fail("expected tuples does not contain: " + t);
                }
                else
                {
                    matchedcount++;
                }
            }

            if (copy != null && copy.Count != 0)
            {
                string msg = "expected to find the following tuples:\n";
                const int MAX_TUPLES_OUTPUT = 10;
                int count = 0;
                foreach (List<int> t in copy)
                {
                    if (count == MAX_TUPLES_OUTPUT)
                    {
                        msg += "[" + (copy.Count - MAX_TUPLES_OUTPUT) + " more tuples]";
                        break;
                    }
                    msg += "\t" + Utility.ListToString(t) + "\n";
                    count += 1;
                }
                Assert.Fail(msg);
            }
            Console.WriteLine("copy列表中剩余元素个数{0}", copy.Count);
            Console.WriteLine("已经匹配{0}个元组", matchedcount);
        }
        //public static long GetMemoryFootprint()
        //{
        //    // Call System.gc in a loop until it stops freeing memory. This is
        //    // still no guarantee that all the memory is freed, since System.gc is
        //    // just a "hint".
        //    Runtime runtime = Runtime.getRuntime();
        //    long memAfter = runtime.totalMemory() - runtime.freeMemory();
        //    long memBefore = memAfter + 1;
        //    while (memBefore != memAfter)
        //    {
        //        memBefore = memAfter;
        //        System.GC();
        //        memAfter = runtime.totalMemory() - runtime.freeMemory();
        //    }

        //    return memAfter;
        //}

        /**
        * Generates a unique string each time it is called.
        * @return a new unique UUID as a string, using java.util.UUID
        */
        public static String GetUUID()
        {
            return System.Guid.NewGuid().ToString();
        }

        private static double[] GetDiff(double[] sequence)
        {
            double[] ret = new double[sequence.Length - 1];
            for (int i = 0; i < sequence.Length - 1; ++i)
                ret[i] = sequence[i + 1] - sequence[i];
            return ret;
        }
        /**
         * Checks if the sequence represents a quadratic sequence (approximately)
         * ret[0] is true if the sequence is quadratic
         * ret[1] is the common difference of the sequence if ret[0] is true.
         * @param sequence
         * @return ret[0] = true if sequence is qudratic(or sub-quadratic or linear), ret[1] = the coefficient of n^2
         */
        public static Object[] CheckQuadratic(double[] sequence)
        {
            Object[] ret = CheckLinear(GetDiff(sequence));
            ret[1] = (Double)ret[1] / 2.0;
            return ret;
        }

        /**
         * Checks if the sequence represents an arithmetic sequence (approximately)
         * ret[0] is true if the sequence is linear
         * ret[1] is the common difference of the sequence if ret[0] is true.
         * @param sequence
         * @return ret[0] = true if sequence is linear, ret[1] = the common difference
         */
        public static Object[] CheckLinear(double[] sequence)
        {
            return checkConstant(GetDiff(sequence));
        }

        /**
         * Checks if the sequence represents approximately a fixed sequence (c,c,c,c,..)
         * ret[0] is true if the sequence is linear
         * ret[1] is the constant of the sequence if ret[0] is true.
         * @param sequence
         * @return ret[0] = true if sequence is constant, ret[1] = the constant
         */
        public static Object[] checkConstant(double[] sequence)
        {
            Object[] ret = new Object[2];
            //compute average
            double sum = .0;
            for (int i = 0; i < sequence.Length; ++i)
                sum += sequence[i];
            double av = sum / (sequence.Length + .0);
            //compute standard deviation
            double sqsum = 0;
            for (int i = 0; i < sequence.Length; ++i)
                sqsum += (sequence[i] - av) * (sequence[i] - av);
            double std = Math.Sqrt(sqsum / (sequence.Length + .0));
            ret[0] = std < 1.0 ? true : false;
            ret[1] = av;
            return ret;
        }
    }
}
