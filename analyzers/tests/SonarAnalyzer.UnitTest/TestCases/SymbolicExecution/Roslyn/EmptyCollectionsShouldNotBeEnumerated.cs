﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

class CollectionTests
{
    private IEnumerable<int> items = new List<int> { 1, 2, 3 };
    private IDictionary<int, int> dictionaryItems = new Dictionary<int, int>();

    private static bool Predicate(int i) => true;
    private static void Action(int i) { }
    private List<int> GetList() => null;
    private HashSet<int> GetSet() => null;
    private Queue<int> GetQueue() => null;
    private Stack<int> GetStack() => null;
    private ObservableCollection<int> GetObservableCollection() => null;
    private int[] GetArray() => null;
    private Dictionary<int, int> GetDictionary() => null;

    public void DefaultConstructor()
    {
        var list = new List<int>();
        list.Clear();                   // Noncompliant {{Remove this call, the collection is known to be empty here.}}
//      ^^^^^^^^^^^^
        var set = new HashSet<int>();
        set.Clear();                    // Noncompliant
        var queue = new Queue<int>();
        queue.Clear();                  // Noncompliant
        var stack = new Stack<int>();
        stack.Clear();                  // Noncompliant
        var obs = new ObservableCollection<int>();
        obs.Clear();                    // Noncompliant
        var array = new int[0];
        array.Clone();                  // Noncompliant
        var dict = new Dictionary<int, int>();
        dict.Clear();                   // Noncompliant
    }

    public void ConstructorWithCapacity()
    {
        var list = new List<int>(5);
        list.Clear();                   // Noncompliant
        var set = new HashSet<int>(5);
        set.Clear();                    // Noncompliant
        var queue = new Queue<int>(5);
        queue.Clear();                  // Noncompliant
        var stack = new Stack<int>(5);
        stack.Clear();                  // Noncompliant
        var dict = new Dictionary<int, int>(5);
        dict.Clear();                   // Noncompliant
    }

    public void ArrayWithCapacity()
    {
        int zero = 0;
        int five = 5;
        const int ZERO = 0;
        const int FIVE = 5;

        var array = new int[5];
        array.Clone();                  // Compliant
        array = new int[2 + 3];
        array.Clone();                  // Compliant
        array = new int[five];
        array.Clone();                  // Compliant
        array = new int[FIVE];
        array.Clone();                  // Compliant
        array = new int[0];
        array.Clone();                  // Noncompliant
        array = new int[2 - 2];
        array.Clone();                  // Noncompliant
        array = new int[zero];
        array.Clone();                  // FN
        array = new int[ZERO];
        array.Clone();                  // Noncompliant
    }

    public void ConstructorWithEnumerable()
    {
        var list = new List<int>(items);
        list.Clear();                   // Compliant
        var set = new HashSet<int>(items);
        set.Clear();                    // Compliant
        set = new HashSet<int>(items, EqualityComparer<int>.Default);
        set.Clear();                    // Compliant
        set = new HashSet<int>(comparer: EqualityComparer<int>.Default, collection: items);
        set.Clear();                    // Compliant
        var queue = new Queue<int>(items);
        queue.Clear();                  // Compliant
        var stack = new Stack<int>(items);
        stack.Clear();                  // Compliant
        var obs = new ObservableCollection<int>(items);
        obs.Clear();                    // Compliant
        var dict = new Dictionary<int, int>(dictionaryItems);
        dict.Clear();                   // Compliant
    }

    public void ConstructorWithEnumerableWithConstraint(bool condition)
    {
        var baseCollection = new List<int>();
        var set = new HashSet<int>(baseCollection);
        set.Clear();                    // Noncompliant

        set = new HashSet<int>(baseCollection, EqualityComparer<int>.Default);
        set.Clear();                    // Noncompliant

        set = new HashSet<int>(comparer: EqualityComparer<int>.Default, collection: baseCollection);
        set.Clear();                    // Noncompliant

        set = new HashSet<int>(condition ? baseCollection : baseCollection);
        set.Clear();                    // Noncompliant

        baseCollection.Add(1);
        set = new HashSet<int>(baseCollection);
        set.Clear();                    // Compliant
    }

