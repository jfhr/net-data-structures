# net-data-structures

This repository contains implementations of various data structures in .NET, such as links and matrices.
They are designed to be usable from any .NET language.

All the examples shown here are verified with unit tests, see the `NetDataStructures.ReadmeExamples` folder.

## LinkedList&lt;T&gt;

Generic, singly-linked list type.

```CSharp
IList<string> list = new Lists.LinkedList<string>();
list.Add("foo");
Console.WriteLine(list.IndexOf("foo"));  // 0
```

## ArrayList&lt;T&gt;

Generic dynamic array-list.

```CSharp
IList<string> list = new ArrayList<string>(4);
list.Add("foo");
Console.WriteLine(list.Count);  // 1
```

## Matrix

Integer matrix that supports basic mathematical operations.

```CSharp
Matrix even = new Matrix(new int[,] {
    { 2, 2 },
    { 4, 4 },
});

Matrix odd = new Matrix(new int[,] {
    { 1, 1 },
    { 3, 3 },
});

Matrix product = even * odd;

Console.WriteLine(product[1, 1]);  // 16
```
