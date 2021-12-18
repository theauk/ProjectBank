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
        switch (role)
        {
            case Admin:
                return Role.Admin;
            case Supervisor:
                return Role.Supervisor;
            case Student:
                return Role.Student;
            default:
                return Role.Unknown;
        }
    }
}
