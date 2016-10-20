using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class Logger
{
    private static System.IO.StreamWriter file;

    public static void Log(string message)
    {
        try
        {
            if (file == null)
            {
                if (System.IO.Directory.Exists("Logs"))
                    System.IO.Directory.CreateDirectory("Logs");
                file = new System.IO.StreamWriter(String.Format(@"Logs\{0:d.M.yyyy HH.mm.ss}.log", DateTime.Now), true);
                file.NewLine = Environment.NewLine;
                file.AutoFlush = true;
            }

            file.WriteLine(String.Format("{0:HH:mm:ss:fff}: ", DateTime.Now) + message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static void Close()
    {
        file?.Close();
    }
}
