namespace Infrastructure.Logging;

public class LogDetail
{
    public string FullName { get; set; }
    public string User { get; set; }
    public string MethodName { get; set; }
    public List<LogParameter> Parameters { get; set; }

    public LogDetail()
    {
        FullName = string.Empty;
        User = string.Empty;
        MethodName = string.Empty;
        Parameters = new List<LogParameter>();
    }

    public LogDetail(string fullName, string methodName, string user, List<LogParameter> parameters)
    {
        FullName = fullName;
        MethodName = methodName;
        User = user;
        Parameters = parameters;
    }
}
