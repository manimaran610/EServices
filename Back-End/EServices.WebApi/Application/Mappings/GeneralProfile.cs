using System.Collections.Generic;
using Application.Features.CustomerDetails.Commands.CreateCustomerDetail;
using Application.Features.CustomerDetails.Queries.GetAllCustomerDetails;
using Application.Features.Instruments.Commands.CreateInstrument;
using Application.Features.Instruments.Queries.GetAllInstruments;
using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Queries.GetAllProducts;
using Application.Features.Rooms.Commands.CreateRoom;
using Application.Features.Rooms.Commands.UpdateRoom;
using Application.Features.Rooms.Queries.GetAllRooms;
using Application.Features.Rooms.Seeds;
using Application.Features.Trainees.Commands.CreateTrainee;
using Application.Features.Trainees.Queries.GetAllTrainees;
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

            CreateMap<CustomerDetail, GetAllCustomerDetailsViewModel>()
            .ForMember(m => m.FormTypeName, opt => opt.MapFrom(entity => entity.FormType.ToString()))
            .ReverseMap();
            CreateMap<CreateCustomerDetailCommand, CustomerDetail>();
            CreateMap<GetAllCustomerDetailsQuery, RequestParameter>();

            CreateMap<Room, GetAllRoomsViewModel>()
            .ForMember(e => e.RoomGrills, opt => opt.MapFrom(entity => entity.RoomGrills))
            .ForMember(e => e.RoomLocations, opt => opt.MapFrom(entity => entity.RoomLocations))
            .ReverseMap();

            CreateMap<CreateRoomCommand, Room>()
            .ForMember(e => e.RoomLocations, opt => opt.Ignore())
            .ForMember(e => e.RoomGrills, opt => opt.Ignore());

            CreateMap<GetAllRoomsQuery, RequestParameter>();

            CreateMap<UpdateRoomCommand, Room>()
             .ForMember(e => e.RoomLocations, opt => opt.Ignore())
             .ForMember(e => e.RoomGrills, opt => opt.Ignore());

            CreateMap<GetAllRoomsQuery, RequestParameter>();

            CreateMap<GrillDTO, RoomGrill>();
            CreateMap<RoomGrill, GrillDTO>().ReverseMap();

            CreateMap<LocationDTO, RoomLocation>();
            CreateMap<RoomLocation, LocationDTO>().ReverseMap();

            CreateMap<GetAllLogsQuery, RequestParameter>();

            
            CreateMap<Trainee, GetAllTraineesViewModel>().ReverseMap();
            CreateMap<CreateTraineeCommand, Trainee>();
            CreateMap<GetAllTraineesQuery, RequestParameter>();




        }
    }
}
