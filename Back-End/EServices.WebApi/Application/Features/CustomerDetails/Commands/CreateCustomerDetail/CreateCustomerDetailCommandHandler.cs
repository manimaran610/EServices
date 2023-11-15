
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.CustomerDetails.Commands.CreateCustomerDetail
{
    public class CreateCustomerDetailCommandHandler : IRequestHandler<CreateCustomerDetailCommand, Response<int>>
    {
        private readonly ICustomerDetailRepositoryAsync _customerDetailRepository;
        private readonly IInstrumentRepositoryAsync _instrumentRepository;

        private readonly IMapper _mapper;
        public CreateCustomerDetailCommandHandler(
            ICustomerDetailRepositoryAsync customerDetailRepository,
            IInstrumentRepositoryAsync instrumentRepositoryAsync,
            IMapper mapper)
        {
            _customerDetailRepository = customerDetailRepository;
            _instrumentRepository = instrumentRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateCustomerDetailCommand request, CancellationToken cancellationToken)
        {
            var customerDetail = _mapper.Map<Domain.Entities.CustomerDetail>(request);
            var instrument =await _instrumentRepository.GetByIdAsync(request.InstrumentId);
            if (instrument == null) 
              throw new ApiException($"Instrument does not exists with InstrumentId -{request.InstrumentId} ");
   

            await _customerDetailRepository.AddAsync(customerDetail);
            return new Response<int>(customerDetail.Id);
        }

    }
}