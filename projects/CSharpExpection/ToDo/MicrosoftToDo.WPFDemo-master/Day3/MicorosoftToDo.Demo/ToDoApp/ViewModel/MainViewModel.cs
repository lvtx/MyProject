using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using ToDoApp.Models;

namespace ToDoApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            MenuModels = new ObservableCollection<MenuModel>();
            menuModels.Add(new MenuModel() { IconFont = "\xe635", Title = "�ҵ�һ��", BackColor = "#218868", Count = 5, Display = false });
            menuModels.Add(new MenuModel() { IconFont = "\xe6b6", Title = "��Ҫ", BackColor = "#EE3B3B", Count = 5 });
            menuModels.Add(new MenuModel() { IconFont = "\xe6e1", Title = "�Ѽƻ��ճ�", BackColor = "#218868", Count = 5 });
            menuModels.Add(new MenuModel() { IconFont = "\xe614", Title = "�ѷ������", BackColor = "#EE3B3B", Count = 5 });
            menuModels.Add(new MenuModel() { IconFont = "\xe755", Title = "����", BackColor = "#218868", Count = 5 });


            SelectedCommand = new RelayCommand<MenuModel>(t => Select(t));
        }

        private ObservableCollection<MenuModel> menuModels;

        public ObservableCollection<MenuModel> MenuModels
        {
            get { return menuModels; }
            set { menuModels = value; RaisePropertyChanged(); }
        }

        public RelayCommand<MenuModel> SelectedCommand { get; set; }

        private MenuModel menuModel;

        public MenuModel MenuModel
        {
            get { return menuModel; }
            set { menuModel = value; RaisePropertyChanged(); }
        }


        private void Select(MenuModel model)
        {
            MenuModel = model;
        }
    }
}