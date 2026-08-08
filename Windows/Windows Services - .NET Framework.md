Windows Services with .NET Framework

This code demonstrates two simple techniques for Windows services in .NET. First, it shows how to create a Console application which also can be installed as a Windows service – this way you can write a Windows service and easily test and debug it without installing it, and without using multiple projects. Second, it shows how to set the service name at install-time instead of compile-time.

Create the project as a Console application, rather than a Windows service. Then just add the following code, and organize your codebase as you wish.

```csharp
using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
 
namespace ConsoleApp1 {
    public partial class Service1 : ServiceBase {
        static void Main(string[] args) {
            var service = new Service1();
            if (Environment.UserInteractive) {
                try {
                    service.OnStart(args);
                } catch (Exception ex) {
                    Console.WriteLine(ex);
                } finally {
                    Console.WriteLine("DONE - Press <ENTER> to exit.");
                    service.OnStop();
                    Console.ReadLine();
                }
            } else {
                Run(service);
            }
        }
 
        protected override void OnStart(string[] args) { /* implementation */ }
        protected override void OnStop(){ /* implementation */ }
    }
 
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer {
        public ProjectInstaller() {
            string[] commandlineArgs = Environment.GetCommandLineArgs();
 
            string servicename;
            string servicedisplayname;
            ParseServiceNameSwitches(commandlineArgs, out servicename, out servicedisplayname);
 
            _serviceProcessInstaller = new ServiceProcessInstaller();
            _serviceInstaller = new ServiceInstaller();
 
            _serviceInstaller.ServiceName = servicename;
            _serviceInstaller.DisplayName = servicedisplayname;
 
            Installers.AddRange(new Installer[] { _serviceProcessInstaller, _serviceInstaller });
        }
 
        private ServiceProcessInstaller _serviceProcessInstaller;
        private ServiceInstaller _serviceInstaller;
 
        private void SetServicePropertiesFromCommandLine(ServiceInstaller serviceInstaller) {
            string[] commandlineArgs = Environment.GetCommandLineArgs();
 
            string servicename;
            string servicedisplayname;
            ParseServiceNameSwitches(commandlineArgs, out servicename, out servicedisplayname);
 
            serviceInstaller.ServiceName = servicename;
            serviceInstaller.DisplayName = servicedisplayname;
        }
 
        // http://www.runeibsen.dk/?p=153
        private void ParseServiceNameSwitches(string[] args, out string name, out string displayName) {
            var nameSwitch = args.FirstOrDefault(x => x.StartsWith("/servicename"));
            var displayNameSwitch = args.FirstOrDefault(x => x.StartsWith("/servicedisplayname"));
 
            if (nameSwitch == null)
                throw new ArgumentException("Argument 'servicename' is missing");
            if (displayNameSwitch == null)
                throw new ArgumentException("Argument 'servicedisplayname' is missing");
            if (!(nameSwitch.Contains('=') || nameSwitch.Split('=').Length < 2))
                throw new ArgumentNullException("The /servicename switch is malformed");
 
            if (!(displayNameSwitch.Contains('=') || displayNameSwitch.Split('=').Length < 2))
                throw new ArgumentNullException("The /servicedisplaynameswitch switch is malformed");
 
            name = nameSwitch.Split('=')[1];
            displayName = displayNameSwitch.Split('=')[1];
 
            name = name.Trim('"');
            displayName = displayName.Trim('"');
        }
    }
}
```

```shell
installutil -i /servicename=”my service” /servicedisplayname=”my display name” ConsoleApp1.exe
```