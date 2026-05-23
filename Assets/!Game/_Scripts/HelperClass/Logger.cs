using UnityEngine;
public static class Logger
{
    public struct LogStyles
    {
        public string color;
        public string size;
        public bool bold;
        public bool italic;
        public LogLevel logLevel;
        public LogStyles(string color = "", string size = "", bool bold = true, bool italic = false, LogLevel logLevel = LogLevel.Info) : this()
        {
            this.color = color;
            this.size = size;
            this.bold = bold;
            this.italic = italic;
            this.logLevel = logLevel;
        }
    }
    private static object StyleMessage(object message, LogStyles styles)
    {
        if (styles.color == "")
        {
            switch (styles.logLevel)
            {
                case LogLevel.Info:
                    styles.color = "grey";
                    break;
                case LogLevel.Debug:
                    styles.color = "white";
                    break;
                case LogLevel.Warning:
                    styles.color = "yellow";
                    break;
                case LogLevel.Error:
                    styles.color = LogColors.red;
                    break;
                default:
                    break;
            }
        }

        void StyleTag(string tagName, string tagValue = "")
        {
            message = $"<{tagName}={tagValue}>{message}</{tagName}>";
        }

        if (styles.color != "") { StyleTag("color", styles.color); }
        if (styles.bold) { StyleTag("b"); }
        if (styles.italic) { StyleTag("i"); }
        if (styles.size != "") { StyleTag("size", styles.size); }

        return message;
    }
    public static void LogSigned(string sender, object message, LogStyles styles = new(), Object context = null)
    {
        string signature = $"<b>{sender}:</b>";
        string finalText = $"{signature} {message}";
        Log(finalText, styles, context);
    }
    public static void Log(object message, LogStyles styles = new(), Object context = null)
    {
        message = StyleMessage(message, styles);
        if (styles.logLevel == LogLevel.Warning) Debug.LogWarning(message, context);
        else if (styles.logLevel == LogLevel.Error) Debug.LogError(message, context);
        else Debug.Log(message, context);
    }
    public static void BigLog(object message, LogStyles styles = new(), Object context = null)
    {
        if (styles.size == "") styles.size = "40";
        Log(message, styles, context);
    }

    public static void Warning(object message, LogStyles styles = new(), Object context = null)
    {
        styles.logLevel = LogLevel.Warning;
        Log(message, styles, context);
    }
    public static void Error(object message, LogStyles styles = new(), Object context = null)
    {
        styles.logLevel = LogLevel.Error;
        Log(message, styles, context);
    }
}

public static class LogColors
{
    public static string green => "#79FF3F";
    public static string red => "#FF3F3F";
    public static string blue => "#3FAFFF";
    public static string Green(string s) { return $"<color={green}>{s}</color>"; }
    public static string Red(string s) { return $"<color={red}>{s}</color>"; }
    public static string Blue(string s) { return $"<color={blue}>{s}</color>"; }
}
public enum LogLevel
{
    Info,
    Debug,
    Warning,
    Error
}