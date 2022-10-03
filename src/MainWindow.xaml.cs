using Microsoft.Extensions.Configuration;
using NonInvasiveKeyboardHookLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RemoteDesktopLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private xml.ServerCollection mServers;
        private xml.Server mSelectedServer = null;

        

        private ObservableCollection<xml.Server> mDatasource = new ObservableCollection<xml.Server>();
        private KeyboardHookManager keyboardHookManager = new KeyboardHookManager();

        public MainWindow()
        {
            this.WindowState = WindowState.Minimized;

            //hotkey : '<>' key
            keyboardHookManager.RegisterHotkey(
                NonInvasiveKeyboardHookLibrary.ModifierKeys.Control
                | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift,
                0xE2, () =>
            {
                Dispatcher.Invoke(new Action(() => {
                    this.WindowState = WindowState.Normal;
                    //tbFilter.Focus();
                    }));
                
            });
            keyboardHookManager.Start();

            InitializeComponent();

            
            
            

            this.Topmost = true;

            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            string ServersListPath = configuration["ServersListPath"];

            mServers = xml.ServerCollection.Deserialize(ServersListPath);

            foreach (var server in mServers)
            {
                mDatasource.Add(server);
            }

            dgServers.ItemsSource = mDatasource;
        }

        protected override void OnClosed(EventArgs e)
        {
            keyboardHookManager.Stop();
            base.OnClosed(e);
        }


        private void Log(string txt)
        {
            Dispatcher.Invoke(new Action(() => tbLog.AppendText(txt + Environment.NewLine)));

        }

        private void dgServers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count>0)
            {
                Log("select " + ((xml.Server)e.AddedItems[0]).Host);
            }
        }

        private void dgServers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var s = dgServers.SelectedItem as xml.Server;
                if (s != null)
                {
                    mSelectedServer = s;
                    LaunchRemoteDesktop(s.Host);
                }  
            }
            catch (Exception exc)
            {
                Log(exc.ToString());
            }
        }

        private void LaunchRemoteDesktop(string host)
        {
            var process = new Process();
            process.StartInfo.FileName = @"mstsc";
            process.StartInfo.Arguments = $"/v:{host} /prompt";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();

            this.WindowState = WindowState.Minimized;
        }

        private void tbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                mDatasource.Clear();
                var filters = tbFilter.Text.Split(' ');
                foreach (var server in mServers)
                {
                    bool test = true;
                    foreach (var filter in filters)
                    {
                        if (string.IsNullOrEmpty(filter)) continue;

                        test = test && server.Description.Contains(filter, StringComparison.InvariantCultureIgnoreCase)
                        || server.Name.Contains(filter, StringComparison.InvariantCultureIgnoreCase)
                        || server.Host.Contains(filter, StringComparison.InvariantCultureIgnoreCase)
                        || server.IP.Contains(filter, StringComparison.InvariantCultureIgnoreCase);
                    }

                    if (test)
                    {
                        mDatasource.Add(server);                       
                    }
                }

                if (mDatasource.Count == 0)
                {
                    dgServers.SelectedItem = null;
                    mSelectedServer = null;
                }
                else if (mSelectedServer == null)
                {
                    dgServers.SelectedIndex = 0;
                    mSelectedServer = dgServers.SelectedItem as xml.Server;
                }
                else
                {
                    if (mDatasource.Contains(mSelectedServer))
                    {
                        dgServers.SelectedItem = mSelectedServer;
                    } else
                    {
                        dgServers.SelectedIndex = 0;
                        mSelectedServer = dgServers.SelectedItem as xml.Server;
                    }
                }
            }
            catch(Exception exc)
            {
                Log(exc.ToString());
            }
        }

        private void Window_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //hack to make the windows on top even if some application is in full screen mode
            //var window = (Window)sender;
            //window.Topmost = true;
            
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            tbFilter.Focus();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Down || e.Key == Key.Up)
                {
                    int i = -1;
                    if (dgServers.SelectedItem == null)
                    {
                        i = e.Key == Key.Down ? 0 : dgServers.Items.Count - 1;
                    }
                    else
                    {
                        i = dgServers.SelectedIndex + (e.Key == Key.Down ? 1 : -1);
                    }

                    if (i >= 0 && i < dgServers.Items.Count)
                    {
                        dgServers.SelectedIndex = i;
                        mSelectedServer = dgServers.SelectedItem as xml.Server;
                    }

                    e.Handled = true;
                }
                else if (e.Key == Key.Enter)
                {
                    if (dgServers.SelectedItem == null) dgServers.SelectedIndex = 0;

                    var selectedServer = dgServers.SelectedItem as xml.Server;
                    LaunchRemoteDesktop(selectedServer.Host);

                    Log($"connect to {selectedServer.Host}");

                    e.Handled = true;
                }
            }
            catch (Exception exc)
            {
                Log(exc.ToString());
            }
        }
        
    }
}
