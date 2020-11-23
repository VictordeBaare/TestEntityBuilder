using System;
using System.Collections.Generic;

namespace TestBuilder
{
    public interface ITestEntityBuilderConfig
    {
        List<Type> Configs { get; set; }

        string Namespace { get; set; }
    }
}
