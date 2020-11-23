using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestBuilder
{
    public class FileWriter
    {
        private readonly TaskLoggingHelper log;

        public FileWriter(TaskLoggingHelper log)
        {
            this.log = log;
        }

        public void WriteOutputToFile(CodeFile codeFile)
        {
            log.LogMessage(MessageImportance.High, codeFile.Path);

            if (File.Exists(codeFile.Path))
            {
                if (!FileComparer.IsFileContentEqual(codeFile.Path, codeFile.GeneratedCode))
                {
                    File.WriteAllText(codeFile.Path, codeFile.GeneratedCode, Encoding.UTF8);
                }
            }
            else
            {
                File.WriteAllText(codeFile.Path, codeFile.GeneratedCode);
            }
        }
    }
}
