/*
* This class is the major class which filters the noisy data
* to make sure the sensor data calculated is the most stable data
*/

ï»¿using System;
using System.Collections.Generic;


namespace SensorTest
{
	public class GravitySensorData
	{
		/*
		 * Declaring class variables
		 */

		public double xComponent;
		public double yComponent;
		public double zComponent;
		public double pressureComponent;
		public double cumulative_gravity;


		public List<double> xList;
		public List<double> yList;
		public List<double> zList;
		public List<double> pressure;

		double xGMax, xGMin;
		double yGMax, yGMin;
		double zGMax, zGMin;

		double pMax, pMin;

		public int indexG;
		public int indexP;

		public Boolean recount;
		public Boolean collect;

		public int arrSize = 10;

		private double gravity_Value = 9.8067;

		public string deviceid { get; set; }
		public string deviceinfo { get; set; }

		public double latitutde { get; set; }
		public double longitude { get; set; }
		public double altitude { get; set; }

		public double error_gravity { get; set; }

		public double pressure_service { get; set; }
		public double error_pressure { get; set; }

		public double accelerometer_x { get; set; }
		public double accelerometer_y { get; set; }
		public double accelerometer_z { get; set; }

		//Magnetic Field
		public double magneticfeild_x { get; set; }
		public double magneticfeild_y { get; set; }
		public double magneticfeild_z { get; set; }

		//Gyroscope
		public double gyroscope_x { get; set; }
		public double gyroscope_y { get; set; }
		public double gyroscope_z { get; set; }

		//height
		public double height { get; set; }

		//humidity
		public double humidity { get; set; }

		//temperature
		public double temperature { get; set; }

		//Orientation Field
		public double azimuth_angle { get; set; }
		public double pitch_angle { get; set; }
		public double roll_angle { get; set; }


		//Battery status
		public string battery_status { get; set; }
		public double battery_level { get; set; }
		public double battery_voltage { get; set; }
		public double battery_temperature { get; set; }
		public string battery_technology { get; set; }

		/*
		 * Returns absolute gravity from cumulative gravity 
		 */
		public double getGravity(){
			cumulative_gravity = (double) Math.Pow( (float)(xComponent * xComponent + yComponent * yComponent + zComponent * zComponent), 0.5);
			return cumulative_gravity;

		}
		
		/*
		 * Returns difference in gravity from our g values to our recorded data
		 */
		public double getErrorGravity(){
			getGravity ();
			error_gravity = cumulative_gravity - gravity_Value;
			return error_gravity;
		}

		/*
		 * Returns difference in ressure from barometer data and weather api data
		 */
		public double getErrorPressure(){
			error_pressure = pressureComponent - pressure_service;

			if(double.IsNaN(error_pressure)){
				error_pressure = 0.0;
				pressureComponent = 0.0;
			}

			return error_pressure;
		}

		/*
		 * Default Constructor 
		 */
		public GravitySensorData ()
		{
			xList = new List<double> ();
			yList = new List<double> ();
			zList = new List<double> ();

			pressure = new List<double> ();

			double xGMax= 0.0f, xGMin= 0.0f;
			double yGMax= 0.0f, yGMin= 0.0f;
			double zGMax= 0.0f, zGMin= 0.0f;

			double pMax = 0.0f, pMin = 0.0f;

			indexG = 0;
			indexP = 0;

			recount = false;
			collect = false;
		}

		/*
		 * Clearing out negative values in gravity x,y,z axis
		 */
		public void absolutionAndAdd(double x1, double y1, double z1){
			if (x1 < 0.0) {
				x1 = x1 * -1.0;
			}
			if (y1 < 0.0) {
				y1 = y1 * -1.0;
			}
			if (z1 < 0.0) {
				z1 = z1 * -1.0;
			}

			addGravity (x1, y1, z1);
		}

		/*
		 * Adding gravity information like queue
		 */ 
		public void addGravity(double x1, double y1, double z1){

			//Setting minimum values
			if (indexG == 0) {
				xGMin = x1;
				yGMin = y1;
				zGMin = z1;
			}

			if (x1 > xGMax) {
				xGMax = x1;
			}

			if (y1 > yGMax) {
				yGMax = y1;
			}

			if (z1 > zGMax) {
				zGMax = z1;
			}

			if (x1 < xGMin) {
				xGMin = x1;
			}

			if (y1 < yGMin) {
				yGMin = y1;
			}

			if (z1 < zGMin) {
				zGMin = z1;			
			}


			if (xList.Count >= arrSize) {
				xList [indexG % arrSize] = x1;
				yList [indexG % arrSize] = y1;
				zList [indexG % arrSize] = z1;

			} else {
				xList.Add(x1);
				yList.Add(y1);
				zList.Add(z1);
			}
			
			++indexG;
		}

		/*
		 * Adding pressure information like a queue
		 */ 
		public void addPressure(double p1){

			if (pressure.Count >= arrSize) {
				pressure [indexP % arrSize] = p1;
			} else {
				pressure.Add(p1);
			}
			++indexP;
		}

		/*
		 * Clearing the class like a clean slate
		 */
		public void clear(){
			xList.Clear ();
			yList.Clear ();
			zList.Clear ();
			pressure.Clear ();

			double xGMax= 0.0, xGMin= 0.0;
			double yGMax= 0.0, yGMin= 0.0;
			double zGMax= 0.0, zGMin= 0.0;

			double pMax = 0.0, pMin = 0.0;

			indexG = 0;
			indexP = 0;

			xComponent = 0.0;
			yComponent= 0.0;
			zComponent= 0.0;
			pressureComponent= 0.0;

			recount = false;
		}

		/*
		 * Getting average gravity and pressure component
		 */
		public void getGravityPressure(){

			double dx=0.0, dy=0.0, dz=0.0, pc=0.0;

			foreach(double d in xList ){
				dx = dx + d; 
			}
			xComponent = dx / xList.Count;

			foreach(double d in yList ){
				dy = dy + d; 
			}

			yComponent = dy / yList.Count;

			foreach(double d in zList ){
				dz = dz + d; 
			}
			zComponent = dz / zList.Count;

			foreach (double d in pressure) {
				pc = pc + d;
			}
			pressureComponent = pc / pressure.Count;

			//Calculating error gravity
			getErrorGravity ();

			//Calculating error pressure
			getErrorPressure ();
		}
			

		/*
		 * Filtering noise from barometer and pressure data
		 */ 
		public void calculateBarometerGravityData(){

			//Values are stable
			if ( ( ((xGMax - xGMin) <= 0.15) && ((yGMax - yGMin) <= 0.15) 
				&& ((zGMax - zGMin) <= 0.15)) && ((pMax - pMin) <= 0.5) 
				&& (xList.Count >=arrSize) &&(yList.Count >=arrSize) &&(zList.Count >=arrSize) /* &&  (pressure.Count >= arrSize) */ )   {
			
				recount = false;

				//Record the gravity data
				//Record the pressure data
				getGravityPressure ();

			} else {
				//Recount
				recount = true;
			}
		}

	}
}

