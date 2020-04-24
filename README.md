# net-data-structures

This repository contains implementations of various data structures in .NET, such as lists and matrices.
They are designed to be usable from any .NET language.

All the examples shown here are verified with unit tests, see the `NetDataStructures.ReadmeExamples` folder.

## Lists

Various generic lists. All types implement the `IList<T>` interface.
See Microsoft's documentation for this interface.

### ArrayList&lt;T&gt;

An implementation similar to the builtin `List<T>` with slight performance improvements.

```CSharp
IList<string> list = new ArrayList<string>(4);
list.Add("foo");
Console.WriteLine(list.Count);  // 1
```

### RecursiveSinglyLinkedList&lt;T&gt;

Significantly worse performance than the builtin `LinkedList<T>`, which is doubly-linked.
Not recommended for real-world use.

```CSharp
IList<string> list = new RecursiveSinglyLinkedList<string>();
list.Add("foo");
Console.WriteLine(list.IndexOf("foo"));  // 0
```

## Matrix

Integer matrix that supports basic mathematical operations, including:

- matrix ± matrix
- matrix * scalar
- matrix * matrix

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
