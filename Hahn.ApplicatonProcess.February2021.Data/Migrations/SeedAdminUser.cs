﻿
using Hahn.ApplicatonProcess.February2021.Data.Helpers;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;


namespace Hahn.ApplicatonProcess.February2021.Data.Migrations
{
    [DbContext(typeof(HahnDbContext))]
    [Migration("CustomMigration1_SeedAdminUser")]
    public class SeedAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var adminPassword = "admin".WithBCrypt();
            migrationBuilder.Sql($@"
                DECLARE @password NVARCHAR(MAX) = N'{adminPassword}'
                 if not exists(select * from dbo.users where EMAIL = N'admin@hahn.com' )
                INSERT INTO [dbo].[tbl_users]
                           ([EMAIL]
                           ,[PASSWORD]
                           ,[FIRST_NAME]
                           ,[LAST_NAME]
                           ,[IsDeleted])
                     VALUES
                           (N'admin@hahn.com'
                           ,@password
                           ,N'admin'
                           ,N'admin'
                           ,0)

                DECLARE @adminId INT = @@IDENTITY

                INSERT INTO dbo.UserRoles
                        ( RoleId, UserId )
                SELECT RoleId, @adminId FROM dbo.Roles
                WHERE DefaultRoleName = 'Administrator'
                ");
        }

    }
}
