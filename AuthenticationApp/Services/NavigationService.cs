using System;
using System.Threading.Tasks;
using AuthenticationApp.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationApp.Services;

public class NavigationService : INavigationService
{
    private MainWindowViewModel? _mainWindowViewModel;
    private readonly IServiceProvider _serviceProperty;

    public NavigationService(IServiceProvider serviceProperty)
    {
        _serviceProperty = serviceProperty;
    }

    public Task NavigateTo<T>() where T : ViewModelBase
    {
        if(_mainWindowViewModel == null == null)
            return Task.CompletedTask;

        var vm = _serviceProperty.GetRequiredService<T>();
        _mainWindowViewModel.CurrentViewModel = vm;
        return Task.CompletedTask;
    }

    public void SetMainViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
    }
}