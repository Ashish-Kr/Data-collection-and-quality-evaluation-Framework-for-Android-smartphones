using System;
using Newtonsoft.Json;

namespace SensorTest
{
	public class DeviceData
	{

		public string Id { get; set; }

		//Device
		[JsonProperty(PropertyName = "deviceid")]
		public string deviceid { get; set; }
		[JsonProperty(PropertyName = "deviceinfo")]
		public string deviceinfo { get; set; }

		//GPS
		[JsonProperty(PropertyName = "latitutde")]
		public double latitutde { get; set; }
		[JsonProperty(PropertyName = "longitude")]
		public double longitude { get; set; }
		[JsonProperty(PropertyName = "altitude")]
		public double altitude { get; set; }

		//Gravity
		[JsonProperty(PropertyName = "gravity_cumulative")]
		public double gravity_cumulative { get; set; }
		[JsonProperty(PropertyName = "gravity_x")]
		public double gravity_x  { get; set; }
		[JsonProperty(PropertyName = "gravity_y")]
		public double gravity_y { get; set; }
		[JsonProperty(PropertyName = "gravity_z")]
		public double gravity_z { get; set; }
		[JsonProperty(PropertyName = "error_gravity")]
		public double error_gravity { get; set; }

		//Pressure
		[JsonProperty(PropertyName = "pressure_device")]
		public double pressure_device { get; set; }
		[JsonProperty(PropertyName = "pressure_service")]
		public double pressure_service { get; set; }
		[JsonProperty(PropertyName = "error_pressure")]
		public double error_pressure { get; set; }
		[JsonProperty(PropertyName = "accelerometer_x")]
		public double accelerometer_x { get; set; }
		[JsonProperty(PropertyName = "accelerometer_y")]
		public double accelerometer_y { get; set; }
		[JsonProperty(PropertyName = "accelerometer_z")]
		public double accelerometer_z { get; set; }

		//Magnetic Field
		[JsonProperty(PropertyName = "magneticfeild_x")]
		public double magneticfeild_x { get; set; }
		[JsonProperty(PropertyName = "magneticfeild_y")]
		public double magneticfeild_y { get; set; }
		[JsonProperty(PropertyName = "magneticfeild_z")]
		public double magneticfeild_z { get; set; }

		//Gyroscope
		[JsonProperty(PropertyName = "gyroscope_x")]
		public double gyroscope_x { get; set; }
		[JsonProperty(PropertyName = "gyroscope_y")]
		public double gyroscope_y { get; set; }
		[JsonProperty(PropertyName = "gyroscope_z")]
		public double gyroscope_z { get; set; }

		//height
		[JsonProperty(PropertyName = "height")]
		public double height { get; set; }

		//humidity
		[JsonProperty(PropertyName = "humidity")]
		public double humidity { get; set; }

		//temperature
		[JsonProperty(PropertyName = "temperature")]
		public double temperature { get; set; }

		//Orientation
		[JsonProperty(PropertyName = "Azimuth")]
		public double azimuth_angle { get; set; }
		[JsonProperty(PropertyName = "Pitch")]
		public double pitch_angle { get; set; }
		[JsonProperty(PropertyName = "Roll")]
		public double roll_angle { get; set; }

		//Battery status
		[JsonProperty(PropertyName = "battery_status")]
		public string battery_status { get; set; }
		[JsonProperty(PropertyName = "battery_level")]
		public double battery_level { get; set; }
		[JsonProperty(PropertyName = "battery_voltage")]
		public double battery_voltage { get; set; }
		[JsonProperty(PropertyName = "battery_temperature")]
		public double battery_temperature { get; set; }
		[JsonProperty(PropertyName = "battery_technology")]
		public string battery_technology { get; set; }

		//TimeStamp
		[JsonProperty(PropertyName = "timestamp")]
		public DateTime timestamp { get; set; }

		//completed property
		[JsonProperty(PropertyName = "complete")]
		public bool complete { get; set; }

		/*
		 * Device wrapper to wrap the information into
		 * json object
		 */
		public class DeviceDataWrapper : Java.Lang.Object
		{
			public DeviceData DeviceDataItem { get; private set; }

			public DeviceDataWrapper(DeviceData item)
			{
				this.DeviceDataItem = item;
			}
		}
		
		/*
		 * Adding gravity and pressure information to json file
		 */
		public void AddGravitySensorData(GravitySensorData gsData){
		
			gravity_x = gsData.xComponent;
			gravity_y = gsData.yComponent;
			gravity_z = gsData.zComponent;

			//pressureData = gsData.pressureComponent;
		}
			
	}


}

