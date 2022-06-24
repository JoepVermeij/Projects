using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.IndividueleOpdrachten.KerkhofsDavy
{
    public abstract class WebDeveloper : Profiel
    {
        public bool IsRemote { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public string FavoriteLanguage { get; set; }

        public int NumberOfLeaveDays { get; set; }
        public List<DateTime> UsedLeaveDays { get; }

        public void UseLeaveDay(DateTime leaveDay)
        {
            if (NumberOfLeaveDays != 0)
            {
                UsedLeaveDays.Add(leaveDay);
                NumberOfLeaveDays--;
            }
        }
        public void UseLeaveDayToday()
        {
            if (NumberOfLeaveDays != 0)
            {
                UsedLeaveDays.Add(DateTime.Now);
                NumberOfLeaveDays--;
            }
        }
        public string GetAdress() => $"{Country}, {City}, {Street} {HouseNumber}";
    }
}
