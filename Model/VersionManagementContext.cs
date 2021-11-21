using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VersionManagement
{
    public class VersionManagementContext : DbContext
    {

        public VersionManagementContext(DbContextOptions<VersionManagementContext> options): base(options){

        }

        public DbSet<Projet> Projet { get; set; }
        public DbSet<Version> Version { get; set; }
        public DbSet<DeploimentHistory> DeploimentHistory { get; set; }
        public DbSet<Environment> Environment { get; set; }

        
        public DbSet<ProjetEnvironment> ProjetEnvironment { get; set; }
    }

}