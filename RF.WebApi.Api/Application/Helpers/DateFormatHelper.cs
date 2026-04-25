namespace RF.WebApi.Api.Application.Helpers
{
    /// <summary>
    /// Central date formatting helper — mirrors the CurrencyType pattern.
    /// Services receive the Account object and call these helpers, exactly the same
    /// way PdfService calls GetCurrencyCulture(account.CurrencyType).
    /// </summary>
    public static class DateFormatHelper
    {
        // System-wide defaults — used when the account has no override set
        public const string DefaultDateFormat      = "dd-MMMM-yyyy";   // e.g. 25-April-2026
        public const string DefaultShortDateFormat = "dd-MMM-yyyy";    // e.g. 25-Apr-2026

        /// <summary>
        /// Returns the effective long date format for the given account value.
        /// Falls back to <see cref="DefaultDateFormat"/> when null/empty.
        /// </summary>
        public static string GetDateFormat(string? accountDateFormat)
            => string.IsNullOrWhiteSpace(accountDateFormat) ? DefaultDateFormat : accountDateFormat;

        /// <summary>
        /// Returns the effective short date format for the given account value.
        /// Falls back to <see cref="DefaultShortDateFormat"/> when null/empty.
        /// </summary>
        public static string GetShortDateFormat(string? accountShortDateFormat)
            => string.IsNullOrWhiteSpace(accountShortDateFormat) ? DefaultShortDateFormat : accountShortDateFormat;

        /// <summary>
        /// Formats a <see cref="DateOnly"/> using the account's long date format (or system default).
        /// Usage: DateFormatHelper.Format(bill.Date, account.DateFormat)
        /// </summary>
        public static string Format(DateOnly date, string? accountDateFormat)
            => date.ToString(GetDateFormat(accountDateFormat));

        /// <summary>
        /// Formats a nullable <see cref="DateOnly"/> using the account's long date format.
        /// Returns empty string when date is null.
        /// </summary>
        public static string Format(DateOnly? date, string? accountDateFormat)
            => date.HasValue ? Format(date.Value, accountDateFormat) : string.Empty;

        /// <summary>
        /// Formats a <see cref="DateOnly"/> using the account's short date format (or system default).
        /// Usage: DateFormatHelper.FormatShort(bill.Date, account.ShortDateFormat)
        /// </summary>
        public static string FormatShort(DateOnly date, string? accountShortDateFormat)
            => date.ToString(GetShortDateFormat(accountShortDateFormat));

        /// <summary>
        /// Formats a nullable <see cref="DateOnly"/> using the account's short date format.
        /// Returns empty string when date is null.
        /// </summary>
        public static string FormatShort(DateOnly? date, string? accountShortDateFormat)
            => date.HasValue ? FormatShort(date.Value, accountShortDateFormat) : string.Empty;

        /// <summary>
        /// Validates if a date format string is valid for .NET date formatting.
        /// Returns true if null/whitespace (as we fallback to defaults).
        /// </summary>
        public static bool IsValidFormat(string? format)
        {
            if (string.IsNullOrWhiteSpace(format)) return true;

            try
            {
                // Try formatting a test date with the provided string
                DateTime.Now.ToString(format);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
