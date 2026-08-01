using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Infrastructure
{
    public static class NationalCodeValidator
    {
        public static bool IsValid(string? nationalCode)
        {
            if (string.IsNullOrWhiteSpace(nationalCode))
            {
                return false;
            }

            nationalCode = nationalCode.Trim();

            // کد ملی باید دقیقاً 10 رقم باشد
            if (nationalCode.Length != 10)
            {
                return false;
            }

            // همه کاراکترها باید عدد باشند
            if (!nationalCode.All(char.IsDigit))
            {
                return false;
            }

            // کدهایی مانند 0000000000 یا 1111111111 نامعتبر هستند
            if (nationalCode.Distinct().Count() == 1)
            {
                return false;
            }

            int sum = 0;

            for (int i = 0; i < 9; i++)
            {
                int digit = nationalCode[i] - '0';

                sum += digit * (10 - i);
            }

            int remainder = sum % 11;

            int controlDigit = nationalCode[9] - '0';

            if (remainder < 2)
            {
                return controlDigit == remainder;
            }

            return controlDigit == 11 - remainder;
        }
    }
}
