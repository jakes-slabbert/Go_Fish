namespace GoFish.Web
{
    public static class GlobalSettings
    {
        public static string Version { get; set; } = string.Empty;

        /// <summary>
        /// Get is used by the Suspended middelware to check if the customer's site should be active
        /// </summary>
        public static bool? IsSuspended { get; set; }
    }
}
