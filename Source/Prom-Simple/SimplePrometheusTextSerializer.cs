using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Krowiorsch;

namespace Prom_Simple
{
    public class SimplePrometheusTextSerializer<T>
    {
        string _prefix;

        enum PromType { Counter, Gauge }

        readonly Dictionary<PropertyInfo, EntryInfo> _propertyCache = new Dictionary<PropertyInfo, EntryInfo>();

        public SimplePrometheusTextSerializer<T> Initialize(string prefix, Dictionary<string,string> globalLabels = null)
        {
            globalLabels = globalLabels ?? new Dictionary<string, string>();

            _prefix = prefix;

            var props = ReadProperties(typeof(T));

            foreach (var propertyInfo in props)
            {
                var builder = new StringBuilder();

                var (name, description, type) = GetEntryInfo(propertyInfo);
                var labels = ReadLabels(propertyInfo);
                foreach (var keyValuePair in globalLabels)
                {
                    labels.Add(keyValuePair.Key, keyValuePair.Value);
                }

                var labelString = labels.Any() ? BuildLabelString(labels) : string.Empty;

                var valuePrefix = $"{name}{labelString}";

                // write entries
                builder.Append($"# HELP {name} {description}\n");
                builder.Append($"# TYPE {name} {type.ToString().ToLowerInvariant()}\n");

                _propertyCache.Add(propertyInfo, new EntryInfo
                {
                    Name = name,
                    Fields = builder.ToString(),
                    ValuePrefix = valuePrefix
                });
            }

            return this;
        }

        string BuildLabelString(Dictionary<string, string> labels)
        {
            var builder = new StringBuilder();

            builder.Append("{");
            foreach (var pair in labels)
            {
                builder.Append($"{pair.Key}=\"{pair.Value}\",");
            }

            builder.Append("}");
            return builder.ToString();
        }

        public string Serialize(T state)
        {
            var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var builder = new StringBuilder();

            foreach (var entry in _propertyCache)
            {
                var value = entry.Key.GetValue(state);
                builder.Append(entry.Value.Fields);
                builder.Append($"{entry.Value.ValuePrefix} {value} {timestamp}\n");
            }

            return builder.ToString();
        }

        PropertyInfo[] ReadProperties(Type stateType)
        {
            return stateType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        (string name, string description, PromType type) GetEntryInfo(PropertyInfo info)
        {
            var name = $"{_prefix}_{SnakeNaming(info.Name)}".ToLowerInvariant();

            var attributes = info.GetCustomAttributes(typeof(PromDescription), false);
            var description = !attributes.Any()
                ? string.Empty
                : (attributes.First() as PromDescription).Description;

            var typeAttribute = info.GetCustomAttributes(typeof(PromCounter), false);

            PromType promType;

            if (info.GetCustomAttributes(typeof(PromCounter), false).Any())
                promType = PromType.Counter;
            else if (info.GetCustomAttributes(typeof(PromGauge), false).Any())
                promType = PromType.Gauge;
            else
                promType = promType = PromType.Counter;        // Standard


            return (name, description, promType);
        }

        Dictionary<string, string> ReadLabels(PropertyInfo info)
        {
            return info.GetCustomAttributes<PromLabel>(false).ToDictionary(s => s.Key, s => s.Label);
        }


        string SnakeNaming(string input)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]) && i != 0)
                    sb.Append($"_{input[i]}");
                else
                    sb.Append(input[i]);
            }

            return sb.ToString();
        }

        class EntryInfo
        {
            public string Name { get; set; }
            public string Fields { get; set; }
            public string ValuePrefix { get; set; }
        }
    }
}
