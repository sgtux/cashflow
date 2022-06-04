namespace Cashflow.Api.Extensions
{
    public static class StringExtensions
    {
        public static string FormatToLike(this string value) => $"%{value}%";
    }
}