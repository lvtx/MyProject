using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using PartContracts;

namespace DownLoadOnDemandUseMEF
{
    public partial class MainPage : UserControl, IPartImportsSatisfiedNotification
    {
        public MainPage()
        {
            InitializeComponent();

            var deployCata = new DeploymentCatalog("SilverlightSatelliteApp1.xap");
            App.aggCata.Catalogs.Add(deployCata);
            deployCata.DownloadCompleted += deployCata_DownloadCompleted;
            deployCata.DownloadAsync();
            CompositionInitializer.SatisfyImports(this);
        }

        void deployCata_DownloadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
                MessageBox.Show(e.Error.Message);
        }
        [ImportMany(typeof(IUIPart), AllowRecomposition = true)]
        public List<UserControl> UIParts { get; set; }

        /// <summary>
        /// 当组件组合时，更新可视化用户界面
        /// </summary>
        public void OnImportsSatisfied()
        {
            ControlContainer.Items.Clear();
            foreach (UserControl ctl in UIParts)
                ControlContainer.Items.Add(ctl);

        }
    }
}
