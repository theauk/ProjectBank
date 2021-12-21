namespace ProjectBank.Core;

public enum Role
{
    Unknown,
    Admin,
    Supervisor,
    Student,
}

public static class Roles
{
    public const string SuperAdmin = nameof(SuperAdmin);
    public const string Admin = nameof(Admin);
    public const string Supervisor = nameof(Supervisor);
    public const string Student = nameof(Student);

    public static Role GetRole(string role)
    {
        return role switch
        {
            Admin => Role.Admin,
            Supervisor => Role.Supervisor,
            Student => Role.Student,
            _ => Role.Unknown
        };
    }
}
