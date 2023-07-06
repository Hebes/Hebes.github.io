using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;



namespace ACFrameworkCore
{
    /// <summary>
    /// 日志
    /// </summary>
    public class DLog
    {
        /// <summary>
        /// Log的包裹类  服务器版本
        /// </summary>
        class ConsoleLogger : ILogger
        {
            public void Log(string msg, LogCoLor logCoLor = LogCoLor.None) => WriteConsoleLog(msg, logCoLor);
            public void Warn(string msg) => WriteConsoleLog(msg, LogCoLor.DarkYellow);
            public void Error(string msg) => WriteConsoleLog(msg, LogCoLor.DarkRed);

            /// <summary>
            /// 消息的颜色
            /// </summary>
            /// <param name="msg"></param>
            /// <param name="color"></param>
            private void WriteConsoleLog(string msg, LogCoLor color)
            {
                switch (color)
                {
                    default:
                    case LogCoLor.None:
                        Console.WriteLine(msg);
                        break;
                    case LogCoLor.DarkRed:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LogCoLor.Green:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LogCoLor.Blue:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LogCoLor.Cyan:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LogCoLor.Magenta:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LogCoLor.DarkYellow:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                }
            }
        }

        /// <summary>
        /// Log的包裹类  Unity版本
        /// </summary>
        class UnityLogger : ILogger
        {
            //反射获取Unity的日志输出系统
            Type type = Type.GetType("UnityEngine.Debug, UnityEngine");

            public void Log(string msg, LogCoLor LogCoLor)
            {
                if (LogCoLor != LogCoLor.None)
                    msg = ColorUnityLog(msg, LogCoLor);
                type.GetMethod("Log", new Type[] { typeof(object) })?.Invoke(null, new object[] { msg });
            }
            public void Warn(string msg)
            {
                type.GetMethod("LogWarning", new Type[] { typeof(object) })?.Invoke(null, new object[] { msg });
            }
            public void Error(string msg)
            {
                type.GetMethod("LogError", new Type[] { typeof(object) })?.Invoke(null, new object[] { msg });
            }

            private string ColorUnityLog(string msg, LogCoLor color)
            {
                switch (color)
                {
                    default:
                    case LogCoLor.None:
                        msg = string.Format($"<coLor=#FF000O>{msg}</coLor>");
                        break;
                    case LogCoLor.DarkRed:
                        msg = string.Format($"<coLor=#FF000O>{msg}</coLor>");
                        break;
                    case LogCoLor.Green:
                        msg = string.Format($"<coLor=#00FF00>{msg}</coLor>");
                        break;
                    case LogCoLor.Blue:
                        msg = string.Format($"<coLor=#0000FF>{msg}</coLor>");
                        break;
                    case LogCoLor.Cyan:
                        msg = string.Format($"<coLor=#00FFFF>{msg}</coLor>");
                        break;
                    case LogCoLor.Magenta:
                        msg = string.Format($"<coLor=#FF00FF>{msg}</coLor>");
                        break;
                    case LogCoLor.DarkYellow:
                        msg = string.Format($"<coLor=#FFff0O>{msg}</coLor>");
                        break;
                }
                return msg;
            }
        }

        /// <summary>
        /// 配置文件
        /// </summary>
        public static LogConfig cfg;
        /// <summary>
        /// 输出的日志类型
        /// </summary>
        private static ILogger logger;
        private static StreamWriter LogFiLeWriter = null;

        private const string logLock = "PELogLock";

