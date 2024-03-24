using Application.Filters;
using Application.Interfaces.Repositories;
using Application.Parameters;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequestParameter, IRequest<PagedResponse<IEnumerable<GetAllProductsViewModel>>>
    {
        public int Offset { get; set; }
        public int Count { get; set; }
        public string Filter { get; set; }
        public string Sort { get; set; }
    }
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PagedResponse<IEnumerable<GetAllProductsViewModel>>>
    {
        private readonly IProductRepositoryAsync _productRepository;
        private readonly IMapper _mapper;
        public GetAllProductsQueryHandler(IProductRepositoryAsync productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllProductsViewModel>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllProductsParameter>(request);
            var product = await _productRepository.GetPagedReponseAsync(validFilter.Offset, validFilter.Count);
            var productViewModel = _mapper.Map<IEnumerable<GetAllProductsViewModel>>(product.pagedResponse);
            return new PagedResponse<IEnumerable<GetAllProductsViewModel>>(productViewModel, validFilter.Offset, validFilter.Count);
        }
    }
}
