using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;


namespace MiniFrameWork.Util
{
    public static class Reflector
    {
        private const BindingFlags CommonFlags = BindingFlags.Public | BindingFlags.NonPublic;

        static public BindingFlags MemberAccess = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase;
        /// <summary>
        /// 
        /// </summary>
        public static object CreateInstance(Type type, params object[] args)
        {
            return Reflector.InvokeMember(
                type, null, null,
                Reflector.CommonFlags | BindingFlags.CreateInstance | BindingFlags.Instance, args);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void SetField(object target, string fieldName, object value)
        {
            Reflector.InvokeMember(
                target.GetType(), target, fieldName,
                Reflector.CommonFlags | BindingFlags.SetField | BindingFlags.Instance, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static object GetField(object target, string fieldName)
        {
            return Reflector.InvokeMember(
                target.GetType(), target, fieldName,
                Reflector.CommonFlags | BindingFlags.GetField | BindingFlags.Instance);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool SetProperty(object target, string propertyName, object value)
        {

            try
            {
                

                Reflector.InvokeMember(
                    target.GetType(), target, propertyName.Trim(),
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.SetProperty | BindingFlags.Instance, value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }




        }


        public static bool isProperty(object target, string propertyName)
        {
            try
            {
                Reflector.InvokeMember(
                target.GetType(), target, propertyName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.GetProperty | BindingFlags.Instance);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        
        /// <summary>
        /// 
        /// </summary>
        public static object GetProperty(object target, string propertyName)
        {
            object lRetorno = null;
            try
            {
                lRetorno = Reflector.InvokeMember(
                target.GetType(), target, propertyName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.GetProperty | BindingFlags.Instance);

                return lRetorno;
            }
            catch (Exception)
            {
                return null;
            }

        }


        public static PropertyInfo getPropertyInfo(object target, string propertyName)
        {
            Type lTipo = (Type)target;
            
            return lTipo.GetProperty(propertyName, MemberAccess);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void StaticSetField(Type type, string fieldName, object value)
        {
            Reflector.InvokeMember(
                type, null, fieldName,
                Reflector.CommonFlags | BindingFlags.SetField | BindingFlags.Static, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static object StaticGetField(Type type, string fieldName)
        {
            return Reflector.InvokeMember(
                type, null, fieldName,
                Reflector.CommonFlags | BindingFlags.GetField | BindingFlags.Static);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void StaticSetProperty(Type type, string propertyName, object value)
        {
            Reflector.InvokeMember(
                type, null, propertyName,
                Reflector.CommonFlags | BindingFlags.SetProperty | BindingFlags.Static, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static object StaticGetProperty(Type type, string propertyName)
        {
            return Reflector.InvokeMember(
                type, null, propertyName,
                Reflector.CommonFlags | BindingFlags.GetProperty | BindingFlags.Static);
        }

        /// <summary>
        /// 
        /// </summary>
        public static object CallMethod(object target, string methodName, params object[] args)
        {
            return Reflector.InvokeMember(
                target.GetType(), target, methodName,
                Reflector.CommonFlags | BindingFlags.InvokeMethod | BindingFlags.Instance, args);
        }

        /// <summary>
        /// 
        /// </summary>
        public static object StaticCallMethod(Type type, string memberName, params object[] args)
        {
            return Reflector.InvokeMember(
                type, null, null,
                Reflector.CommonFlags | BindingFlags.InvokeMethod | BindingFlags.Static, args);
        }

        /// <summary>
        /// 
        /// </summary>
        private static object InvokeMember( Type type, object target, string memberName, BindingFlags flags, params object[] args)
        {
            
            return type.InvokeMember(memberName, flags, null, target, args);
        }

        public static bool isNull(object pObj)
        {
            bool lRetorno = false;

            if (pObj == null)
            {
                lRetorno = true;
            }
            else
            {
                if (pObj.GetType() == typeof(string))
                {
                    if (((string)pObj) == string.Empty)
                    {
                        lRetorno = true;
                    }
                }

            }
            return lRetorno;
        }
    }

}
//3634-6804 / 