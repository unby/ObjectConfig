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
        public readonly Queue<JContainer> AllProperty = new Queue<JContainer>();
        public async Task<JObject> Parse(ConfigElement configElement)
        {
            List<JContainer> props = new List<JContainer>(configElement.Childs.Count);

            foreach (var item in configElement.Childs)
            {
                var child = await ParseConfigElement(item);
                if (child != null)
                {
                    props.Add(child);
                    AllProperty.Enqueue(child);
                }
            }

            var root = new JObject(props.ToArray());
            AllProperty.Enqueue(root);
            return root;
        }

        private async Task<JContainer> ParseConfigElement(ConfigElement configElement)
        {
            try
            {
                switch (configElement.TypeElement.TypeNode)
                {
                    case TypeNode.Complex:
                        List<JContainer> props = new List<JContainer>(configElement.Childs.Count);
                        foreach (var item in configElement.Childs)
                        {
                            var jContainer = await ParseConfigElement(item);
                            props.Add(jContainer);
                            AllProperty.Enqueue(jContainer);
                        }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        if (configElement.Parrent.TypeElement.TypeNode == TypeNode.Array)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                        {
                            return new JObject(props.ToArray());
                        }
                        else
                        {
                            return new JProperty(configElement.TypeElement.Name, new JObject(props.ToArray()));
                        }

                    case TypeNode.Array:
                        if (configElement.Childs.Any())
                        {
                            List<JContainer> array = new List<JContainer>(configElement.Childs.Count);

                            foreach (var item in configElement.Childs)
                            {
                                var jContainer = await ParseConfigElement(item);
                                array.Add(jContainer);
                                AllProperty.Enqueue(jContainer);
                            }

                            return new JProperty(configElement.TypeElement.Name, array.ToArray());
                        }
                        else
                        {
#nullable disable
                            var array = configElement.Value.Select(s => s.Value).ToArray();
                            return new JProperty(configElement.TypeElement.Name, array);
#nullable enable
                        }
                    default:
                        return new JProperty(configElement.TypeElement.Name,
                            ParseByType(configElement.Value[0].Value, configElement.TypeElement.TypeNode));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
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
