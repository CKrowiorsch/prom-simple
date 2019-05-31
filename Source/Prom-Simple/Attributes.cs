using System;
using System.Data.SqlTypes;

namespace Prom_Simple
{
    public class PromCounter : Attribute
    {

    }


    public class PromGauge : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class PromLabel : Attribute
    {
        public string Key { get; set; }
        public string Label { get; set; }

        public PromLabel(string key, string label)
        {
            Key = key;
            Label = label;
        }
    }
}