using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
    {
    public class ComboBoxItemDto<T>
        {
        public T Value { get; set; }
        public string Label { get; set; }
        public string Code { get; set; }
    }

    }
