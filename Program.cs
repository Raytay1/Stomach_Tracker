/* Make a program that tracks how many times throughout
 * the year that Zena complained about her stomach hurting
 */

// Core
/* When program starts check current date and then use that to
 * change corresponding value
 */

// First line of file means file is legit

/* Idea 1: Store data in file, 
 * 0 on first line means on 01/01/23 Zena's stomach did not hurt
 * 1 on second line means on 01/02/23 Zena stomach did hurt
 */

/* Idea 2: piggy back off of idea 1 but load data from file
 * into array for easy data manipulation and storing back into 
 * file.
 */

using System.Xml.Linq;
using System.Text;

namespace Hurts
{
    class Tracker
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\Ray\\source\\repos\\Tracker_App\\Tracker_App\\data.txt";
            string path_tmp = "C:\\Users\\Ray\\source\\repos\\Tracker_App\\Tracker_App\\data_tmp.txt";
            string ex_path = "Z:\\data.txt";
            string[] data;


            init_file(path);
            data = load_file(path);

            Console.Clear();
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Cyan;

            DateTime today = DateTime.Today;
            Console.WriteLine("Today's date: {0}", today.ToString("MM/dd"));
            Console.WriteLine("Entry for today:");

            Console.ResetColor();

            (int left, int top) = Console.GetCursorPosition();
            ConsoleKeyInfo key;
            var option = 0;
            var color = "   \u001b[32m";
            bool isSelected = false;
            
            while (!isSelected)
            {                
                Console.SetCursorPosition(left, top);

                Console.WriteLine($"{(option == 0 ? color : "   ")}No\u001b[0m");
                Console.WriteLine($"{(option == 1 ? color : "   ")}Yes\u001b[0m");
                Console.WriteLine($"{(option == 2 ? color : "   ")}Enter Date\u001b[0m");

                key = Console.ReadKey(false);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        option = option == 0 ? 2 : option - 1;
                        break;

                    case ConsoleKey.DownArrow:
                        option = option == 2 ? 0 : option + 1;
                        break;

                    case ConsoleKey.Enter:
                        isSelected = true;
                        break;
                }
            }

            if (option == 2)
            {
                isSelected = false;
                while (!isSelected)
                {
                    Console.Clear();
                    Console.OutputEncoding = Encoding.UTF8;
                    Console.CursorVisible = true;
                    Console.ForegroundColor = ConsoleColor.Cyan;

                    Console.WriteLine("Date Format: MM/DD");
                    Console.Write("Enter date: ");
                    Console.ResetColor();
                    string user_input = Console.ReadLine();

                    if (user_input.Length != 0)
                    {
                        string[] subs = user_input.Split("/");
                        try
                        {
                            DateTime user_date = new DateTime(2023, Int32.Parse(subs[0]), Int32.Parse(subs[1]));
                            Console.WriteLine(user_date.ToString());
                            if (user_date > DateTime.Today)
                            {
                                Console.WriteLine("Past today's date");
                            }
                            else
                            {
                                data[date_num(user_date)] = "1";
                                break;
                            }
                            Thread.Sleep(1000);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("invalid date");
                            Thread.Sleep(1000);
                        }

                    }
                }
            }
            else
            {
                data[date_num()] = option.ToString();
            }
            

            save_file(path_tmp, path, data);
            backup_file(path, ex_path);
            Console.Clear();
            Console.WriteLine("Data Entered");
            Thread.Sleep(2000);
        }

        private static void init_file(string path)
        {
            if (File.Exists(path))
            {
                string s = "";
                using (StreamReader fs = File.OpenText(path))
                {
                    s = fs.ReadLine();
                }
                if (s != "1")
                    build_file(path);
            }
            else
                build_file(path);

        }

        private static void build_file(string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(1);
                for (int i = 0; i < 366; i++)
                    writer.WriteLine("0");
            }
        }

        private static string[] load_file(string path)
        {
            string[] data = File.ReadAllLines(path);

            return data;
        }

        private static void save_file(string tmp_path, string path, string[] data)
        {
            try
            {
                File.WriteAllLines(tmp_path, data);
            }
            catch (IOException e)
            {
                Console.WriteLine($"Could not write to file:");
                Console.WriteLine(e.Message);
            }

            try
            {
                File.Delete(path);
            }
            catch (IOException e)
            {
                Console.WriteLine($"File could not be deleted:");
                Console.WriteLine(e.Message);
            }

            try
            {
                File.Move(tmp_path, path);
            }
            catch (IOException e)
            {
                Console.WriteLine($"Could not rename file:");
                Console.WriteLine(e.Message);
            }
        }

        private static int date_num()
        {
            DateTime today = DateTime.Today;
            DateTime startdate = new DateTime(today.Year - 1, 12, 31);
            int num = (today.Date - startdate.Date).Days;

            return num;
        }

        private static int date_num(DateTime date)
        {
            DateTime startdate = new DateTime(date.Year - 1, 12, 31);
            int num = (date.Date - startdate.Date).Days;

            return num;
        }

        private static void backup_file(string path, string backup)
        {
            try
            {
                // Will not overwrite if the destination file already exists.
                File.Copy(path, backup, true);
            }

            // Catch exception if the file was already copied.
            catch (IOException copyError)
            {
                Console.WriteLine(copyError.Message);
            }
        }
    }
}