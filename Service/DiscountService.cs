using FLyTicketService.DTO;
using FLyTicketService.Mapper;
using FLyTicketService.Model;
using FLyTicketService.Repositories.Interfaces;
using FLyTicketService.Service.Interfaces;
using FLyTicketService.Services.Interfaces;
using FLyTicketService.Shared;

namespace FLyTicketService.Service;

public class DiscountService: IDiscountService
{
    #region Fields

    private readonly IGenericRepository<Discount> _discountRepository;
    private readonly IGenericRepository<Condition> _conditionRepository;
    private readonly IFlightPriceService _flightPriceService;
    private readonly ILogger<DiscountService> _logger;

    #endregion

    #region Constructors

    public DiscountService(
        IGenericRepository<Discount> discountRepository,
        ILogger<DiscountService> logger,
        IGenericRepository<Condition> conditionRepository,
        IFlightPriceService flightPriceService
    )
    {
        _conditionRepository = conditionRepository;
        _flightPriceService = flightPriceService;
        _discountRepository = discountRepository;
        _logger = logger;
    }

    #endregion

    #region Methods

    public async Task<OperationResult<DiscountDTO?>> GetDiscountAsync(string name)
    {
        _logger.LogInformation($"Getting discount type with name {name}");

        Discount? discount = await _discountRepository.GetByAsync(x => x.Name == name);

        if (discount == null)
        {
            _logger.LogWarning("Discount type not found");
            return new OperationResult<DiscountDTO?>(OperationStatus.NotFound, "Discount type not found", null);
        }

        DiscountDTO discountDTO = discount.ToDTO();

        _logger.LogInformation("Discount type retrieved successfully");
        return new OperationResult<DiscountDTO?>(OperationStatus.Ok, "Discount type retrieved successfully", discountDTO);
    }

    public async Task<OperationResult<IEnumerable<DiscountDTO>>> GetAllDiscountAsync()
    {
        _logger.LogInformation($"Getting all discount");

        IEnumerable<Discount> discounts = await _discountRepository.GetAllAsync();

        IEnumerable<DiscountDTO> discountDTOs = discounts.Select(d => d.ToDTO());

        _logger.LogInformation("All discount types retrieved successfully");
        return new OperationResult<IEnumerable<DiscountDTO>>(OperationStatus.Ok, "All discount types retrieved successfully", discountDTOs);
    }

    public async Task<OperationResult<bool>> AddDiscountAsync(DiscountDTO discountType)
    {
        _logger.LogInformation($"Adding new discount type: {discountType.Name}");

        Discount discount = discountType.ToDomain();

        await _discountRepository.AddAsync(discount);

        _logger.LogInformation("Discount type added successfully");
        return new OperationResult<bool>(OperationStatus.Ok, "Discount type added successfully", true);
    }

    public async Task<OperationResult<bool>> UpdateDiscountAsync(Guid discountId, DiscountDTO discountType)
    {
        _logger.LogInformation($"Updating discount type with ID {discountId}");

        Discount? discount = await _discountRepository.GetByAsync(x => x.DiscountId == discountId);

        if (discount == null)
        {
            _logger.LogWarning("Discount type not found");
            return new OperationResult<bool>(OperationStatus.NotFound, "Discount type not found", false);
        }

        discount.Name = discountType.Name;
        discount.Value = discountType.Value;
        discount.Description = discountType.Description;
        discount.Conditions = discountType.Conditions.Select( condition => condition.ToDomain());

        await _discountRepository.UpdateAsync(discount);

        _logger.LogInformation("Discount type updated successfully");
        return new OperationResult<bool>(OperationStatus.Ok, "Discount type updated successfully", true);
    }

    public async Task<OperationResult<bool>> DeleteDiscountAsync(Guid discountId)
    {
        _logger.LogInformation($"Deleting discount type with ID {discountId}");

        Discount? discount = await _discountRepository.GetByAsync(x => x.DiscountId == discountId);

        if (discount == null)
        {
            _logger.LogWarning("Discount type not found");
            return new OperationResult<bool>(OperationStatus.NotFound, "Discount type not found", false);
        }

        await _discountRepository.RemoveAsync(discount.DiscountId);

        _logger.LogInformation("Discount type deleted successfully");
        return new OperationResult<bool>(OperationStatus.Ok, "Discount type deleted successfully", true);
    }

    #endregion
}