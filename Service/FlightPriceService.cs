using FLyTicketService.Model;
using FLyTicketService.Model.Enums;
using FLyTicketService.Repositories.Interfaces;
using FLyTicketService.Service.Interfaces;
using System.Reflection;

namespace FLyTicketService.Service
{
    public class FlightPriceService : IFlightPriceService
    {
        #region Constants

        private const decimal minPrice = 20m;

        #endregion

        #region Fields

        private readonly IGenericRepository<Discount> _genericDiscountRepository;

        #endregion

        #region Constructors

        public FlightPriceService(IGenericRepository<Discount> genericDiscountRepository)
        {
            _genericDiscountRepository = genericDiscountRepository;
        }

        #endregion

        #region Methods

        public (decimal, decimal) ApplyDiscounts(IEnumerable<Discount> discounts, Ticket ticket)
        {
            decimal price = ticket.FlightSeat.FlightSchedule.Price;
            decimal discountValue = 0;

            foreach (Discount discount in discounts)
            {
                if (IsDiscountApplicable(discount, ticket))
                {
                    discountValue += Math.Max(price - discount.Value, minPrice);
                }
            }

            return (price - discountValue, discountValue);
        }

        public async Task<List<Discount>> GetAllApplicableDiscountsAsync(Ticket ticket)
        {
            List<Discount> applicableDiscounts = new List<Discount>();
            IEnumerable<Discount> allDiscounts = await _genericDiscountRepository.GetAllAsync();

            if (allDiscounts != null)
            {
                foreach (Discount discount in allDiscounts)
                {
                    if (IsDiscountApplicable(discount, ticket))
                    {
                        applicableDiscounts.Add(discount);
                    }
                }
            }

            return applicableDiscounts;
        }

        public bool IsDiscountApplicable(Discount discount, Ticket ticket)
        {
            foreach (Condition condition in discount.Conditions)
            {
                object? targetObject = GetTargetObject(condition, ticket);

                if (targetObject == null || !EvaluateCondition(condition, targetObject))
                {
                    return false;
                }
            }

            return true;
        }

        private object? GetTargetObject(Condition condition, Ticket ticket)
        {
            return condition.Category switch
            {
                DiscountCategory.Tenant => ticket.Tenant,
                DiscountCategory.Destination => ticket.FlightSeat.FlightSchedule.Destination,
                DiscountCategory.Airline => ticket.FlightSeat.FlightSchedule.Airline,
                DiscountCategory.Aircraft => ticket.FlightSeat.FlightSchedule.Aircraft,
                DiscountCategory.Origin => ticket.FlightSeat.FlightSchedule.Origin,
                DiscountCategory.Arrival => ticket.FlightSeat.FlightSchedule.Arrival,
                DiscountCategory.Departure => ticket.FlightSeat.FlightSchedule.Departure,
                var _ => null
            };
        }

        private bool CompareValues(object propertyValue, string? conditionValue, DiscountCondition conditionType)
        {
            if (propertyValue == null)
            {
                return false;
            }

            switch (conditionType)
            {
                case DiscountCondition.Equal:
                    return propertyValue.ToString().Equals(conditionValue, StringComparison.OrdinalIgnoreCase);

                case DiscountCondition.NotEqual:
                    return !propertyValue.ToString().Equals(conditionValue, StringComparison.OrdinalIgnoreCase);

                case DiscountCondition.GreaterThan:
                    return CompareNumeric(propertyValue, conditionValue, (a, b) => a > b);

                case DiscountCondition.LessThan:
                    return CompareNumeric(propertyValue, conditionValue, (a, b) => a < b);

                case DiscountCondition.GreaterThanOrEqual:
                    return CompareNumeric(propertyValue, conditionValue, (a, b) => a >= b);

                case DiscountCondition.LessThanOrEqual:
                    return CompareNumeric(propertyValue, conditionValue, (a, b) => a <= b);

                case DiscountCondition.Contains:
                    return propertyValue.ToString().Contains(conditionValue, StringComparison.OrdinalIgnoreCase);

                case DiscountCondition.NotContains:
                    return !propertyValue.ToString().Contains(conditionValue, StringComparison.OrdinalIgnoreCase);

                case DiscountCondition.StartsWith:
                    return propertyValue.ToString().StartsWith(conditionValue, StringComparison.OrdinalIgnoreCase);

                case DiscountCondition.EndsWith:
                    return propertyValue.ToString().EndsWith(conditionValue, StringComparison.OrdinalIgnoreCase);

                case DiscountCondition.DayOfWeek:
                    if (propertyValue is DateTime dateTimeValue && Enum.TryParse(conditionValue, out DayOfWeek dayOfWeek))
                    {
                        return dateTimeValue.DayOfWeek == dayOfWeek;
                    }

                    return false;

                case DiscountCondition.ToDay:
                    if (propertyValue is DateTime dateValue)
                    {
                        return dateValue.Date == DateTime.Now.Date;
                    }

                    if (propertyValue is DateTimeOffset dateOffsetValue)
                    {
                        return dateOffsetValue.Date == DateTime.Now.Date;
                    }

                    return false;

                default:
                    throw new InvalidOperationException($"Unsupported DiscountCondition: {conditionType}");
            }
        }

        private bool EvaluateCondition(Condition condition, object targetObject)
        {
            object propertyValue;

            if (condition.Property.Equals("self", StringComparison.OrdinalIgnoreCase))
            {
                propertyValue = targetObject; // Obiekt jest porównywany bezpośrednio
            }
            else
            {
                PropertyInfo? property = targetObject.GetType()
                                                     .GetProperty(condition.Property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (property == null)
                {
                    throw new InvalidOperationException($"Property {condition.Property} not found on {targetObject.GetType().Name}");
                }

                propertyValue = property.GetValue(targetObject);
            }

            return CompareValues(propertyValue, condition.ConditionValue, condition.ConditionType);
        }

        private bool CompareNumeric(object propertyValue, string? conditionValue, Func<decimal, decimal, bool> comparator)
        {
            if (decimal.TryParse(propertyValue.ToString(), out decimal propertyNumeric) &&
                decimal.TryParse(conditionValue, out decimal conditionNumeric))
            {
                return comparator(propertyNumeric, conditionNumeric);
            }

            return false;
        }

        #endregion
    }
}