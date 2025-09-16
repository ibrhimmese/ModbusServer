using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Infrastructure.Logging;

//public class LogParameter
//{
//    public string Name { get; set; }
//    public object Value { get; set; }
//    public string Type { get; set; }


//    public LogParameter()
//    {
//        Name = string.Empty;
//        Value = string.Empty;
//        Type = string.Empty;
//    }
//    public LogParameter(string name, object value, string type)
//    {
//        Name = name;
//        Value = value;
//        Type = type;
//    }
//}


public class LogParameter
{
    public string Name { get; set; }

    private object _value;
    public object Value
    {
        get => _value;
        set => _value = MaskPassword(value);
    }

    public string Type { get; set; }

    public LogParameter()
    {
        Name = string.Empty;
        Value = string.Empty;
        Type = string.Empty;
    }

    public LogParameter(string name, object value, string type)
    {
        Name = name;
        Value = value; // setter tetiklenir, maskeleme yapılır
        Type = type;
    }

    private object MaskPassword(object value)
    {
        if (value == null) return null;

        try
        {
            // JSON'a çevir
            var json = JsonSerializer.Serialize(value);

            // Regex ile Password alanlarını maskele
            var masked = Regex.Replace(
                json,
                "\"Password\"\\s*:\\s*\".*?\"",
                "\"Password\":\"****\"",
                RegexOptions.IgnoreCase
            );

            // JSON tekrar parse edilip object döndürülüyor
            return JsonSerializer.Deserialize<object>(masked) ?? value;
        }
        catch
        {
            // Fallback: string içinde password varsa basit maskele
            var str = value.ToString() ?? string.Empty;
            return Regex.Replace(
                str,
                "(?i)(\"?password\"?\\s*[:=]\\s*)(\".*?\"|[^,}\\s]*)",
                "$1\"****\""
            );
        }
    }
}

