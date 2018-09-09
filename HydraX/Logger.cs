/*
 *  HydraX Logger - Copyright 2018 Philip/Scobalula
 *  
 *  This file is subject to the license terms set out in the
 *  "LICENSE.txt" file. 
 * 
 */
using PhilUtil;

namespace HydraX
{
    class LoggingUtil
    {
        public static Logger ActiveLogger = new Logger("HydraX - Active", "HydraX-Log.txt");

        public static void WriteLog(string message, MessageType messageType)
        {
            ActiveLogger.Log(message, messageType);
        }
    }
}
