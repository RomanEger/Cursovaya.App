using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CursovayaApp.WPF.Commands;
using CursovayaApp.WPF.Services;

namespace CursovayaApp.WPF.ViewModels
{
    public class BooksViewModel : ViewModelBase
    {
        private RelayCommand _goBackCommand;

        public RelayCommand GoBackCommand
        {
            get
            {
                return _goBackCommand ??= new RelayCommand(obj =>
                {
                    if(MyFrame.frame.CanGoBack)
                        MyFrame.frame.GoBack();
                });
            }
        }

    }
}
