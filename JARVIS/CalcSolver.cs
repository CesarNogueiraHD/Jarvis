using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JARVIS
{
    public class CalcSolver
    {
        public static string Solve(string expression)
        {
            string[] parts = expression.Split(' ');

            double x = double.Parse(parts[0]);
            double y = double.Parse(parts[2]);
            double z = 0.0;

            switch (parts[1])
            {
                case "vezes":
                    z = x * y;
                    break;
                case "mais":
                    z = x + y;
                    break;
                case "menos":
                    z = x - y;
                    break;
                case "por":
                    z = x / y;
                    break;
            }

            return z.ToString();
        }
    }
}
