using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Services;

namespace Velora.Application.Services
{
public class LinkResolverService : ILinkResolverService
    {
        private readonly ILinkTypeService _linkTypeService;
        private readonly IPageService _pageService;
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IProductBrandService _productBrandService;
        private readonly IContentItemService _contentItemService;

        public LinkResolverService(
            ILinkTypeService linkTypeService,
            IPageService pageService,
            IProductService productService,
            IProductCategoryService productCategoryService,
            IProductBrandService productBrandService,
            IContentItemService contentItemService)
        {
            _linkTypeService = linkTypeService;
            _pageService = pageService;
            _productService = productService;
            _productCategoryService = productCategoryService;
            _productBrandService = productBrandService;
            _contentItemService = contentItemService;
        }

        public async Task<string?> GetUrlAsync(
            Guid? linkTypeId,
            Guid? targetId,
            string? currentUrl = null)
        {
            if (!linkTypeId.HasValue)
                return currentUrl;

            var linkType = await _linkTypeService.GetByIdAsync(linkTypeId.Value);
            if (!linkType.Success || linkType.Data == null)
                return currentUrl;

            var code = linkType.Data.Code?.ToUpperInvariant();


            // لینک خارجی
            if (code == "EXTERNAL")
                return currentUrl;

            // لینک داخلی بدون Target
            if (!targetId.HasValue)
                return null;

            switch (code)
            {
                case "PAGE":
                    {
                        var pages = await _pageService.GetAllViews();

                        var page = pages
                            .FirstOrDefault(x => x.Id == targetId.Value);

                        return page != null
                            ? $"/{page.Slug}"
                            : null;
                    }

                case "PRODUCT":
                    {
                        var products = await _productService.GetAllViews();

                        var product = products
                            .FirstOrDefault(x => x.Id == targetId.Value);

                        return product != null
                            ? $"/products/{product.Slug}"
                            : null;
                    }

                case "CATEGORY":
                    {
                        var categories =
                            await _productCategoryService.GetAllViews();

                        var category = categories
                            .FirstOrDefault(x => x.Id == targetId.Value);

                        return category != null
                            ? $"/products?page=1&categorySlug={category.Slug}"
                            : null;
                    }

                case "BRAND":
                    {
                        var brands =
                            await _productBrandService.GetAllViews();

                        var brand = brands
                            .FirstOrDefault(x => x.Id == targetId.Value);

                        return brand != null
                            ? $"/products?page=1&brandSlug={brand.Slug}"
                            : null;
                    }

                case "NEWS":
                    {
                        var news = await _contentItemService.GetAllViews();

                        var item = news.FirstOrDefault(x =>
                            x.Id == targetId.Value &&
                            x.ContentType == "0");

                        return item != null
                            ? $"/news/{item.Slug}"
                            : null;
                    }

                case "ARTICLE":
                    {
                        var articles = await _contentItemService.GetAllViews();

                        var item = articles.FirstOrDefault(x =>
                            x.Id == targetId.Value &&
                            x.ContentType == "1");

                        return item != null
                            ? $"/articles/{item.Slug}"
                            : null;
                    }

                default:
                    return currentUrl;
            }
        }
    }


}
