namespace ACFrameworkCore
{
    public static class LogExtension
    {
        /// <summary>
        /// 打印日志
        /// </summary>
        /// <param name="obj">拓展的类型</param>
        /// <param name="Log">字符串</param>
        /// <param name="args">参数</param>
        public static void Log(this object obj, string Log, params object[] args)
        {
            DLog.Log(string.Format(Log, args));
        }
        /// <summary>
        /// 打印日志
        /// </summary>
        /// <param name="obj">拓展的类型</param>
        /// <param name="Log">字符串</param>
        public static void Log(this object obj, object Log)
        {
            DLog.Log(Log);
        }
        /// <summary>
        /// 打印带有颜色的日志
        /// </summary>
        /// <param name="obj">拓展的类型</param>
        /// <param name="logCoLorEnum">颜色</param>
        /// <param name="Log">字符串</param>
        /// <param name="args">参数</param>
        public static void Log(this object obj, LogCoLor logCoLorEnum, string Log, params object[] args)
        {
            DLog.Log(logCoLorEnum, string.Format(Log, args));
        }
        /// <summary>
        /// 打印带有颜色的日志
        /// </summary>
        /// <param name="obj">拓展的类型</param>
        /// <param name="logCoLorEnum">颜色</param>
        /// <param name="Log">字符串</param>
        public static void Log(this object obj, LogCoLor logCoLorEnum, object Log)
        {
            DLog.Log(logCoLorEnum, Log);
        }

        /// <summary>
        /// 打印堆栈
        /// </summary>
        /// <param name="obj">拓展的类型</param>
        /// <param name="Log">字符串</param>
        /// <param name="args">参数</param>
        public static void Trace(this object obj, string Log, params object[] args)
        {
            DLog.Trace(string.Format(Log, args));
        }
        /// <summary>
        /// 打印堆栈
        /// </summary>
        /// <param name="obj">拓展的类型</param>
        /// <param name="Log">字符串</param>
        public static void Trace(this object obj, object Log)
        {
            DLog.Trace(Log);
        }

        /// <summary>
        /// 打印警告日志
        /// </summary>
        /// <param name="obj">拓展的类型</param>
        /// <param name="Log">字符串</param>
        /// <param name="args">参数</param>
        public static void Warn(this object obj, string Log, params object[] args)
        {
            DLog.Warn(string.Format(Log, args));
        }
        /// <summary>
        /// 打印警告日志
        /// </summary>
        /// <param name="obj">拓展的类型</param>
        /// <param name="Log">字符串</param>
        public static void Warn(this object obj, object Log)
        {
            DLog.Warn(Log);
        }

        /// <summary>
        /// 打印错误日志
        /// </summary>
        /// <param name="obj">拓展的类型</param>
        /// <param name="Log">字符串</param>
        /// <param name="args">参数</param>
        public static void Error(this object obj, string Log, params object[] args)
        {
            DLog.Error(string.Format(Log, args));
        }
        /// <summary>
        /// 打印错误日志
        /// </summary>
        /// <param name="obj">拓展的类型</param>
        /// <param name="Log">字符串</param>
        public static void Error(this object obj, object Log)
        {
            DLog.Error(Log);
        }
    }
}
