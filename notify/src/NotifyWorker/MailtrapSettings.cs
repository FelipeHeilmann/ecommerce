namespace NotifyWorker;

public class MailtrapSettings
{
    public string Host {  get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string SenderMail { get; set; } = string.Empty;
    public string Username {  get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int Port { get; set; }
}
