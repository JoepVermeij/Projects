using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.IndividueleOpdrachten.SleursBen
{
    public abstract class Tester : Profiel
    {
        public double Divide(double a, double b)
        {
            if(b == 0)
            {
                throw new Exception("Delen door nul");
            }
            else
            {
                return a / b;
            }
        }
        public int CountExceptionsForDivision(List<DivideArgs> argumentsForDivision)
        {
            int count = 0;
            foreach (DivideArgs item in argumentsForDivision)
            {
                try
                {
                    Divide(item.A, item.B);
                }
                catch (Exception)
                {
                    count++;
                }
            }
            return count;

        }

        public override string ToString()
        {
            return $"De tester {this.GetFirstName()} zegt: {this.GetRandomPersonalQuote()}";
        }
        public class DivideArgs
        {
            public double A { get; set; }
            public double B { get; set; }
        }
    }
}
