using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Services
{
    public interface IEmailService: IBaseService
    {
        Task SendAsync(
            string to,
            string subject,
            string htmlBody,
            string? fromName = null,
            CancellationToken cancellationToken = default);
    }
}
