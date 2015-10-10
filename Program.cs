using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace javac
{
    class Program
    {
        static void Main(string[] args)
        {
            String recurse = args[0];
            var temp = recurse.Split('\\');
            String outdir = "";
            for (int i = 0; i < temp.Length - 1; i++)
            {
                outdir += temp[i] + "\\";
            }
            Process p = new Process();
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.ErrorDialog = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = @"C:\Program Files\Java\jdk1.8.0_60\bin\javac.exe";
            p.StartInfo.Arguments = recurse;
            p.Start();
            String className = recurse.Split('\\').Last().Split('.').First();
            if (!p.WaitForExit(Int32.MaxValue))
            {
                p.Kill();
            }
            if (File.Exists(outdir + className + ".class"))
            {
                File.WriteAllText(outdir + className + ".mf", "Main-Class: " + className + "\r\n");
                p.StartInfo.FileName = @"C:\Program Files\Java\jdk1.8.0_60\bin\jar.exe";
                p.StartInfo.Arguments = @"cvfm " + outdir + className + ".jar" + " " + outdir + className + ".mf" + " -C " + outdir + " " + className + ".class";
                p.Start();
                if (!p.WaitForExit(Int32.MaxValue))
                {
                    p.Kill();
                }
                if (File.Exists(outdir + className + ".jar"))
                {
                    Console.WriteLine("success.");
                }
                else
                {
                    Console.WriteLine(outdir + className + ".jar");
                    Console.WriteLine("fail.");
                }
            }
            else
            {
                Console.WriteLine("fail.");
            }
        }
    }
}
