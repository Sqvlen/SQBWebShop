using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController : BaseApiController
{
    private readonly IGenericRepository<Product> _productsRepository;
    private readonly IGenericRepository<ProductBrand> _productBrandRepository;
    private readonly IGenericRepository<ProductType> _productTypeRepository;
    private readonly IMapper _mapper;

    public ProductsController(IGenericRepository<Product> productsRepository,
        IGenericRepository<ProductBrand> productBrandRepository,
        IGenericRepository<ProductType> productTypeRepository,
        IMapper mapper)
    {
        _productsRepository = productsRepository;
        _productBrandRepository = productBrandRepository;
        _productTypeRepository = productTypeRepository;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
    {
        var specification = new ProductsWithTypesAndBrands();
        
        var products = await _productsRepository
            .ListAsync(specification);
        
        return Ok(_mapper
            .Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
    {
        var specification = new ProductsWithTypesAndBrands(id);
        
        var product = await _productsRepository
            .GetEntityWithSpecification(specification);

        if (product == null)
            return NotFound(new ApiResponse(404));
        
        return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
    {
        return Ok(await _productBrandRepository.ListAllAsync());
    }
    
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
    {
        return Ok(await _productTypeRepository.ListAllAsync());
    }
}