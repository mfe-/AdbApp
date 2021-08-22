using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using AndroidX.Core.App;
using Google.Android.Material.Snackbar;
using Prism;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdbApp.Droid
{
    [Activity(Theme = "@style/MainTheme",
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = AdbApp.Droid.Resource.Layout.Tabbar;
            ToolbarResource = AdbApp.Droid.Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App(new AndroidInitializer()));

            CheckAndRequestPermissions();
        }

        private void CheckAndRequestPermissions()
        {
            var permissions = new List<string>();
            //system permissions which cannot be requested are commented out
            CheckAndAddPermission(permissions, Manifest.Permission.Internet);
            //CheckAndAddPermission(permissions, Manifest.Permission.ClearAppCache);
            //CheckAndAddPermission(permissions, Manifest.Permission.ClearAppUserData);
            //CheckAndAddPermission(permissions, Manifest.Permission.DeleteCacheFiles);
            //CheckAndAddPermission(permissions, Manifest.Permission.DeletePackages);
            //CheckAndAddPermission(permissions, Manifest.Permission.Dump);
            CheckAndAddPermission(permissions, Manifest.Permission.GetPackageSize);
            //CheckAndAddPermission(permissions, Manifest.Permission.InstallPackages);
            CheckAndAddPermission(permissions, Manifest.Permission.KillBackgroundProcesses);
            //CheckAndAddPermission(permissions, Manifest.Permission.PackageUsageStats);
            CheckAndAddPermission(permissions, Manifest.Permission.ReadExternalStorage);
            //for reading logs - permission is restricted for firmware manufacturers 
            CheckAndAddPermission(permissions, Manifest.Permission.ReadLogs);
            //CheckAndAddPermission(permissions, Manifest.Permission.Reboot);
            CheckAndAddPermission(permissions, Manifest.Permission.ReceiveBootCompleted);
            //CheckAndAddPermission(permissions, Manifest.Permission.RestartPackages);
            //required for dumpsys - permission is restricted for firmware manufacturers 
            CheckAndAddPermission(permissions, Manifest.Permission.Dump);
            //CheckAndAddPermission(permissions, Manifest.Permission.SetDebugApp);
            //CheckAndAddPermission(permissions, Manifest.Permission.UpdateDeviceStats);
            CheckAndAddPermission(permissions, Manifest.Permission.WriteExternalStorage);
            //CheckAndAddPermission(permissions, Manifest.Permission.WriteSettings);
            CheckAndAddPermission(permissions, Manifest.Permission.WriteUserDictionary);
            if(permissions.Any())
            {
                ActivityCompat.RequestPermissions(this, permissions.ToArray(), 1000);
            }

        }

        private Permission CheckAndAddPermission(List<string> permissions, string permissionToCheck)
        {
            Permission permission = CheckSelfPermission(permissionToCheck);
            if (permission != (int)Permission.Granted)
            {
                permissions.Add(permissionToCheck);
            }

            return permission;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            switch (requestCode)
            {
                case 1000:
                    {
                        if (grantResults.Length > 0)
                        {
                            for (int i = 0; i < grantResults.Length; i++)
                            {
                                string permission = permissions[i];
                                Permission grantResult = grantResults[i];
                                if (Permission.Denied == grantResult)
                                {
                                    Toast.MakeText(this, $"Permission {grantResult} to {permission}", ToastLength.Short).Show();
                                }
                            }
                        }
                        return;
                    }
                default:
                    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                    break;
            }
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            containerRegistry.Register<IAdbService,AdbService>();
        }
    }
}

