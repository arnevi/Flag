using System;
using System.Linq;
using Xunit;

using Flag.Parser;

namespace FlagTest
{
    public class UnitTestFlagParser
    {
        [Fact]
        public void TestParser()
        {
            OptionParser p = new OptionParser();

            p.AddStringOption("I");
            p.AddStringOption("f file");
            p.AddStringOption("t filetype");

            p.AddBoolOption("h help");
            p.AddBoolOption("v version");

            var argv = "a -I/lib1 b -I=/lib2 -I /lib3 c -help -file/dev/null --filetype=txt -filetype cpp d".Split(" ");

            var rest = p.Parse(argv);

            Assert.True(rest.Length == 4);  // a b c d
            Assert.True(Array.IndexOf(rest, "a") != -1);
            Assert.True(Array.IndexOf(rest, "b") != -1);
            Assert.True(Array.IndexOf(rest, "c") != -1);
            Assert.True(Array.IndexOf(rest, "d") != -1);

            // Any flag that represents the same option, should return the same value
            Assert.True(p.IsSet("file"));
            Assert.True(p.IsSet("f"));

            Assert.True(p.IsSet("help"));
            Assert.True(p.IsSet("filetype"));

            Assert.True(p.IsSet("I"));
            Assert.False(p.IsSet("version"));

            var iOptions = p.GetAll("I");
            Assert.True(iOptions.Length == 3);

            // They all have to be here, but can come in any order
            Assert.True(Array.IndexOf(iOptions, "/lib1") != -1);
            Assert.True(Array.IndexOf(iOptions, "/lib2") != -1);
            Assert.True(Array.IndexOf(iOptions, "/lib3") != -1);

            Assert.Equal("/dev/null", p.Get("file"));

            var typeOptions = p.GetAll("filetype");
            Assert.True(typeOptions.Length == 2);
            Assert.True(Array.IndexOf(typeOptions, "txt") != -1);
            Assert.True(Array.IndexOf(typeOptions, "cpp") != -1);

            // Unset all flags, ready to parse new input
            p.Reset();

            Assert.False(p.IsSet("file"));
            Assert.False(p.IsSet("f"));

            Assert.False(p.IsSet("help"));
            Assert.False(p.IsSet("filetype"));

            Assert.False(p.IsSet("I"));
            Assert.False(p.IsSet("version"));

            // Allow juxtaposition of boolean flags
            rest = p.Parse("a b c -hv d".Split(" "));

            Assert.True(rest.Length == 4);
            Assert.True(p.IsSet("help"));
            Assert.True(p.IsSet("version"));            
        }



        [Fact]
        public void TestParser2()
        {
            OptionParser p = new OptionParser();

            p.AddStringOption("I include");
            p.AddStringOption("x xtract");
            p.AddStringOption("t type");

            p.AddBoolOption("h help");
            p.AddBoolOption("V version");

            var argv = "aa -I/lib1 bb -include=/lib2 -I /lib3 cc -help -xtract/dev/null --type=txt -type cpp dd".Split(" ");

            var rest = p.Parse(argv);

            Assert.True(rest.Length == 4);  // aa bb cc dd
            Assert.True(Array.IndexOf(rest, "aa") != -1);
            Assert.True(Array.IndexOf(rest, "bb") != -1);
            Assert.True(Array.IndexOf(rest, "cc") != -1);
            Assert.True(Array.IndexOf(rest, "dd") != -1);

            // Any flag that represents the same option, should return the same value
            Assert.True(p.IsSet("xtract"));
            Assert.True(p.IsSet("x"));

            Assert.True(p.IsSet("help"));
            Assert.True(p.IsSet("type"));

            Assert.True(p.IsSet("I"));
            Assert.False(p.IsSet("V"));

            var iOptions = p.GetAll("I");
            Assert.True(iOptions.Length == 3);

            // They all have to be here, but can come in any order
            Assert.True(Array.IndexOf(iOptions, "/lib1") != -1);
            Assert.True(Array.IndexOf(iOptions, "/lib2") != -1);
            Assert.True(Array.IndexOf(iOptions, "/lib3") != -1);

            Assert.Equal("/dev/null", p.Get("xtract"));

            var typeOptions = p.GetAll("type");
            Assert.True(typeOptions.Length == 2);
            Assert.True(Array.IndexOf(typeOptions, "txt") != -1);
            Assert.True(Array.IndexOf(typeOptions, "cpp") != -1);

            // Unset all flags, ready to parse new input
            p.Reset();

            Assert.False(p.IsSet("xtract"));
            Assert.False(p.IsSet("x"));

            Assert.False(p.IsSet("help"));
            Assert.False(p.IsSet("type"));

            Assert.False(p.IsSet("I"));
            Assert.False(p.IsSet("version"));

            // Allow juxtaposition of boolean flags
            rest = p.Parse("a b c -hV d".Split(" "));

            Assert.True(rest.Length == 4);
            Assert.True(p.IsSet("help"));
            Assert.True(p.IsSet("version"));            
        }

    }
}