    public void ConstructorWithEmptyInitializer()
    {
        var list = new List<int> { };
        list.Clear();                   // Noncompliant
        var set = new HashSet<int> { };
        set.Clear();                    // Noncompliant
        var queue = new Queue<int> { };
        queue.Clear();                  // Noncompliant
        var stack = new Stack<int> { };
        stack.Clear();                  // Noncompliant
        var obs = new ObservableCollection<int> { };
        obs.Clear();                    // Noncompliant
        var array = new int[] { };
        array.Clone();                  // Noncompliant
        var dict = new Dictionary<int, int> { };
        dict.Clear();                   // Noncompliant
    }

    public void ConstructorWithInitializer()
    {
        var list = new List<int> { 1, 2, 3 };
        list.Clear();                   // Compliant
        var set = new HashSet<int> { 1, 2, 3 };
        set.Clear();                    // Compliant
        var obs = new ObservableCollection<int> { 1, 2, 3 };
        obs.Clear();                    // Compliant
        var array = new int[] { 1, 2, 3 };
        array.Clone();                  // Compliant
        var dict = new Dictionary<int, int>
        {
            [1] = 1,
            [2] = 2,
            [3] = 3,
        };
        dict.Clear();                   // Noncompliant FP
    }

    public void Other_Initialization()
    {
        var list = GetList();
        list.Clear();                   // Compliant
        var set = GetSet();
        set.Clear();                    // Compliant
        var queue = GetQueue();
        queue.Clear();                  // Compliant
        var stack = GetStack();
        stack.Clear();                  // Compliant
        var obs = GetObservableCollection();
        obs.Clear();                    // Compliant
        var array = GetArray();
        array.Clone();                  // Compliant
        var dict = GetDictionary();
        dict.Clear();                   // Compliant

        array = Array.Empty<int>();
        array.Clone();                  // FN
        var enumerable = Enumerable.Empty<int>();
        enumerable.GetEnumerator();     // FN
    }

