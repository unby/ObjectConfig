using Newtonsoft.Json.Linq;
using ObjectConfig.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjectConfig
{
    public class ObjectConfigReader
    {
        private readonly Config _config;

        public ObjectConfigReader(Config config)
        {
            this._config = config;
            CrateTime = DateTimeOffset.UtcNow;
        }

        public readonly DateTimeOffset CrateTime;

        public readonly List<ConfigElement> AllProperty = new List<ConfigElement>();

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
            var root = new ConfigElement(new TypeElement(), null, _config, null);
            AllProperty.Add(root);
            foreach (var node in jObj)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                var confElem = await ReadChild(node.Value, node.Key, root, deep);
                root.Childs.Add(confElem);
                AllProperty.Add(confElem);
#pragma warning restore CS8604 // Possible null reference argument.
            }
            _config.ConfigElement.Add(root);
            return root;
        }

        private TypeNode GetType(JToken token)
        {
            return token.Type switch
            {
                JTokenType.Object => TypeNode.Complex,
                JTokenType.Array => TypeNode.Array,
                JTokenType.Integer => TypeNode.Integer,
                JTokenType.Float => TypeNode.Float,
                JTokenType.String => TypeNode.String,
                JTokenType.Boolean => TypeNode.Boolean,
                JTokenType.Null => TypeNode.Null,
                JTokenType.Date => TypeNode.Date,
                JTokenType.Guid => TypeNode.Guid,
                JTokenType.Uri => TypeNode.Uri,
                JTokenType.TimeSpan => TypeNode.TimeSpan,
                _ => TypeNode.None,
            };
        }

        private async Task<ConfigElement?> ReadChild(JToken node, string key, ConfigElement parrent, int deep)
        {
            ConfigElement? res = null;
            var childType = GetType(node);
            if (parrent.Type.Type == TypeNode.Array && childType != TypeNode.Complex)
            {
                parrent.Value.Add(new ValueElement(node.ToString(), parrent, parrent.Type, CrateTime));
                return null;
            }

            (ConfigElement element, TypeElement Type) temp;
            switch (childType)
            {
                case TypeNode.None:
                    break;
                case TypeNode.Complex:
                    if (deep == 0)
                    {
                       temp = CreateConfigElement(TypeNode.Complex, key, parrent);
                       res = temp.element;
                        res.Value.Add(new ValueElement(node.ToString(), temp.element, temp.Type, CrateTime));
                    }
                    else
                    {
                        if (node.Type == JTokenType.Property)
                        {
                            throw new Exception("JTokenType.Property " + node.ToString());
                        }

                        if (node is JObject jobject)
                        {
                            temp= CreateConfigElement(TypeNode.Complex, key, parrent);
                            res = temp.element;
                            foreach (var item in jobject)
                            {
#pragma warning disable CS8604 // Possible null reference argument.
                                var child = await ReadChild(item.Value, item.Key, res, --deep);
#pragma warning restore CS8604 // Possible null reference argument.
                                if (child != null)
                                {
                                    res.Childs.Add(child);
                                    AllProperty.Add(child);
                                }
                            }
                        }
                    }
                    break;
                case TypeNode.Array:
                    temp = CreateConfigElement(TypeNode.Array, key, parrent);
                    res = temp.element;
                    res.Value.Add(new ValueElement(null, res, temp.Type, CrateTime));
                    foreach (var item in node)
                    {
                        var result = await ReadChild(item, key, res, deep);
                        if (result != null)
                        {
                            res.Childs.Add(result);
                            AllProperty.Add(result);
                        }
                    }
                    break;
                case TypeNode.Integer:
                    temp = CreateConfigElement(TypeNode.Integer, key, parrent);
                    res = temp.element;
                    res.Value.Add(new ValueElement(node.ToString(), res, temp.Type, CrateTime));
                    break;
                case TypeNode.Float:
                    temp = CreateConfigElement(TypeNode.Float, key, parrent);
                    res = temp.element;
                    res.Value.Add(new ValueElement(node.ToString(), res, temp.Type, CrateTime));
                    break;
                case TypeNode.String:
                    temp = CreateConfigElement(TypeNode.String, key, parrent);
                    res = temp.element;
                    res.Value.Add(new ValueElement(node.ToString(), res, temp.Type, CrateTime));
                    break;
                case TypeNode.Boolean:
                    temp = CreateConfigElement(TypeNode.Boolean, key, parrent);
                    res = temp.element;
                    res.Value.Add(new ValueElement(node.ToString(), res, temp.Type, CrateTime));
                    break;
                case TypeNode.Null:
                    temp = CreateConfigElement(TypeNode.Null, key, parrent);
                    res = temp.element;
                    res.Value.Add(new ValueElement(null, res, temp.Type, CrateTime));
                    break;
                case TypeNode.Date:
                    if (node.ToString().Contains("+"))
                    {
                        temp = CreateConfigElement(TypeNode.DateTimeOffset, key, parrent);
                        res = temp.element;
                        var dateTime = node.ToObject<DateTimeOffset>();

                        res.Value.Add(new ValueElement(dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz"), res, temp.Type, CrateTime));
                    }
                    else
                    {
                        temp = CreateConfigElement(TypeNode.Date, key, parrent);
                        res = temp.element;
                        var dateTime = node.ToObject<DateTime>();
                        res.Value.Add(new ValueElement(dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffff"), res, temp.Type, CrateTime));
                    }
                    break;
                case TypeNode.Guid:
                    temp = CreateConfigElement(TypeNode.Guid, key, parrent);
                    res = temp.element;
                    res.Value.Add(new ValueElement(node.ToString(), res, temp.Type, CrateTime));
                    break;
                case TypeNode.Uri:
                    temp = CreateConfigElement(TypeNode.Uri, key, parrent);
                    res = temp.element;
                    res.Value.Add(new ValueElement(node.ToString(), res, temp.Type, CrateTime));
                    break;
                case TypeNode.TimeSpan:
                    temp = CreateConfigElement(TypeNode.TimeSpan, key, parrent);
                    res = temp.element;
                    res.Value.Add(new ValueElement(node.ToString(), res, temp.Type, CrateTime));
                    break;
                default:
                    break;
            }

            return res;
        }

        private readonly Dictionary<string, TypeElement> _types = new Dictionary<string, TypeElement>();
        private (ConfigElement element, TypeElement Type) CreateConfigElement(TypeNode nodeType, string nodeKey, ConfigElement parrent)
        {
            var path = parrent.Path + "." + nodeKey;
            return (new ConfigElement(null, parrent, _config, path), CreateType(nodeType, nodeKey, path));
        }

        private TypeElement CreateType(TypeNode nodeType, string nodeKey, string nodePath)
        {
            if (_types.TryGetValue(nodePath, out var type))
            {
                return type;
            }

            var newType = new TypeElement(nodeType, nodeKey);
            _types.Add(nodePath, newType);
            return newType;
        }
    }
}
