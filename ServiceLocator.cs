/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:tdidf.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using TFIDF.Model;
using TFIDF.Service;
using TFIDF.ViewModel;

namespace TFIDF
{
    public class ServiceLocator
    {
        static ServiceLocator()
        {
            Microsoft.Practices.ServiceLocation.
                ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<IStemmerService, StemmerService>();
            SimpleIoc.Default.Register<IDocumentsReaderService, DocumentsReaderService>();
            SimpleIoc.Default.Register<IKeywordsService, KeywordsService>();
            SimpleIoc.Default.Register<IFileDialogService, FileDialogService>();
            SimpleIoc.Default.Register<IWordNetService, WordNetService>();

            /*
            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }
             */

            SimpleIoc.Default.Register<MainViewModel>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.
                    ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public static void Cleanup()
        {
        }
    }
}