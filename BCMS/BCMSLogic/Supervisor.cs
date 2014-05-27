using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCMS.Models
{
    public class Supervisor
    {

        private string name;
        private double amount;

        public Supervisor(string name, double amount)
        {
            this.name = name;
            this.amount = amount;
        }

        public double ReturnAmount()
        {
            return amount;
        }

        public string ReturnName()
        {
            return name;
        }

        public void SetAmount(double amount)
        {
            this.amount = amount;
        }
    }
}