using System;

namespace Navigation
{
    class NavigationMain
    {
        public static void Execute()
        {
            var navigationManager = new NavigationManager();

            var quantities = new int[] {5, 5, 5, 5, 5, 1, 2, 3, 4, 3, 2, 1};

            for (int i = 0; i < quantities.Length; i++)
            {
                if (i % 2 == 0)
                {
                    navigationManager.AddRow<Element1>(quantities[i]);
                }
                else
                {
                    navigationManager.AddRow<Element2>(quantities[i]);
                }
            }

            navigationManager.Start();
            
            var repeat = true;
            while (repeat)
            {
                var keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.Q)
                {
                    repeat = false;
                    navigationManager.Stop();
                }
                else
                {
                    navigationManager.ExecuteCommand(keyInfo.Key);
                }
            }
        }
    }
}