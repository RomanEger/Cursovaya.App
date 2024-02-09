﻿using CursovayaApp.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Services;
using CursovayaApp.WPF.Views;
using CursovayaApp.WPF.Views.Windows;

namespace CursovayaApp.WPF.ViewModels
{
    public partial class BooksViewModel
    {
        private RelayCommand _goBackCommand;
        public RelayCommand GoBackCommand =>
            _goBackCommand ??= new RelayCommand(obj =>
                {
                    if (MyFrame.frame.CanGoBack)
                        if (_loggedUser.CurrentUser.RoleId != 1)
                        {
                            if (MessageBox.Show(
                                    "Это действие приведет к выходу из аккаунта. Вы уверены, что хотите продолжить?",
                                    "Выход",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                                MyFrame.frame.GoBack();
                        }
                        else
                            MyFrame.frame.GoBack();
                });

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand =>
            _saveCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        var localBooksList = DbClass.entities.Books.ToList();
                        foreach (var item in Books)
                        {
                            var publishingId = DbClass.entities.PublishingHouses.Where(x => x.Name == item.Publishing).Select(x => x.Id).FirstOrDefault();
                            var authorId = DbClass.entities.Authors.Where(x => x.FullName == item.AuthorFullName).Select(x => x.Id).FirstOrDefault();
                            var book = new Book()
                            {
                                Id = item.Id,
                                Quantity = item.Quantity,
                                Title = item.Title,
                                PublishingHouseId = publishingId,
                                AuthorId = authorId
                            };
                            DbClass.entities.Books.AddOrUpdate(book);

                        }

                        DbClass.entities.SaveChanges();
                        MessageBox.Show("Изменения успешно сохранены");
                    }
                    catch (Exception ex)
                    {
                        //string fileName = $@"C:\Users\error{DateTime.Now}.txt";
                        //FileStream fileStream = new FileStream(fileName, FileMode.Create);
                        //StreamWriter sw = new StreamWriter(fileStream);
                        //sw.Write(ex.Message);
                        //sw.Close();
                    }
                });

        private RelayCommand _addCommand;
        public RelayCommand AddCommand =>
            _addCommand ??= new RelayCommand(obj =>
                {
                    _addOrUpdateBooksView = new(this);
                    SelectedBook = new BookView(0);
                    Books.Add(SelectedBook);
                    _addOrUpdateBooksView.ShowDialog();
                });

        private RelayCommand _updateCommand;
        public RelayCommand UpdateCommand =>
            _updateCommand ??= new RelayCommand(obj =>
                {
                    _addOrUpdateBooksView = new(this);
                    _addOrUpdateBooksView.ShowDialog();
                });

        private RelayCommand _firstBooksCommand;
        public RelayCommand FirstBooksCommand =>
            _firstBooksCommand ??= new RelayCommand(obj => Pagination.FirstT(ref _books, _sortedListBooks));

        private RelayCommand _backBooksCommand;
        public RelayCommand BackBooksCommand =>
            _backBooksCommand ??= new RelayCommand(obj => Pagination.BackT(ref _books, _sortedListBooks));

        private RelayCommand _forwardBooksCommand;
        public RelayCommand ForwardBooksCommand =>
            _forwardBooksCommand ??= new RelayCommand(obj => Pagination.ForwardT(ref _books, _sortedListBooks));

        private RelayCommand _lastBooksCommand;
        public RelayCommand LastBooksCommand =>
            _lastBooksCommand ??= new RelayCommand(obj => Pagination.LastT(ref _books, _sortedListBooks));

