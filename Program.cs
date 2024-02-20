// See https://aka.ms/new-console-template for more information
using memory;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;




namespace ConsoleApp
{
    class Program
    {

        public static List<MenuOption>? options;
        public static bool showMainMenu = true;

        public static List<Level> questionData = new List<Level>();

        const string UNDERLINE_START = "\x1B[4m";
        const string UNDERLINE_END = "\x1B[0m";


        static void Main(string[] args)
        {
            try
            {
                string basePath = AppContext.BaseDirectory;
                string newPath = Path.GetFullPath(Path.Combine(basePath, @"..\..\..\"));
                string jsonData = File.ReadAllText($"{newPath}/data.json");
                questionData = JsonSerializer.Deserialize<List<Level>>(jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());   
            }

            if (questionData == null )
            {
                Environment.Exit(0);
            }

            //Create options that you want your menu to have
            options = new List<MenuOption>
            {
                new MenuOption("Level 1", () =>  WriteText(0, 0)),
                new MenuOption("Level 2", () =>  WriteText(1, 0)),
                new MenuOption("Level 3", () =>  WriteText(2, 0)),
                new MenuOption("Exit", () => Environment.Exit(0)),
            };

            // Write the menu out
            WriteMenu(options, options[0], 0);
        }



        //// Default action of all the options. You can create more methods

        static void WriteText(int level, int index)
        {
            Console.Clear();
            bool test = true;

            

            Console.WriteLine();
            WordWrapper.Wrap($"{questionData[level].data[index].text}");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("[X] back to menu      [ENTER] go to question");
            ConsoleKeyInfo keyinfo2;
            while (test)
            {
                keyinfo2 = Console.ReadKey();
                
                if (keyinfo2.Key == ConsoleKey.X)
                {
                    test = false;
                    WriteMenu(options, options.First(), 0);
                }

                if (keyinfo2.Key == ConsoleKey.Enter)
                {
                    test = false;
                    WriteQuestion(level, index, 0, -1);
                }
            }
                
        }

        static void WriteScore(int level)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("  _                _                             _      _           _ \r\n | |              | |                           | |    | |         | |\r\n | | _____   _____| |   ___ ___  _ __ ___  _ __ | | ___| |_ ___  __| |\r\n | |/ _ \\ \\ / / _ \\ |  / __/ _ \\| '_ ` _ \\| '_ \\| |/ _ \\ __/ _ \\/ _` |\r\n | |  __/\\ V /  __/ | | (_| (_) | | | | | | |_) | |  __/ ||  __/ (_| |\r\n |_|\\___| \\_/ \\___|_|  \\___\\___/|_| |_| |_| .__/|_|\\___|\\__\\___|\\__,_|\r\n                                          | |                         \r\n                                          |_|                         ");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"Score: {questionData[level].score} / {questionData[level].data.Count}");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

            WriteMenu(options, options[0], 0);
        }


        static void WriteQuestion(int level, int dataIndex, int menuIndex, int questionIndex)
        {
            if (questionIndex == -1)
            {
                Random rnd = new Random();
                questionIndex = rnd.Next(0, questionData[level].data[dataIndex].questions.Count - 1);
            }

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($" {questionData[level].data[dataIndex].questions[questionIndex].question}");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();


            foreach (Answer answer in questionData[level].data[dataIndex].questions[questionIndex].answers)
            {
                if (answer == questionData[level].data[dataIndex].questions[questionIndex].answers[menuIndex])
                {
                    Console.WriteLine($"    >{UNDERLINE_START}{answer.text}{UNDERLINE_END}");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"    { answer.text}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("[UP ARROW / DOWN ARROW] navigate      [ENTER] select");

            // Store key info in here
            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();

                // Handle each key input (down arrow will write the menu again with a different selected item)
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (menuIndex + 1 < questionData[level].data[dataIndex].questions[questionIndex].answers.Count)
                    {
                        menuIndex++;
                        WriteQuestion(level, dataIndex, menuIndex, questionIndex);
                        break;
                    }
                }
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (menuIndex - 1 >= 0)
                    {
                        menuIndex--;
                        WriteQuestion(level, dataIndex, menuIndex, questionIndex);
                        break;
                    }
                }
                // Handle different action for the option
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    if (questionData[level].data[dataIndex].questions[questionIndex].answers[menuIndex].correctAnswer == true)
                    {
                        questionData[level].score++;
                    }
                    if (dataIndex == questionData[level].data.Count -1)
                    {
                        // end of level scorescreen needs to come here with an option to return to main menu
                        WriteScore(level);
                    } else
                    {
                        WriteText(level, (dataIndex + 1));
                    }
                    break;
                }
            } while (true);

        }


        static void WriteMenu(List<MenuOption> options, MenuOption selectedOption, int index)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("                                            \r\n                                            \r\n  _ __ ___   ___ _ __ ___   ___  _ __ _   _ \r\n | '_ ` _ \\ / _ \\ '_ ` _ \\ / _ \\| '__| | | |\r\n | | | | | |  __/ | | | | | (_) | |  | |_| |\r\n |_| |_| |_|\\___|_| |_| |_|\\___/|_|   \\__, |\r\n                                       __/ |\r\n                                      |___/ ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            foreach (MenuOption option in options)
            {
                if (option == selectedOption)
                {
                    Console.WriteLine($"    >{UNDERLINE_START}{option.Name}{UNDERLINE_END}");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"  {option.Name}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("[UP ARROW / DOWN ARROW] navigate      [ENTER] select");

            // Store key info in here
            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();

                // Handle each key input (down arrow will write the menu again with a different selected item)
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 < options.Count)
                    {
                        index++;
                        WriteMenu(options, options[index], index);
                        break;
                    }
                }
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteMenu(options, options[index], index);
                        break;
                    }
                }
                // Handle different action for the option
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    questionData[index].score = 0;
                    options[index].Selected.Invoke();
                    break;
                }
            } while (true);
        }
    }
}

    

