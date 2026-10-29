
# Database Migrations

## Initial migration

The `InitialCreate` migration creates the initial schema for the Library Management System.

## Create a migration

```bash
dotnet ef migrations add MigrationName
```

## Apply migrations

```bash
dotnet ef database update
```

## Review migrations

```bash
dotnet ef migrations list
```

## Important

Commit migration files to GitHub so the database schema can be recreated by other developers.

Do not commit database passwords, private connection strings, or generated database files.
