using System;
using System.Windows;
using System.Windows.Controls;
using KochetkovLibraryFramework;  //моя библиотека

namespace KochetkovWPF
{
    public partial class MainWindow : Window
    {
        //хранение выражения
        string expr = "";

        KochetkovClass calc = new KochetkovClass();

        //конструктор окна 
        public MainWindow()
        {
            InitializeComponent();
        }

        //обработчик цифр
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            expr = expr + btn.Content.ToString();
            InputTextBox.Text = expr;
        }

        //обработчик точки
        private void Dot_Click(object sender, RoutedEventArgs e)
        {
            expr = expr + ".";
            InputTextBox.Text = expr;
        }

        //обработчик скобок
        private void LeftParenthesis_Click(object sender, RoutedEventArgs e)
        {
            expr = expr + "(";
            InputTextBox.Text = expr;
        }

        private void RightParenthesis_Click(object sender, RoutedEventArgs e)
        {
            expr = expr + ")";
            InputTextBox.Text = expr;
        }

        //обработчик операций
        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string op = btn.Content.ToString();

            expr = expr + " " + op + " ";
            InputTextBox.Text = expr;
            ResultTextBox.Text = expr;
        }

        //обработчик равно
        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            if (expr == "")
                return;

            try
            {
                double res = CalcExpr(expr);

                if (double.IsInfinity(res))
                {
                    ResultTextBox.Text = expr + " = Ошибка";
                    InputTextBox.Text = "Ошибка";
                    expr = "";
                }
                else
                {
                    ResultTextBox.Text = expr + " =";
                    InputTextBox.Text = res.ToString();
                    expr = res.ToString();
                }
            }
            catch
            {
                ResultTextBox.Text = expr + " = Ошибка";
                InputTextBox.Text = "Ошибка";
                expr = "";
            }
        }

        //метод для вычисления со скобками
        private double CalcExpr(string str)
        {
            str = str.Replace(" ", "");

            while (str.Contains("("))
            {
                int start = str.LastIndexOf('(');
                int end = str.IndexOf(')', start);

                string inside = str.Substring(start + 1, end - start - 1);
                double insideRes = CalcSimple(inside);

                str = str.Substring(0, start) + insideRes.ToString() + str.Substring(end + 1);
            }

            return CalcSimple(str);
        }

        //метод для простых выражений
        private double CalcSimple(string str)
        {
            //обрабатываем два минуса подряд
            while (str.Contains("--"))
            {
                str = str.Replace("--", "+");
            }
            while (str.Contains("+-"))
            {
                str = str.Replace("+-", "-");
            }

            //считаем степени (минус применяется после степени)
            while (true)
            {
                int pos = -1;
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == '^')
                    {
                        pos = i;
                        break;
                    }
                }

                if (pos == -1) break;

                //находим число для возведения в степень (без минуса)
                int left = pos - 1;
                while (left >= 0 && (char.IsDigit(str[left]) || str[left] == '.'))
                {
                    left--;
                }
                left++;

                //проверяем, есть ли минус перед числом (НО он не входит в степень)
                bool hasMinus = false;
                if (left > 0 && str[left - 1] == '-')
                {
                    //проверяем что это не операция (перед минусом нет цифры)
                    if (left - 1 == 0 || (left - 1 > 0 && !char.IsDigit(str[left - 2]) && str[left - 2] != '.'))
                    {
                        hasMinus = true;
                    }
                }

                //правое число
                int right = pos + 1;
                while (right < str.Length && (char.IsDigit(str[right]) || str[right] == '.'))
                {
                    right++;
                }

                string leftStr = str.Substring(left, pos - left);
                string rightStr = str.Substring(pos + 1, right - pos - 1);

                double a = Convert.ToDouble(leftStr, System.Globalization.CultureInfo.InvariantCulture);
                double b = Convert.ToDouble(rightStr, System.Globalization.CultureInfo.InvariantCulture);

                //сначала степень
                double r = Math.Pow(a, b);

                //потом минус если есть
                if (hasMinus)
                {
                    r = -r;
                    left--; //убираем минус из выражения
                }

                str = str.Substring(0, left) + r.ToString() + str.Substring(right);
            }

            //считаем умножение и деление
            while (true)
            {
                int pos = -1;
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == '*' || str[i] == '/')
                    {
                        pos = i;
                        break;
                    }
                }

                if (pos == -1) break;

                int left = pos - 1;
                while (left >= 0 && (char.IsDigit(str[left]) || str[left] == '.'))
                {
                    left--;
                }

                bool hasMinus = false;
                if (left >= 0 && str[left] == '-')
                {
                    if (left == 0 || (left > 0 && !char.IsDigit(str[left - 1]) && str[left - 1] != '.'))
                    {
                        hasMinus = true;
                    }
                }
                if (hasMinus) left--;
                left++;

                int right = pos + 1;
                while (right < str.Length && (char.IsDigit(str[right]) || str[right] == '.'))
                {
                    right++;
                }

                string leftStr = str.Substring(left, pos - left);
                string rightStr = str.Substring(pos + 1, right - pos - 1);

                double a = Convert.ToDouble(leftStr, System.Globalization.CultureInfo.InvariantCulture);
                if (hasMinus) a = -a;
                double b = Convert.ToDouble(rightStr, System.Globalization.CultureInfo.InvariantCulture);

                double r = 0;
                if (str[pos] == '*') r = a * b;
                else if (str[pos] == '/')
                {
                    if (b == 0) return double.PositiveInfinity;
                    r = a / b;
                }

                str = str.Substring(0, left) + r.ToString() + str.Substring(right);
            }

            //считаем сложение и вычитание
            double total = 0;
            string num = "";
            char lastOp = '+';

            for (int i = 0; i <= str.Length; i++)
            {
                if (i < str.Length && (char.IsDigit(str[i]) || str[i] == '.'))
                {
                    num += str[i];
                }
                else
                {
                    if (num != "")
                    {
                        double n = Convert.ToDouble(num, System.Globalization.CultureInfo.InvariantCulture);
                        if (lastOp == '+') total += n;
                        else if (lastOp == '-') total -= n;
                        num = "";
                    }

                    if (i < str.Length && (str[i] == '+' || str[i] == '-'))
                    {
                        lastOp = str[i];
                    }
                }
            }

            return total;
        }

        //очистка
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            InputTextBox.Text = "0";
            ResultTextBox.Text = "";
            expr = "";
        }

        //удаление символа
        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            if (expr.Length > 0)
            {
                expr = expr.Substring(0, expr.Length - 1);
                if (expr == "") InputTextBox.Text = "0";
                else InputTextBox.Text = expr;
            }
        }
    }
}