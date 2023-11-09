using Application.Features.CustomerDetails.Commands.CreateCustomerDetail;
using Application.Features.CustomerDetails.Queries.GetAllCustomerDetails;
using Application.Features.Instruments.Commands.CreateInstrument;
using Application.Features.Instruments.Queries.GetAllInstruments;
using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Queries.GetAllProducts;
using Application.Filters;
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
            .ForMember(m => m.SerialNo, opt => opt.MapFrom(entity => entity.SerialNumber))
            .ReverseMap();
            CreateMap<CreateInstrumentCommand, Instrument>();
            CreateMap<GetAllInstrumentsQuery, RequestParameter>();

            CreateMap<CustomerDetail, GetAllCustomerDetailsViewModel>().ReverseMap();
            CreateMap<CreateCustomerDetailCommand, CustomerDetail>();
            CreateMap<GetAllCustomerDetailsQuery, RequestParameter>();



        }
    }
}
