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
                OnPropertyChanged();
            }
        }

        public PaginationService(int entitiesAtPage)
        {
            TsAtPage = entitiesAtPage;
        }
        

        public void InsertToUsers(ref ICollection<T> collection, List<T> listT)
        {
            try
            {
                collection ??= new ObservableCollection<T>();
                if (listT.Count <= TsAtPage)
                {
                    collection?.Clear();
                    foreach (var item in listT)
                    {
                        collection?.Add(item);
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
                    collection?.Clear();
                    for (int index = IndexT; index < listT.Count; index++)
                        collection?.Add(listT[index]);
                }
                else
                {
                    collection?.Clear();
                    for (int index = IndexT; index < IndexT+TsAtPage; index++)
                        collection?.Add(listT[index]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void FirstT(ref ICollection<T>  collection, List<T> listT)
        {
            IndexT = 0;
            InsertToUsers(ref collection, listT);
        }

        public void BackT(ref ICollection<T> collection, List<T> listT)
        {
            if (IndexT < TsAtPage)
                IndexT = 0;
            else
                IndexT -= TsAtPage;
            InsertToUsers(ref collection, listT);
        }

        public void ForwardT(ref ICollection<T> collection, List<T> listT)
        {
            var i = listT.Count - IndexT;
            var canGoForward = i >= TsAtPage;
            if (canGoForward)
                IndexT += TsAtPage;
            InsertToUsers(ref collection, listT);
        }

        public void LastT(ref ICollection<T> collection, List<T> listT)
        {
            IndexT = listT.Count - TsAtPage;
            if (IndexT <= 0)
                IndexT = 0;
            else if (IndexT < TsAtPage)
                IndexT = listT.Count - IndexT;
            InsertToUsers(ref collection, listT);
        } 

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
