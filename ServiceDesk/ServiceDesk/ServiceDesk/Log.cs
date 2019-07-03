using Android.Text;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceDesk
{
    /// <summary>
    /// Класс описывающий логирование
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Записывает сообщение в Log
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteMessage(string msg)
        {
            Android.Util.Log.WriteLine(Android.Util.LogPriority.Info, "SERVICE DESK LOGER", GetLocation() + msg);       
        }

        private static string GetLocation()
        {
            string className = typeof(Log).Name;
            StackTraceElement[] traces = Thread.CurrentThread().GetStackTrace();
            bool found = false;

            for(int i = 0; i < traces.Length; i++)
            {
                StackTraceElement trace = traces[i];

                try
                {
                    if (found)
                    {
                        if (!trace.ClassName.StartsWith(className))
                        {
                            var anonClass = Class.ForName(trace.ClassName);
                            return $"[ {GetClassName(anonClass)} : {trace.MethodName} : {trace.LineNumber} ]";
                        }
                    }else if (trace.ClassName.StartsWith(className))
                    {
                        found = true;
                        continue;
                    }
                }
                catch (ClassNotFoundException)
                {
                       
                }
            }

            return "[]";
        }

        private static string GetClassName<T>(T anonClass)
        {
            if(anonClass != null)
            {
                //if (!TextUtils.IsEmpty(anonClass.GetType().Name))
                //{
                //    return anonClass.GetType().Name;
                //}

                return GetClassName(anonClass.GetType().FullName);
            }
            return "";
        }
    }
}
