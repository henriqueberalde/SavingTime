namespace SavingTime.Business
{
    public class RecordIntegrityViolationException : Exception
    {
        public RecordIntegrityViolationException(string message) : base(message) { }
    }
}
