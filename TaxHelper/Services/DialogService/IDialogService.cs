namespace TaxHelper.Services.DialogService;

public interface IDialogService
{
    void ShowError(string message, string title = "Ошибка");
    void ShowWarning(string message, string title = "Предупреждение");
    void ShowInfo(string message, string title = "Информация");
    string? OpenFileDialog(string filter, string title);
}
