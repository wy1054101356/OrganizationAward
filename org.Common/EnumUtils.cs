using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace org.Common
{



    public class EnumStyle
    {
        public string Description { get; set; }

        public string Style { get; set; }
    }

    public static class EnumUtils
    {

        /// <summary>
        /// 绑定Enum到DropList
        /// </summary>
        /// <param name="type"></param>
        /// <param name="selectID">选中的ID</param>
        /// <returns></returns>
        public static string BindDropListByEnum(object obj, int selectID = 0)
        {
            Type t = obj.GetType();
            StringBuilder sb = new StringBuilder();
            var list = EnumUtils.ToList(t);
            if (list != null && list.Count > 0)
            {
                list = list.OrderBy(n => n.ID).ToList();
                foreach (var item in list)
                {
                    if (item.ID == selectID)
                        sb.Append("<option selected=\"selected\" value=\"" + item.ID + "\">" + item.Description + "</option>");
                    else
                        sb.Append("<option value=\"" + item.ID + "\">" + item.Description + "</option>");
                }
            }
            return sb.ToString();
        }

        private static ConcurrentDictionary<Enum, string> _concurrentDictionary = new ConcurrentDictionary<Enum, string>();
        private static ConcurrentDictionary<Type, Dictionary<string, string>> _concurrentDicDictionary = new ConcurrentDictionary<Type, Dictionary<string, string>>();
        /// <summary>
        /// 锁对象
        /// </summary>
        private static object objLock = new object();


        /// <summary>
        /// 把枚举转换成为列表
        /// </summary>
        public static List<EnumObject> EnumList(object obj)
        {
            Type t = obj.GetType();
            StringBuilder sb = new StringBuilder();
            var list = ToList(t);
            return list;
        }

        /// <summary>
        /// 获取枚举的描述信息(Descripion)。
        /// 支持位域，如果是位域组合值，多个按分隔符组合。
        /// </summary>
        public static string GetDescription(this Enum @this)
        {
            return GetDescriptionStyle(@this).Description;
        }

        /// <summary>
        /// 带样式的描述
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static EnumStyle GetDescriptionStyle(this Enum @this)
        {
            EnumStyle eo = new EnumStyle();
            var val = GetDescriptionVal(@this);
            if (val.IndexOf("|") != -1)
            {
                var strs = val.Split('|');
                eo.Description = strs[0];
                eo.Style = strs[1];
            }
            else
            {
                eo.Description = val;
            }
            return eo;
        }

        /// <summary>
        /// 获取枚举的描述信息(Descripion)。
        /// 支持位域，如果是位域组合值，多个按分隔符组合。
        /// </summary>
        public static string GetDescriptionVal(this Enum @this)
        {
            return _concurrentDictionary.GetOrAdd(@this, (key) =>
            {
                var type = key.GetType();
                var field = type.GetField(key.ToString());
                //如果field为null则应该是组合位域值，
                var val = field == null ? key.GetDescriptions() : GetDescription(field);
                return val;
            });
        }


        ///// <summary>
        ///// 获取枚举的描述信息(Descripion)。
        ///// 支持位域，如果是位域组合值，多个按分隔符组合。
        ///// </summary>
        //public static string GetDescription(this Enum @this)
        //{
        //    return _concurrentDictionary.GetOrAdd(@this, (key) =>
        //    {
        //        var type = key.GetType();
        //        var field = type.GetField(key.ToString());
        //        //如果field为null则应该是组合位域值，
        //        var val = field == null ? key.GetDescriptions() : GetDescription(field);
        //        if (val.IndexOf("|") != -1)
        //        {
        //            var strs = val.Split('|');
        //            return strs[0];
        //        }
        //        return val;
        //    });
        //}


        /// <summary>
        /// 获取枚举的说明
        /// </summary>
        /// <param name="split">位枚举的分割符号（仅对位枚举有作用）</param>
        public static string GetDescriptions(this Enum em, string split = ",")
        {
            var names = em.ToString().Split(',');
            string[] res = new string[names.Length];
            var type = em.GetType();
            for (int i = 0; i < names.Length; i++)
            {
                var field = type.GetField(names[i].Trim());
                if (field == null) continue;
                res[i] = GetDescription(field);
            }
            return string.Join(split, res);
        }

        private static string GetDescription(FieldInfo field)
        {
            var att = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute), false);
            return att == null ? field.Name : ((DescriptionAttribute)att).Description;
        }

        /// <summary>
        /// 把枚举转换成为列表
        /// </summary>
        public static List<EnumObject> ToList(this Type type)
        {
            List<EnumObject> list = new List<EnumObject>();
            foreach (object obj in Enum.GetValues(type))
            {
                list.Add(new EnumObject((Enum)obj));
            }
            return list;
        }

        /// <summary>
        /// 构造UTable枚举json样式 eg.{"Resource":{"value":0,"name":"Resource","text":"自有资源"},"New":{"value":1,"name":"New","text":"业务费用"}}
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Dictionary<string, EnumModel> GetEnumList(this Type type)
        {
            Dictionary<string, EnumModel> list = new Dictionary<string, EnumModel>();
            foreach (object obj in Enum.GetValues(type))
            {
                list.Add(((Enum)obj).ToString(), new EnumModel((Enum)obj));
            }
            return list;
        }

        ///<summary>  
        /// 获取枚举值+描述  
        ///</summary>  
        ///<param name="enumType">Type,该参数的格式为typeof(需要读的枚举类型)</param>  
        ///<returns>键值对</returns>  
        public static Dictionary<string, string> GetEnumItemValueDesc(Type enumType)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            Type typeDescription = typeof(DescriptionAttribute);
            FieldInfo[] fields = enumType.GetFields();
            string strText = string.Empty;
            string strValue = string.Empty;
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    strValue = ((int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        DescriptionAttribute aa = (DescriptionAttribute)arr[0];
                        strText = aa.Description;
                    }
                    else
                    {
                        strText = field.Name;
                    }
                    dic.Add(strValue, strText);
                }
            }

            return dic;


        }

        /// <summary>
        /// 获取枚举类型键值对
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetEunItemValueAndDesc(Type em)
        {
            return _concurrentDicDictionary.GetOrAdd(em, (key) =>
            {
                var type = key.GetType();
                if (_concurrentDicDictionary.ContainsKey(key))
                    return _concurrentDicDictionary[key];
                else
                    return GetEnumItemValueDesc(em);
            });
        }
        ///<summary>  
        /// 获取枚举项+描述  
        ///</summary>  
        ///<param name="enumType">Type,该参数的格式为typeof(需要读的枚举类型)</param>  
        ///<returns>键值对</returns>  
        public static Dictionary<string, string> GetEnumItemDesc(Type enumType)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            FieldInfo[] fieldinfos = enumType.GetFields();
            foreach (FieldInfo field in fieldinfos)
            {
                if (field.FieldType.IsEnum)
                {
                    Object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    dic.Add(field.Name, ((DescriptionAttribute)objs[0]).Description);
                }
            }
            return dic;
        }
        /// <summary>  
        /// 获取枚举项描述信息 例如GetEnumDesc(Days.Sunday)  
        /// </summary>  
        /// <param name="en">枚举项 如Days.Sunday</param>  
        /// <returns></returns>  
        public static string GetEnumDesc(this Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }

        /// <summary>
        /// 将注释转换成枚举值，匹配不上返回Null
        /// </summary>
        /// <param name="type"></param>
        /// <param name="strDescription"></param>
        /// <returns></returns>
        public static int? GetEnumValByDescription(this Type type, string strDescription)
        {
            int? enumVal = null;
            foreach (object obj in Enum.GetValues(type))
            {
                Enum nEnum = (Enum)obj;
                if (nEnum.GetDescription() == strDescription)
                {
                    enumVal = (int)Convert.ChangeType(nEnum, typeof(int));
                }
            }
            return enumVal;
        }
    }

    public struct EnumModel
    {
        public EnumModel(Enum um)
        {
            this.value = (int)Convert.ChangeType(um, typeof(int));
            this.name = um.ToString();
            this.text = um.GetDescription();
        }
        public int value { get; set; }
        public string name { get; set; }
        public string text { get; set; }
    }

    public struct EnumObject
    {
        public EnumObject(Enum um, string style = null)
        {
            this.ID = (int)Convert.ChangeType(um, typeof(int));
            this.Name = um.ToString();
            this.Description = um.GetDescription();
            this.Style = style;
        }

        public EnumObject(int id, string name)
        {
            this.ID = id;
            this.Name = this.Description = name;
            this.Style = null;
        }

        public EnumObject(int id, string name, string description, string style)
        {
            this.ID = id;
            this.Name = name;
            this.Description = description;
            this.Style = style;
        }

        public int ID;

        public string Name;

        public string Description;

        public string Style;
    }
}
