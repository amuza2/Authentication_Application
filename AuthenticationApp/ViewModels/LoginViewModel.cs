using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using AuthenticationApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AuthenticationApp.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly IAuthenticationService _authenticationService;

    [ObservableProperty] [Required] [EmailAddress] private string _email = string.Empty;
    
    [ObservableProperty] [Required] [MinLength(6)] private string _password = string.Empty;

    [ObservableProperty] private bool _isLoading;
    
    [ObservableProperty] private string _errorMessage = string.Empty;

    public LoginViewModel(INavigationService navigationService, IAuthenticationService authenticationService)
    {
        _navigationService = navigationService;
        _authenticationService = authenticationService;
    }

    public LoginViewModel() : this(null!, null!){ }

    [RelayCommand]
    private async Task NavigateToRegister()
    {
        await _navigationService.NavigateTo<RegisterViewModel>();
    }

    [RelayCommand]
    private async Task Login()
    {
        ValidateAllProperties();
        if (HasErrors)
            return;
        
        ErrorMessage = string.Empty;

        try
        {
            IsLoading = true;
            await Task.Delay(2000);
            
            var user = await _authenticationService.LoginUserAsync(Email, Password);
            if (user != null)
            {
                await _navigationService.NavigateTo<HomeViewModel>();
            }
            else
            {
                ErrorMessage = "Invalid Email or Password";
            }

        }
        catch (Exception e)
        {
            ErrorMessage = $"Login Failed: {e.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}