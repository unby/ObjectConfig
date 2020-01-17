using Newtonsoft.Json.Linq;
using ObjectConfig.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObjectConfig
{
    public class ObjectConfigReader
    {
        private readonly Config config;

        public ObjectConfigReader(Config config)
        {
            this.config = config;
        }

        public List<ConfigElement> AllNodes = new List<ConfigElement>();

        public async Task<ConfigElement> Parse(string jsonString, int deep = 20)
        {
            return await ParseJObject(JObject.Parse(jsonString), deep);
        }

        public async Task<ConfigElement> Parse(JObject jObject, int deep = 20)
        {
            return await ParseJObject(jObject, deep);
        }

        public async Task<ConfigElement> Parse(object @object, int deep = 20)
        {
            return await ParseJObject(JObject.FromObject(@object), deep);
        }

        private async Task<ConfigElement> ParseJObject(JObject jObj, int deep)
        {
            var root = new ConfigElement(new TypeElement(), null, config, null);
            AllNodes.Add(root);
            foreach (var node in jObj)
            {
                var confElem = await ReadChild(node.Value, node.Key, root, deep);
                root.Childs.Add(confElem);
                AllNodes.Add(confElem);
            }
            config.ConfigElement.Add(root);
            var c = types.Count();
            var ca = AllNodes.Count();
            Console.WriteLine(c > ca);
            return root;
        }

        private TypeNode GetType(JToken token, int deep)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    return TypeNode.Complex;
                case JTokenType.Array:
                    return TypeNode.Array;
                case JTokenType.Integer:
                    return TypeNode.Integer;
                case JTokenType.Float:
                    return TypeNode.Float;
                case JTokenType.String:
                    return TypeNode.String;
                case JTokenType.Boolean:
                    return TypeNode.Boolean;
                case JTokenType.Null:
                    return TypeNode.Null;
                case JTokenType.Date:
                    return TypeNode.Date;
                case JTokenType.Guid:
                    return TypeNode.Guid;
                case JTokenType.Uri:
                    return TypeNode.Uri;
                case JTokenType.TimeSpan:
                    return TypeNode.TimeSpan;
                default:
                    return TypeNode.None;
            }
        }

        private async Task<ConfigElement> ReadChild(JToken node, string key, ConfigElement parrent, int deep)
        {
            ConfigElement res = null;
            var childType = GetType(node, deep);
            if (parrent.Type.Type == TypeNode.Array && childType != TypeNode.Complex)
            {
                parrent.Value.Add(new ValueElement(node.ToString(), parrent.Type));
                return null;
            }
            switch (childType)
            {
                case TypeNode.None:
                    break;
                case TypeNode.Complex:
                    if (deep == 0)
                    {
                        res = CreateConfigElement(TypeNode.Complex, key, parrent);// new ConfigElement(new TypeElement(TypeNode.Complex, key), parrent, config);
                        res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    }
                    else
                    {
                        if (node.Type == JTokenType.Property)
                        {
                            throw new Exception("JTokenType.Property " + node.ToString());
                        }
                        else
                        {
                            JObject jobject = node as JObject;
                            res = CreateConfigElement(TypeNode.Complex, key, parrent); // new ConfigElement(new TypeElement(TypeNode.Complex, key), parrent, config);
                            foreach (var item in jobject)
                            {
                                res.Childs.Add(await ReadChild(item.Value, item.Key, res, --deep));

                            }
                        }
                    }
                    break;
                case TypeNode.Array:
                    res = CreateConfigElement(TypeNode.Array, key, parrent); //new ConfigElement(new TypeElement(TypeNode.Array, key), parrent, config);
                    foreach (var item in node)
                    {
                        var result = await ReadChild(item, key, res, deep);
                        if (result != null)
                            res.Childs.Add(result);
                    }
                    break;
                case TypeNode.Integer:
                    res = CreateConfigElement(TypeNode.Integer, key, parrent);
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    break;
                case TypeNode.Float:
                    res = CreateConfigElement(TypeNode.Float, key, parrent);
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    break;
                case TypeNode.String:
                    res = CreateConfigElement(TypeNode.String, key, parrent);
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    break;
                case TypeNode.Boolean:
                    res = CreateConfigElement(TypeNode.Boolean, key, parrent);
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    break;
                case TypeNode.Null:
                    res = CreateConfigElement(TypeNode.Null, key, parrent);
                    res.Value.Add(new ValueElement(null, res.Type));
                    break;
                case TypeNode.Date:
                    if (node.ToString().Contains("+"))
                    {
                        var dateTime = node.ToObject<DateTimeOffset>();
                        res = CreateConfigElement(TypeNode.DateTimeOffset, key, parrent);
                        res.Value.Add(new ValueElement(dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz"), res.Type));
                    }
                    else
                    {
                        var dateTime = node.ToObject<DateTime>();
                        res = CreateConfigElement(TypeNode.Date, key, parrent);
                        res.Value.Add(new ValueElement(dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffff"), res.Type));
                    }
                    break;
                case TypeNode.Guid:
                    res = CreateConfigElement(TypeNode.Guid, key, parrent);
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    break;
                case TypeNode.Uri:
                    res = CreateConfigElement(TypeNode.Uri, key, parrent);
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    break;
                case TypeNode.TimeSpan:
                    res = CreateConfigElement(TypeNode.TimeSpan, key, parrent);
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    break;
                default:
                    break;
            }

            AllNodes.Add(res);
            return res;
        }
        Dictionary<string, TypeElement> types = new Dictionary<string, TypeElement>();
        private ConfigElement CreateConfigElement(TypeNode nodeType, string nodeKey, ConfigElement parrent)
        {
            var path = parrent.Path + "." + nodeKey;
            return new ConfigElement(CreateType(nodeType, nodeKey, path), parrent, config, path);
        }

        private TypeElement CreateType(TypeNode nodeType, string nodeKey, string nodePath)
        {
            if (types.TryGetValue(nodePath, out var type))
                return type;
            else
            {
                var newType = new TypeElement(nodeType, nodeKey);
                types.Add(nodePath, newType);
                return newType;
            }
        }
    }
}
