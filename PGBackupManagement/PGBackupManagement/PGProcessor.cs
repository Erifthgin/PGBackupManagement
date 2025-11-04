using PGBackupManagement.Models;
using System.Diagnostics;
using System.Windows;

namespace PGBackupManagement;

internal static class PGProcessor
{
    public static async Task RunBackup(SettingModel settingModel, string externalServerPassword, string internalServerPassword)
    {
        if (!await StartProcess(settingModel.ExternalServer, externalServerPassword,
            settingModel.ExternalServer.DatabaseName, settingModel.PostgresPath, true))
        {
            return;
        }

        if (await StartProcess(settingModel.InternalServer, internalServerPassword,
            settingModel.ExternalServer.DatabaseName, settingModel.PostgresPath))
        {
            MessageBox.Show("База успешно восстановлена!");
        }
    }

    private static async Task<bool> StartProcess(ServerModel serverModel, 
        string password, 
        string backupFilePath, 
        string pgPath, 
        bool isBackup = false, 
        CancellationToken cancellationToken = default)
    {
        var fileName = isBackup ? "pg_dump" : "pg_restore";
        var processInfo = new ProcessStartInfo
        {
            FileName = pgPath + "\\" + fileName,
            Arguments = isBackup
                ? GetBackupCommand(serverModel, backupFilePath)
                : GetRestoreCommand(serverModel, backupFilePath),
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        processInfo.EnvironmentVariables["PGPASSWORD"] = password;

        using var process = Process.Start(processInfo);
        var error = await process.StandardError.ReadToEndAsync(cancellationToken);
        await process.WaitForExitAsync(cancellationToken);

        if (process.ExitCode != 0)
        {
            MessageBox.Show($"При выполнении команды '{processInfo.Arguments}' возникла ошибка: {error}");
            return false;
        }

        return true;
    }

    private static string GetRestoreCommand(ServerModel serverModel, string filePath)
    {
        return $"-h {serverModel.Host} -p {serverModel.Port} -U {serverModel.User} -d {serverModel.DatabaseName} \"{filePath}.bak\"";
    }

    private static string GetBackupCommand(ServerModel serverModel, string filePath)
    {
        return $"-h {serverModel.Host} -p {serverModel.Port} -U {serverModel.User} -c -C -Fc -f \"{filePath}.bak\" {serverModel.DatabaseName}";
    }
}
