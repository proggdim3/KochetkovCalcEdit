using Microsoft.VisualStudio.TestTools.UnitTesting;
using KochetkovLibraryFramework;// моя библиотека

namespace KochetkovTests
{
    [TestClass]
    public class KochetkovClassTests
    {
        //сложение
        [TestMethod]
        public void test_3plus3()
        {
            dynamic result = KochetkovClass.Execute(3, '+', 3);
            Assert.AreEqual(6, result);
        }

        //вычитание
        [TestMethod]
        public void test_10minus4()
        {
            dynamic result = KochetkovClass.Execute(10, '-', 4);
            Assert.AreEqual(6, result);
        }

        //умножение
        [TestMethod]
        public void test_6times7()
        {
            dynamic result = KochetkovClass.Execute(6, '*', 7);
            Assert.AreEqual(42, result);
        }

        //деление
        [TestMethod]
        public void test_15divide3()
        {
            dynamic result = KochetkovClass.Execute(15, '/', 3);
            Assert.AreEqual(5, result);
        }

        //степень
        [TestMethod]
        public void test_2to3()
        {
            dynamic result = KochetkovClass.Execute(2, '^', 3);
            Assert.AreEqual(8, result);
        }

        //деление на ноль
        [TestMethod]
        public void test_10divide0()
        {
            dynamic result = KochetkovClass.Execute(10, '/', 0);
            Assert.AreEqual(double.PositiveInfinity, result);
        }

        //метод calculate
        [TestMethod]
        public void test_calculate_5plus3()
        {
            KochetkovClass calculator = new KochetkovClass();
            double result = calculator.Calculate(5, 3, "+");
            Assert.AreEqual(8, result);
        }
    }
}
