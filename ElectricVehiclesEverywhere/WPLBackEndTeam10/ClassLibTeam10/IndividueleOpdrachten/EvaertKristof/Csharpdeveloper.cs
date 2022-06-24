using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.IndividueleOpdrachten.EvaertKristof
{
    public abstract class Csharpdeveloper : Profiel
    {
        public bool IsRemote { get; set; }
        public double DistanceToWork { get; set; }
        public bool IsBiker { get; set; }
        public string IsFavoriteLanguage { get; set; }
        public double CalculateBicicleBonus()
        {
            if (!IsBiker || IsRemote)
            {
                return 0;
            }
            else
            {
                return Math.Round(DistanceToWork*0.333,2);
            }
        }
        public override string ToString()
        {
            string x = "";
            if (IsBiker)
            {
                x = "Ik ben een C# developer en ik fiets";
            }
            else if (IsRemote && !IsBiker)
            {
                x = "Ik ben milieuvriendelijk en ik schrijf mijn code thuis.";
            }
            else
            {
                x = "“Ik ben een C# Developer";
            }
            return x;
        }
    }
}