    public void Methods_Raise_Issue()
    {
        int i;

        var list = new List<int>();
        list.BinarySearch(5);           // Noncompliant
        list.Clear();                   // Noncompliant
        list.Contains(5);               // Noncompliant
        list.ConvertAll(x => x);        // Noncompliant
        list.CopyTo(null, 1);           // Noncompliant
        list.Exists(Predicate);         // Noncompliant
        list.Find(Predicate);           // Noncompliant
        list.FindAll(Predicate);        // Noncompliant
        list.FindIndex(Predicate);      // Noncompliant
        list.FindLast(Predicate);       // Noncompliant
        list.FindLastIndex(Predicate);  // Noncompliant
        list.ForEach(Action);           // Noncompliant
        list.GetEnumerator();           // Noncompliant
        list.GetRange(1, 5);            // Noncompliant
        list.IndexOf(5);                // Noncompliant
        list.LastIndexOf(5);            // Noncompliant
        list.Remove(5);                 // Noncompliant
        list.RemoveAll(Predicate);      // Noncompliant
        list.RemoveAt(1);               // Noncompliant
        list.RemoveRange(1, 5);         // Noncompliant
        list.Reverse();                 // Noncompliant
        list.Sort();                    // Noncompliant
        list.TrueForAll(Predicate);     // Noncompliant
        _ = list[1];                    // Compliant, should be part of S6466
        list[1] = 5;                    // Compliant, should be part of S6466

        var set = new HashSet<int>();
        set.Clear();                    // Noncompliant
        set.Contains(5);                // Noncompliant
        set.CopyTo(null, 1);            // Noncompliant
        set.ExceptWith(items);          // Noncompliant
        set.GetEnumerator();            // Noncompliant
        set.IntersectWith(items);       // Noncompliant
        set.IsProperSubsetOf(items);    // Noncompliant
        set.IsProperSupersetOf(items);  // Noncompliant
        set.IsSubsetOf(items);          // Noncompliant
        set.IsSupersetOf(items);        // Noncompliant
        set.Overlaps(items);            // Noncompliant
        set.Remove(5);                  // Noncompliant
        set.RemoveWhere(Predicate);     // Noncompliant
        set.SymmetricExceptWith(items); // Noncompliant, also learns NotEmpty
        set = new HashSet<int>();
        set.TryGetValue(5, out i);      // Noncompliant
        set.UnionWith(items);           // Noncompliant, also learns NotEmpty

        var queue = new Queue<int>();
        queue.Clear();                  // Noncompliant
        queue.Contains(5);              // Noncompliant
        queue.CopyTo(null, 1);          // Noncompliant
        queue.Dequeue();                // Noncompliant
        queue.GetEnumerator();          // Noncompliant
        queue.Peek();                   // Noncompliant

        var stack = new Stack<int>();
        stack.Clear();                  // Noncompliant
        stack.Contains(5);              // Noncompliant
        stack.CopyTo(null, 0);          // Noncompliant
        stack.GetEnumerator();          // Noncompliant
        stack.Peek();                   // Noncompliant
        stack.Pop();                    // Noncompliant

        var obs = new ObservableCollection<int>();
        obs.Clear();                    // Noncompliant
        obs.Contains(5);                // Noncompliant
        obs.CopyTo(null, 1);            // Noncompliant
        obs.GetEnumerator();            // Noncompliant
        obs.IndexOf(5);                 // Noncompliant
        obs.Move(1, 2);                 // Noncompliant
        obs.Remove(5);                  // Noncompliant
        obs.RemoveAt(1);                // Noncompliant
        _ = obs[1];                     // Compliant, should be part of S6466
        obs[1] = 5;                     // Compliant, should be part of S6466

        var array = new int[0];
        array.Clone();                  // Noncompliant
        array.CopyTo(null, 1);          // Noncompliant
        array.GetEnumerator();          // Noncompliant
        array.GetLength(1);             // Noncompliant
        array.GetLongLength(1);         // Noncompliant
        array.GetLowerBound(1);         // Noncompliant
        array.GetUpperBound(1);         // Noncompliant
        array.GetValue(1);              // Noncompliant
        array.Initialize();             // Noncompliant
        array.SetValue(5, 1);           // Noncompliant
        _ = array[1];                   // Compliant, should be part of S6466
        array[1] = 5;                   // Compliant, should be part of S6466

        var dict = new Dictionary<int, int>();
        dict.Clear();                   // Noncompliant
        dict.ContainsKey(1);            // Noncompliant
        dict.ContainsValue(5);          // Noncompliant
        dict.GetEnumerator();           // Noncompliant
        dict.Remove(5);                 // Noncompliant
        dict.TryGetValue(1, out i);     // Noncompliant
        _ = dict[1];                    // Compliant, should be part of S6466
    }

    public void Methods_Ignored()
    {
        int i;

        var list = new List<int>();
        list.AsReadOnly();
        list.GetHashCode();
        list.GetType();
        list.Equals(items);
        list.ToString();
        list.TrimExcess();
        list.ToArray();

        var set = new HashSet<int>();
        set.Equals(items);
        set.GetHashCode();
        set.GetObjectData(null, new StreamingContext());
        set.GetType();
        set.OnDeserialization(null);
        set.SetEquals(items);
        set.TrimExcess();

        var queue = new Queue<int>();
        queue.Equals(items);
        queue.GetHashCode();
        queue.GetType();
        queue.ToArray();
        queue.ToString();
        queue.TrimExcess();

        var stack = new Stack<int>();
        stack.GetHashCode();
        stack.Equals(items);
        stack.GetType();
        stack.ToArray();
        stack.ToString();
        stack.TrimExcess();

        var obs = new ObservableCollection<int>();
        obs.GetHashCode();
        obs.Equals(items);
        obs.GetType();
        obs.ToString();
        _ = obs.Count;
        obs.CollectionChanged += (s, e) => throw new NotImplementedException();

        var array = new int[0];
        array.GetHashCode();
        array.Equals(new object());
        array.GetType();
        array.ToString();
        _ = array.Length;

        var dict = new Dictionary<int, int>();
        dict.GetHashCode();
        dict.GetObjectData(null, new StreamingContext());
        dict.Equals(items);
        dict.GetType();
        dict.OnDeserialization(null);
        dict.ToString();
        dict[5] = 5;
        (((dict[5]))) = 5;
    }

