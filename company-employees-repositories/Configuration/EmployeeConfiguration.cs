using company_employees_entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace company_employees_repositories.Configuration;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasData(
            new Employee
            {
                Id = new Guid("68850305-27ec-4ec4-a2d0-9038662748eb"),
                Name = "Claudette Cabahug",
                Age = 39,
                Position = "CEO",
                CompanyId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870")
            },
            new Employee
            {
                Id = new Guid("b239f19a-386a-4004-93ca-db769bd11058"),
                Name = "Norah Cabahug",
                Age = 6,
                Position = "CEO",
                CompanyId = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3")
            }
        );
    }
}