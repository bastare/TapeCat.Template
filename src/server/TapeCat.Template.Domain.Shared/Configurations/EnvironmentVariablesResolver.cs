namespace TapeCat.Template.Domain.Shared.Configurations;

public static class EnvironmentVariablesResolver
{
    public static class PostgresSQLVariables
    {
        public static string PostgresServer => ResolveEnvironmentVariable("POSTGRES_SERVER");

        public static string PostgresUser => ResolveEnvironmentVariable("POSTGRES_USER");

        public static string PostgresPassword => ResolveEnvironmentVariable("POSTGRES_PASSWORD");

        public static string PostgresDB => ResolveEnvironmentVariable("POSTGRES_DB");

        public static string ToConnectionString()
            => string.Join(
                separator: ';',
                value:
                [
                    $"User ID={PostgresUser}",
                    $"Password={PostgresPassword}",
                    $"Host={PostgresServer}",
                    "Port=5432",
                    $"Database={PostgresDB}",
                    "Pooling=true",
                    "Connection Lifetime=30"
                ]);
    }

    private static string ResolveEnvironmentVariable(string? environmentVeritableName)
    {
        NotNullOrEmpty(environmentVeritableName);

        return Environment.GetEnvironmentVariable(environmentVeritableName!) ??
            throw new ArgumentException(
                $"No variable with this name: {environmentVeritableName}",
                nameof(environmentVeritableName));
    }
}