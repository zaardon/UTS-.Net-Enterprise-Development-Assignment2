﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BlueManagementConsultingSystem
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BlueModelContainer : DbContext
    {
        public BlueModelContainer()
            : base("name=BlueModelContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Consultant> Consultants { get; set; }
        public DbSet<Supervisor> Supervisors { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Budget> Budgets { get; set; }
    }
}
