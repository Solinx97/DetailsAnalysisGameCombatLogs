using CombatAnalysis.UploadingLogsApp.ViewModels.Base;
using System.Threading.Tasks;

namespace CombatAnalysis.UploadingLogsApp.Interfaces;

public interface INavigationService
{
    Task NavigateTo<TViewModel>() where TViewModel : ViewModelBase;
}