    public void Methods_Set_NotEmpty()
    {
        var list = new List<int>();
        list.Add(5);
        list.Clear();                   // Compliant
        list = new List<int>();
        list.AddRange(items);
        list.Clear();                   // Compliant
        list = new List<int>();
        list.Insert(1, 5);
        list.Clear();                   // Compliant
        list = new List<int>();
        list.InsertRange(1, items);
        list.Clear();                   // Compliant

        var set = new HashSet<int>();
        set.Add(1);
        set.Clear();                    // Compliant
        set = new HashSet<int>();
        set.SymmetricExceptWith(items); // Noncompliant
        set.Clear();                    // Compliant
        set = new HashSet<int>();
        set.UnionWith(items);           // Noncompliant
        set.Clear();                    // Compliant

        var queue = new Queue<int>();
        queue.Enqueue(5);
        queue.Clear();                  // Compliant

        var stack = new Stack<int>();
        stack.Push(5);
        stack.Clear();                  // Compliant

        var obs = new ObservableCollection<int>();
        obs.Add(5);
        obs.Clear();                    // Compliant
        obs = new ObservableCollection<int>();
        obs.Insert(0, 5);
        obs.Clear();                    // Compliant

        var dict = new Dictionary<int, int>();
        dict.Add(1, 5);
        dict.Clear();                   // Compliant
    }

    public void Method_Set_Empty(List<int> list, HashSet<int> set, Queue<int> queue, Stack<int> stack, ObservableCollection<int> obs, Dictionary<int, int> dict)
    {
        list.Clear();                   // Compliant
        list.Clear();                   // FN

        set.Clear();                    // Compliant
        set.Clear();                    // FN

        queue.Clear();                  // Compliant
        queue.Clear();                  // FN

        stack.Clear();                  // Compliant
        stack.Clear();                  // FN

        obs.Clear();                    // Compliant
        obs.Clear();                    // FN

        dict.Clear();                   // Compliant
        dict.Clear();                   // FN

        var empty = new List<int>();
        list.Add(5);
        list.Intersect(empty);          // Compliant
        list.Clear();                   // FN
    }
}

class AdvancedTests
{
    public void UnknownExtensionMethods()
    {
        var list = new List<int>();
        list.CustomExtensionMethod();                       // Compliant
        list.Clear();                                       // Noncompliant FP
    }

    public void WellKnownExtensionMethods()
    {
        var list = new List<int>();
        list.All(x => true);                                // FN
        list.Any();                                         // FN
        list.AsEnumerable();
        list.AsQueryable();
        list.AsReadOnly();
        list.Average();                                     // FN
        list.Cast<byte>();                                  // FN
        list.Concat(list);                                  // FN
        list.Contains(5, EqualityComparer<int>.Default);    // FN
        list.Count();                                       // FN
        list.DefaultIfEmpty();                              // FN
        list.Distinct();                                    // FN
        list.Except(list);                                  // FN
        list.First();                                       // FN
        list.FirstOrDefault();                              // FN
        list.GroupBy(x => x);
        list.GroupJoin(list, x => x, x => x, (x, y) => x);  // FN
        list.Intersect(list);                               // FN
        list.Join(list, x => x, x => x, (x, y) => x);       // FN
        list.Last();                                        // FN
        list.LastOrDefault();                               // FN
        list.LongCount();                                   // FN
        list.Max();                                         // FN
        list.Min();                                         // FN
        list.OfType<int>();                                 // FN
        list.OrderBy(x => x);
        list.OrderByDescending(x => x);
        list.Select(x => x);                                // FN
        list.SelectMany(x => new int[5]);                   // FN
        list.SequenceEqual(list);                           // FN
        list.Single();                                      // FN
        list.SingleOrDefault();                             // FN
        list.Skip(1);                                       // FN
        list.SkipWhile(x => true);                          // FN
        list.Sum();                                         // FN
        list.Take(1);                                       // FN
        list.TakeWhile(x => true);                          // FN
        list.ToArray();                                     // FN
        list.ToDictionary(x => x);                          // FN
        list.ToList();                                      // FN
        list.ToLookup(x => x);
        list.Union(list);                                   // FN
        list.Where(x => true);                              // FN
        list.Zip(list, (x, y) => x);                        // FN
        Enumerable.Reverse(list);                           // FN
        list.Clear();                                       // Noncompliant
    }

