using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShareSquare.Data.Models
{
    public abstract class BaseEntity
    {
        [DisplayName("Created At")]
        public DateTime? Created_at { get; set; }

        [DisplayName("Updated At")]
        public DateTime? Updated_at { get; set; }
    }
}
