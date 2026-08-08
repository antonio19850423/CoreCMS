using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ProductRatingSummaryDto
    {
        public decimal AverageRate { get; set; }

        public int TotalReviews { get; set; }

        public int SatisfactionPercentage { get; set; }

        public int FiveStarCount { get; set; }

        public int FourStarCount { get; set; }

        public int ThreeStarCount { get; set; }

        public int TwoStarCount { get; set; }

        public int OneStarCount { get; set; }
    }
}
