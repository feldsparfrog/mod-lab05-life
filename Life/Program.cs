using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace cli_life
{
    public class Settings
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int CellSize { get; set; }
        public double LiveDensity { get; set; }
        public int MaxSteps { get; set; }
        public int Downtime { get; set; }
        public int StablePeriod { get; set; }
    }
    public class Cell
    {
        public bool IsAlive;
        public readonly List<Cell> neighbors = new List<Cell>();
        private bool IsAliveNext;
        public void DetermineNextLiveState()
        {
            int liveNeighbors = neighbors.Where(x => x.IsAlive).Count();
            if (IsAlive)
                IsAliveNext = liveNeighbors == 2 || liveNeighbors == 3;
            else
                IsAliveNext = liveNeighbors == 3;
        }
        public void Advance()
        {
            IsAlive = IsAliveNext;
        }
    }
    public class Board
    {
        public readonly Cell[,] Cells;
        public readonly int CellSize;
        public int Generation;

        public int Columns { get { return Cells.GetLength(0); } }
        public int Rows { get { return Cells.GetLength(1); } }
        public int Width { get { return Columns * CellSize; } }
        public int Height { get { return Rows * CellSize; } }

        public Board(int width, int height, int cellSize, double liveDensity = .1)
        {
            Generation = 1;
            CellSize = cellSize;
            Cells = new Cell[width / cellSize, height / cellSize];
            for (int x = 0; x < Columns; x++)
                for (int y = 0; y < Rows; y++)
                    Cells[x, y] = new Cell();

            ConnectNeighbors();
            Randomize(liveDensity);
        }
        public Board(int cellSize, char[,] cells, int generation)
        {
            Generation = generation;
            CellSize = cellSize;
            Cells = new Cell[cells.GetLength(0), cells.GetLength(1)];
            for (int x = 0; x < Columns; x++)
                for (int y = 0; y < Rows; y++)
                    Cells[x, y] = new Cell();

            ConnectNeighbors();
            SetStates(cells);
        }
        readonly Random rand = new Random();
        public void Randomize(double liveDensity)
        {
            foreach (var cell in Cells)
                cell.IsAlive = rand.NextDouble() < liveDensity;
        }
        public void SetStates(char[,] cells)
        {
            for (int x = 0; x < Columns; x++)
                for (int y = 0; y < Rows; y++)
                    Cells[x, y].IsAlive = cells[x, y] == '*';
        }
        public int CellsAlive()
        {
            int n = 0;
            foreach (var cell in Cells)
                if (cell.IsAlive) n++;
            return n;
        }
        public void Advance()
        {
            Generation++;
            foreach (var cell in Cells)
                cell.DetermineNextLiveState();
            foreach (var cell in Cells)
                cell.Advance();
        }
        private void ConnectNeighbors()
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    int xL = (x > 0) ? x - 1 : Columns - 1;
                    int xR = (x < Columns - 1) ? x + 1 : 0;

                    int yT = (y > 0) ? y - 1 : Rows - 1;
                    int yB = (y < Rows - 1) ? y + 1 : 0;

                    Cells[x, y].neighbors.Add(Cells[xL, yT]);
                    Cells[x, y].neighbors.Add(Cells[x, yT]);
                    Cells[x, y].neighbors.Add(Cells[xR, yT]);
                    Cells[x, y].neighbors.Add(Cells[xL, y]);
                    Cells[x, y].neighbors.Add(Cells[xR, y]);
                    Cells[x, y].neighbors.Add(Cells[xL, yB]);
                    Cells[x, y].neighbors.Add(Cells[x, yB]);
                    Cells[x, y].neighbors.Add(Cells[xR, yB]);
                }
            }
        }
        public static Board ReadFromFile(string filePath)
        {
            StreamReader streamReader = new(filePath);
            int[] parameters = streamReader.ReadLine().Split().Select(int.Parse).ToArray();
            char[,] cells = new char[parameters[0] / parameters[2], parameters[1] / parameters[2]];
            for (int i = 0; i < cells.GetLength(1); i++)
            {
                string nextLine = streamReader.ReadLine();
                for (int j = 0; j < cells.GetLength(0); j++)
                {
                    cells[j, i] = nextLine[j];
                }
            }
            streamReader.Close();
            Board board = new(parameters[2], cells, parameters[3]);
            return board;
        }
        public void WriteToFile(string filePath)
        {
            StreamWriter streamWriter = new(filePath);
            string parameters = Width.ToString() + " " + Height.ToString() +
                " " + CellSize.ToString() + " " + Generation.ToString();
            streamWriter.WriteLine(parameters);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    var cell = Cells[j, i];
                    if (cell.IsAlive)
                    {
                        streamWriter.Write('*');
                    }
                    else
                    {
                        streamWriter.Write('.');
                    }
                }
                streamWriter.Write('\n');
            }
            streamWriter.Close();
        }
    }
    class Program
    {
        static Board board;
        static int[] recentlyAliveCells;
        static int reachedStability;
        static int maxSteps;
        static int downtime;
        static int stabilityLevel;
        static private void Reset()
        {
            reachedStability = -1;
            string fileName = "settings.json";
            string settings = File.ReadAllText(fileName)!;
            Settings boardParameters = JsonSerializer.Deserialize<Settings>(settings);
            board = new(boardParameters.Width, boardParameters.Height, boardParameters.CellSize, boardParameters.LiveDensity);
            maxSteps = boardParameters.MaxSteps;
            downtime = boardParameters.Downtime;
            stabilityLevel = boardParameters.StablePeriod;
            recentlyAliveCells = new int[stabilityLevel];
            for (int i = 0; i < recentlyAliveCells.Length; i++)
                recentlyAliveCells[i] = 0;
        }
        static void Render()
        {
            Console.WriteLine("Поколение " + board.Generation);
            recentlyAliveCells[board.Generation % stabilityLevel] = board.CellsAlive();
            for (int row = 0; row < board.Rows; row++)
            {
                for (int col = 0; col < board.Columns; col++)
                {
                    var cell = board.Cells[col, row];
                    if (cell.IsAlive)
                    {
                        Console.Write('*');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.Write('\n');
            }
            if (reachedStability < 0)
            {
                bool isStable = true;
                for (int i = 1; i < recentlyAliveCells.Length; i++)
                {
                    if (recentlyAliveCells[i] != recentlyAliveCells[i - 1]) isStable = false;
                }
                if (isStable)
                {
                    reachedStability = board.Generation;
                    string newData = reachedStability.ToString() + '\n';
                    File.AppendAllText("data_raw.txt", newData);
                    int[] ysPre = File.ReadAllLines("data_raw.txt").Select(int.Parse).ToArray();
                    if (ysPre.Length == 90)
                    {
                        Plot(ysPre);
                    }
                }
            }
            else Console.WriteLine("Стабильное состояние достигнуто на поколении " + reachedStability);
        }
        static void Plot(int[] ysPre)
        {
            File.Delete("data_raw.txt");
            StreamWriter streamWriter = new("data.txt");
            double[] xs = { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9 };
            double[] ys = new double[10];
            for (int i = 0; i < xs.Length; i++)
            {
                streamWriter.WriteLine("Плотность: " + xs[i]);
                streamWriter.Write("Число поколений: ");
                ys[i] = 0;
                for (int j = 0; j < ys.Length; j++)
                {
                    int nextResult = ysPre[10 * i + j];
                    streamWriter.Write(nextResult + " ");
                    ys[i] += nextResult;
                }
                ys[i] /= 10;
                streamWriter.WriteLine("\nСреднее число поколений: " + ys[i]);
            }
            streamWriter.Close();
            ScottPlot.Plot graph = new ScottPlot.Plot();
            graph.Add.Scatter(xs, ys);
            graph.Title("Переход в стабильную фазу");
            graph.XLabel("Плотность");
            graph.YLabel("Среднее число поколений");
            graph.SavePng("plot.png", 1024, 1024);
        } 
        static void Main(string[] args)
        {
            Reset();
            Console.WriteLine("Введите название файла, который надо загрузить: ");
            string filePathRead = Console.ReadLine() + ".txt";
            if (File.Exists(filePathRead))
            {
                board = Board.ReadFromFile(filePathRead);
            }
            int stepsToDo = maxSteps;
            while (true)
            {
                Console.Clear();
                Render();
                if (stepsToDo == 0 || reachedStability > 0)
                {
                    Console.WriteLine("Достигнута стабильность или максимальное число поколений.");
                    Console.WriteLine("Введите название файла, в котором надо сохранить:");
                    string filePathWrite = Console.ReadLine() + ".txt";
                    board.WriteToFile(filePathWrite);
                    Console.WriteLine("Сколько ещё поколений пройти? Введите неположительное число для завершения.");
                    stepsToDo = Convert.ToInt32(Console.ReadLine());
                    if (stepsToDo <= 0) return;
                }
                else Thread.Sleep(downtime);
                board.Advance();
                stepsToDo--;
            }
        }
    }
}
