﻿using Newtonsoft.Json.Linq;
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

        private Task<JObject> ParseJson(string jsonString) 
        {
            return Task.FromResult(JObject.Parse(jsonString));
        }


        public async Task<ConfigElement> Parse(string jsonString, int deep = 3)
        {
            var jObj = await ParseJson(jsonString);
            var res = new ConfigElement(new TypeElement(), null, config);

            foreach (var node in jObj)
            {
                res.Childs.Add(await ReadChild(node.Value, node.Key, res, deep));
            }
            config.ConfigElement = res;
            return res;
        }

        public TypeNode GetType(JToken token, int deep)
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

        public async Task<ConfigElement> ReadChild(JToken node, string key, ConfigElement parrent, int deep) 
        {
            ConfigElement res = null;
            switch (GetType(node, deep))
            {
                case TypeNode.None:
                    return res;
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
                    return res;
                case TypeNode.Array:
                    res = new ConfigElement(new TypeElement(TypeNode.Array, key), parrent, config);
                    foreach (var item in node)
                    {
                        res.Childs.Add(await ReadChild(item, key, res, deep));
                    }
                    return res;
                case TypeNode.Integer:
                    res = new ConfigElement(new TypeElement(TypeNode.Integer, key), parrent, config);
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    return res;
                case TypeNode.Float:
                    res = new ConfigElement(new TypeElement(TypeNode.Float, key), parrent, config);
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    return res;
                case TypeNode.String:
                    res = new ConfigElement(new TypeElement(TypeNode.String, key), parrent, config);
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    return res;
                case TypeNode.Boolean:
                    res = new ConfigElement(new TypeElement(TypeNode.Boolean, key), parrent, config);
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    return res;
                case TypeNode.Null:
                    res = new ConfigElement(new TypeElement(TypeNode.Null, key), parrent, config);
                    res.Value.Add(new ValueElement(null, res.Type));
                    return res;
                case TypeNode.Date:
                    res = new ConfigElement(new TypeElement(TypeNode.Date, key), parrent, config);
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    return res;
                case TypeNode.Guid:
                    res = new ConfigElement(new TypeElement(TypeNode.Guid, key), parrent, config);
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    return res;
                case TypeNode.Uri:
                    res = new ConfigElement(new TypeElement(TypeNode.Uri, key), parrent, config);
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    return res;
                case TypeNode.TimeSpan:
                    res = new ConfigElement(new TypeElement(TypeNode.TimeSpan, key), parrent, config);
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    return res;          
                default:
                    return res;
            }
        }
        
    }
}
