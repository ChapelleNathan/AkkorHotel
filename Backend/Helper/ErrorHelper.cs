using System.ComponentModel;
using System.Reflection;
using Backend.Enum;

namespace Backend.Helper;

public class ErrorHelper
{
    public static string GetErrorMessage(ErrorMessageEnum errorMessageEnum)
    {
        var fieldInfo = errorMessageEnum.GetType().GetField(errorMessageEnum.ToString());
        if (fieldInfo is null) return GetErrorMessage(ErrorMessageEnum.Sup500UnknownError);

        var attribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
        return attribute is null ? GetErrorMessage(ErrorMessageEnum.Sup500NoErrorDescription) : attribute.Description;
    }
}