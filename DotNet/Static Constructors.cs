using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Main");

        // The first time you instantiate an object of a type, the static constructor runs first, one time,
        // followed by the object's constructor
        new Foo().Go();
        Foo.StaticGo();
        Foo.StaticGo();
        /* Output:
        Foo static constructor
        Foo constructor
        Foo Go
        Foo StaticGo
        Foo StaticGo
        */

        // Or if you call a static method:
        Job.StaticGo();
        /* Output:
        Job static constructor
        Job StaticGo
        */

        // For generic types, a static constructor fires once for every different generic type you pass in
        new Bar<int>().Go();
        new Bar<string>().Go();
        Bar<int>.StaticGo();
        Bar<string>.StaticGo();
        /* Output:
        Bar static constructor for type Int32
        Bar constructor for type Int32
        Bar Go for type Int32
        Bar static constructor for type String
        Bar constructor for type String
        Bar Go for type String
        Bar StaticGo for type Int32
        Bar StaticGo for type String
        */

        // When the static constructor fires for a child type, the parent type's static constructor runs immediately
        // afterward, assuming it hasn't already fired before
        Console.WriteLine("Calling new Child()");
        new Child();
        Console.WriteLine("Calling new Parent()");
        new Parent();
        /* Output:
        Calling new Child()
        Child static constructor
        Parent static constructor
        Calling new Parent()
        */

        // For generics, the static constructor of the generic type is not fired
        Console.WriteLine("Creating list of Something");
        var somethings = new List<Something>();
        Console.WriteLine("Adding a new Something to the list");
        somethings.Add(new Something());
        /* Output:
        Creating list of Something
        Adding a new Something to the list
        Something static constructor
        */

        // Static fields get a value as part of the static constructor, the same way instance fields do
        new Radio();
        /* Output
        GetZero
        Radio static constructor
        */
    }
}

class Radio
{
    private static readonly int _zero = GetZero();

    static Radio() => Console.WriteLine("Radio static constructor");

    private static int GetZero() { Console.WriteLine("GetZero"); return 0; }
}

class Foo
{
    static Foo() => Console.WriteLine("Foo static constructor");
    public Foo() => Console.WriteLine("Foo constructor");
    public void Go() => Console.WriteLine("Foo Go");
    public static void StaticGo() => Console.WriteLine("Foo StaticGo");
}

class Bar<T>
{
    static Bar() => Console.WriteLine($"Bar static constructor for type {typeof(T).Name}");
    public Bar() => Console.WriteLine($"Bar constructor for type {typeof(T).Name}");
    public void Go() => Console.WriteLine($"Bar Go for type {typeof(T).Name}");
    public static void StaticGo() => Console.WriteLine($"Bar StaticGo for type {typeof(T).Name}");
}

class Parent
{
    static Parent() => Console.WriteLine("Parent static constructor");
}

class Child : Parent
{
    static Child() => Console.WriteLine("Child static constructor");
}

class Something { static Something() => Console.WriteLine("Something static constructor"); }

static class Job
{
    static Job() => Console.WriteLine("Job static constructor");
    public static void StaticGo() => Console.WriteLine("Job StaticGo");
}