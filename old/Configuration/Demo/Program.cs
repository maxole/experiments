using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ConfiguratorManager;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // задаем нужную культуру
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");

                var manager = new FileConfigurationManager();
                manager.Configure();

                var form = new PropertyForm();
                foreach (var collection in manager.GetDecorators().Select(e => e.Value))
                {
                    form.AddDecorator(collection);                    
                }
                form.SaveAction = () =>
                {
                    try
                    {
                        manager.Save();
                        MessageBox.Show("ok");
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("i had some error: " + exception);
                    }
                };
                form.ShowDialog();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }            
        }
    }
}
