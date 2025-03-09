using Company.FirstProject.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.FirstProject.DAL.Data.Configrations
{
    public class DepartmentConfigruations : IEntityTypeConfiguration<Department>
    {
        void IEntityTypeConfiguration<Department>.Configure(EntityTypeBuilder<Department> builder)
        {
            builder.Property(D => D.Id).UseIdentityColumn(1,1);
        }
    }
}