        /// <summary>
        /// 初始化设置
        /// </summary>
        public static void InitSettings(LogConfig cfg = null)
        {
            cfg = DLog.cfg = cfg == null ? new LogConfig() : cfg;
            switch (DLog.cfg.eLoggerType)
            {
                case LoggerType.Unity:
                    logger = new UnityLogger();
                    break;
                case LoggerType.Console:
                    logger = new ConsoleLogger();
                    break;
                default:
                    break;
            }
            if (cfg.enableSave == false)
                return;

            if (cfg.enableCover)
            {
                string path = cfg.savePath + cfg.saveName;
                try
                {
                    if (Directory.Exists(cfg.savePath))//存在这个路径
                    {
                        if (File.Exists(path))//存在这个文件
                        {
                            File.Delete(path);
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(cfg.savePath);
                    }
                    LogFiLeWriter = File.AppendText(path);
                    LogFiLeWriter.AutoFlush = true;
                }
                catch (Exception) { LogFiLeWriter = null; }
            }
            else
            {
                string prefix = DateTime.Now.ToString("yyyyMMdd@HH-mm-s");
                string path = cfg.savePath + prefix + cfg.saveName;
                try
                {
                    logger.Log("主动日志的输出路径为：" + path);
                    if (Directory.Exists(cfg.savePath) == false)
                    {
                        Directory.CreateDirectory(cfg.savePath);
                    }
                    LogFiLeWriter = File.AppendText(path);
                    LogFiLeWriter.AutoFlush = true;
                }
                catch (Exception) { LogFiLeWriter = null; }
            }
        }

        /// <summary>
        /// 打印日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        public static void Log(string msg, params object[] args)
        {
            if (cfg.enableLog == false)
                return;
            msg = DecorateLog(string.Format(msg, args));
            lock (logLock)
            {
                logger.Log(msg);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[L]{msg}"));
            }
        }

        /// <summary>
        /// 打印日志
        /// </summary>
        /// <param name="obj"></param>
        public static void Log(object obj)
        {
            if (cfg.enableLog == false)
                return;
            string msg = DecorateLog(obj.ToString());
            lock (logLock)
            {
                logger.Log(msg);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[L]{msg}"));
            }
        }

        /// <summary>
        /// 打印带有颜色的日志
        /// </summary>
        /// <param name="logCoLorEnum">颜色</param>
        /// <param name="msg">字符串</param>
        /// <param name="args">消息</param>
        public static void Log(LogCoLor logCoLorEnum, string msg, params object[] args)
        {
            if (cfg.enableLog == false)
                return;
            msg = DecorateLog(string.Format(msg, args));
            lock (logLock)
            {
                logger.Log(msg, logCoLorEnum);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[L]{msg}"));
            }
        }

        /// <summary>
        ///  打印带有颜色的日志
        /// </summary>
        /// <param name="logCoLorEnum">颜色</param>
        /// <param name="obj">泛型</param>
        public static void Log(LogCoLor logCoLorEnum, object obj)
        {
            if (cfg.enableLog == false)
                return;
            string msg = DecorateLog(obj.ToString());
            lock (logLock)
            {
                logger.Log(msg, logCoLorEnum);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[L]{msg}"));
            }
        }

        /// <summary>
        /// 打印堆栈
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        public static void Trace(string msg, params object[] args)
        {
            if (cfg.enableLog == false)
                return;
            msg = DecorateLog(string.Format(msg, args), true);
            lock (logLock)
            {
                logger.Log(msg, LogCoLor.Magenta);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[T]{msg}"));
            }
        }

        /// <summary>
        /// 打印堆栈
        /// </summary>
        /// <param name="obj"></param>
        public static void Trace(object obj)
        {
            if (cfg.enableLog == false)
                return;
            string msg = DecorateLog(obj.ToString());
            lock (logLock)
            {
                logger.Log(msg, LogCoLor.Magenta, true);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[T]{msg}"));
            }
        }

        /// <summary>
        /// 打印警告日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        public static void Warn(string msg, params object[] args)
        {
            if (cfg.enableLog == false)
                return;
            msg = DecorateLog(string.Format(msg, args));
            lock (logLock)
            {
                logger.Warn(msg);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[W]{msg}"));
            }
        }
        /// <summary>
        /// 打印警告日志
        /// </summary>
        /// <param name="obj"></param>
        public static void Warn(object obj)
        {
            if (cfg.enableLog == false)
                return;
            string msg = DecorateLog(obj.ToString());
            lock (logLock)
            {
                logger.Warn(msg);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[W]{msg}"));
            }
        }

        /// <summary>
        /// 打印错误日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        public static void Error(string msg, params object[] args)
        {
            if (cfg.enableLog == false)
                return;
            msg = DecorateLog(string.Format(msg, args), true);
            lock (logLock)
            {
                logger.Error(msg);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[E]{msg}"));
            }
        }

        /// <summary>
        /// 打印错误日志
        /// </summary>
        /// <param name="obj"></param>
        public static void Error(object obj)
        {
            if (cfg.enableLog == false)
                return;
            string msg = DecorateLog(obj.ToString(), true);
            lock (logLock)
            {
                logger.Error(msg);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[E]{msg}"));
            }
        }

        #region Tool
        /// <summary>
        /// 日志打印
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="isTrace"></param>
        /// <returns></returns>
        private static string DecorateLog(string msg, bool isTrace = false)
        {
            StringBuilder sb = new StringBuilder(cfg.LogPrefix, 100);
            if (cfg.enableTime)//启用时间
                sb.AppendFormat($"时间:{DateTime.Now.ToString("hh:mm:ss--fff")}");
            if (cfg.enableThreadID)//启用线程
                sb.AppendFormat($"{GetThreadID()}");
            sb.AppendFormat($" {cfg.LogSeparate} {msg}");//日志分离
            if (isTrace)//是否追踪日志 堆栈
                sb.AppendFormat($" \nStackTrace:{GetLogTrace()}");
            return sb.ToString();
        }

        /// <summary>
        /// 获取日志追踪
        /// </summary>
        /// <returns></returns>
        private static string GetLogTrace()
        {
            StackTrace st = new StackTrace(3, true);//3 跳跃3帧 true-> 获取场下文信息
            string traceInfo = string.Empty;
            for (int i = 0; i < st.FrameCount; i++)
            {
                StackFrame sf = st.GetFrame(i);
                traceInfo += string.Format($"\n\t{sf.GetFileName()}::{sf.GetMethod()}line:{sf.GetFileLineNumber()}");
                //traceInfo += string.Format($"\n\t{sf.GetFileName()}::\n\t{sf.GetMethod()}\tline:{sf.GetFileLineNumber()}");
                //traceInfo += string.Format($"\n\t脚本:{sf.GetFileName()}::方法{sf.GetMethod()}行: {sf.GetFileLineNumber()}");
            }
            return traceInfo;
        }

        /// <summary>
        /// 获取线程Id
        /// </summary>
        /// <returns></returns>
        private static object GetThreadID()
        {
            return string.Format($" ThreadID:{Thread.CurrentThread.ManagedThreadId}");
        }

        /// <summary>
        /// 日志写入文件
        /// </summary>
        /// <param name="msg"></param>
        private static void WriteToFile(string msg)
        {
            if (cfg.enableSave && LogFiLeWriter != null)
            {
                try
                {
                    LogFiLeWriter.WriteLine(msg);
                }
                catch
                {
                    LogFiLeWriter = null;
                    return;
                }
            }
        }

        /// <summary>
        /// 打印数组数据For Debug
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="prefix"></param>
        /// <param name="printer"></param>
        public static void PrintBytesArray(byte[] bytes, string prefix, Action<string> printer = null)
        {
            string str = prefix + "->\n";
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i % 10 == 0)
                {
                    str += bytes[i] + "\n";
                }
                str += bytes[i] + " ";
            }
            if (printer != null)
            {
                printer(str);
            }
            else
            {
                Log(str);
            }
        }
        #endregion
    }
}
