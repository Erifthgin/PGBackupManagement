using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PGBackupManagement.Models;

public class ServerModel : INotifyPropertyChanged
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5432;
    public string User { get; set; } = "postgres";
    public string DatabaseName { get; init; }

    public void Copy(ServerModel serverModel)
    {
        Host = serverModel.Host;
        Port = serverModel.Port;
        User = serverModel.User;
        OnPropertyChanged(nameof(Host));
        OnPropertyChanged(nameof(Port));
        OnPropertyChanged(nameof(User));
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}
