using ApiClient.Catalog.Product.Events;
using Catalog.API.Services;
using MassTransit;

namespace Catalog.API.Consumers;

public class ProductBalanceUpdateConsumer : IConsumer<ProductBalanceUpdateMessage>
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductBalanceUpdateConsumer> _logger;

    public ProductBalanceUpdateConsumer(IProductService productService, ILogger<ProductBalanceUpdateConsumer> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProductBalanceUpdateMessage> context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        
        if (context.Message is null)
        {
            throw new InvalidOperationException("Message is not allowed Null.");
        }
        
        if (context.Message.Products is null || !context.Message.Products.Any())
        {
            throw new InvalidOperationException("Products is not allowed Null.");
        }

        foreach (var product in context.Message.Products)
        {
            var isExisted = await _productService.CheckExistingAsync(product.ProductCode, PropertyName.Code);

            if (!isExisted)
            {
                throw new InvalidOperationException($"Product with '{product.ProductCode}' is not existed.");
            }
        }

        var result = await _productService.ReduceProductBalanceAsync(context.Message.Products.ToList());
        
        if (result)
        {
            _logger.LogInformation("ProductBalanceUpdateMessage consumed sucessfully. ReceiptNumber: {ReceiptNumber}", context.Message.ReceiptNumber);
        }
        else
        {
            _logger.LogInformation("ProductBalanceUpdateMessage consumed failed. ReceiptNumber: {ReceiptNumber}", context.Message.ReceiptNumber);
        }
    }
}