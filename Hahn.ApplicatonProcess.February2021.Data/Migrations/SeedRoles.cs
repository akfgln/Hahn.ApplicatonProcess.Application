﻿using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hahn.ApplicatonProcess.February2021.Data.Migrations
{
    [DbContext(typeof(HahnDbContext))]
    [Migration("CustomMigration_SeedRoles")]
    public class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

            if not exists(select * from dbo.Roles where DefaultRoleName = N'Administrator' )
              INSERT INTO dbo.Roles
                      ( DefaultRoleName,IsDeleted,CreateDate,ModifiedDate )
              VALUES  ( N'Administrator',0,GETDATE(),GETDATE())

		    if not exists(select * from dbo.Roles where DefaultRoleName = N'Manager' )
              INSERT INTO dbo.Roles
                      ( DefaultRoleName,IsDeleted,CreateDate,ModifiedDate )
              VALUES  ( N'Manager',0,GETDATE(),GETDATE())	

            if not exists(select * from dbo.Roles where DefaultRoleName = N'Administrator,Manager' )
               INSERT INTO dbo.Roles
                      ( DefaultRoleName,IsDeleted,CreateDate,ModifiedDate )
              VALUES  ( N'Administrator,Manager',0,GETDATE(),GETDATE())
           ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                  delete from  dbo.Roles
                ");
        }
    }
}
