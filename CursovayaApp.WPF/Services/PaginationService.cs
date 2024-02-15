using System.ComponentModel;
using System.Runtime.CompilerServices;
using CursovayaApp.WPF.Models.DbModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace CursovayaApp.WPF.Services
{
    public class PaginationService<T> : INotifyPropertyChanged  where T : TableBase
    {
        public readonly int TsAtPage;
        public int IndexT = 0;
        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged("Count");
            }
        }

        public PaginationService(int entitiesAtPage)
        {
            TsAtPage = entitiesAtPage;
        }
        

        public void InsertToUsers(ref ICollection<T> Tcollection, List<T> listT)
        {
            try
            {
                Tcollection ??= new ObservableCollection<T>();
                if (listT.Count <= TsAtPage)
                {
                    Tcollection?.Clear();
                    foreach (var item in listT)
                    {
                        Tcollection?.Add(item);
                    }
                    return;
                }


                var i = listT.Count - IndexT;
                if (i <= 0)
                {
                    if (TsAtPage <= IndexT)
                        IndexT -= TsAtPage;
                    else
                        IndexT = 0;
                    i = TsAtPage;
                }

                if (TsAtPage > i)
                {
                    Tcollection?.Clear();
                    for (int index = IndexT; index < listT.Count; index++)
                        Tcollection?.Add(listT[index]);
                }
                else
                {
                    Tcollection?.Clear();
                    for (int index = IndexT; index < IndexT+TsAtPage; index++)
                        Tcollection?.Add(listT[index]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void FirstT(ref ICollection<T>  TCollection, List<T> listT)
        {
            IndexT = 0;
            InsertToUsers(ref TCollection, listT);
        }

        public void BackT(ref ICollection<T> TCollection, List<T> listT)
        {
            if (IndexT < TsAtPage)
                IndexT = 0;
            else
                IndexT -= TsAtPage;
            InsertToUsers(ref TCollection, listT);
        }

        public void ForwardT(ref ICollection<T> TCollection, List<T> listT)
        {
            var i = listT.Count - IndexT;
            var canGoForward = i >= TsAtPage;
            if (canGoForward)
                IndexT += TsAtPage;
            InsertToUsers(ref TCollection, listT);
        }

        public void LastT(ref ICollection<T> TCollection, List<T> listT)
        {
            IndexT = listT.Count - TsAtPage;
            if (IndexT <= 0)
                IndexT = 0;
            else if (IndexT < TsAtPage)
                IndexT = listT.Count - IndexT;
            InsertToUsers(ref TCollection, listT);
        } 

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
