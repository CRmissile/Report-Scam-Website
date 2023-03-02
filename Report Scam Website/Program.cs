using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ConsoleApp
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //Ask user for URL and description
            Console.Write("Please enter a URL string: ");
            string urlInput = Console.ReadLine();
            Console.Write("Please enter a description string: ");
            string descriptionInput = Console.ReadLine();

            //Read websites from .json
            string json = File.ReadAllText("websites.json");
            dynamic websitesObject = JsonConvert.DeserializeObject(json);
            string[] websites = websitesObject.Websites.ToObject<string[]>();

            //Loop through websites
            int i = 0;
            while (i < websites.Length)
            {
                //Check if website is already open
                if (Process.GetProcessesByName("librewolf").Length == 0)
                {
                    //Open website
                    System.Diagnostics.Process.Start(@"C:\Program Files\LibreWolf\librewolf.exe", websites[i]);

                    //Copy URL to Clipboard
                    Clipboard.SetText(urlInput);
                    WriteLineColor($"\n\nopened <*blue*>{websites[i]}<*/*>; copied the URL <*blue*>{urlInput}<*/*> to Clipboard. Press any key to continue.");

                    Console.ReadKey();

                    //Copy description to Clipboard
                    Clipboard.SetText(descriptionInput);

                    WriteLineColor($"Copied <*blue*>{descriptionInput}<*/*> to Clipboard. Close Browser when finished to continue.");


                    i++;
                }
            }
            Console.WriteLine("\nAll sites finished!");

            Console.ReadKey();
        }

        static void WriteColor(string str)
        {
            var fgStack = new Stack<ConsoleColor>();
            var bgStack = new Stack<ConsoleColor>();
            var parts = str.Split(new[] { "<*" }, StringSplitOptions.None);
            foreach (var part in parts)
            {
                var tokens = part.Split(new[] { "*>" }, StringSplitOptions.None);
                if (tokens.Length == 1)
                {
                    Console.Write(tokens[0]);
                }
                else
                {
                    if (!String.IsNullOrEmpty(tokens[0]))
                    {
                        ConsoleColor color;
                        if (tokens[0][0] == '!')
                        {
                            if (Enum.TryParse(tokens[0].Substring(1), true, out color))
                            {
                                bgStack.Push(Console.BackgroundColor);
                                Console.BackgroundColor = color;
                            }
                        }
                        else if (tokens[0][0] == '/')
                        {
                            if (tokens[0].Length > 1 && tokens[0][1] == '!')
                            {
                                if (bgStack.Count > 0)
                                    Console.BackgroundColor = bgStack.Pop();
                            }
                            else
                            {
                                if (fgStack.Count > 0)
                                    Console.ForegroundColor = fgStack.Pop();
                            }
                        }
                        else
                        {
                            if (Enum.TryParse(tokens[0], true, out color))
                            {
                                fgStack.Push(Console.ForegroundColor);
                                Console.ForegroundColor = color;
                            }
                        }
                    }
                    for (int j = 1; j < tokens.Length; j++)
                        Console.Write(tokens[j]);
                }
            }
        }

        static void WriteLineColor(string str)
        {
            WriteColor(str);
            Console.WriteLine();
        }

        static void WriteColor(string str, params object[] arg)
        {
            WriteColor(String.Format(str, arg));
        }

        static void WriteLineColor(string str, params object[] arg)
        {
            WriteColor(String.Format(str, arg));
            Console.WriteLine();
        }
    }
}




//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using Newtonsoft.Json;

//namespace ConsoleApp
//{
//    class Program
//    {
//        [STAThread]
//        static void Main(string[] args)
//        {
//            //Ask user for URL and description
//            Console.Write("Please enter a URL string: ");
//            string urlInput = Console.ReadLine();
//            Console.Write("Please enter a description string: ");
//            string descriptionInput = Console.ReadLine();

//            //Read websites from .json
//            string json = File.ReadAllText("websites.json");
//            dynamic websitesObject = JsonConvert.DeserializeObject(json);
//            string[] websites = websitesObject.Websites.ToObject<string[]>();

//            //Loop through websites
//            int i = 0;
//            while (i < websites.Length)
//            {
//                //Check if website is already open
//                if (Process.GetProcessesByName("librewolf").Length == 0)
//                {
//                    //Open website
//                    System.Diagnostics.Process.Start(@"C:\Program Files\LibreWolf\librewolf.exe", websites[i]);

//                    //Copy URL to Clipboard
//                    Clipboard.SetText(urlInput);
//                    WriteLineColor($"\n\nopened <*blue*>{websites[i]}<*/*>; copied the URL <*blue*>{urlInput}<*/*> to Clipboard. Press any key to continue.");

//                    Console.ReadKey();

//                    //Copy description to Clipboard
//                    Clipboard.SetText(descriptionInput);

//                    WriteLineColor($"Copied <*blue*>{descriptionInput}<*/*> to Clipboard. Close Browser when finished to continue.");


//                    i++;
//                }
//            }

//            Console.WriteLine("\nAll sites finished!");

//            Console.ReadKey();
//        }

//        static void WriteColor(string str)
//        {
//            var fgStack = new Stack<ConsoleColor>();
//            var bgStack = new Stack<ConsoleColor>();
//            var parts = str.Split(new[] { "<*" }, StringSplitOptions.None);
//            foreach (var part in parts)
//            {
//                var tokens = part.Split(new[] { "*>" }, StringSplitOptions.None);
//                if (tokens.Length == 1)
//                {
//                    Console.Write(tokens[0]);
//                }
//                else
//                {
//                    if (!String.IsNullOrEmpty(tokens[0]))
//                    {
//                        ConsoleColor color;
//                        if (tokens[0][0] == '!')
//                        {
//                            if (Enum.TryParse(tokens[0].Substring(1), true, out color))
//                            {
//                                bgStack.Push(Console.BackgroundColor);
//                                Console.BackgroundColor = color;
//                            }
//                        }
//                        else if (tokens[0][0] == '/')
//                        {
//                            if (tokens[0].Length > 1 && tokens[0][1] == '!')
//                            {
//                                if (bgStack.Count > 0)
//                                    Console.BackgroundColor = bgStack.Pop();
//                            }
//                            else
//                            {
//                                if (fgStack.Count > 0)
//                                    Console.ForegroundColor = fgStack.Pop();
//                            }
//                        }
//                        else
//                        {
//                            if (Enum.TryParse(tokens[0], true, out color))
//                            {
//                                fgStack.Push(Console.ForegroundColor);
//                                Console.ForegroundColor = color;
//                            }
//                        }
//                    }
//                    for (int j = 1; j < tokens.Length; j++)
//                        Console.Write(tokens[j]);
//                }
//            }
//        }

//        static void WriteLineColor(string str)
//        {
//            WriteColor(str);
//            Console.WriteLine();
//        }

//        static void WriteColor(string str, params object[] arg)
//        {
//            WriteColor(String.Format(str, arg));
//        }

//        static void WriteLineColor(string str, params object[] arg)
//        {
//            WriteColor(String.Format(str, arg));
//            Console.WriteLine();
//        }
//    }
//}