    public void PassingAsArgument_Removes_Constraints()
    {
        var list = new List<int>();
        Foo(list);
        list.Clear();   // Noncompliant FP
    }

    public void HigherRank_And_Jagged_Array()
    {
        var array1 = new int[0, 0];
        array1.Clone(); // Noncompliant
        var array2 = new int[0, 4];
        array2.Clone(); // Noncompliant
        var array3 = new int[5, 4];
        array3.Clone(); // Compliant
        int[][] array4 = new int[0][];
        array4.Clone(); // Noncompliant
        int[][] array5 = new int[1][];
        array5.Clone(); // Compliant
    }

    void Foo(IEnumerable<int> items) { }
}

static class CustomExtensions
{
    public static void CustomExtensionMethod(this List<int> list) { }
}

// This simulates the Dictionary from .NetCore 2.0+.
public class NetCoreDictionary<TKey, TValue> : Dictionary<TKey, TValue>
{
    public void TestTryAdd()
    {
        var dict = new NetCoreDictionary<string, object>();
        if (dict.TryAdd("foo", new object()))   // Compliant
        {
        }
    }

    public bool TryAdd(TKey key, TValue value)
    {
        return true;
    }
}

class Flows
{
    public void Conditional_Add(bool condition)
    {
        var list = new List<int>();
        if (condition)
        {
            list.Add(5);
        }
        list.Clear();   // Compliant
    }

    public void Conditional_Add_With_Loop(bool condition)
    {
        var list = new List<int>();
        while (true)
        {
            if (condition)
            {
                list.Add(5);
                break;
            }
        }
        list.Clear();   // Compliant
    }

    // https://github.com/SonarSource/sonar-dotnet/issues/4261
    public void AddPassedAsParameter()
    {
        var list = new List<int>();
        DoSomething(list.Add);
        list.Clear();   // Compliant

        list = new List<int>();
        DoSomething(list.Clear); // Noncompliant

        DoSomething(StaticMethodWithoutInstance);

        list = new List<int>();
        DoSomething(x => list.Add(x));
        list.Clear();    // Noncompliant FP, we don't analyze sub CFGs for lambdas

        list = new List<int>();
        Action<int> add = list.Add;
        list.Clear();    // FN
        add(5);
        list.Clear();    // Compliant, but will break when we learn Empty from Clear()

        list = new List<int>();
        Action clear = list.Clear;  // Noncompliant FP
        clear();                    // FN
        clear();                    // FN

        list = new List<int> { 42 };
        clear = list.Clear;         // Compliant
        clear();                    // Compliant
        add(5);                     // Adds to another instance, not the current list
        clear();                    // FN
    }

    private static void StaticMethodWithoutInstance() { }

    public void Count()
    {
        var list = GetList();
        if (list.Count == 0)
            list.Clear();       // FN
        else
            list.Clear();       // Compliant

        list = GetList();
        if (list.Count() == 0)
            list.Clear();       // FN
        else
            list.Clear();       // Compliant

        list = GetList();
        if (list.Count != 0)
            list.Clear();       // Compliant
        else
            list.Clear();       // FN

        list = GetList();
        if (list.Count() != 0)
            list.Clear();       // Compliant
        else
            list.Clear();       // FN

        list = GetList();
        if (list.Count > 0)
            list.Clear();       // Compliant
        else
            list.Clear();       // FN

        list = GetList();
        if (list.Count() > 0)
            list.Clear();       // Compliant
        else
            list.Clear();       // FN

        list = GetList();
        if (list.Count > 1)
            list.Clear();       // Compliant
        else
            list.Clear();       // Compliant

        list = GetList();
        if (list.Count() > 1)
            list.Clear();       // Compliant
        else
            list.Clear();       // Compliant
    }

