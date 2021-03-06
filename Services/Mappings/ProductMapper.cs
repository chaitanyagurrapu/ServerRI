﻿#region Using directives

using AutoMapper;
using Core.Extensions;
using Data.Entities;
using Models.Products;

#endregion


namespace Services.Mappings
{
    internal class ProductMapper : BaseMapper
    {
        internal override void ConfigureMap(ConfigurationStore mapperConfig)
        {
            MapProducts(mapperConfig);
        }

        private static void MapProducts(ConfigurationStore mapperConfig)
        {
            mapperConfig.CreateMap<Product, ProductModel>()
               .ForMember(d => d.Id, opt => opt.MapFrom(s => s.ProductID))
               .ReverseMap();

            mapperConfig.CreateMap<ProductModelCreateRequest, Product>()
                .ForMember(d => d.ProductID, opt => opt.MapFrom(s => s.Id))
                .ReverseMap();

            mapperConfig.CreateMap<Product, ProductModelResponse>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.ProductID))
                .ForMember(d => d.LastModifiedDate, opt => opt.MapFrom(s => s.ModifiedDate))
                .ForMember(d => d.ParentProductCategoryID, opt => opt.MapFrom(s => s.ProductCategory.ParentProductCategoryID))
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.ProductCategory.Name))
                .ForMember(d => d.CatalogDescription, opt => opt.MapFrom(s => s.ProductType.CatalogDescription))
                .ForMember(d => d.ProductTypeName, opt => opt.MapFrom(s => s.ProductType.Name))
                .ReverseMap();

            mapperConfig.CreateMap<ProductCategoryModel, ProductCategory>()
                .IgnorePropertiesThatHaveNotBeenSet()
                .ReverseMap();

            mapperConfig.CreateMap<ProductModelUpdateRequest, Product>()
                .IgnorePropertiesThatHaveNotBeenSet()
                .ReverseMap();
        }

    }
}