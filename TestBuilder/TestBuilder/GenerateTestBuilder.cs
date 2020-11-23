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
            
            if(Config != null) 
            {
                Log.LogMessage(MessageImportance.High, string.Join(",", Config.Select(x => x.ItemSpec)));
            }
            else
            {
                Log.LogMessage(MessageImportance.High, "No config files found");
            }

            Log.LogMessage(MessageImportance.High, $"{ProjectPath}");
            Log.LogMessage(MessageImportance.High, $"{AssemblyName}");

            return true;
        }

        public ITaskItem[] Config { get; set; }

        [Required]
        public string ProjectPath { get; set; }

        public string AssemblyName { get; set; }
    }
}
