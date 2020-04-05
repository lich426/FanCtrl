using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NZXTSharp
{
    class DebugController
    {
        private StreamWriter _File;

        public DebugController(string RelativePath)
        {
            _File = new StreamWriter(RelativePath);
        }

        internal void Write(string ToWrite)
        {
            WriteToFile(ToWrite);
        }

        private void WriteToFile(string ToWrite)
        {
            _File.WriteLine(ToWrite);
        }
    }
}
