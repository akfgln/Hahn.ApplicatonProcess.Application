
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hahn.ApplicatonProcess.February2021.Data.Migrations
{

    public static class DbProcess
    {
        public static void AddNewColumn(this MigrationBuilder migrationBuilder, string colName, string colType, string schemaName, string tableName)
        {

            migrationBuilder.Sql($@"
                IF NOT EXISTS 
                (
                    SELECT * 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE table_name = '{tableName}'
                    AND column_name = '{colName}'
                )
                BEGIN
                    ALTER TABLE [{schemaName}].[{tableName}]
                                ADD [{colName}] {colType}
                END
            ELSE
                BEGIN

                 print 'Column [{colName}] already exists in Table [{schemaName}].[{tableName}]'

                END

            ");
        }

        public static void DropColumn(this MigrationBuilder migrationBuilder, string colName, string schemaName, string tableName)
        {

            migrationBuilder.Sql($@"
                IF EXISTS 
                (
                    SELECT * 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE table_name = '{tableName}'
                    AND column_name = '{colName}'
                )
                BEGIN
                    ALTER TABLE [{schemaName}].[{tableName}]
                                DROP COLUMN [{colName}]
                END
            ");
        }

        public static void DropIndex(this MigrationBuilder migrationBuilder, string indexName, string schemaName, string tableName)
        {
            migrationBuilder.Sql($@"
            DROP INDEX IF EXISTS {indexName}
            ON {schemaName}.{tableName};
            ");
        }

        //public static void AddForeignKey(this MigrationBuilder migrationBuilder, string tableName, string foreignKey, string referenceTable, string referenceColumn)
        //{
        //    migrationBuilder.Sql($@"
        //      ALTER TABLE {tableName}
        //      ADD FOREIGN KEY ({foreignKey}) REFERENCES {referenceTable}({referenceColumn});
        //       "
        //        );
        //}

        public static void AddForeignKey(this MigrationBuilder migrationBuilder, string tableName, string foreignKey, string referenceTable, string referenceColumn)
        {
            migrationBuilder.Sql($@"
                   IF NOT EXISTS 
                (
                   SELECT TOP(1) a.COLUMN_NAME FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS b JOIN 
                     INFORMATION_SCHEMA.KEY_COLUMN_USAGE a ON a.CONSTRAINT_CATALOG = b.CONSTRAINT_CATALOG 
                     AND a.CONSTRAINT_NAME = b.CONSTRAINT_NAME WHERE a.COLUMN_NAME = '{foreignKey}' and a.TABLE_NAME='{tableName}')
                     BEGIN
                       ALTER TABLE {tableName}
                       ADD FOREIGN KEY ({foreignKey}) REFERENCES {referenceTable}({referenceColumn})
                     END
                 ;");
        }

        public static void DropForeignKey(this MigrationBuilder migrationBuilder, string foreignKey, string foreignKeyName, string tableName)
        {
            migrationBuilder.Sql($@"
                IF EXISTS 
                (
                   SELECT TOP(1) a.COLUMN_NAME FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS b JOIN 
                     INFORMATION_SCHEMA.KEY_COLUMN_USAGE a ON a.CONSTRAINT_CATALOG = b.CONSTRAINT_CATALOG 
                     AND a.CONSTRAINT_NAME = b.CONSTRAINT_NAME WHERE a.COLUMN_NAME = '{foreignKey}' and a.TABLE_NAME='{tableName}')
                     BEGIN
                       ALTER TABLE {tableName}
                       DROP CONSTRAINT {foreignKeyName}
                     END
                 ;");
        }

        public static void CreateTable(this MigrationBuilder migrationBuilder, string tableName, string primaryKey)
        {
            migrationBuilder.Sql($@"
              IF NOT EXISTS(
               SELECT [name]
               FROM sys.tables
               WHERE [name] = '{tableName}'
	         )
	    CREATE TABLE {tableName} ({primaryKey} int IDENTITY(1,1) PRIMARY KEY)
            ");
        }

        public static void AlterColumnType(this MigrationBuilder migrationBuilder, string tableName, string columnName, string dataType)
        {
            migrationBuilder.Sql($@"
               ALTER TABLE {tableName}
               ALTER COLUMN {columnName} {dataType};
             ");
        }
    }
}
