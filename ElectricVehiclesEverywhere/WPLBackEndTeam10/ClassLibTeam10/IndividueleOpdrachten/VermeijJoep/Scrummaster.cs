using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.IndividueleOpdrachten.VermeijJoep
{
    public abstract class Scrummaster : Profiel
    {
        public bool IsAgile { get; set; }
        public DateTime CommencementOfEmployment { get; set; }

        public int YearsOfEmployment 
        {
            get { return CommencementOfEmployment.Year - DateTime.Now.Year; }
        }
        public Dictionary<string, TimeSpan> Tickets
        {
            get;
        }

        public void AddTicket(string ticketName, TimeSpan ticketTimeSpan)
        {
            if (!Tickets.ContainsKey(ticketName))
            {
                Tickets.Add(ticketName, ticketTimeSpan);
            }
        }
        public void AddTicket(string ticketName)
        {
            if (!Tickets.ContainsKey(ticketName))
            {
                TimeSpan eenUur = new TimeSpan(1, 0, 0);
                Tickets.Add(ticketName, eenUur);
            }
        }
        public void NewSprint()
        {
            Tickets.Clear();
        }
    }
}
