using System;
using System.Collections.Generic;
using System.Text;

namespace DelegatePractice
{
    internal class DelegateEgs
    {

        //1.What is a delegate?
        //Definition: A delegate is a type-safe reference to a method.It specifies the method's parameter types and return type, allowing methods with a matching signature to be referenced and invoked through the delegate.
        //In simple terms, a delegate allows you to store a method reference in a variable and call that method through the variable.

      
            // Example - 1
            public delegate void delegateName(string msg);

            public void Display(string msg)
            {
                Console.WriteLine(msg);
            }

            public void Eg1()
            {
                delegateName a = Display;
                a("hello");

            }


            // Example - 2 

            //Action<string> saves you from declaring a custom delegate
            //when you only need a delegate that accepts a string and returns nothing.

           public void Display1(string msg)
            {
                Console.WriteLine(msg);
            }

            public void Eg2()
            {
                Action<string> m = Display1;
                m("hello all");
            }

    

    // Example 3 : 

    // Func<int, bool> — accepts an integer and returns a Boolean value.
    // Func<int,int,bool> - accepts two integer values and returns Boolean Value
    // Func<string, int> - accepts string value and returns Integer as return value

    public int Check(string s,char ch)
    {
        return s.IndexOf(ch);
    }

    public void Eg3()
    {
        Func<string,char,int> m = Check;
        Console.WriteLine(m("world", 'r'));
        Console.WriteLine(m("hello" , 'z'));
    }


    

    // Example 4 :

    public bool Check(string s)
    {
        return s == new string(s.Reverse().ToArray()) ;
    }

    public void Eg4()
    {
        Predicate<string> m = Check;
        Console.WriteLine(m("world"));
        Console.WriteLine(m("helleh"));
    }

    
         
    // Example - 5 
    // Why can't we pass the body of the method as as an argumnet using lambda expression?

    public void Eg5()
    {
        Predicate<string> m = msg => msg == new string(msg.Reverse().ToArray());
        Console.WriteLine(m("world"));
        Console.WriteLine(m("helleh"));
    }


   
         
    // Example - 6

    public void CheckString(string s, Predicate<string> condition)
    {
        Console.WriteLine(condition(s));
    }
    public bool IsPalindrome(string s)
    {
        return s == new string(s.Reverse().ToArray());
    }
    public void Eg6()
    {
        CheckString("malayalam", IsPalindrome);
        CheckString("Hello", (s) => s.Distinct().Count() == s.Length);
        CheckString("programming", s => s.Length > 8);
        CheckString("level", s => s[0] == s[s.Length - 1]);
    }

   

    // Example - 7

    public void CheckValue1<T>(T t, Predicate<T> condition)
    {
        Console.WriteLine(condition(t));
    }

    public void Eg7()
    {
        CheckValue1("abcba", s => s == new string(s.Reverse().ToArray()));
        CheckValue1("level", s => s[0] == s[s.Length - 1]);
        CheckValue1(new int[] { 1, 2, 4, 5 }, arr => arr[0] == arr[arr.Length - 1]);

    }

    

    // Example - 7

    public void CheckValue<T,TResult>(T t, Func<T, TResult> condition)
    {
        Console.WriteLine(condition(t));
    }

    public void Eg8()
    {
        CheckValue("abcba", s => s == new string(s.Reverse().ToArray()));
        CheckValue("level", s => s[0] +""+ s[s.Length - 1]);
        CheckValue(new int[] { 1, 2, 4, 5 }, arr => arr[0] + arr[arr.Length - 1]);

    }

    }
}
