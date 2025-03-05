namespace FLyTicketService.Shared
{
    public class OperationResult<T>
    {
        public OperationStatus Status { get; set; }
        public string? Message { get; set; }
        public T? Result { get; set; }
        public bool IsSuccessStatusCode() => Status.ToInt() >= 200 && Status.ToInt() < 300;

        public OperationResult(OperationStatus status, string? message, T? result)
        {
            Status = status;
            Message = message;
            Result = result;
        }


    }
}