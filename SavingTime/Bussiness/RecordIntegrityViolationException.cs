namespace SavingTime.Bussiness
{
    public class RecordIntegrityViolationException : Exception
    {
        public RecordIntegrityViolationException(string message) : base(message) { }
    }
}
