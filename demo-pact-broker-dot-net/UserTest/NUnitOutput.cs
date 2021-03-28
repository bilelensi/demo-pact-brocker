using PactNet.Infrastructure.Outputters;
using System;

namespace UserTest
{     public class NUnitOutput : IOutput
    {
        public bool HasError { get; private set; } = false;
        public void WriteLine(string line)
        {
            Console.WriteLine(line);
            if (line.Contains("FAILED"))
            {
                HasError = true;
                Console.Error.WriteLine(line);
            }
        }
    }
}
