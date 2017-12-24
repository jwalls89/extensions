using NUnitLite;
using System.Reflection;

namespace Walls.Julian.Log4Net.Extensions.Tests.Unit
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Assembly thisAssembly = Assembly.GetEntryAssembly();
            return new AutoRun(thisAssembly).Execute(args);
        }
    }
}
