using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using System.ComponentModel;
using System.Collections.Generic;
using PartContracts;
using System.Collections.ObjectModel;

namespace DownLoadOnDemandUseMEF
{
    [Export]
    public class MainPageViewModel : IPartImportsSatisfiedNotification,

        INotifyPropertyChanged
    {

        [ImportMany(typeof(IUIPart), AllowRecomposition = true)]
        public ObservableCollection <UserControl> UIParts { get; set; }


        public void OnImportsSatisfied()
        {
         
            OnPropertyChanged("UIParts");

        }


        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged(string property)
        {

            var handler = PropertyChanged;

            if (handler != null)

                PropertyChanged(this, new PropertyChangedEventArgs(

                    property));


        }

    }
}
