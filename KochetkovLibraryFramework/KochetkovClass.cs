using System;

namespace KochetkovLibraryFramework
{
    public class KochetkovClass
    {
        //метод ариф операций
        public static dynamic Execute(dynamic first, char op, dynamic second)
        {
            dynamic result = 0;
            switch (op)
            {
                case '+':
                    result = first + second; break;
                case '-':
                    result = first - second; break;
                case '*':
                    result = first * second; break;
                case '/':
                    //проверка деления на ноль
                    if (second == 0)
                    {
                        result = double.PositiveInfinity; //возвращаем бесконечность
                    }
                    else
                    {
                        result = first / second;
                    }
                    break;
                case '^':
                    result = Math.Pow(first, second); break;
                default:
                    result = 0; break;
            }
            return result;
        }
        //метод для вызова из WPF
        public double Calculate(double num1, double num2, string operation)
        {
            char op = operation[0];
            dynamic result = Execute(num1, op, num2);
            return Convert.ToDouble(result);
        }
    }
}