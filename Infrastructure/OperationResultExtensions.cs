using Microsoft.AspNetCore.Mvc;

namespace FLyTicketService.Infrastructure
{
    public static class OperationResultExtensions
    {
        public static int ToInt(this OperationStatus status)
        {
            return (int)status;
        }

        public static IActionResult GetResult<T>(this OperationResult<T> result)
        {
            if (result.IsSuccessStatusCode())
            {
                return new ObjectResult(result.Result)
                {
                    StatusCode = result.Status.ToInt()
                };
            }
            else
            {
                return new ObjectResult(result.Message)
                {
                    StatusCode = result.Status.ToInt()
                };
            }

        }
    }
}
