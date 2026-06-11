using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class FieldRule
    {
        public bool Enabled { get; set; }
        public string Type { get; set; } // text, textarea, repeater, image
    }
}
