using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;

namespace Velora.Application.Services
{
    public class SitemapService : ISitemapService
    {
        private readonly IConfiguration _configuration;
        private readonly ICmsConfigurationService _cmsConfigurationService;
        private readonly IPageService _pageService;
        private readonly IContentItemService _contentItemService;
        private readonly IProductService _productService;

        public SitemapService(
            IConfiguration configuration,
            ICmsConfigurationService cmsConfigurationService,
            IPageService pageService,
            IContentItemService contentItemService,
            IProductService productService)
        {
            _configuration = configuration;
            _cmsConfigurationService = cmsConfigurationService;
            _pageService = pageService;
            _contentItemService = contentItemService;
            _productService = productService;
        }

        public async Task<List<SitemapUrlDto>> GetSitemapUrlsAsync()
        {
            var result = new List<SitemapUrlDto>();
            var addedUrls = new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

            // 1. خواندن دامنه سایت از فایل JSON
            var baseUrl = await GetSiteBaseUrlAsync();

            // 2. دریافت تنظیمات فعال CMS
            var cmsConfiguration = await _cmsConfigurationService
                .Query()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IsActive);

            // اگر تنظیمات CMS وجود ندارد یا SEO غیرفعال است
            if (cmsConfiguration == null ||
                cmsConfiguration.EnableSeo != true)
            {
                return result;
            }

            // 3. صفحات فعال و منتشرشده
            var pages = await _pageService
                .Query()
                .AsNoTracking()
                .Where(x => x.IsActive && x.IsPublished)
                .ToListAsync();

            // 4. صفحات عمومی و ثابت
            // FAQ و Privacy جداگانه و طبق تنظیمات CMS اضافه می‌شوند
            foreach (var page in pages.Where(x =>
                         x.IsDynamic != true &&
                         !string.Equals(
                             x.Slug, "faq",
                             StringComparison.OrdinalIgnoreCase) &&
                         !string.Equals(
                             x.Slug, "privacy-policy",
                             StringComparison.OrdinalIgnoreCase)))
            {
                if (string.IsNullOrWhiteSpace(page.Slug)|| page.Slug.ToLower()== "layout")
                    continue;

                // صفحه خانه باید به ریشه سایت اشاره کند
                var path = page.IsHome || page.Slug.ToLower()=="home"
                    ? "/"
                    : $"/{page.Slug.Trim('/')}";

                AddUrl(
                    result,
                    addedUrls,
                    baseUrl,
                    path,
                    page.UpdatedAt ?? page.CreatedAt);
            }

            // 5. صفحات داینامیک
            if (cmsConfiguration.EnableDynamicPages == true)
            {
                foreach (var page in pages.Where(x =>
                             x.IsDynamic == true))
                {
                    if (string.IsNullOrWhiteSpace(page.Slug))
                        continue;

                    AddUrl(
                        result,
                        addedUrls,
                        baseUrl,
                        $"/page/{page.Slug.Trim('/')}",
                        page.UpdatedAt ?? page.CreatedAt);
                }
            }

            // 6. صفحه FAQ
            if (cmsConfiguration.EnableFaq == true)
            {
                var faqPage = pages.FirstOrDefault(x =>
                    string.Equals(
                        x.Slug,
                        "faq",
                        StringComparison.OrdinalIgnoreCase) &&
                    x.IsDynamic != true);

                if (faqPage != null)
                {
                    AddUrl(
                        result,
                        addedUrls,
                        baseUrl,
                        "/faq",
                        faqPage.UpdatedAt ?? faqPage.CreatedAt);
                }
            }

            // 7. صفحه حریم خصوصی
            if (cmsConfiguration.EnablePrivacy == true)
            {
                var privacyPage = pages.FirstOrDefault(x =>
                    string.Equals(
                        x.Slug,
                        "privacy-policy",
                        StringComparison.OrdinalIgnoreCase) &&
                    x.IsDynamic != true);

                if (privacyPage != null)
                {
                    AddUrl(
                        result,
                        addedUrls,
                        baseUrl,
                        "/privacy-policy",
                        privacyPage.UpdatedAt ?? privacyPage.CreatedAt);
                }
            }

            // 8. اخبار و مقالات
            var contentTypes = new List<(bool Enabled, string Type, string Prefix)>
        {
            (cmsConfiguration.EnableNews == true, "0", "/news"),
            (cmsConfiguration.EnableBlog == true, "1", "/articles")
        };

            foreach (var contentType in contentTypes.Where(x => x.Enabled))
            {
                var items = await _contentItemService
                    .Query()
                    .AsNoTracking()
                    .Where(x =>
                        x.ContentType == contentType.Type &&
                        x.IsActive &&
                        x.IsPublished &&
                        !x.IsDeleted &&
                        x.Slug != null &&
                        x.Slug != "")
                    .ToListAsync();

                foreach (var item in items)
                {
                    AddUrl(
                        result,
                        addedUrls,
                        baseUrl,
                        $"{contentType.Prefix}/{item.Slug!.Trim('/')}",
                        item.UpdatedAt ?? item.CreatedAt);
                }
            }

            // 9. محصولات فروشگاه
            if (cmsConfiguration.EnableShop == true)
            {
                var products = await _productService
                    .Query()
                    .AsNoTracking()
                    .Where(x =>
                        x.IsActive == true &&
                        x.IsPublished == true &&
                        x.Slug != null &&
                        x.Slug != "")
                    .ToListAsync();

                foreach (var product in products)
                {
                    AddUrl(
                        result,
                        addedUrls,
                        baseUrl,
                        $"/products/{product.Slug!.Trim('/')}",
                        product.UpdatedAt ?? product.CreatedAt);
                }
            }

            return result;
        }

        private Task<string> GetSiteBaseUrlAsync()
        {
            var baseUrl = _configuration["Seed:SiteSettings:BaseUrl"];

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new InvalidOperationException(
                    "Seed:SiteSettings:BaseUrl در appsettings.json پیدا نشد."
                );
            }

            return Task.FromResult(baseUrl.Trim().TrimEnd('/'));
        }

        private static void AddUrl(
            List<SitemapUrlDto> result,
            HashSet<string> addedUrls,
            string baseUrl,
            string path,
            DateTime? lastModified)
        {
            var normalizedPath = string.IsNullOrWhiteSpace(path)
                ? "/"
                : "/" + path.Trim('/');

            var absoluteUrl =
                $"{baseUrl.TrimEnd('/')}{normalizedPath}";

            if (!addedUrls.Add(absoluteUrl))
                return;

            result.Add(new SitemapUrlDto
            {
                Url = absoluteUrl,
                LastModified = lastModified
            });
        }
    }
    }
