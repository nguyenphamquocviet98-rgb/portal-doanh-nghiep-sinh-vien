using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InternshipPortal.Helpers;

// HTML number inputs submit a dot decimal separator regardless of the server's region.
public class EvaluationScoreModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext context)
    {
        var value = context.ValueProvider.GetValue(context.ModelName);
        if (value == ValueProviderResult.None) return Task.CompletedTask;
        context.ModelState.SetModelValue(context.ModelName, value);
        var input = value.FirstValue?.Trim();
        if (string.IsNullOrEmpty(input))
            context.Result = ModelBindingResult.Success(null);
        else if (decimal.TryParse(input, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture, out var score))
            context.Result = ModelBindingResult.Success(score);
        else
            context.ModelState.AddModelError(context.ModelName, "Điểm không hợp lệ. Dùng dấu chấm cho phần thập phân.");
        return Task.CompletedTask;
    }
}
