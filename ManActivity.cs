using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Hardware;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Provider;
using Android.Telephony;
using Android.Net.Wifi;
using Java.Net;
using Java.Lang;
using System.Net;
using Microsoft.WindowsAzure.MobileServices;
using SensorTest;

namespace SensorTest
{
	[Activity (Label = "@string/app_name", MainLauncher = true, Icon="@drawable/ic_launcher")]
	public class MainActivity : Activity 
	{
		/*
		 * Declaring Class Vairables 
		 */

		//Telephony Manager,wifi,battery manger objects
		TelephonyManager telephonyManager; 
		WifiManager wifiManager;
		BatteryManager batteryManager;

		//Text Views
		private TextView _IpTextView;
		private TextView _deviceIdTextView;

		//Text View for battery data
		private TextView batteryStatusTextView;
		private TextView batteryLevelTextView;
		private TextView batteryVoltageTextView;
		private TextView batteryTemperatureTextView;
		private TextView batteryTechnologyTextView;

		//Device informations
		string locationInfo;
		string device_ID;
		string device_Info; //Manufactures and model information

		//Battery informations
		string battery_status;
		string battery_Technology;
		double battery_level;
		string battery_temperature;
		string battery_voltage;

		public static MobileServiceClient MobileService;

		/*
		 * Create Method
		 * Fires when the activity is started
		 */
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate(bundle);
			//setting the main layout Main.xml
			SetContentView(Resource.Layout.Main);

			//Getting telephony and wifi information from the system service
			telephonyManager = (TelephonyManager)this.GetSystemService(Context.TelephonyService);
			wifiManager= (WifiManager)this.GetSystemService(Service.WifiService); 


			//Declaring the command ubttons
			var cmd_SensorStatus = FindViewById<Button> (Resource.Id.sensorStatusButton);
			var cmd_Calculate = FindViewById<Button> (Resource.Id.cmdCalculate);

			//Adding text view by id
			_deviceIdTextView = FindViewById<TextView> (Resource.Id.textDeviceID);

			batteryStatusTextView = FindViewById<TextView> (Resource.Id.textBatteryStatus);
			batteryLevelTextView = FindViewById<TextView> (Resource.Id.textBatteryLevel);
			batteryVoltageTextView = FindViewById<TextView> (Resource.Id.textBatteryVoltage);
			batteryTemperatureTextView = FindViewById<TextView> (Resource.Id.textBatteryTemperature);
			batteryTechnologyTextView = FindViewById<TextView> (Resource.Id.textBatteryTechnology);

			_IpTextView = FindViewById<TextView> (Resource.Id.textIP);
			//Adding text to the text views from device information
			_deviceIdTextView.Text = telephonyManager.DeviceId.ToString ();

			//Registering battery related information
			registerBatteryLevelReceiver();


			device_ID = _deviceIdTextView.Text;
			device_Info = Build.Manufacturer.ToString ()+ " " + Build.Model.ToString ();

			batteryStatusTextView.Text = battery_status;
			batteryLevelTextView.Text = battery_level.ToString();
			batteryVoltageTextView.Text = battery_voltage.ToString();
			batteryTemperatureTextView.Text = battery_temperature.ToString();
			batteryTechnologyTextView.Text = battery_Technology;

			//locationInfo = telephonyManager.CellLocation.ToString ();

			//Getting localip address
			int ip = wifiManager.ConnectionInfo.IpAddress;
			_IpTextView.Text = string.Format("{0}.{1}.{2}.{3}",(ip & 0xff),(ip >> 8 & 0xff),(ip >> 16 & 0xff),(ip >> 24 & 0xff));

			//Adding even listeners buttons
			cmd_SensorStatus.Click += (object sender, EventArgs e) => {sensorStatusButtonClick();};
			cmd_Calculate.Click += (object sender, EventArgs e) => {calculateButtonClick();};

		}
			

		/*
		* This method deal out with sensor status button click
		* It opens the Sensor Status Screen
		*/
		public void sensorStatusButtonClick(){
			var intent = new Intent(this, typeof(SensorStatusActivity));
			intent.PutExtra("device_Id",device_ID);
			intent.PutExtra("device_Info",device_Info);

			/*
			intent.PutExtra("batteryStatus", battery_status);
			intent.PutExtra("batteryLevel",battery_level);
			intent.PutExtra("batteryVoltage",battery_voltage);
			intent.PutExtra("batteryTemperature",battery_temperature);
			intent.PutExtra("batteryTechnology",battery_Technology);
			*/


			StartActivity(intent);
		}

		/*Fist it will check if the device is 
		 * registered to the service or not 
		 * if not registered then it will 
		 * register otherwise it would record
		 * the serviceThis will call the web 
		 * service back to recalculate the details */
		public void calculateButtonClick(){

		}

		
		/*
		 * Gets location wifi address from all the adapters,
		 * ether wifi or if directly converted to internet
		 */
		public string getLocalIpAddress() {
			try {
				for (var en = NetworkInterface.NetworkInterfaces; en.HasMoreElements;) {
					NetworkInterface intf = (NetworkInterface) en.NextElement();
					for (var enumIpAddr = intf.InetAddresses; enumIpAddr.HasMoreElements;) {
						InetAddress inetAddress = (InetAddress) enumIpAddr.NextElement();
						if (!inetAddress.IsLoopbackAddress) {
							//int ip =  inetAddress.GetHashCode();  //String.Format(inetAddress  //Formatter.formatIpAddress(inetAddress.hashCode());

							int ip = inetAddress.GetHashCode();

							string ipStr = string.Format("{0}.{1}.{2}.{3}",(ip & 0xff),(ip >> 8 & 0xff),(ip >> 16 & 0xff),(ip >> 24 & 0xff));

							return ipStr;
						}
					}
				}
			} catch (SocketException ex) {
				//Log.e(TAG, ex.toString());
			} 
			return null;
		}



		/*
	 	 * Battery Related Broadcast event registration 
	 	 * 
		 */
		private void registerBatteryLevelReceiver() {
			IntentFilter filter = new IntentFilter(Intent.ActionBatteryChanged);
			var battery = RegisterReceiver(null, filter);

			int level   = battery.GetIntExtra(BatteryManager.ExtraLevel, -1);
			int scale = battery.GetIntExtra(BatteryManager.ExtraScale, -1);
			double level_0_to_100 = System.Math.Floor (level * 100D / scale);

			battery_level = level_0_to_100;
			if (battery.GetIntExtra (BatteryManager.ExtraStatus, 0) == 2) {
				battery_status = "Charging";
			} else {
				battery_status = "Discharging";
			}

			battery_Technology = battery.GetStringExtra(BatteryManager.ExtraTechnology );
			double bat_Temp = (double) battery.GetIntExtra(BatteryManager.ExtraTemperature, 0);
			double bat_Volt = (double) battery.GetIntExtra(BatteryManager.ExtraVoltage,0);

			battery_temperature = (bat_Temp / 10).ToString() + '°'.ToString()+ "C";
			battery_voltage = (bat_Volt / 1000).ToString() +  "Volts";	
		}
			

	}
		
}



