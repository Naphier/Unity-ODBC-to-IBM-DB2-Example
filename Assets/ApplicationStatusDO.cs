
public class ApplicationStatusDO
{
    public string applicationId;
    public string applicant;
    public string currentProcess;
    public string from;
    public string to;
    public string start;
    public string duration;
    public string view;

    public override string ToString()
    {
        return string.Format(
            "{0}, {1}, {2}, {3}, " + 
            "{4}, {5}, {6}, {7}", 
            applicationId, applicant, currentProcess, from, to, start, duration, view);
    }

    public string ToStringWithProperties()
    {
        return string.Format(
           "applicationId: {0}, applicant: {1}, currentProcess: {2}, from: {3}, " +
           "to: {4}, start: {5}, duration: {6}, view: {7}",
           applicationId, applicant, currentProcess, from, to, start, duration, view);
    }
}

