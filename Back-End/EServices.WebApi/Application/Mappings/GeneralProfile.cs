using Application.Features.Instruments.Commands.CreateInstrument;
using Application.Features.Instruments.Queries.GetAllInstruments;
using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Queries.GetAllProducts;
using AutoMapper;
using Domain.Entities;


namespace Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Product, GetAllProductsViewModel>().ReverseMap();
            CreateMap<CreateProductCommand, Product>();
            CreateMap<GetAllProductsQuery, GetAllProductsParameter>();

            CreateMap<Instrument, GetAllInstrumentsViewModel>()
            .ForMember(m => m.SerialNo,opt =>opt.MapFrom(entity => entity.SerialNumber))
            .ReverseMap();
            CreateMap<CreateInstrumentCommand, Instrument>();

        }
    }
}
