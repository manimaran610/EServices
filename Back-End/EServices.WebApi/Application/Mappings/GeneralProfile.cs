using Application.Features.Instruments.Commands.CreateInstrument;
using Application.Features.Instruments.Queries.GetAllInstruments;
using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Queries.GetAllProducts;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Product, GetAllProductsViewModel>().ReverseMap();
            CreateMap<CreateProductCommand, Product>();
            CreateMap<GetAllProductsQuery, GetAllProductsParameter>();

            CreateMap<Instrument, GetAllInstrumentsViewModel>().ReverseMap();
            CreateMap<CreateInstrumentCommand, Instrument>();

        }
    }
}
