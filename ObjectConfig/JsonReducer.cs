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
                var child = await ParseCobfigElement(item);
                if (child != null)
                    props.Add(child);
            }

            var root = new JObject(props.ToArray());
            return root;
        }

        private async Task<JContainer> ParseCobfigElement(ConfigElement configElement)
        {
            switch (configElement.Type.Type)
            {
                case TypeNode.Complex:
                    List<JContainer> props = new List<JContainer>(configElement.Childs.Count);
                    foreach (var item in configElement.Childs)
                    {
                        props.Add(await ParseCobfigElement(item));
                    }

                    if (configElement.Parrent.Type.Type == TypeNode.Array)
                        return new JObject(props.ToArray());
                    else
                        return new JProperty(configElement.Type.Name, new JObject(props.ToArray()));

                case TypeNode.Array:
                    if (configElement.Childs.Any())
                    {
                        List<JContainer> array = new List<JContainer>(configElement.Childs.Count);

                        foreach (var item in configElement.Childs)
                        {
                            array.Add(await ParseCobfigElement(item));
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

        object ParseByType(string value, TypeNode type)
        {
            switch (type)
            {
                case TypeNode.Integer:
                    return long.Parse(value);
                case TypeNode.Float:
                    return double.Parse(value);
                case TypeNode.String:
                    return value;
                case TypeNode.Boolean:
                    return bool.Parse(value);
                case TypeNode.DateTimeOffset:
                    return DateTimeOffset.Parse(value);
                case TypeNode.Date:
                    return DateTime.Parse(value);
                case TypeNode.Guid:
                    return Guid.Parse(value);
                case TypeNode.Uri:
                    return new Uri(value);
                case TypeNode.TimeSpan:
                    return TimeSpan.Parse(value);
                default:
                    return null;
            }
        }
    }
}
