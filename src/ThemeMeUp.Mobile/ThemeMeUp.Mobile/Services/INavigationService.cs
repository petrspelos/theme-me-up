using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThemeMeUp.Mobile.ViewModels;

namespace ThemeMeUp.Mobile.Services
{
    public interface INavigationService
    {
        Task OpenFilterPageAsync();
        Task OpenSettingsPageAsync();
    }
}
