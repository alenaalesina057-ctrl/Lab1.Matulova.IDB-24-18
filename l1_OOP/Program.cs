using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


//ЗАДАНИЕ 1

public class MySet<T>
{
    public List<T> Elements { get; }
    public MySet(IEnumerable<T> collection)
    {
        Elements = collection.Distinct().ToList();
    }
    //объединение
    public static MySet<T> operator |(MySet<T> set1, MySet<T> set2)
    {
        return new MySet<T>(set1.Elements.Concat(set2.Elements));
    }
    //разность
    public static MySet<T> operator -(MySet<T> set1, MySet<T> set2)
    {
        return new MySet<T>(set1.Elements.Where(e => !set2.Elements.Contains(e)));
    }
    //пересечение
    public static MySet<T> operator &(MySet<T> set1, MySet<T> set2)
    {
        return new MySet<T>(set1.Elements.Where(e => set2.Elements.Contains(e)));
    }
    //симметричное пересечение
    public static MySet<T> operator /(MySet<T> set1, MySet<T> set2)
    {
        var diff1 = set1 - set2;
        var diff2 = set2 - set1;
        return diff1 | diff2;
    }
    //проверка на равенство
    public static bool operator ==(MySet<T> set1, MySet<T> set2)
    {
        if (ReferenceEquals(set1, set2)) return true;
        if (set1 is null || set2 is null) return false;
        return set1.Elements.Count == set2.Elements.Count && !set1.Elements.Except(set2.Elements).Any();
    }
    //проверка на неравенство
    public static bool operator !=(MySet<T> set1, MySet<T> set2) => !(set1 == set2);

    public override string ToString() => "{" + string.Join(", ", Elements) + "}";
}






//ЗАДАНИЕ 2

public interface IVisitor
{
    void VisitParagraph(Paragraph p);
    void VisitImage(Image img);
    void VisitTable(Table t);
}

public abstract class Element
{
    public abstract void Accept(IVisitor visitor);
}

//элемент-текст
public class Paragraph : Element
{
    public string Text { get; set; }
    public override void Accept(IVisitor visitor) => visitor.VisitParagraph(this);
}

//элемент-изображение
public class Image : Element
{
    public string LinqOnImage { get; set; }
    public override void Accept(IVisitor visitor) => visitor.VisitImage(this);
}

//элемент-таблица
public class Table : Element
{
    public string Content { get; set; }
    public override void Accept(IVisitor visitor) => visitor.VisitTable(this);
}

//экспорт в HTML 
public class HtmlVisitor : IVisitor
{
    public void VisitParagraph(Paragraph p) => Console.WriteLine($"<p>{p.Text}</p>");
    public void VisitImage(Image img) => Console.WriteLine($"<img src='{img.LinqOnImage}' />");
    public void VisitTable(Table t) => Console.WriteLine($"<table>{t.Content}</table>");
}

//экспорт в Markdown 
public class MarkdownVisitor : IVisitor
{
    public void VisitParagraph(Paragraph p) => Console.WriteLine(p.Text);
    public void VisitImage(Image img) => Console.WriteLine($"![alt text]({img.LinqOnImage})");
    public void VisitTable(Table t) => Console.WriteLine($"| {t.Content} |");
}

//Сборка документов. 
public class Document
{
    private List<Element> _elements = new();
    public void Add(Element el) => _elements.Add(el);
    public void Export(IVisitor visitor)
    {
        foreach (var el in _elements) el.Accept(visitor);
    }
}








//ЗАДАНИЕ 3

public class Tree<T>
{
    public T Value { get; set; }
    public List<Tree<T>> Children { get; set; } = new(); 
    public Tree(T value) => Value = value;

    // вывод
    public void Print()
    {
        Console.WriteLine(Value);

        if (Children.Count > 0)
        {
            foreach (var child in Children)
            {
                child.Print();
            }
        }
    }
}



class Сheck
{
    static void Main()
    {
        // Задание 1
        var setA = new MySet<int>(new[] { 1, 2, 3, 5 });
        var setB = new MySet<int>(new[] { 3, 4, 5 });
        Console.WriteLine($"Set A: {setA}");
        Console.WriteLine($"Union (|): {setA | setB}"); // {1, 2, 3, 4, 5}
        Console.WriteLine($"Intersection (&): {setA & setB}"); // {3,5}
        Console.WriteLine($"Difference (-): {setA - setB}"); // {1,2}
        Console.WriteLine($"Symmetrical difference (/): {setA / setB}"); // {1,2,4}
        Console.WriteLine($"Equality (==): {setA == setB}"); // {no}
        Console.WriteLine($"Inequality (!=): {setA != setB}"); // {yes}



        //Задание 3
        var root = new Tree<string>("Root");
        var stick1 = new Tree<string>("Stick 1");

        root.Children.Add(stick1);
        root.Children.Add(new Tree<string>("Stick 2"));
        stick1.Children.Add(new Tree<string>("Leaf 1"));
        stick1.Children.Add(new Tree<string>("Leaf 2"));

        root.Print();
    }
}