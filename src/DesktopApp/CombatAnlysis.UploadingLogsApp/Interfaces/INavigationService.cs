using CombatAnalysis.UploadingLogsApp.ViewModels;
using System.Threading.Tasks;

namespace CombatAnalysis.UploadingLogsApp.Interfaces;

public interface INavigationService
{
    Task NavigateTo<TViewModel>() where TViewModel : ViewModelBase;
}
