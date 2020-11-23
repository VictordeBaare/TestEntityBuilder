using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

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

                var assembly = Assembly.Load(AssemblyName);

                if(assembly == null)
                {
                    Log.LogMessage(MessageImportance.High, "Assembly not loaded");
                }
                else
                {
                    var configs = assembly.GetTypes().Where(x => x.IsClass && typeof(ITestEntityBuilderConfig).IsAssignableFrom(x));
                    var writer = new FileWriter(Log);

                    if(configs != null && configs.Any())
                    {
                        Log.LogMessage(MessageImportance.High, "Implementation Found");

                        foreach (var config in configs)
                        {
                            var item = (ITestEntityBuilderConfig)Activator.CreateInstance(config);

                            foreach(var typesToGenerate in item.Configs)
                            {
                                var stringBuilder = new IndentedStringBuilder();

                                stringBuilder.AppendLine($"namespace {item.Namespace}")
                                    .AppendLine("{");

                                using (stringBuilder.Indent())
                                {
                                    stringBuilder.AppendLine($"using {typesToGenerate.Namespace};")
                                        .AppendLine($"using System;")
                                        .AppendLine($"using System.Linq;")
                                        .AppendLine($"public partial class {typesToGenerate.Name}Builder")
                                        .AppendLine("{");

                                    using (stringBuilder.Indent())
                                    {
                                        stringBuilder.AppendLine($"private {typesToGenerate.Name} _entity");
                                        stringBuilder.AppendLine();
                                        stringBuilder.AppendLine($"private {typesToGenerate.Name}Builder()")
                                            .AppendLine("{");

                                        using (stringBuilder.Indent())
                                        {
                                            stringBuilder.AppendLine($"_entity = new {typesToGenerate}();");
                                            stringBuilder.AppendLine($"InitializeEntity(_entity);");
                                        }

                                        stringBuilder.AppendLine("}");
                                        stringBuilder.AppendLine();
                                        stringBuilder.AppendLine($"partial InitializeEntity({typesToGenerate.Name} entity);");

                                        stringBuilder.AppendLine($"public static {typesToGenerate.Name}Builder Valid()")
                                            .AppendLine("{");

                                        using (stringBuilder.Indent())
                                        {
                                            stringBuilder.AppendLine($"return new {typesToGenerate.Name}Builder()");
                                        }

                                        stringBuilder.AppendLine("}");
                                    }

                                    stringBuilder.AppendLine("}");
                                }

                                stringBuilder.AppendLine("}");

                                var file = new CodeFile
                                {
                                    GeneratedCode = stringBuilder.ToString(),
                                    Path = Path.GetDirectoryName(ProjectPath),
                                };

                                writer.WriteOutputToFile(file);
                            }
                        }
                        
                    }
                    else
                    {
                        Log.LogMessage(MessageImportance.High, "No implementations found");
                    }
                }
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
