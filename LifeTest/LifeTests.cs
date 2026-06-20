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
        public void TestResurrect()
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
        public void TestStayAlive2()
        {
            Cell cell = new Cell { IsAlive = true };
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.DetermineNextLiveState();
            cell.Advance();
            Assert.IsTrue(cell.IsAlive);
        }
        [TestMethod]
        public void TestStayAlive3()
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
        public void TestStayDead()
        {
            Cell cell = new Cell { IsAlive = false };
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.DetermineNextLiveState();
            cell.Advance();
            Assert.IsFalse(cell.IsAlive);
        }
        [TestMethod]
        public void TestLoneliness()
        {
            Cell cell = new Cell { IsAlive = true };
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.DetermineNextLiveState();
            cell.Advance();
            Assert.IsFalse(cell.IsAlive);
        }
        [TestMethod]
        public void TestOverpopulation()
        {
            Cell cell = new Cell { IsAlive = true };
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.DetermineNextLiveState();
            cell.Advance();
            Assert.IsFalse(cell.IsAlive);
        }
        [TestMethod]
        public void TestCreateBoard()
        {
            Board board = new Board(25, 20, 1, 0);
            Assert.IsTrue(board.Columns == 25 && board.Rows == 20);
        }
        [TestMethod]
        public void TestPopulate()
        {
            Board board = new Board(10, 10, 1, 0);
            for (int i = 0; i < 10; i++)
            {
                board.Cells[i, i].IsAlive = true;
            }
            Assert.IsTrue(board.CellsAlive() == 10);
        }
        [TestMethod]
        public void TestAliveCellsExist()
        {
            Board board = new Board(10, 10, 1, 0);
            board.Randomize(1);
            Assert.IsTrue(board.CellsAlive() > 0);
        }
        [TestMethod]
        public void TestSave()
        {
            Board board = new Board(10, 10, 1, 0);
            board.WriteToFile("test.txt");
            Assert.IsTrue(File.Exists("test.txt"));
            File.Delete("test.txt");
        }
        [TestMethod]
        public void TestSaveAndLoad()
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
        public void TestBlock()
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
        public void TestHive()
        {
            Board board = new Board(6, 6, 1, 0);
            board.Cells[3, 1].IsAlive = true;
            board.Cells[2, 2].IsAlive = true;
            board.Cells[2, 3].IsAlive = true;
            board.Cells[4, 2].IsAlive = true;
            board.Cells[4, 3].IsAlive = true;
            board.Cells[3, 4].IsAlive = true;
            board.Advance();
            Assert.IsTrue(board.CellsAlive() == 6 &&
                board.Cells[3, 1].IsAlive && board.Cells[2, 2].IsAlive &&
                board.Cells[2, 3].IsAlive && board.Cells[4, 2].IsAlive &&
                board.Cells[4, 3].IsAlive && board.Cells[3, 4].IsAlive);
        }
        [TestMethod]
        public void TestBlinker()
        {
            Board board = new Board(7, 7, 1, 0);
            board.Cells[3, 2].IsAlive = true;
            board.Cells[3, 3].IsAlive = true;
            board.Cells[3, 4].IsAlive = true;
            board.Advance();
            Assert.IsTrue(board.CellsAlive() == 3 &&
                board.Cells[2, 3].IsAlive && board.Cells[3, 3].IsAlive &&
                board.Cells[4, 3].IsAlive);
            board.Advance();
            Assert.IsTrue(board.CellsAlive() == 3 &&
                board.Cells[3, 2].IsAlive && board.Cells[3, 3].IsAlive &&
                board.Cells[3, 4].IsAlive);
        }
        [TestMethod]
        public void TestGlider()
        {
            Board board = new Board(7, 7, 1, 0);
            board.Cells[1, 2].IsAlive = true;
            board.Cells[2, 2].IsAlive = true;
            board.Cells[3, 2].IsAlive = true;
            board.Cells[3, 3].IsAlive = true;
            board.Cells[2, 4].IsAlive = true;
            board.Advance();
            board.Advance();
            board.Advance();
            board.Advance();
            Assert.IsTrue(board.CellsAlive() == 5 && board.Generation == 5 && 
                board.Cells[2, 1].IsAlive && board.Cells[3, 1].IsAlive &&
                board.Cells[4, 1].IsAlive && board.Cells[4, 2].IsAlive && 
                board.Cells[3, 3].IsAlive);
        }
        [TestMethod]
        public void Test8Structure()
        {
            Board board = new Board(12, 12, 1, 0);
            board.Cells[3, 3].IsAlive = true;
            board.Cells[4, 3].IsAlive = true;
            board.Cells[5, 3].IsAlive = true;
            board.Cells[3, 4].IsAlive = true;
            board.Cells[4, 4].IsAlive = true;
            board.Cells[5, 4].IsAlive = true;
            board.Cells[3, 5].IsAlive = true;
            board.Cells[4, 5].IsAlive = true;
            board.Cells[5, 5].IsAlive = true;
            board.Cells[6, 6].IsAlive = true;
            board.Cells[7, 6].IsAlive = true;
            board.Cells[8, 6].IsAlive = true;
            board.Cells[6, 7].IsAlive = true;
            board.Cells[7, 7].IsAlive = true;
            board.Cells[8, 7].IsAlive = true;
            board.Cells[6, 8].IsAlive = true;
            board.Cells[7, 8].IsAlive = true;
            board.Cells[8, 8].IsAlive = true;
            for (int i = 0; i < 8; i++)
                board.Advance();
            Assert.IsTrue(board.CellsAlive() == 18 && board.Generation == 9 &&
                board.Cells[3, 3].IsAlive && board.Cells[4, 3].IsAlive &&
                board.Cells[5, 3].IsAlive && board.Cells[3, 4].IsAlive &&
                board.Cells[4, 4].IsAlive && board.Cells[5, 4].IsAlive &&
                board.Cells[3, 5].IsAlive && board.Cells[4, 5].IsAlive &&
                board.Cells[5, 5].IsAlive && board.Cells[6, 6].IsAlive &&
                board.Cells[7, 6].IsAlive && board.Cells[8, 6].IsAlive &&
                board.Cells[6, 7].IsAlive && board.Cells[7, 7].IsAlive &&
                board.Cells[8, 7].IsAlive && board.Cells[6, 8].IsAlive &&
                board.Cells[7, 8].IsAlive && board.Cells[8, 8].IsAlive);
        }
        [TestMethod]
        public void TestTStructure()
        {
            Board board = new Board(11, 11, 1, 0);
            board.Cells[4, 5].IsAlive = true;
            board.Cells[5, 5].IsAlive = true;
            board.Cells[6, 5].IsAlive = true;
            board.Cells[5, 6].IsAlive = true;
            for (int i = 0; i < 9; i++)
                board.Advance();
            Assert.IsTrue(board.CellsAlive() == 12 && board.Generation == 10 &&
                board.Cells[4, 2].IsAlive && board.Cells[5, 2].IsAlive &&
                board.Cells[6, 2].IsAlive && board.Cells[4, 8].IsAlive &&
                board.Cells[5, 8].IsAlive && board.Cells[6, 8].IsAlive &&
                board.Cells[2, 4].IsAlive && board.Cells[2, 5].IsAlive &&
                board.Cells[2, 6].IsAlive && board.Cells[8, 4].IsAlive &&
                board.Cells[8, 5].IsAlive && board.Cells[8, 6].IsAlive);
            board.Advance();
            Assert.IsTrue(board.CellsAlive() == 12 && board.Generation == 11 &&
                board.Cells[5, 1].IsAlive && board.Cells[5, 2].IsAlive &&
                board.Cells[5, 3].IsAlive && board.Cells[5, 7].IsAlive &&
                board.Cells[5, 8].IsAlive && board.Cells[5, 9].IsAlive &&
                board.Cells[1, 5].IsAlive && board.Cells[2, 5].IsAlive &&
                board.Cells[3, 5].IsAlive && board.Cells[7, 5].IsAlive &&
                board.Cells[8, 5].IsAlive && board.Cells[9, 5].IsAlive);
        }
        [TestMethod]
        public void TestDiagonalDies()
        {
            Board board = new Board(12, 12, 1, 0);
            board.Cells[1, 1].IsAlive = true;
            board.Cells[2, 2].IsAlive = true;
            board.Cells[3, 3].IsAlive = true;
            board.Cells[4, 4].IsAlive = true;
            board.Cells[5, 5].IsAlive = true;
            board.Cells[6, 6].IsAlive = true;
            board.Cells[7, 7].IsAlive = true;
            board.Cells[8, 8].IsAlive = true;
            board.Cells[9, 9].IsAlive = true;
            board.Cells[10, 10].IsAlive = true;
            for (int i = 0; i < 5; i++)
            {
                board.Advance();
                Assert.IsTrue(board.CellsAlive() == 8 - i * 2 && board.Generation == i + 2);
            }
        }
        [TestMethod]
        public void Test3x3Square()
        {
            Board board = new Board(11, 11, 1, 0);
            board.Cells[4, 4].IsAlive = true;
            board.Cells[4, 5].IsAlive = true;
            board.Cells[4, 6].IsAlive = true;
            board.Cells[5, 4].IsAlive = true;
            board.Cells[5, 5].IsAlive = true;
            board.Cells[5, 6].IsAlive = true;
            board.Cells[6, 4].IsAlive = true;
            board.Cells[6, 5].IsAlive = true;
            board.Cells[6, 6].IsAlive = true;
            for (int i = 0; i < 5; i++)
                board.Advance();
            Assert.IsTrue(board.CellsAlive() == 12 && board.Generation == 6 &&
                board.Cells[4, 2].IsAlive && board.Cells[5, 2].IsAlive &&
                board.Cells[6, 2].IsAlive && board.Cells[4, 8].IsAlive &&
                board.Cells[5, 8].IsAlive && board.Cells[6, 8].IsAlive &&
                board.Cells[2, 4].IsAlive && board.Cells[2, 5].IsAlive &&
                board.Cells[2, 6].IsAlive && board.Cells[8, 4].IsAlive &&
                board.Cells[8, 5].IsAlive && board.Cells[8, 6].IsAlive);
            board.Advance();
            Assert.IsTrue(board.CellsAlive() == 12 && board.Generation == 7 &&
                board.Cells[5, 1].IsAlive && board.Cells[5, 2].IsAlive &&
                board.Cells[5, 3].IsAlive && board.Cells[5, 7].IsAlive &&
                board.Cells[5, 8].IsAlive && board.Cells[5, 9].IsAlive &&
                board.Cells[1, 5].IsAlive && board.Cells[2, 5].IsAlive &&
                board.Cells[3, 5].IsAlive && board.Cells[7, 5].IsAlive &&
                board.Cells[8, 5].IsAlive && board.Cells[9, 5].IsAlive);
        }
        [TestMethod]
        public void TestApiaryLine()
        {
            Board board = new Board(19, 19, 1, 0);
            board.Cells[6, 9].IsAlive = true;
            board.Cells[7, 9].IsAlive = true;
            board.Cells[8, 9].IsAlive = true;
            board.Cells[9, 9].IsAlive = true;
            board.Cells[10, 9].IsAlive = true;
            board.Cells[11, 9].IsAlive = true;
            board.Cells[12, 9].IsAlive = true;
            for (int i = 0; i < 14; i++)
                board.Advance();
            Assert.IsTrue(board.CellsAlive() == 24 && board.Generation == 15 &&
                board.Cells[3, 9].IsAlive && board.Cells[4, 8].IsAlive &&
                board.Cells[5, 8].IsAlive && board.Cells[4, 10].IsAlive &&
                board.Cells[5, 10].IsAlive && board.Cells[6, 9].IsAlive &&
                board.Cells[12, 9].IsAlive && board.Cells[13, 8].IsAlive &&
                board.Cells[14, 8].IsAlive && board.Cells[15, 9].IsAlive &&
                board.Cells[13, 10].IsAlive && board.Cells[14, 10].IsAlive &&
                board.Cells[9, 3].IsAlive && board.Cells[8, 4].IsAlive &&
                board.Cells[8, 5].IsAlive && board.Cells[10, 4].IsAlive &&
                board.Cells[10, 5].IsAlive && board.Cells[9, 6].IsAlive &&
                board.Cells[9, 12].IsAlive && board.Cells[8, 13].IsAlive &&
                board.Cells[8, 14].IsAlive && board.Cells[9, 15].IsAlive &&
                board.Cells[10, 13].IsAlive && board.Cells[10, 14].IsAlive);
            board.Advance();
            Assert.IsTrue(board.CellsAlive() == 24 && board.Generation == 16 &&
                board.Cells[3, 9].IsAlive && board.Cells[4, 8].IsAlive &&
                board.Cells[5, 8].IsAlive && board.Cells[4, 10].IsAlive &&
                board.Cells[5, 10].IsAlive && board.Cells[6, 9].IsAlive &&
                board.Cells[12, 9].IsAlive && board.Cells[13, 8].IsAlive &&
                board.Cells[14, 8].IsAlive && board.Cells[15, 9].IsAlive &&
                board.Cells[13, 10].IsAlive && board.Cells[14, 10].IsAlive &&
                board.Cells[9, 3].IsAlive && board.Cells[8, 4].IsAlive &&
                board.Cells[8, 5].IsAlive && board.Cells[10, 4].IsAlive &&
                board.Cells[10, 5].IsAlive && board.Cells[9, 6].IsAlive &&
                board.Cells[9, 12].IsAlive && board.Cells[8, 13].IsAlive &&
                board.Cells[8, 14].IsAlive && board.Cells[9, 15].IsAlive &&
                board.Cells[10, 13].IsAlive && board.Cells[10, 14].IsAlive);
        }
        [TestMethod]
        public void TestApiary5x5Square()
        {
            Board board = new Board(19, 19, 1, 0);
            board.Cells[7, 7].IsAlive = true;
            board.Cells[8, 7].IsAlive = true;
            board.Cells[9, 7].IsAlive = true;
            board.Cells[10, 7].IsAlive = true;
            board.Cells[11, 7].IsAlive = true;
            board.Cells[7, 8].IsAlive = true;
            board.Cells[8, 8].IsAlive = true;
            board.Cells[9, 8].IsAlive = true;
            board.Cells[10, 8].IsAlive = true;
            board.Cells[11, 8].IsAlive = true;
            board.Cells[7, 9].IsAlive = true;
            board.Cells[8, 9].IsAlive = true;
            board.Cells[9, 9].IsAlive = true;
            board.Cells[10, 9].IsAlive = true;
            board.Cells[11, 9].IsAlive = true;
            board.Cells[7, 10].IsAlive = true;
            board.Cells[8, 10].IsAlive = true;
            board.Cells[9, 10].IsAlive = true;
            board.Cells[10, 10].IsAlive = true;
            board.Cells[11, 10].IsAlive = true;
            board.Cells[7, 11].IsAlive = true;
            board.Cells[8, 11].IsAlive = true;
            board.Cells[9, 11].IsAlive = true;
            board.Cells[10, 11].IsAlive = true;
            board.Cells[11, 11].IsAlive = true;
            for (int i = 0; i < 11; i++)
                board.Advance();
            Assert.IsTrue(board.CellsAlive() == 24 && board.Generation == 12 &&
                board.Cells[3, 9].IsAlive && board.Cells[4, 8].IsAlive &&
                board.Cells[5, 8].IsAlive && board.Cells[4, 10].IsAlive &&
                board.Cells[5, 10].IsAlive && board.Cells[6, 9].IsAlive &&
                board.Cells[12, 9].IsAlive && board.Cells[13, 8].IsAlive &&
                board.Cells[14, 8].IsAlive && board.Cells[15, 9].IsAlive &&
                board.Cells[13, 10].IsAlive && board.Cells[14, 10].IsAlive &&
                board.Cells[9, 3].IsAlive && board.Cells[8, 4].IsAlive &&
                board.Cells[8, 5].IsAlive && board.Cells[10, 4].IsAlive &&
                board.Cells[10, 5].IsAlive && board.Cells[9, 6].IsAlive &&
                board.Cells[9, 12].IsAlive && board.Cells[8, 13].IsAlive &&
                board.Cells[8, 14].IsAlive && board.Cells[9, 15].IsAlive &&
                board.Cells[10, 13].IsAlive && board.Cells[10, 14].IsAlive);
            board.Advance();
            Assert.IsTrue(board.CellsAlive() == 24 && board.Generation == 13 &&
                board.Cells[3, 9].IsAlive && board.Cells[4, 8].IsAlive &&
                board.Cells[5, 8].IsAlive && board.Cells[4, 10].IsAlive &&
                board.Cells[5, 10].IsAlive && board.Cells[6, 9].IsAlive &&
                board.Cells[12, 9].IsAlive && board.Cells[13, 8].IsAlive &&
                board.Cells[14, 8].IsAlive && board.Cells[15, 9].IsAlive &&
                board.Cells[13, 10].IsAlive && board.Cells[14, 10].IsAlive &&
                board.Cells[9, 3].IsAlive && board.Cells[8, 4].IsAlive &&
                board.Cells[8, 5].IsAlive && board.Cells[10, 4].IsAlive &&
                board.Cells[10, 5].IsAlive && board.Cells[9, 6].IsAlive &&
                board.Cells[9, 12].IsAlive && board.Cells[8, 13].IsAlive &&
                board.Cells[8, 14].IsAlive && board.Cells[9, 15].IsAlive &&
                board.Cells[10, 13].IsAlive && board.Cells[10, 14].IsAlive);
        }
    }
}
