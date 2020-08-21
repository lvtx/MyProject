using System;
using System.Runtime.CompilerServices;

namespace SimpleDB
{
    public class Debug
    {
        private static readonly int DEBUG_LEVEL;
        static Debug()
        {
            //String debug = System.getProperty("simpledb.Debug");
            String debug = Environment.GetEnvironmentVariable("SimpleDb.Debug");
            if (debug == null)
            {
                // No system property = disabled
                DEBUG_LEVEL = -1;
            }
            else if (debug == "")
            {
                // Empty property = level 0
                DEBUG_LEVEL = 0;
            }
            else
            {
                DEBUG_LEVEL = int.Parse(debug);
            }
        }

        private static readonly int DEFAULT_LEVEL = 0;

        /** Log message if the log level >= level. Uses printf. */
        public static void Log(int level, String message,params object[] args)
        {
            if (IsEnabled(level))
            {
                Console.WriteLine("{0}{1}", message, args);
            }
        }

        /** @return true if level is being logged. */
        public static bool IsEnabled(int level)
        {
            return level <= DEBUG_LEVEL;
        }

        /** @return true if the default level is being logged. */
        public static bool IsEnabled()
        {
            return IsEnabled(DEFAULT_LEVEL);
        }

        /** Logs message at the default log level. */
        public static void Log(String message,params object[] args)
        {
            Log(DEFAULT_LEVEL, message, args);
        }
    }
}
