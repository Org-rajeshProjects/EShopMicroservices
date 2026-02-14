using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Discount.Grpc.Protos;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContex contex, ILogger<DiscountService> logger) : DiscountPrtoService.DiscountPrtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
            var coupon = await contex.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);


            if (coupon == null)
            {
                coupon = new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
            }

            logger.LogInformation($"Discount is retreived for ProductName: {coupon.ProductName}, Amount: {coupon.Amount}");


            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {

        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon == null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));

        contex.Coupons.Add(coupon);
        await contex.SaveChangesAsync();

        logger.LogInformation($"Discount is successfully created. Id: {coupon.Id}, ProductName: {coupon.ProductName}, Amount: {coupon.Amount}");

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    // Pseudocode / plan (detailed):
    // 1. Validate the incoming request and its Coupon payload.
    // 2. Convert the request.Coupon to the domain entity (Coupon) using Mapster.
    // 3. Ensure the DTO has a valid Id; if not, throw RpcException with InvalidArgument.
    // 4. Query the DbContext for an existing coupon by Id (async).
    // 5. If existing coupon is null, throw RpcException with NotFound.
    // 6. Copy relevant updatable fields from the incoming coupon to the existing entity.
    // 7. Save changes to the database.
    // 8. Log the update and return the updated CouponModel (mapped from the entity).
    public override async Task<CouponModel> updateDiscount(updateDiscountRequest request, ServerCallContext context)
    {
        if (request == null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Request is null"));

        if (request.Coupon == null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Coupon payload is null"));

        var incoming = request.Coupon.Adapt<Coupon>();
        if (incoming == null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));

        if (incoming.Id <= 0)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid coupon id"));

        var existing = await contex.Coupons.FirstOrDefaultAsync(x => x.Id == incoming.Id);
        if (existing == null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Coupon with Id {incoming.Id} not found"));

        // Update only mutable fields
        existing.ProductName = incoming.ProductName;
        existing.Amount = incoming.Amount;
        existing.Description = incoming.Description;

        // Persist changes
        contex.Coupons.Update(existing);
        await contex.SaveChangesAsync();

        logger.LogInformation($"Discount is successfully updated. Id: {existing.Id}, ProductName: {existing.ProductName}, Amount: {existing.Amount}");

        var couponModel = existing.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await contex.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
        if (coupon == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName:{request.ProductName} not found"));
        }

        contex.Coupons.Remove(coupon);
        await contex.SaveChangesAsync();

        logger.LogInformation($"Discount is successfully deleted. ProductName:{coupon.ProductName}");
        return new DeleteDiscountResponse { Success = true };
    }
}
