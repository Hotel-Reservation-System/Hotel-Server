MIGRATIONS
==========


WHAT IS A MIGRATION? 
--------------------

The Schema is a more specific map to the database than the Data Model. The schema of a database 
defines tables, specifies columns and their types, and the relationships between tables, i.e. it 
concretely describes the structure and topology of the database. It also imposes constraints on data 
that enters the database. Please see DbContext/Context.cs for more on the difference between the 
Data Model and Schema.

Organizations may have more than one instance of a database. For instance, a company may have a 
development environment where software is developed, a production environment where stable software 
runs and perhaps a demo environment for demonstrating products to clients. If the software team 
decides to add make modifications to the database's topology, they have a problem. A database's 
structure changes if tables, columns or their relations are added, deleted or modified. In short, 
if any of these fundamental entities are changed, the database's schema will change. When the schema
changes, the development team will have to upgrade all instances of the database by migrating them
from the old schema to the new, modified version.

How can all instances of a database be updated automatically and reliably to the latest schema? To 
do this, we need version control to track changes to the schema. Version control for a database's 
schema is called SCHEMA MIGRATIONS or just MIGRATIONS. A Migration file contains a set of changes to 
a database's current schema. These transformations may include adding, modifying, renaming and 
deleting/dropping tables and columns or the re-adjustment of table relations. A migration file will 
also have a way to revert its own changes. A migrations file may also contain scripts to enact 
transformations and to insert default data into the database by populating tables and columns.

Migrations are constantly generated in the development stage of an application, because that's when 
the schema is changing the most. Large architectural changes and feature churn necessitate frequent 
alterations to the database's schema. Migrations are also needed when upgrading a database, 
switching to another database vendor, or moving a database into the cloud. Having a version 
controlled history of the database's schema (which captures the changes made to the database over 
time) gives you the ability to migrate the database to older or newer versions of the schema. 
Migrations are a convenient way to track your database's schema over time in a predictabe and 
consistent way. 


This is the workflow for changing the schema and generating migrations:

In the Development Environment:
1) When you need to add tables or modify relations in the database, edit the Data Model in 
   Context.cs.
2) Because the Data Model has been modified, you need to generate a new migrations file.
3) You run the migration file in the development environment to verify that it works as intended. 


In the Production, Demo and other Environments:
4) Deploy and run the migrations file in other environments to update these instances of the 
   database to the latest version of the schema.
5) Verify that migration has gone smoothly in these environments as well.


CONTENTS OF A MIGRATIONS FILE
-----------------------------

Migrations are generated or applied with CLI commands. When the command is given, it appears that 
Entity Framework Core combines the Data Model with type information from Model classes to generate 
a migration file. Generating a Migration will create a new file in the Migrations folder. It will 
contain a a user-named class that is a subclass of Entity Framework Core's Migration class. You may 
need to manually remove things from either method if it added more than the changes you made to the 
Data Model. Make sure you read the migration file before applying it.

Every migration file will contain an Up() and Down() method. The Up() method contains 
transformations to the schema of the Database. The Down() method just rolls back transformations in 
the Up() method i.e. it returns the schema to the state in the Up() method of the previous 
migration. The database schema should be unchanged if a call to Up() is followed by a Down(). If 
another, new migration file has been generated and needs to be applied to the database, call its 
Up() method. To revert these transformations, call its Down() method. 

The final set of transformations in a long chain of such changes is described in the Up() method of 
the last migration. The current state of a database's schema is the result of calling the Up() 
methods of all previous Migrations. 


Here is a answer from StackOverflow that expounds on migrations:

    DB Migrations make changes to your database to reflect the changes made to the Entity Framework 
    Model. These changes are added to the database with the Up method.
    
    When you want to rollback a change (e.g. by rolling back a changeset in TFS or Git), the changes 
    in the database have to be rolled back also because otherwise your Entity Framework model is 
    out-of-sync with the database. This is what the Down method is for. It undo's all changes to the 
    database that were done when the Up method was run against the database.
    
    The Seed method gives you the ability to Insert, Update or Delete data which is sometimes needed 
    when you change the model of the database. So the Seed method is optional and only necessary 
    when you need to modify existing data or add new data to make the model working.
    
    - Ric .Net
    
Source: https://stackoverflow.com/questions/36650268/the-difference-between-the-up-and-down-methods-in-the-migration-file


GENERATING, APPLYING AND REMOVING MIGRATIONS
--------------------------------------------

A migrations file can be auto-generated on command. 

Migrations files for .NET Core projects can be generated either through .NET Core CLI or terminals
built into IDEs. Note that Jetbrains Rider cannot and does not support Visual Studio's Package 
Manager Console commands. Here are two basic commands for generating and using migrations. Check the 
Entity Framework Core docs for more.


1. GENERATING MIGRATIONS: Creates a new migration. The migration name should describe in pascal 
case, the changes you made to the data model. 

Visual Studio: add-migration <MigrationName> 
E.g.: add-migration AddUserUserPreferenceUserInformationRoleUserRoleTables


2. UPDATING DATABASE TO THE LATEST MIGRATION: Runs the Up() method of all migrations. 

Visual Studio: update-database
 
 




 
 

 
 