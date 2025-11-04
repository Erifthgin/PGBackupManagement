namespace PGBackupManagement.Models;

public class SettingModel
{
    public string PostgresPath { get; init; } = "C:\\Program Files\\PostgreSQL\\17\\bin";
    public ServerModel ExternalServer { get; init; } = new();
    public ServerModel InternalServer { get; init; } = new();
    public bool CopyServer { get; init; } = false;
}
