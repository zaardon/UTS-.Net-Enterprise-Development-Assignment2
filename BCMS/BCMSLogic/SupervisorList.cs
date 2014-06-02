using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCMS.Models
{
    public class SupervisorList
    {

        private List<Supervisor> list;

        public SupervisorList()
        {
            list = new List<Supervisor>();
        }

        /*
         * This method creates a Supervisor object (based on a name and a total amount of expense spending)
         * and adds them to a List of Supervisors.
         */
        public void AddToList(string name, double amount)
        {
            if (list.Count() == 0)
            {
                list.Add(new Supervisor(name, amount));
            }
            else
            {
                for (int i = 0; i < list.Count(); i++)
                {
                    if (list[i].ReturnName() == name)
                    {
                        list[i].SetAmount(amount + list[i].ReturnAmount());
                        break;
                    }
                    if (i == (list.Count()-1))
                    {
                        list.Add(new Supervisor(name, amount));
                        break;
                    }
                }
            }
        }

        public List<Supervisor> ReturnSupervisors()
        {
            return list;
        }
    }
}