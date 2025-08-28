using System.Threading.Tasks;
using AuthenticationApp.ViewModels;

namespace AuthenticationApp.Services;

public interface INavigationService
{
    Task NavigateTo<T>() where T : ViewModelBase;
}