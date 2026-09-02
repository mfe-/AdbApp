using Java.Lang;

namespace AdbApp.Maui.Services;

public static class SysProp
{
    private static readonly Lazy<Class> SystemPropertiesClass =
        new(() => Class.ForName("android.os.SystemProperties"));

    private static readonly Lazy<Java.Lang.Reflect.Method> GetMethod =
        new(() => SystemPropertiesClass.Value.GetDeclaredMethod("get",
            Class.FromType(typeof(Java.Lang.String))));

    public static string GetProp(string propertyName)
    {
        var result = GetMethod.Value.Invoke(null, new Java.Lang.String(propertyName));
        return result?.ToString() ?? string.Empty;
    }
}
