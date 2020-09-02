# net-data-structures

This repository contains implementations of various data structures in .NET, such as lists and matrices.

## Lists

Various generic lists. All types implement the [`IList<T>`](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1?view=netcore-3.1) interface.

### ArrayList&lt;T&gt;

An implementation similar to the builtin `List<T>`.

```CSharp
IList<string> list = new ArrayList<string>(4);
list.Add("foo");
Console.WriteLine(list.Count);  // 1
```

### RecursiveSinglyLinkedList&lt;T&gt;

Singly-linked list.

```CSharp
IList<string> list = new RecursiveSinglyLinkedList<string>();
list.Add("foo");
Console.WriteLine(list.IndexOf("foo"));  // 0
```

## Vectors & Matrices

Vectors and matrices exist for all underlying numeric types (`Int32Matrix`, `Uint64Vector`, `DoubleMatrix`, etc.) except for `decimal`.
They support basic mathematical operations, such as:

- matrix ± matrix
- matrix * scalar
- matrix * vector
- matrix * matrix

```CSharp
var even = new Int32Matrix(new int[,] {
    { 2, 2 },
    { 4, 4 },
});

var odd = new Int32Matrix(new int[,] {
    { 1, 1 },
    { 3, 3 },
});

Int32Matrix product = even * odd;

Console.WriteLine(product[1, 1]);  // 16
```
