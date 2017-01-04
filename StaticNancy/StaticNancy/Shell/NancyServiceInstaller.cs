using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace StaticNancy.Shell
{
    [RunInstaller(true)]
    public class NancyServiceInstaller : Installer
    {
        ServiceProcessInstaller installer;
        ServiceInstaller serviceInstaller;
        IContainer components = null;

        public NancyServiceInstaller()
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            this.installer = new ServiceProcessInstaller();
            this.serviceInstaller = new ServiceInstaller();
            // 
            // installer
            // 
            this.installer.Account = ServiceAccount.NetworkService;
            // 
            // serviceInstaller
            // 
            this.serviceInstaller.DisplayName = "Nancy Service";
            this.serviceInstaller.ServiceName = "Nancy Service";
            this.serviceInstaller.Description = "A Nancy service that hosts static content held as embedded resources in assemblies";
            this.serviceInstaller.StartType = ServiceStartMode.Automatic;
            this.serviceInstaller.DelayedAutoStart = true;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new Installer[] { this.installer, this.serviceInstaller });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
