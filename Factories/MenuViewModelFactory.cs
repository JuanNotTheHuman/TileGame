using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.ViewModels;

namespace TileGame.Factories
{
    public class MenuViewModelFactory
    {
        private readonly INavigationService _navigationService;

        public MenuViewModelFactory(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public MenuViewModel CreateMenuViewModel()
        {
            return new MenuViewModel(_navigationService);
        }
    }

}
