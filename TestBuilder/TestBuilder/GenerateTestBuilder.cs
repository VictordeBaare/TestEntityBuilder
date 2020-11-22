using Microsoft.Build.Utilities;
using System;

namespace TestBuilder
{
    public class GenerateTestBuilder : Task
    {
        public override bool Execute()
        {
            Log.LogMessage(Microsoft.Build.Framework.MessageImportance.High, "Test2");
            Log.LogMessage(Microsoft.Build.Framework.MessageImportance.High, Config);
            return true;
        }

        public string Config { get; set; }
    }
}
