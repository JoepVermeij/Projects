using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.IndividueleOpdrachten.VanheesBenjamin
{
    public abstract class Designer : Profiel
    {
        public bool IsDigital { get; set; }
        public string PortfolioURL { get; set; }

        public override string ToString()
        {
            StringBuilder designerString = new StringBuilder();
            if (IsDigital)
            {
                designerString.Append("Digital ");
            }
            designerString.Append("Designer: ");
            designerString.Append(GetFullName());

            if (!String.IsNullOrEmpty(PortfolioURL))
            {
                designerString.Append($" heeft een portfolio op de volgende link ");
                designerString.Append(PortfolioURL);
            }
            return designerString.ToString();
        }
    }
}
