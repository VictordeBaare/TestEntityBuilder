using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Linq;

namespace TestBuilder
{
    public class GenerateTestBuilder : Task
    {
        public override bool Execute()
        {
            Log.LogMessage(MessageImportance.High, "Test2");
            Log.LogMessage(MessageImportance.High, string.Join(",", Config.Select(x => x.ItemSpec)));
            return true;
        }

        public ITaskItem[] Config { get; set; }
    }
}
