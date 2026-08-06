using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;

namespace Velora.Application.Shared.Dtos
{
    public class ShippingMethodCityDto
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ShippingMethodId { get; set; }

        public Guid CityId { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }

        public bool IsTest { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

    }
}
