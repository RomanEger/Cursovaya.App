using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CursovayaApp.WPF.Repository;
using CursovayaApp.WPF.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CursovayaApp.WPF.Models.DbModels
{
    [NotMapped]
    public class BookView : TableBase, INotifyPropertyChanged
    {
        private readonly IGenericRepository<ReasonReg> _repositoryReasonReg;
        private readonly IGenericRepository<ReasonDereg> _repositoryReasonDereg;

        public BookView(int quantity)
        {
            OldQuantity = quantity;
            _repositoryReasonReg = new GenericRepository<ReasonReg>(new ApplicationContext());
            _repositoryReasonDereg = new GenericRepository<ReasonDereg>(new ApplicationContext());
        }

        private int _id;

        public override int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private string _title;
        public string Title
        {
            get => _title; 
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private string _authorFullName;

        public string AuthorFullName
        {
            get => _authorFullName;
            set
            {
                _authorFullName = value;
                OnPropertyChanged();
            }
        }

        private int _oldQuantity;
        public int OldQuantity
        {
            get => _oldQuantity;
            init => _oldQuantity = value;
        }

        private int _quantity;

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity > value)
                {
                    QuantityToUpdate = _oldQuantity - value;
                    ForAdd = false;
                }
                else
                {
                    QuantityToUpdate = value - _oldQuantity;
                    ForAdd = true;
                }
                _quantity = value;
                OnPropertyChanged();
            }
        }

        private string _publishing;

        public string Publishing
        {
            get=>_publishing;
            set
            {
                _publishing = value;
                OnPropertyChanged();
            }
        }

        private int _quantityToUpdate;

        public int QuantityToUpdate
        {
            get => _quantityToUpdate;
            set
            {
                _quantityToUpdate = value;
                OnPropertyChanged();
            }
        }

        private bool _forAdd = false;

        public bool ForAdd
        {
            get => _forAdd;
            set
            {
                _forAdd = value;
                OnPropertyChanged();
            }
        }

        public void GetReasons()
        {
            try
            {
                if (ForAdd)
                {
                    var l = _repositoryReasonReg.GetAll().Select(x => x.Name).ToList();
                    ListReasons = new ObservableCollection<string>(l);
                }
                else
                {
                    var l = _repositoryReasonDereg.GetAll().Select(x => x.Name).ToList();
                    ListReasons = new ObservableCollection<string>(l);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private IEnumerable<string> _listReasons = new List<string>();
        public IEnumerable<string> ListReasons
        {
            get => _listReasons;
            set
            {
                _listReasons = value;
                OnPropertyChanged();
            }
        }
    }
}
