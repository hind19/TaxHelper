using System.Windows;
using Microsoft.Win32;

namespace TaxHelper.Services.DialogService;

public class DialogService : IDialogService
{
    public void ShowError(string message, string title = "Ошибка")
    {
        ShowMwessage(message, title,MessageBoxImage.Error);
    }

    public void ShowWarning(string message, string title = "Предупреждение")
    {
        ShowMwessage(message, title,MessageBoxImage.Warning);
    }

    public void ShowInfo(string message, string title = "Информация")
    {
        ShowMwessage(message, title,MessageBoxImage.Information);
    }

    public string? OpenFileDialog(string filter, string title)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = filter,
            Title = title
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                return openFileDialog.FileName;
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при получении курса валюты: {ex.Message}");
            }
        }
        return string.Empty;
    }

    private void ShowMwessage(string message, string title, MessageBoxImage icon)
    {
        MessageBox.Show(message, title, MessageBoxButton.OK, icon);
    }
}