        private RelayCommand _addOrUpdateBookCommand;
        public RelayCommand AddOrUpdateBookCommand =>
            _addOrUpdateBookCommand ??= new RelayCommand(obj =>
                {
                    if (Books.Any(x => x.Id == SelectedBook.Id))
                    {
                        if (!AuthorsForAdd.Contains(SelectedBook.AuthorFullName))
                        {
                            if (MessageBox.Show(
                                    "Данный автор еще не представлен в нашей библиотеке. Хотите его добавить?",
                                    "",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                var author = new Author()
                                {
                                    FullName = SelectedBook.AuthorFullName
                                };
                                _addOrUpdateAuthorsView = new(author, this, false);
                                _addOrUpdateAuthorsView.ShowDialog();
                            }
                            else return;
                        }

                        var aId = DbClass.entities.Authors.Where(x => x.FullName == SelectedBook.AuthorFullName).Select(x => x.Id).FirstOrDefault();
                        var pId = DbClass.entities.PublishingHouses.Where(x => x.Name == SelectedBook.Publishing).Select(x => x.Id).FirstOrDefault();

                        var book = DbClass.entities.Books.FirstOrDefault(x => x.Id == SelectedBook.Id) ?? new Book();
                        book.Id = SelectedBook.Id;
                        book.Quantity = SelectedBook.Quantity;
                        book.Title = SelectedBook.Title;
                        book.AuthorId = aId;
                        book.PublishingHouseId = pId;

                        DbClass.entities.Books.AddOrUpdate(book);
                        DbClass.entities.SaveChanges();
                        if (SelectedBook.ForAdd)
                        {
                            var rId = DbClass.entities.ReasonsReg.Where(x => x.Name == SelectedReason).Select(x => x.Id)
                                .FirstOrDefault();
                            var bId = DbClass.entities.Books.Where(x => x.AuthorId == book.AuthorId && x.Title == book.Title && x.PublishingHouseId == x.PublishingHouseId).Select(x => x.Id).FirstOrDefault();
                            var regBook = new RegBook()
                            {
                                BookId = bId,
                                ReasonId = rId,
                                DateOfReg = DateTime.Now,
                                UserId = _loggedUser.CurrentUser.Id,
                                RegQuantity = SelectedBook.QuantityToUpdate
                            };
                            DbClass.entities.RegBooks.Add(regBook);
                        }
                        else
                        {
                            var dId = DbClass.entities.ReasonsReg.Where(x => x.Name == SelectedReason).Select(x => x.Id)
                                .FirstOrDefault();
                            var bId = DbClass.entities.Books.Where(x => x.AuthorId == book.AuthorId && x.Title == book.Title && x.PublishingHouseId == x.PublishingHouseId).Select(x => x.Id).FirstOrDefault();
                            var deregBook = new DeregBook()
                            {
                                BookId = bId,
                                ReasonId = dId,
                                DateOfDereg = DateTime.Now,
                                UserId = _loggedUser.CurrentUser.Id,
                                DeregQuantity = SelectedBook.QuantityToUpdate
                            };
                            DbClass.entities.DeregBooks.Add(deregBook);

                        }
                        DbClass.entities.SaveChanges();
                        _addOrUpdateBooksView.Close();
                    }
                });

        private RelayCommand _addAuthorCommand;
        public RelayCommand AddAuthorCommand =>
            _addAuthorCommand ??= new RelayCommand(obj =>
                {
                    var author = DbClass.entities.Authors.FirstOrDefault(x => x.FullName == SelectedAuthor) ?? new Author();
                    _addOrUpdateAuthorsView = new(author, this, true);
                    _addOrUpdateAuthorsView.ShowDialog();
                });
            
        private RelayCommand _addOrUpdatePublishingCommand;
        public RelayCommand AddOrUpdatePublishingCommand =>
            _addOrUpdatePublishingCommand ??= new RelayCommand(obj =>
                {
                    var dialogResult = MessageBox.Show("Если Вы хотите добавить новое издательство -> Да\n" +
                        "Если Вы хотите изменить существующее издательство? -> Нет", "", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        var publishing = new PublishingHouse();
                        _addOrUpdatePublishingsView = new(publishing, this);
                        _addOrUpdatePublishingsView.ShowDialog();
                    }
                    else if(dialogResult == MessageBoxResult.No)
                    {
                        var window = new ChoosePublishing(this);
                        window.ShowDialog();
                    }
                });

        private RelayCommand _addPublishingCommand;
        public RelayCommand AddPublishingCommand =>
            _addPublishingCommand ??= new RelayCommand(obj =>
            {
                var publishing = DbClass.entities.PublishingHouses.Where(x => x.Name == SelectedPublishing).FirstOrDefault() ?? new PublishingHouse();
                _addOrUpdatePublishingsView = new(publishing, this);
                _addOrUpdatePublishingsView.ShowDialog();
            });

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand =>
           _cancelCommand ??= new RelayCommand(obj =>
                {
                    if (MessageBox.Show("Вы уверены, что хотите отменить все изменения?\nЭто действие удалит текущую запись!",
                        "Отмена изменений",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question) == MessageBoxResult.No) return;
                    
                    int i = Books.ToList().IndexOf(SelectedBook)-1;
                    Books.Remove(SelectedBook);
                    SelectedBook = Books.ElementAt(i);
                    _addOrUpdateBooksView?.Close();
                });
    }
}
