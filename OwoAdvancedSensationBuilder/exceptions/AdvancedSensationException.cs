namespace OwoAdvancedSensationBuilder.exceptions {
    public class AdvancedSensationException: Exception {
        protected AdvancedSensationException() { }

        public AdvancedSensationException(string? message) : base(message) { }

        public AdvancedSensationException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
