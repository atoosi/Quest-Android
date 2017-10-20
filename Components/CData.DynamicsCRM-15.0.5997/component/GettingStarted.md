## Getting Started ##

Follow the tutorial for the appropriate platform:

* [Xamarin.iOS](http://www.cdata.com/kb/articles/xamarin.rst#ios "Xamarin.iOS")
* [Xamarin.Android](http://www.cdata.com/kb/articles/xamarin.rst#android "Xamarin.Android")

See the demos included with the toolkit for examples. 

## Licensing ##

####Trial Versions#####

No special file or value is required for trial licensing. Only trial licenses require the use of the connection object's overloaded constructor that takes a Context object. At runtime the component will display a dialog informing the user a trial license is being used. If a Context object is not passed to the constructor the component will throw a licensing exception at run time.

#####Android#####

If your class extends the Activity class you can simply pass the "this" object to the constructor:

    public class MyClass extends Activity {
    ...
    //Within a method later in the class
    DynamicsCRMConnection conn = new DynamicsCRMConnection(this, connectionString);
    

#####iOS#####

If your class inherits from UIViewController you can simply pass the "this" object to the constructor:

    public partial class myViewController : UIViewController
    {
        DynamicsCRMConnection conn;

        public myViewController (IntPtr handle) : base (handle)
        {
            conn = new DynamicsCRMConnection(this, connectionString);
        }

####Standard (Royalty-Free) Licensing####

The providers are licensed through a .lic file that must be included in the project.

After purchasing a license visit [http://www.cdata.com/lic/?prod=RMR15X](http://www.cdata.com/lic/?prod=RMR15X) to obtain your .lic file.

To include your license in your project follow these steps:

1. Add "System.Data.CData.DynamicsCRM.lic" as an existing item to your project by right-clicking the project from Xamarin Studio and selecting Add -> Add Files.
2. In the properties window for "System.Data.CData.DynamicsCRM.lic" set the Build Action to Embedded Resource.

Once the file is included, you will need to rebuild your project and deploy as normal. 

## Documentation ##

* Developer Center: [www.cdata.com/kb/](http://www.cdata.com/kb/ "CData Online Knowledge Base")
* CData Xamarin Provider for Dynamics CRM Online Help: [http://www.cdata.com/kb/help/redirect.rst?help=RMR15-X](http://www.cdata.com/kb/help/redirect.rst?help=RMR15-X "CData Xamarin Provider for Dynamics CRM Online Help")

## Contact ##

* Email Support: [support@cdata.com](http://www.cdata.com/support/submit.aspx)
* Feature Requests: [www.cdata.com/forms/request/](http://www.cdata.com/forms/request/)

## Legal ##

* Terms &Conditions: [www.cdata.com/company/legal/terms.aspx](http://www.cdata.com/company/legal/terms.aspx)