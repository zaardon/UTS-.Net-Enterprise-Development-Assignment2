using BCMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

     
    namespace BlueConsultingManagementSystem.Models
    {
        public class Expense
        {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public int ExpensePK { get; set; }
     
            [Required]
            public string Description { get; set; }
     
            [Required]
            public double Amount { get; set; }
     
            [Required]
            public CurrencyType CType { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime DateOfExpense { get; set; }
     
            public virtual Report Report { get; set; }
     
            [NotMapped]
            public string FullReport
            {
                get
                {
                    return Description + " " + Amount + "" + CType;
                }
            }

            [NotMapped]
            public double ConvertedAmount
            { 
                get
                {
                    CurrencyConverter converter = new CurrencyConverter();
                    return converter.ConvertCurrencyToAUD(this.CType.ToString(), this.Amount);
                }
                
            
            }

     
            //currency code here somewhere
        }  
    }
     
     
     
   