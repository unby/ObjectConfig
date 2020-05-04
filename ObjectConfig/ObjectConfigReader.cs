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
            try
            {
                ConfigElement? res = null;
                var childType = GetType(node);
                if (parrent.TypeElement.Type == TypeNode.Array && childType != TypeNode.Complex)
                {
                    parrent.Value.Add(new ValueElement(node.ToString(), parrent, CrateTime));
                    return null;
                }

                switch (childType)
                {
                    case TypeNode.None:
                        break;
                    case TypeNode.Complex:
                        if (deep == 0)
                        {
                            res = CreateConfigElement(TypeNode.Complex, key, parrent);
                            res.Value.Add(new ValueElement(node.ToString(), res, CrateTime));
                        }
                        else
                        {
                            if (node.Type == JTokenType.Property)
                            {
                                throw new Exception("JTokenType.Property " + node.ToString());
                            }

                            if (node is JObject jobject)
                            {
                                res = CreateConfigElement(TypeNode.Complex, key, parrent);
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
                        res = CreateConfigElement(TypeNode.Array, key, parrent);
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
                        res = CreateConfigElement(TypeNode.Integer, key, parrent);
                        res.Value.Add(new ValueElement(node.ToString(), res, CrateTime));
                        break;
                    case TypeNode.Float:
                        res = CreateConfigElement(TypeNode.Float, key, parrent);
                        res.Value.Add(new ValueElement(node.ToString(), res, CrateTime));
                        break;
                    case TypeNode.String:
                        res = CreateConfigElement(TypeNode.String, key, parrent);
                        res.Value.Add(new ValueElement(node.ToString(), res, CrateTime));
                        break;
                    case TypeNode.Boolean:
                        res = CreateConfigElement(TypeNode.Boolean, key, parrent);
                        res.Value.Add(new ValueElement(node.ToString(), res, CrateTime));
                        break;
                    case TypeNode.Null:
                        res = CreateConfigElement(TypeNode.Null, key, parrent);
                        res.Value.Add(new ValueElement(null, res, CrateTime));
                        break;
                    case TypeNode.Date:
                        if (node.ToString().Contains("+"))
                        {
                            res = CreateConfigElement(TypeNode.DateTimeOffset, key, parrent);
                            var dateTime = node.ToObject<DateTimeOffset>();

                            res.Value.Add(new ValueElement(dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz"), res,
                                CrateTime));
                        }
                        else
                        {
                            res = CreateConfigElement(TypeNode.Date, key, parrent);

                            var dateTime = node.ToObject<DateTime>();
                            res.Value.Add(new ValueElement(dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffff"), res,
                                CrateTime));
                        }

                        break;
                    case TypeNode.Guid:
                        res = CreateConfigElement(TypeNode.Guid, key, parrent);

                        res.Value.Add(new ValueElement(node.ToString(), res, CrateTime));
                        break;
                    case TypeNode.Uri:
                        res = CreateConfigElement(TypeNode.Uri, key, parrent);
                       res.Value.Add(new ValueElement(node.ToString(), res, CrateTime));
                        break;
                    case TypeNode.TimeSpan:
                        res = CreateConfigElement(TypeNode.TimeSpan, key, parrent);
                        res.Value.Add(new ValueElement(node.ToString(), res, CrateTime));
                        break;
                    default:
                        break;
                }

                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private readonly Dictionary<string, TypeElement> _types = new Dictionary<string, TypeElement>();
        private ConfigElement CreateConfigElement(TypeNode nodeType, string nodeKey, ConfigElement parrent)
        {
            var path = parrent.Path + "." + nodeKey;
            return new ConfigElement(CreateType(nodeType, nodeKey, path), parrent, _config, path);
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
