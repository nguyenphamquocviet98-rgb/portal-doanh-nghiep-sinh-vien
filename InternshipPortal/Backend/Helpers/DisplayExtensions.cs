using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Globalization;

namespace InternshipPortal.Helpers;

public static class DisplayExtensions
{
    public static string DisplayName(this Enum value) => value.GetType().GetField(value.ToString())?.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? value.ToString();
    public static string VietnamDate(this DateTime value) => DateTime.SpecifyKind(value, DateTimeKind.Utc).AddHours(7).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
    public static string VietnamDateTime(this DateTime value) => DateTime.SpecifyKind(value, DateTimeKind.Utc).AddHours(7).ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
}