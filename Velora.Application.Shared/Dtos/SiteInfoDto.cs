using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class SiteInfoDto
    {
        //public HeaderDto Header { get; set; }

        public FooterDto Footer { get; set; }

        //public MenuDto Menu { get; set; }

        public SqlSiteGlobalSetting Settings { get; set; }
    }
}
