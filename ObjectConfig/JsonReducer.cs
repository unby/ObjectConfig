using Newtonsoft.Json.Linq;
using ObjectConfig.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObjectConfig
{
    public class JsonReducer
    {

        public async Task<JObject> Parse(ConfigElement configElement)
        {
            List<JContainer> props = new List<JContainer>(configElement.Childs.Count);
            foreach (var item in configElement.Childs)
            {
                var child = await ParseConfigElement(item);
                if (child != null)
                {
                    props.Add(child);
                }
            }

            var root = new JObject(props.ToArray());
            return root;
        }

        private async Task<JContainer> ParseConfigElement(ConfigElement configElement)
        {
            switch (configElement.Type.Type)
            {
                case TypeNode.Complex:
                    List<JContainer> props = new List<JContainer>(configElement.Childs.Count);
                    foreach (var item in configElement.Childs)
                    {
                        props.Add(await ParseConfigElement(item));
                    }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    if (configElement.Parrent.Type.Type == TypeNode.Array)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    {
                        return new JObject(props.ToArray());
                    }
                    else
                    {
                        return new JProperty(configElement.Type.Name, new JObject(props.ToArray()));
                    }

                case TypeNode.Array:
                    if (configElement.Childs.Any())
                    {
                        List<JContainer> array = new List<JContainer>(configElement.Childs.Count);

                        foreach (var item in configElement.Childs)
                        {
                            array.Add(await ParseConfigElement(item));
                        }
                        return new JProperty(configElement.Type.Name, array.ToArray());
                    }
                    else
                    {
                        var array = configElement.Value.Select(s => s.Value).ToArray();
                        return new JProperty(configElement.Type.Name, array);
                    }
                default:
                    return new JProperty(configElement.Type.Name, ParseByType(configElement.Value[0].Value, configElement.Type.Type));
            }
        }

        private object? ParseByType(string? value, TypeNode type)
        {
            return type switch
            {
                TypeNode.Integer => long.Parse(value),
                TypeNode.Float => double.Parse(value),
                TypeNode.String => value,
                TypeNode.Boolean => bool.Parse(value),
                TypeNode.DateTimeOffset => DateTimeOffset.Parse(value),
                TypeNode.Date => DateTime.Parse(value),
                TypeNode.Guid => Guid.Parse(value),
                TypeNode.Uri => new Uri(value),
                TypeNode.TimeSpan => TimeSpan.Parse(value),
                _ => null,
            };
        }
    }
}
