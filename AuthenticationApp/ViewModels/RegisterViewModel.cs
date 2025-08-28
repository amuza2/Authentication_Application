using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AuthenticationApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AuthenticationApp.ViewModels;

public partial class RegisterViewModel : ViewModelBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly INavigationService _navigationService;
    
    [ObservableProperty] [Required] [StringLength(50)] private string _firstName = string.Empty;
    [ObservableProperty] [Required] [StringLength(50)] private string _lastName = string.Empty;
    [ObservableProperty] [Required] [EmailAddress] private string _email = string.Empty;
    [ObservableProperty] [Required] [MinLength(6)] private string _password = string.Empty;
    [ObservableProperty] [Required] [MinLength(6)] private string _confirmPassword = string.Empty;
    
    [ObservableProperty]  private string _errorMessage = string.Empty;
    [ObservableProperty]  private bool _isLoading;
    
    public RegisterViewModel(INavigationService navigationService, IAuthenticationService authenticationService)
    {
        _navigationService = navigationService;
        _authenticationService = authenticationService;
    }

    public RegisterViewModel() : this(null!, null!){ }

    [RelayCommand]
    private async Task NavigateToLogin()
    {
        await _navigationService.NavigateTo<LoginViewModel>();
    }

    [RelayCommand]
    private async Task Register()
    {
        ValidateAllProperties();
        if (HasErrors)
            return;

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Password and confirm password do not match.";
            return;
        }
        
        ErrorMessage = string.Empty;

        try
        {
            IsLoading = true;

            await Task.Delay(2000);

            if (await _authenticationService.UserExistsAsync(Email))
            {
                ErrorMessage = "A user with this email already exists.";
                return;
            }

            var success = await _authenticationService.RegisterUserAsync(FirstName, LastName, Email, Password);
            if (success)
            {
                await _navigationService.NavigateTo<HomeViewModel>();
            }
        }
        catch (Exception e)
        {
            ErrorMessage = $"Registration failed: {e.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}