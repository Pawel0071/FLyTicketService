namespace FLyTicketService.Infrastructure
{
    public static class OperationResultExtensions
    {
        public static int ToInt(this OperationStatus status)
        {
            return (int)status;
        }
    }
}
