
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Enums;
using MediatR;

namespace Application.Features.CustomerDetails.Commands.CreateCustomerDetail
{
    public class CreateCustomerDetailCommandHandler : IRequestHandler<CreateCustomerDetailCommand, Response<int>>
    {
        private readonly ICustomerDetailRepositoryAsync _customerDetailRepository;
        private readonly IInstrumentRepositoryAsync _instrumentRepository;
        private readonly IMapper _mapper;
        private readonly ITraineeRepositoryAsync _traineeRepositoryAsync;
        public CreateCustomerDetailCommandHandler(
            ICustomerDetailRepositoryAsync customerDetailRepository,
            IInstrumentRepositoryAsync instrumentRepositoryAsync,
            IMapper mapper,
            ITraineeRepositoryAsync traineeRepositoryAsync)
        {
            _traineeRepositoryAsync = traineeRepositoryAsync;
            _customerDetailRepository = customerDetailRepository;
            _instrumentRepository = instrumentRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateCustomerDetailCommand request, CancellationToken cancellationToken)
        {
            var customerDetail = _mapper.Map<Domain.Entities.CustomerDetail>(request);
            var isInstrumentExists = await _instrumentRepository.IsExistsAsync(request.InstrumentId);
            if (!isInstrumentExists)
                throw new ApiException($"Instrument does not exists with InstrumentId -{request.InstrumentId} ");

            var isTraineeExists = await _traineeRepositoryAsync.IsExistsAsync(request.TraineeId);
            if (!isTraineeExists)
                throw new ApiException($"Trainee does not exists with TraineeId -{request.TraineeId} ");

            customerDetail.CustomerNo = await CreateUniqueCustomerIdentifer(request);
            await _customerDetailRepository.AddAsync(customerDetail);
            return new Response<int>(customerDetail.Id);
        }

        private async Task<string> CreateUniqueCustomerIdentifer(CreateCustomerDetailCommand request)
        {
            string id = "001";
            string prefix = $"VP-{GetTypeName(request.FormType)}-{request.Client.Substring(0, 3).ToUpper()}-{DateTime.Now.Year - 2000}{DateTime.Now.ToString("MM")}-";
            var customerDetails = await _customerDetailRepository.GetPagedReponseAsync(0, 1, $"CustomerNo:con:{prefix}", "CustomerNo:desc");
            if (customerDetails.Count > 0)
            {
                var existsCustNo = customerDetails.FirstOrDefault().CustomerNo;
                if (existsCustNo.Split('-').Length == 5)
                {
                    int existsId = Convert.ToInt32(existsCustNo.Split('-')[4]);
                    id = existsId > 0 && existsId <= 8 ? $"00{existsId + 1}" :
                    id = existsId >= 9 && existsId <= 98 ? $"0{existsId + 1}" : $"{existsId + 1}";
                }
            }
            return prefix + id;

        }
        private string GetTypeName(FormType formType)
        {
            string result = string.Empty;
            switch (formType)
            {
                case FormType.ACPH:
                    result = "ACPH";
                    break;
                case FormType.FilterIntegrity:
                    result = "FI";
                    break;
                case FormType.ParticleCountThreeCycle:
                    result = "PC3";
                    break;
                case FormType.ParticleCountSingleCycle:
                    result = "PC1";
                    break;
            }
            return result;
        }
    }
}