using System;

namespace Krowiorsch
{
    public class PromCounter : Attribute
    {

    }


    public class PromGauge : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PromDescription : Attribute
    {
        public string Description { get; set; }

        public PromDescription(string descrition)
        {
            Description = descrition;
        }
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