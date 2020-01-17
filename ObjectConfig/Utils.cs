using System;

namespace ObjectConfig
{
    public static class Utils
    {
        public static readonly string AssembliesLocation = System.IO.Path.GetDirectoryName(typeof(Utils).Assembly.Location);
        public static Guid NewSequentialId => SequentialGuid.SequentialGuidGenerator.Instance.NewGuid(DateTime.Now);

        public static string GetStr => NewSequentialId.ToString();
    }
}
