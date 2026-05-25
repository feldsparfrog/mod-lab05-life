using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using cli_life;
namespace CTEST
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Cell cell = new Cell { IsAlive = false };
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.DetermineNextLiveState();
            cell.Advance();
            Assert.IsTrue(cell.IsAlive);
        }
        [TestMethod]
        public void TestMethod2()
        {
            Cell cell = new Cell { IsAlive = true };
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.DetermineNextLiveState();
            cell.Advance();
            Assert.IsTrue(cell.IsAlive);
        }
        [TestMethod]
        public void TestMethod3()
        {
            Cell cell = new Cell { IsAlive = true };
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.DetermineNextLiveState();
            cell.Advance();
            Assert.IsTrue(cell.IsAlive);
        }
        [TestMethod]
        public void TestMethod4()
        {
            Cell cell = new Cell { IsAlive = false };
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.DetermineNextLiveState();
            cell.Advance();
            Assert.IsTrue(!cell.IsAlive);
        }
        [TestMethod]
        public void TestMethod5()
        {
            Cell cell = new Cell { IsAlive = true };
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.DetermineNextLiveState();
            cell.Advance();
            Assert.IsTrue(!cell.IsAlive);
        }
        [TestMethod]
        public void TestMethod6()
        {
            Cell cell = new Cell { IsAlive = true };
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.DetermineNextLiveState();
            cell.Advance();
            Assert.IsTrue(!cell.IsAlive);
        }
        [TestMethod]
        public void TestMethod7()
        {
            Board board = new Board(25, 20, 1, 0);
            Assert.IsTrue(board.Columns == 25 && board.Rows == 20);
        }
        [TestMethod]
        public void TestMethod8()
        {
            Board board = new Board(10, 10, 1, 0);
            for (int i = 0; i < 10; i++)
            {
                board.Cells[i, i].IsAlive = true;
            }
            Assert.IsTrue(board.CellsAlive() == 10);
        }
        [TestMethod]
        public void TestMethod9()
        {
            Board board = new Board(10, 10, 1, 0);
            board.Randomize(1);
            Assert.IsTrue(board.CellsAlive() > 0);
        }
        [TestMethod]
        public void TestMethod10()
        {
            Board board = new Board(6, 6, 1, 0);
            board.Cells[2, 2].IsAlive = true;
            board.Cells[2, 3].IsAlive = true;
            board.Cells[3, 2].IsAlive = true;
            board.Cells[3, 3].IsAlive = true;
            board.Advance();
            Assert.IsTrue(board.CellsAlive() == 4 &&
                board.Cells[2, 2].IsAlive && board.Cells[2, 3].IsAlive &&
                board.Cells[3, 2].IsAlive && board.Cells[3, 3].IsAlive);
        }
        [TestMethod]
        public void TestMethod11()
        {
            Board board = new Board(7, 7, 1, 0);
            board.Cells[3, 2].IsAlive = true;
            board.Cells[3, 3].IsAlive = true;
            board.Cells[3, 4].IsAlive = true;
            board.Advance();
            Assert.IsTrue(board.CellsAlive() == 3 &&
                board.Cells[2, 3].IsAlive && board.Cells[3, 3].IsAlive &&
                board.Cells[4, 3].IsAlive);
        }
        [TestMethod]
        public void TestMethod12()
        {
            Board board = new Board(7, 7, 1, 0);
            board.Cells[3, 0].IsAlive = true;
            board.Cells[2, 1].IsAlive = true;
            board.Cells[2, 2].IsAlive = true;
            board.Cells[3, 2].IsAlive = true;
            board.Cells[4, 2].IsAlive = true;
            board.Advance();
            board.Advance();
            board.Advance();
            board.Advance();
            Assert.IsTrue(board.CellsAlive() == 5 && board.Generation == 5);
        }
        [TestMethod]
        public void TestMethod13()
        {
            Board board = new Board(10, 10, 1, 0);
            board.WriteToFile("test.txt");
            Assert.IsTrue(File.Exists("test.txt"));
            File.Delete("test.txt");
        }
        [TestMethod]
        public void TestMethod14()
        {
            Board board = new Board(7, 7, 1, 0);
            board.Cells[3, 2].IsAlive = true;
            board.Cells[3, 3].IsAlive = true;
            board.Cells[3, 4].IsAlive = true;
            board.Advance();
            board.Advance();
            board.Advance();
            board.WriteToFile("testLoad.txt");
            Board loaded = Board.ReadFromFile("testLoad.txt");
            Assert.IsTrue(board.CellsAlive() == 3 &&
                board.Cells[2, 3].IsAlive && board.Cells[3, 3].IsAlive &&
                board.Cells[4, 3].IsAlive && board.Generation == 4);
            File.Delete("testLoad.txt");
        }
        [TestMethod]
        public void TestMethod15()
        {
            Board board = new Board(7, 7, 1, 0);
            board.Cells[2, 2].IsAlive = true;
            board.Cells[2, 3].IsAlive = true;
            board.Cells[2, 4].IsAlive = true;
            board.Cells[3, 2].IsAlive = true;
            board.Cells[3, 3].IsAlive = true;
            board.Cells[3, 4].IsAlive = true;
            board.Cells[4, 2].IsAlive = true;
            board.Cells[4, 3].IsAlive = true;
            board.Cells[4, 4].IsAlive = true;
            board.Advance();
            board.Advance();
            Assert.IsTrue(board.CellsAlive() == 12 && board.Generation == 3);
        }
    }
}
