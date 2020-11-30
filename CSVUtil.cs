using System.IO;
using LumenWorks.Framework.IO.Csv;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Castle.Core.Logging;

namespace FTPImportWatch
{
    class CSVUtil
    {
        readonly string CSVDirectoryRoot = "C:\\new\\sftp";
        ConsoleLogger _logheader = new ConsoleLogger();
        
        public bool IterateWindowsFolders()
        {
            _logheader.Info("start " + CSVDirectoryRoot);

            string[] dirs = Directory.GetDirectories(CSVDirectoryRoot, "*", SearchOption.AllDirectories);
            foreach (string dir in dirs)
            {
                var csvFiles = Directory.GetFiles(dir, "*.csv");
                string logMessage;
                foreach (var file in csvFiles)
                {
                    logMessage = file;
                    
                    try {
                        using (CsvReader csv =
                        new CsvReader(new StreamReader(file), true))
                        {
                            string[] headers = csv.GetFieldHeaders();
                            while (csv.ReadNextRecord())
                            {
                                int fieldCount = csv.FieldCount;
                                if (fieldCount != headers.Length)
                                {
                                    throw new System.ArgumentException("CSV header and content length do not always match " + fieldCount + " <-> " + headers.Length + " in " + file);
                                }
                            }
                        }

                        logMessage += " OK";

                        /*
                        https://docs.microsoft.com/en-us/azure/cosmos-db/bulk-executor-dot-net

                        how to add attributes to uploaded docs?
                        */
                    }
                    catch {
                        logMessage += " NO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!";
                    }

                    _logheader.Info(logMessage);
                }
            }

            return true;
        }
    }
}
