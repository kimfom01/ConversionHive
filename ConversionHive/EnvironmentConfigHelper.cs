namespace ConversionHive;

public static class EnvironmentConfigHelper
{
    public static string GetConnectionString(IConfiguration config, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            return config.GetConnectionString("CONNECTION_STRING")
                   ?? throw new NullReferenceException("CONNECTION_STRING missing");
        }

        return Environment.GetEnvironmentVariable("CONNECTION_STRING")
               ?? throw new NullReferenceException("CONNECTION_STRING missing");
    }

    public static string GetIssuer(IConfiguration config, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            return config.GetValue<string>("JWT:ISSUER")
                   ?? throw new NullReferenceException("ISSUER missing");
        }

        return Environment.GetEnvironmentVariable("ISSUER")
               ?? throw new NullReferenceException("ISSUER missing");
    }

    public static string GetAudience(IConfiguration config, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            return config.GetValue<string>("JWT:AUDIENCE")
                   ?? throw new NullReferenceException("AUDIENCE missing");
        }

        return Environment.GetEnvironmentVariable("AUDIENCE")
               ?? throw new NullReferenceException("AUDIENCE missing");
    }

    public static string GetKey(IConfiguration config, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            return config.GetValue<string>("JWT:KEY")
                   ?? throw new NullReferenceException("KEY missing");
        }

        return Environment.GetEnvironmentVariable("KEY")
               ?? throw new NullReferenceException("KEY missing");
    }

    public static string GetSalt(IConfiguration config, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            return config.GetValue<string>("SALT")
                   ?? throw new NullReferenceException("SALT missing");
        }

        return Environment.GetEnvironmentVariable("SALT")
               ?? throw new NullReferenceException("SALT missing");
    }
}