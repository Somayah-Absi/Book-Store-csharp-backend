using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class OrderProductService
    {
        private readonly EcommerceSdaContext _dbContext;

        public OrderProductService(EcommerceSdaContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<decimal> GetTotalOrderPriceAsync(int orderId)
        {
            decimal? totalOrderPrice = await _dbContext.OrderProducts
                .Where(op => op.OrderId == orderId)
                .Select(op => op.Quantity * op.Product.ProductPrice)
                .SumAsync();

            return totalOrderPrice ?? 0;
        }


        public async Task<IEnumerable<OrderProduct>> GetAllOrderProductAsync()
        {
            try
            {
                var orderProducts = await _dbContext.OrderProducts
                .Include(row => row.Order)
                .Include(row => row.Product)
                .Select(row => new OrderProduct
                {
                    OrderProductId = row.OrderProductId,
                    Quantity = row.Quantity,
                    OrderId = row.OrderId,
                    ProductId = row.ProductId,
                    Order = new Order
                    {
                        OrderId = row.Order.OrderId,
                        UserId = row.Order.UserId,
                        Payment = row.Order.Payment,
                        OrderStatus = row.Order.OrderStatus,
                        OrderDate = row.Order.OrderDate
                    },
                    Product = new Product
                    {
                        ProductId = row.Product.ProductId,
                        ProductName = row.Product.ProductName,
                        ProductPrice = row.Product.ProductPrice * (row.Quantity ?? 1),
                    },
                })
                .ToListAsync();

                return orderProducts;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving OrderProduct.", ex);
            }
        }


        public async Task<IEnumerable<OrderProduct>> GetOrderProductByIdAsync(int id)
        {
            try
            {
                var orderProducts = await _dbContext.OrderProducts
                .Include(row => row.Order)
                .Include(row => row.Product)
                .Where(row => row.Order.OrderId == id)
                .Select(row => new OrderProduct
                {
                    OrderProductId = row.OrderProductId,
                    Quantity = row.Quantity,
                    OrderId = row.OrderId,
                    ProductId = row.ProductId,
                    Order = new Order
                    {
                        OrderId = row.Order.OrderId,
                        UserId = row.Order.UserId,
                        Payment = row.Order.Payment,
                        OrderStatus = row.Order.OrderStatus,
                        OrderDate = row.Order.OrderDate
                    },
                    Product = new Product
                    {
                        ProductId = row.Product.ProductId,
                        ProductName = row.Product.ProductName,
                        ProductPrice = row.Product.ProductPrice * (row.Quantity ?? 1),
                    },
                })
                .ToListAsync();
                return orderProducts;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving OrderProduct with ID {id}.", ex);
            }
        }

        public async Task<OrderProduct> AddOrUpdateProductQuantity(OrderProduct product)
        {
            try
            {
                var orderProduct = await _dbContext.OrderProducts
                .FirstOrDefaultAsync(op => op.OrderId == product.OrderId && op.ProductId == product.ProductId);

                if (orderProduct != null)
                {
                    orderProduct.Quantity += product.Quantity;
                }
                else
                {
                    orderProduct = new OrderProduct
                    {
                        OrderId = product.OrderId,
                        ProductId = product.ProductId,
                        Quantity = product.Quantity,
                    };
                    _dbContext.OrderProducts.Add(orderProduct);
                }
                await _dbContext.SaveChangesAsync();
                return orderProduct;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating OrderProduct.", ex);
            }
        }


        public async Task<IEnumerable<OrderProduct>> UpdateProductQuantity(int orderId, int productId, int incrementAmount)
        {
            try
            {
                var orderProducts = await _dbContext.OrderProducts
                    .Where(op => op.OrderId == orderId && op.ProductId == productId)
                    .ToListAsync();

                foreach (var orderProduct in orderProducts)
                {
                    orderProduct.Quantity += incrementAmount;
                }

                await _dbContext.SaveChangesAsync();

                return orderProducts;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while updating OrderProduct with ID {productId}.", ex);
            }
        }


        public async Task<bool> DeleteOrderProduct(int orderProductId)
        {
            try
            {
                var productToDelete = await _dbContext.OrderProducts.FindAsync(orderProductId);
                if (productToDelete != null)
                {
                    _dbContext.OrderProducts.Remove(productToDelete);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while deleting OrderProduct with ID {orderProductId}.", ex);
            }
        }
    }
}


