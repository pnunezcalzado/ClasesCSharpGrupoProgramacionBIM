using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Navigation
{
    public class NavigationManager
    {
        public NavigationManager()
        {
            commands = new Dictionary<ConsoleKey, Action>
            {
                {ConsoleKey.UpArrow, MoveUp},
                {ConsoleKey.DownArrow, MoveDown},
                {ConsoleKey.RightArrow, MoveRight},
                {ConsoleKey.LeftArrow, MoveLeft},
            };
        }

        private Dictionary<ConsoleKey, Action> commands;

        private List<List<ISelect>> ElementRows = new List<List<ISelect>>();

        private ISelect CurrentElement => ElementRows
            .SelectMany(r => r)
            .Single(e => e.IsSelected);

        private List<ISelect> CurrentRow => ElementRows.Single(r => r.Contains(CurrentElement));

        private int CurrentElementIndex => CurrentRow.IndexOf(CurrentElement);

        private int CurrentRowIndex => ElementRows.IndexOf(CurrentRow);

        private double CurrentElementPosition
        {
            get
            {
                if (CurrentRow.Count == 1)
                {
                    return 0;
                }
                else
                {
                    return (double) CurrentElementIndex / (CurrentRow.Count - 1);
                }
            }
        }

        public void AddRow<T>(int numberOfElements)
            where T : ISelect, new()
        {
            ElementRows.Add(
                Enumerable.Repeat(0, numberOfElements)
                    .Select(n => new T() as ISelect)
                    .ToList()
            );
            
            ElementRows[0][0].IsSelected = true;
        }
        
        public void Start()
        {
            Console.Clear();
            Console.WriteLine("Usa las flechas para moverte y q para salir");
            Console.WriteLine(this);
        }
        
        public void Stop()
        {
            Console.Clear();
            Console.WriteLine("Terminado");
        }
        
        public void ExecuteCommand(ConsoleKey key)
        {
            Console.Clear();

            Action command;
            var success = commands.TryGetValue(key, out command);

            if (command != null)
            {
                command.Invoke();
            }
            else
            {
                Console.WriteLine(" --- ");
            }
            Console.WriteLine(this);
        }
        
        private void MoveUp()
        {
            Console.WriteLine("Arriba");

            var newRowIndex = Math.Max(CurrentRowIndex - 1, 0);
            var newElementIndex = (int) (CurrentElementPosition * (ElementRows[newRowIndex].Count - 1));

            CurrentElement.IsSelected = false;
            ElementRows[newRowIndex][newElementIndex].IsSelected = true;
        }
        
        private void MoveDown()
        {
            Console.WriteLine("Abajo");

            var newRowIndex = Math.Min(CurrentRowIndex + 1, ElementRows.Count - 1);
            var newElementIndex = (int) (CurrentElementPosition * (ElementRows[newRowIndex].Count - 1));

            CurrentElement.IsSelected = false;
            ElementRows[newRowIndex][newElementIndex].IsSelected = true;
        }
        
        private void MoveLeft()
        {
            Console.WriteLine("Izquierda");

            var currentRowIndex = CurrentRowIndex;
            var newElementIndex = Math.Max(CurrentElementIndex - 1, 0);

            CurrentElement.IsSelected = false;
            ElementRows[currentRowIndex][newElementIndex].IsSelected = true;
        }

        private void MoveRight()
        {
            Console.WriteLine("Derecha");

            var currentRowIndex = CurrentRowIndex;
            var newElementIndex = Math.Min(CurrentElementIndex + 1, CurrentRow.Count - 1);

            CurrentElement.IsSelected = false;
            ElementRows[currentRowIndex][newElementIndex].IsSelected = true;
        }

        public override string ToString()
        {
            return string.Join("\n", ElementRows.Select(e => string.Join("", e)));
        }
    }
}