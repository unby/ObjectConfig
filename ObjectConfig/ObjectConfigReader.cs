using Newtonsoft.Json.Linq;
using ObjectConfig.Data;
using System;
using System.Collections.Generic;
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
            var res = new ConfigElement(new TypeElement(), null, config);
            AllNodes.Add(res);
            foreach (var node in jObj)
            {
                var confElem = await ReadChild(node.Value, node.Key, res, deep);
                res.Childs.Add(confElem);
                AllNodes.Add(confElem);
            }
            config.ConfigElement.Add(res);
            return res;
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
                        res = new ConfigElement(new TypeElement(TypeNode.Complex, key), parrent, config);
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
                            res = new ConfigElement(new TypeElement(TypeNode.Complex, key), parrent, config);
                            foreach (var item in jobject)
                            {
                                res.Childs.Add(await ReadChild(item.Value,item.Key, res, --deep));

                            }
                        }
                    }
                    break;
                case TypeNode.Array:
                    res = new ConfigElement(new TypeElement(TypeNode.Array, key), parrent, config);
                        foreach (var item in node)
                        {
                            var result = await ReadChild(item, key, res, deep);
                            if (result != null)
                                res.Childs.Add(result);
                        }
                    break;
                case TypeNode.Integer:
                    res = new ConfigElement(new TypeElement(TypeNode.Integer, key), parrent, config);
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    break;
                case TypeNode.Float:
                    res = new ConfigElement(new TypeElement(TypeNode.Float, key), parrent, config);
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    break;
                case TypeNode.String:
                    res = new ConfigElement(new TypeElement(TypeNode.String, key), parrent, config);
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    break;
                case TypeNode.Boolean:
                    res = new ConfigElement(new TypeElement(TypeNode.Boolean, key), parrent, config);
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    break;
                case TypeNode.Null:
                    res = new ConfigElement(new TypeElement(TypeNode.Null, key), parrent, config);
                    res.Value.Add(new ValueElement(null, res.Type));
                    break;
                case TypeNode.Date:
                    res = new ConfigElement(new TypeElement(TypeNode.Date, key), parrent, config);
                    //todo продумать формат даты
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    break;
                case TypeNode.Guid:
                    res = new ConfigElement(new TypeElement(TypeNode.Guid, key), parrent, config);
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    break;
                case TypeNode.Uri:
                    res = new ConfigElement(new TypeElement(TypeNode.Uri, key), parrent, config);
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    break;
                case TypeNode.TimeSpan:
                    res = new ConfigElement(new TypeElement(TypeNode.TimeSpan, key), parrent, config);
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    break;          
                default:
                    break;
            }

            AllNodes.Add(res);
            return res;
        }
        
    }
}
