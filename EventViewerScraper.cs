using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EventViewerScraper
{
    class EventViewerScraper
    {
        static void Main(string[] args)
        {
            DisplayWelcomeMessage();

            try
            {
                WriteFile(GetEventViewerLogs(), Properties.Settings.Default.OutputFilePath);
            }
            catch (Exception e)
            {
                DisplayOutput("FATAL ERROR: " + e.Message);
            }

            if (Properties.Settings.Default.PauseAfterCompletion)
            {
                DisplayOutput("Press any key to terminate execution...");
                Console.ReadKey();
            }
        }

        static void DisplayWelcomeMessage()
        {
            DisplayOutput("EventViewerScraper v1.0");
            DisplayOutput("by Ramon Ecung");
        }

        static string GetEventViewerLogs()
        {
            string eventLogName = Properties.Settings.Default.EventLogName;
            EventLog eventLog = new EventLog();
            eventLog.Log = eventLogName;
            StringBuilder sb = new StringBuilder();

            DisplayOutput("Now processing Event Viewer logs...");

            foreach (EventLogEntry log in eventLog.Entries)
            {
                //DisplayOutput("Now processing: " + log.Message);

                if (log.Source == Properties.Settings.Default.EventViewerSource && FilterMessageByTime(log.TimeWritten))
                {
                    try
                    {
                        //EncodeString(log.Message);
                        DisplayOutput("Now processing line: " + log.Message);
                        string line = log.TimeWritten.ToString("HH:mm") + Properties.Settings.Default.delimiter + log.TimeWritten.ToString("D").Split(',')[0] + Properties.Settings.Default.delimiter + FormatMessage(log.Message) + Properties.Settings.Default.delimiter + isLogValid(log.Message);
                        DisplayOutput(line);
                        sb.AppendLine(line);
                    }
                    catch(Exception e)
                    {
                        DisplayOutput("ERROR: " + e.Message);
                    }
                }
            }

            DisplayOutput("Done processing Event Viewer logs!");

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
                //DisplayOutput(TimeWritten.ToString() + " < " + DateTime.Now.AddMinutes(Properties.Settings.Default.NumberOfMinutesOfLogToRead * -1).ToString());
                return false;
            }
            else
            {
                //DisplayOutput(TimeWritten.ToString() + " > " + DateTime.Now.AddMinutes(Properties.Settings.Default.NumberOfMinutesOfLogToRead * -1).ToString());
                return true;
            }

        }

        static void DisplayOutput(string message)
        {
            Console.WriteLine(message);
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
            DisplayOutput("Attempting to write output file: " + path);
            bool writeStatus;

            try
            {
                System.IO.File.WriteAllText(path, textData);
                writeStatus = true;
            }
            catch(Exception e)
            {
                DisplayOutput("ERROR: " + e.Message);
                writeStatus = false;
            }

            if(writeStatus)
            {
                DisplayOutput("Done writing file.");
            }
            else
            {
                DisplayOutput("Writing the file was unsuccessful!");
            }
            
        }
    }
}
