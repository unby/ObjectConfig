using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectConfig
{
    public static class Utils
    {
        public readonly static string AssembliesLocation = System.IO.Path.GetDirectoryName(typeof(Utils).Assembly.Location);
        public static Guid NewSequentialId()
        {
            return SequentialGuid.SequentialGuidGenerator.Instance.NewGuid();
        }
    }
}
