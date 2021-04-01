using System;

namespace Spear.Inf.Core.Tool
{
    public class Printor
    {
        public static void PrintText(string text)
        {
            Console.WriteLine(text);
        }

        public static void PrintLine()
        {
            PrintText("------------------------------------------------------------");
        }
    }
}
