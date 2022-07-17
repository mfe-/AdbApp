# AdbApp

1. [Download from Play Store - AdbApp](https://play.google.com/store/apps/details?id=get.the.solution.AdbApp)
2. [dev.azure.com Pipeline](https://dev.azure.com/get-the-solution/get-the-solution/_build?definitionId=8)

## Hints

If compiling from source, you propably want to set a lower android api level in `AdbApp.Android/Properties/AndroidManifest.xml` like

`<uses-sdk android:minSdkVersion="yourlevel" android:targetSdkVersion="yourlevel" />`

Targeting newer android api versions are very restrictive regarding reading device information such as serialnumbers of the hardware. Newer api version forbid reading such information complete. You need than to target a lower api version.

For more information see for example https://developer.android.com/reference/android/os/Build#getSerial()
