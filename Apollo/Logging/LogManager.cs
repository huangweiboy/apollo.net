﻿using Com.Ctrip.Framework.Apollo.Logging.Internals;
using Com.Ctrip.Framework.Apollo.Logging.Spi;
using System;
using System.Collections.Generic;

namespace Com.Ctrip.Framework.Apollo.Logging
{
    /// <summary>
    /// 用于创建 <see cref="ILog" /> 实例，主要用于应用程序日志.
    /// </summary>
    public sealed class LogManager
    {
        private static readonly Dictionary<string, ILog> Logs = new Dictionary<string, ILog>();
        private static readonly object LockObject = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="LogManager" /> class. 
        /// </summary>
        /// <remarks>
        /// Uses a private access modifier to prevent instantiation of this class.
        /// </remarks>
        private LogManager()
        { }

        /// <summary>
        /// 通过类型名获取ILog实例。
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>ILog instance</returns>
        public static ILog GetLogger(Type type)
        {
            if (type == null)
            {
                return GetLogger("NoName");
            }
            else
            {
                return GetLogger(type.FullName);
            }
        }


        /// <summary>
        /// 通过字符串名获取ILog实例。
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>ILog instance</returns>
        public static ILog GetLogger(string name)
        {
            var loggerName = name;
            if (string.IsNullOrEmpty(name) || name.Trim().Length == 0)
                loggerName = "defaultLogger";

            if (!Logs.TryGetValue(loggerName, out var log))
            {
                lock (LockObject)
                {
                    if (!Logs.TryGetValue(loggerName, out log))
                    {
                        log = new DefaultLogger(loggerName);

                        Logs.Add(loggerName, log);
                    }
                }
            }

            return log;
        }
    }
}
