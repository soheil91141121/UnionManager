﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UnionManager.Models.DomainModels
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class db_UnionEntities : DbContext
    {
        public db_UnionEntities()
            : base("name=db_UnionEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<ActivityType> ActivityTypes { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<Relation> Relations { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<TradeGroup> TradeGroups { get; set; }
        public virtual DbSet<Trade> Trades { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<VehicleGroup> VehicleGroups { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
    }
}
