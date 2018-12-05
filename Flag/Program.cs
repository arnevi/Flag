using System;
using Flag.Parser;

namespace Flag
{ 
    class Program
    {
        static void Main(string[] args)
        {
            
            var parser = new OptionParser();
                          
            parser.AddStringOption("I include");
            parser.AddStringOption("x xtract");
            parser.AddStringOption("t type");

            parser.AddBoolOption("h help");
            parser.AddBoolOption("V version");

            var arg_test = "aa -I/lib1 bb -include=/lib2 -I /lib3 cc -help -xtract/dev/null --type=txt -type cpp dd".Split(" ");            
            var rest_test = parser.Parse(arg_test);

            for (int i = 0; i < rest_test.Length; i++)
            {
                System.Console.WriteLine("Testing[{0}] = [{1}]", i, rest_test[i]);
            }


            var testing = parser.GetAll("include");            
            for (int i = 0; i < testing.Length; i++)
            {
                System.Console.WriteLine("I Testing[{0}] = [{1}]", i, testing[i]);
            }

            testing = parser.GetAll("t");            
            for (int i = 0; i < testing.Length; i++)
            {
                System.Console.WriteLine("f Testing[{0}] = [{1}]", i, testing[i]);
            }

            testing = parser.GetAll("xtract");            
            for (int i = 0; i < testing.Length; i++)
            {
                System.Console.WriteLine("t Testing[{0}] = [{1}]", i, testing[i]);
            }

            if (parser.IsSet("h"))
                Console.WriteLine("Help!");

            if (parser.IsSet("version"))
                Console.WriteLine("1.0");

            if (parser.IsSet("file"))
                 Console.WriteLine($"file: {parser.Get("file")}");


            parser.Reset();
            Console.WriteLine("Parser resatt ");
            
            rest_test = parser.Parse("a b c -hV d".Split(" "));
            for (int i = 0; i < rest_test.Length; i++)
            {
                System.Console.WriteLine("Testing[{0}] = [{1}]", i, rest_test[i]);
            }

            if (parser.IsSet("help"))
                Console.WriteLine("Help!");

            if (parser.IsSet("V"))
                Console.WriteLine("1.0");

            Console.WriteLine("Ferdig");
            Console.ReadLine();
        }
    }
}
