using CompanyManagmentSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagmentSystem.DAL.Data.Configurations
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            //Fluent API for Department Domain or Entity or model
            builder.Property(e => e.Name).HasColumnType("varchar").HasMaxLength(50).IsRequired();
            builder.Property(e => e.Address).IsRequired();
            builder.Property(e => e.Salary).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.Gender)
                .HasConversion(
                    (Gender) => Gender.ToString(),
                    (GenderAsString) => (Gender) Enum.Parse(typeof(Gender), GenderAsString, true)
                );

        }
    }
}
