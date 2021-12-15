namespace ProjectBank.Core;

public enum Role
{
    Student,
    Admin,
    Supervisor,
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
            case Roles.Admin:
                return Role.Admin;
            case Roles.Supervisor:
                return Role.Supervisor;
            default:
                return Role.Student;
        }
    }
}
