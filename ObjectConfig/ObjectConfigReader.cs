using Newtonsoft.Json.Linq;
using ObjectConfig.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjectConfig
{

    public class Obj { 
        public string Name { get; set; }
        public string Value { get; set; }

    }
    public class ObjectConfigReader
    {
        private Task<JObject> ParseJson(string jsonString) 
        {
            return Task.FromResult(JObject.Parse(jsonString));
        }


        public async Task<ConfigElement> Parse(string jsonString, int deep = 3)
        {
            var jObj = await ParseJson(jsonString);
            var res = new ConfigElement() { Type = new TypeElement() };


            foreach (var node in jObj)
            {
               
                res.Childs.Add(await ReadChild(node.Value, node.Key, res, 3));
            }

            return res;
        }

        public TypeNode GetType(JToken token, int deep)
        {
            switch (token.Type)
            {
               // case JTokenType.Property:
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
                        res = new ConfigElement() { Type = new TypeElement(TypeNode.Complex, key), Parrent = parrent };
                        res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    }
                    else
                    {
                        if (node.Type == JTokenType.Property)
                        {
                            throw new Exception("JTokenType.Property " + node.ToString());
                            string y = node.GetType().ToString();
                            Console.WriteLine(y);
                            foreach (var item in node.Children())
                            {
                              //  parrent.Childs.Add(await ReadChild(item, res, deep));
                            }

                        }
                        else
                        {
                            JObject jobject = node as JObject;
                            res = new ConfigElement() { Type = new TypeElement(TypeNode.Complex, key), Parrent = parrent };
                            foreach (var item in jobject)
                            {
                                res.Childs.Add(await ReadChild(item.Value,item.Key, res, --deep));

                            }
                        }
                    }
                    return res;
                case TypeNode.Array:

                    res = new ConfigElement() { Type = new TypeElement(TypeNode.Array, key), Parrent = parrent };
                    foreach (var item in node)
                    {
                        res.Childs.Add(await ReadChild(item, key, res, deep));
                    }

                    return res;
                case TypeNode.Integer:
                    res = new ConfigElement() { Type = new TypeElement(TypeNode.Integer, key), Parrent = parrent };
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    return res;
                case TypeNode.Float:
                    res = new ConfigElement() { Type = new TypeElement(TypeNode.Float, key), Parrent = parrent };
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    return res;
                case TypeNode.String:
                    res = new ConfigElement() { Type = new TypeElement(TypeNode.String, key), Parrent = parrent };
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    return res;
                case TypeNode.Boolean:
                    res = new ConfigElement() { Type = new TypeElement(TypeNode.Boolean, key), Parrent = parrent };
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    return res;
                case TypeNode.Null:
                    res = new ConfigElement() { Type = new TypeElement(TypeNode.Null, key), Parrent = parrent };
                    res.Value.Add(new ValueElement(null, res.Type));
                    return res;
                case TypeNode.Date:
                    res = new ConfigElement() { Type = new TypeElement(TypeNode.Date, key), Parrent = parrent };
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    return res;
                case TypeNode.Guid:
                    res = new ConfigElement() { Type = new TypeElement(TypeNode.Guid, key), Parrent = parrent };
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    return res;
                case TypeNode.Uri:
                    res = new ConfigElement() { Type = new TypeElement(TypeNode.Uri, key), Parrent = parrent };
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    return res;
                case TypeNode.TimeSpan:
                    res = new ConfigElement() { Type = new TypeElement(TypeNode.TimeSpan, key), Parrent = parrent };
                    res.Value.Add(new ValueElement(node.ToString(), res.Type));
                    return res;          
                default:
                    return res;
            }
        }
        
    }
}
