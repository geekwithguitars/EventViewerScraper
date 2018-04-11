using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EventViewerScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteFile(GetEventViewerLogs(), Properties.Settings.Default.OutputFilePath);
            //Console.ReadKey();
        }

        static void DisplayWelcomeMessage()
        {
            Console.WriteLine("EventViewerScraper v1.0");
            Console.WriteLine("by Ramon Ecung");
        }

        static string GetEventViewerLogs()
        {
            string eventLogName = Properties.Settings.Default.EventLogName;
            EventLog eventLog = new EventLog();
            eventLog.Log = eventLogName;
            StringBuilder sb = new StringBuilder();

            foreach (EventLogEntry log in eventLog.Entries)
            {
                if (log.Source == Properties.Settings.Default.EventViewerSource && FilterMessageByTime(log.TimeWritten))
                {
                    //EncodeString(log.Message);
                    //Console.WriteLine(log.Message);
                    string line = log.TimeWritten.ToString("HH:mm")  + Properties.Settings.Default.delimiter + log.TimeWritten.ToString("D").Split(',')[0] + Properties.Settings.Default.delimiter + FormatMessage(log.Message) + Properties.Settings.Default.delimiter + isLogValid(log.Message);
                    Console.WriteLine(line);
                    sb.AppendLine(line);
                }
            }

            return sb.ToString();
        }

        static int isLogValid(string LogMessage)
        {
            //Put logic here if you want to automatically flag certain logs as valid or not
            return 0;
        }

        static bool FilterMessageByTime(DateTime TimeWritten)
        {
            if(TimeWritten < DateTime.Now.AddMinutes(Properties.Settings.Default.NumberOfMinutesOfLogToRead * -1))
            {
                //Console.WriteLine(TimeWritten.ToString() + " < " + DateTime.Now.AddMinutes(Properties.Settings.Default.NumberOfMinutesOfLogToRead * -1).ToString());
                return false;
            }
            else
            {
                //Console.WriteLine(TimeWritten.ToString() + " > " + DateTime.Now.AddMinutes(Properties.Settings.Default.NumberOfMinutesOfLogToRead * -1).ToString());
                return true;
            }

        }

        static string FormatMessage (string message)
        {
            return RemoveUnwantedCharactersFromString(message);
        }

        static string RemoveUnwantedCharactersFromString (string text)
        {
            return text.Replace(',', ' ').Replace(Environment.NewLine, " ").Replace("\n", " ");
        }

        static void WriteFile (string textData , string path)
        {
            Console.WriteLine("Attempting to write output file: " + path);
            try
            {
                System.IO.File.WriteAllText(path, textData);
            }
            catch(Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
            }
            Console.WriteLine("Done writing file.");
        }
    }
}
