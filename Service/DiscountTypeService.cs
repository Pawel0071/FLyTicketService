using FLyTicketService.DTO;
using FLyTicketService.Infrastructure;
using FLyTicketService.Model;
using FLyTicketService.Model.Enums;
using FLyTicketService.Repositories.Interfaces;
using FLyTicketService.Services.Interfaces;

namespace FLyTicketService.Services
{
    public class DiscountTypeService : IDiscountTypeService
    {
        #region Fields

        private readonly IGenericRepository<DiscountType> _discountTypeRepository;
        private readonly ILogger<DiscountTypeService> _logger;

        #endregion

        #region Constructors

        public DiscountTypeService(IGenericRepository<DiscountType> discountTypeRepository, ILogger<DiscountTypeService> logger)
        {
            _discountTypeRepository = discountTypeRepository;
            _logger = logger;
        }

        #endregion

        #region Methods

        public async Task<OperationResult<DiscountTypeDTO?>> GetDiscountTypeAsync(string name)
        {
            _logger.LogInformation($"Getting discount type with name {name}");

            DiscountType? discountType = await _discountTypeRepository.GetByAsync( x=> x.Name == name);

            if (discountType == null)
            {
                _logger.LogWarning("Discount type not found");
                return new OperationResult<DiscountTypeDTO?>(OperationStatus.NotFound, "Discount type not found", null);
            }

            DiscountTypeDTO discountTypeDTO = new DiscountTypeDTO
            {
                DiscountTypeId = discountType.DiscountTypeId,
                Type = discountType.Type,
                Name = discountType.Name,
                Discount = discountType.Discount,
                Description = discountType.Description,
                Condition = discountType.Condition,
                ConditionValue = discountType.ConditionValue
            };

            _logger.LogInformation("Discount type retrieved successfully");

            return new OperationResult<DiscountTypeDTO?>(OperationStatus.Ok, "Discount type retrieved successfully", discountTypeDTO);
        }

        public async Task<OperationResult<IEnumerable<DiscountTypeDTO>>> GetAllDiscountTypeAsync(DiscountCategory? category)
        {
            _logger.LogInformation("Getting all discount types");

            IEnumerable<DiscountType> discountTypes;
            if (category.HasValue)
            {
                discountTypes = await _discountTypeRepository.FilterByAsync( x => x.Type == category);
            }
            else
            {
                discountTypes = await _discountTypeRepository.GetAllAsync();
            }

            List<DiscountTypeDTO> discountTypeDTOs = new List<DiscountTypeDTO>();
            foreach (DiscountType discountType in discountTypes)
            {
                discountTypeDTOs.Add(new DiscountTypeDTO
                {
                    DiscountTypeId = discountType.DiscountTypeId,
                    Type = discountType.Type,
                    Name = discountType.Name,
                    Discount = discountType.Discount,
                    Description = discountType.Description,
                    Condition = discountType.Condition,
                    ConditionValue = discountType.ConditionValue
                });
            }

            _logger.LogInformation("Discount types retrieved successfully");

            return new OperationResult<IEnumerable<DiscountTypeDTO>>(OperationStatus.Ok, "Discount types retrieved successfully", discountTypeDTOs);
        }

        public async Task<OperationResult<bool>> AddDiscountTypeAsync(DiscountTypeDTO discountType)
        {
            _logger.LogInformation("Adding a new discount type");

            DiscountType newDiscountType = new DiscountType
            {
                DiscountTypeId = Guid.NewGuid(),
                Type = discountType.Type,
                Name = discountType.Name,
                Discount = discountType.Discount,
                Description = discountType.Description,
                Condition = discountType.Condition,
                ConditionValue = discountType.ConditionValue
            };

            bool success = await _discountTypeRepository.AddAsync(newDiscountType);

            _logger.LogInformation(success ? "Discount type added successfully" : "Failed to add discount type");

            return new OperationResult<bool>(
                success ? OperationStatus.Created : OperationStatus.InternalServerError,
                success ? "Discount type added successfully" : "Failed to add discount type",
                success);
        }

        public async Task<OperationResult<bool>> UpdateDiscountTypeAsync(Guid discountId, DiscountTypeDTO discountType)
        {
            _logger.LogInformation($"Updating discount type with ID {discountId}");

            DiscountType? existingDiscountType = await _discountTypeRepository.GetByIdAsync(discountId);
            if (existingDiscountType == null)
            {
                _logger.LogWarning("Discount type not found");
                return new OperationResult<bool>(OperationStatus.NotFound, "Discount type not found", false);
            }

            existingDiscountType.Type = discountType.Type;
            existingDiscountType.Name = discountType.Name;
            existingDiscountType.Discount = discountType.Discount;
            existingDiscountType.Description = discountType.Description;
            existingDiscountType.Condition = discountType.Condition;
            existingDiscountType.ConditionValue = discountType.ConditionValue;

            bool success = await _discountTypeRepository.UpdateAsync(existingDiscountType);

            _logger.LogInformation(success ? "Discount type updated successfully" : "Failed to update discount type");

            return new OperationResult<bool>(
                success ? OperationStatus.Ok : OperationStatus.InternalServerError,
                success ? "Discount type updated successfully" : "Failed to update discount type",
                success);
        }

        public async Task<OperationResult<bool>> DeleteDiscountTypeAsync(Guid discountId)
        {
            _logger.LogInformation($"Deleting discount type with ID {discountId}");

            bool success = await _discountTypeRepository.RemoveAsync(discountId);

            _logger.LogInformation(success ? "Discount type deleted successfully" : "Failed to delete discount type");

            return new OperationResult<bool>(
                success ? OperationStatus.NoContent : OperationStatus.NotFound,
                success ? "Discount type deleted successfully" : "Failed to delete discount type",
                success);
        }

        #endregion
    }
}
