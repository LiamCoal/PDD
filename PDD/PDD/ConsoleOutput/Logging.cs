using System;
using System.IO;
using System.Runtime.CompilerServices;
// ReSharper disable ExplicitCallerInfoArgument
// ReSharper disable UnusedMember.Global

namespace PDD.ConsoleOutput
{
    public static class Logging
    {
        private enum Severity
        {
            Info,
            Warning,
            Error,
            FatalError
        }

        private static readonly StreamWriter LogFile = new StreamWriter("Output.log");

        private static string SeverityToString(Severity severity)
            // ReSharper disable once UseStringInterpolation
            => string.Format("{0,10}", severity switch
            {
                Severity.Info => "INFO",
                Severity.Warning => "WARNING",
                Severity.Error => "ERROR",
                Severity.FatalError => "FATAL",
                _ => "CAT"
            });

        private static void Log(Severity severity, string msg,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string callerName = "???",
            [CallerFilePath] string callerPath = "???")
        {
            var methodName = callerName;
            var str = $"{SeverityToString(severity)} {FormatDate} [{methodName} : {callerPath} @ {lineNumber}] # {msg}";
            Console.WriteLine(str);
            LogFile.WriteLine(str);
        }

        private static DateTime Date => DateTime.Now;
        private static object FormatDate => $"{Date.Hour:D2}:{Date.Minute:D2}:{Date.Second:D2}.{Date.Millisecond:D4}";

        // Note: The logging functions propagate the line number. This is intentional, so that an actually useful line
        // number is printed.
        //
        // That's why the ExplicitCallerInfoArgument warning is disabled, so that it's less annoying.
        
        // Special version of Log that looks farther in the stack trace.
        // ReSharper disable once ExplicitCallerInfoArgument
        private static void Log0(Severity severity, string msg,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string callerName = "???",
            [CallerFilePath] string callerPath = "???") =>
            Log(severity, msg, lineNumber, callerName, callerPath);

        
        // ReSharper disable once ExplicitCallerInfoArgument
        public static void Info(string msg, 
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string callerName = "???",
            [CallerFilePath] string callerPath = "???") => Log0(Severity.Info, msg, lineNumber, callerName, callerPath);
        // ReSharper disable once ExplicitCallerInfoArgument
        public static void Warning(string msg, 
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string callerName = "???",
            [CallerFilePath] string callerPath = "???") => Log0(Severity.Warning, msg, lineNumber, callerName, callerPath);
        // ReSharper disable once ExplicitCallerInfoArgument
        public static void Error(string msg, 
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string callerName = "???",
            [CallerFilePath] string callerPath = "???") => Log0(Severity.Error, msg, lineNumber, callerName, callerPath);
        // ReSharper disable once ExplicitCallerInfoArgument
        public static void FatalError(string msg, 
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string callerName = "???",
            [CallerFilePath] string callerPath = "???") => Log0(Severity.FatalError, msg, lineNumber, callerName, callerPath);

        internal static void Close() => LogFile.Close();
    }
}