    private static void DoSomething(Action<int> callback) { }
    private static void DoSomething(Action callback) { }
    private List<int> GetList() => null;
}


class Flows2
{
    public static string UrlDecode(string s, Encoding e)
    {
        long len = s.Length;
        var bytes = new List<byte>();
        int xchar;
        char ch;

        for (int i = 0; i < len; i++)
        {
            ch = s[i];
            if (ch == '%' && i + 2 < len && s[i + 1] != '%')
            {
                if (s[i + 1] == 'u' && i + 5 < len)
                {
                    // unicode hex sequence
                    xchar = GetChar(s, i + 2, 4);
                    if (xchar != -1)
                    {
                        WriteCharBytes(bytes, (char)xchar, e);
                        i += 5;
                    }
                    else
                        WriteCharBytes(bytes, '%', e);
                }
                else if ((xchar = GetChar(s, i + 1, 2)) != -1)
                {
                    WriteCharBytes(bytes, (char)xchar, e);
                    i += 2;
                }
                else
                {
                    WriteCharBytes(bytes, '%', e);
                }
                continue;
            }

            if (ch == '+')
                WriteCharBytes(bytes, ' ', e);
            else
                WriteCharBytes(bytes, ch, e);
        }

        byte[] buf = bytes.ToArray();
        bytes = null;
        return e.GetString(buf);

    }

    private static void WriteCharBytes(List<byte> bytes, char v, Encoding e)
    {
    }

    private static int GetChar(string s, int v1, int v2)
    {
        return 1;
    }
}

// See https://github.com/SonarSource/sonar-dotnet/issues/1002
class Program
{
    public Program(string id)
    {
        if (string.IsNullOrEmpty(id))
            throw new ArgumentNullException(nameof(id));
    }
}

class LargeCfg
{
    // Large CFG that causes the exploded graph to hit the exploration limit
    // See https://github.com/SonarSource/sonar-dotnet/issues/767 (comments!)
    private static void MethodName(object[] table, object[][] aoTable2, ref Dictionary<double, string> dictPorts)
    {
        try
        {
            string sValue = "-1";
            switch (5)
            {
                case 3:
                    sValue = "";
                    break;
                case 4:
                    sValue = "";
                    break;
                case 5:
                    sValue = "";
                    break;
            }

            var list = new List<double>();
            for (int i = 0; i < table.Length; i++)
            {
                if (Convert.ToInt32(table[3]) == 1 /* Normal */ &&
                    Convert.ToInt32(aoTable2[5][9]) == 1 /* On */)
                {
                    string sValue2;
                    switch (Convert.ToInt32(aoTable2[5][4]))
                    {
                        default:
                            sValue2 = "";
                            break;
                    }

                    if (dictPorts.ContainsKey(5))
                    {
                        list.Add(7);
                    }
                }
                else
                {
                    dictPorts.Add(5, sValue);
                }
            }

            if (list.Count > 0)
            {
                list.Sort();    // Compliant
            }
        }
        catch
        {
            // silently do nothing
        }
    }
}

// https://github.com/SonarSource/sonar-dotnet/issues/4478
public class Repro_4478
{
    public void Main()
    {
        var list = new List<String>();
        AddInLocalFunction();
        list.Clear();   // Noncompliant FP

        void AddInLocalFunction()
        {
            list.Add("Item1");
        }
    }
}

// https://github.com/SonarSource/sonar-dotnet/issues/2147
public class Repro_2147
{
    private List<string> _lines;

    public virtual object Clone()
    {
        var clonedEntry = (Repro_2147)MemberwiseClone();
        clonedEntry._lines = new List<string>();
        _lines.ForEach(x => { });   // Compliant, different field
        return clonedEntry;
    }
}