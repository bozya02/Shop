﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Core
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ShopBozyaEntities : DbContext
    {
        private static ShopBozyaEntities _context;
        public ShopBozyaEntities()
            : base("name=ShopBozyaEntities")
        {
        }

        public static ShopBozyaEntities GetContext()
        {
            if (_context == null)
                _context = new ShopBozyaEntities();

            return _context;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCountry> ProductCountries { get; set; }
        public virtual DbSet<ProductIntake> ProductIntakes { get; set; }
        public virtual DbSet<ProductIntakeProduct> ProductIntakeProducts { get; set; }
        public virtual DbSet<ProductOrder> ProductOrders { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<StatusIntake> StatusIntakes { get; set; }
        public virtual DbSet<StatusOrder> StatusOrders { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Worker> Workers { get; set; }
    }
}
