using System.Windows.Controls;
namespace TileGame.Services
{
    public interface INavigationService
    {
        void NavigateToPage(Page page);
    }
    public class NavigationService : INavigationService
    {
        private readonly Frame _frame;

        public NavigationService(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateToPage(Page page)
        {
            _frame.Navigate(page);
        }
    }
}

