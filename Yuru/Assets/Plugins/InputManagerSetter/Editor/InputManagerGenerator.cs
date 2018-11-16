using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace InputManagerSetter.Editor {
	public class InputManagerSetter {
		[MenuItem("Util/Set InputManager")]
		public static void SetInputManager() {
			InputManagerGenerator inputManagerGenerator = new InputManagerGenerator();
			inputManagerGenerator.Clear(); 
			StreamReader sr = new StreamReader(Application.dataPath + "/Plugins/InputManagerSetter/Text/InputSetting.csv");
			while (sr.Peek () > -1) {
				string line = sr.ReadLine ();
				string[] csvDatas = line.Split(',');
				var joynum = csvDatas[0].Contains("1") ? 1 : csvDatas[0].Contains("2") ? 2 : 0;
				var name = csvDatas[0].Split('!').First();
				if (csvDatas[1] != "axis") {
					string xAxis = "joystick " + (joynum != 0 ? joynum + " " : "");
					switch (csvDatas[1]) {
						case "A":
							xAxis += "button 0";
							break;
						case "B":
							xAxis += "button 1";
							break;
						case "X":
							xAxis += "button 2";
							break;
						case "Y":
							xAxis += "button 3";
							break;
						case "L1":
							xAxis += "button 4";
							break;
						case "R1":
							xAxis += "button 5";
							break;
						case "L2":
							//3rdaxis>0
							break;
						case "R2":
							//3rdaxis<0
							break;
						case "Start":
							xAxis += "button 7";
							break;
					}
					if (csvDatas[1] == "L2" || csvDatas[1] == "R2") {
						inputManagerGenerator.AddAxis(InputAxis.PadAxisSetting(name, 3,joynum));
					} else {
						inputManagerGenerator.AddAxis(InputAxis.ButtonSetting(name, xAxis,joynum));
					}

					if (csvDatas[2] == "..") inputManagerGenerator.AddAxis(InputAxis.KeySetting(name, ","));
					else if (csvDatas[2] != "axis") inputManagerGenerator.AddAxis(InputAxis.KeySetting(name, csvDatas[2]));
					else {
						int num = csvDatas[3].Contains("X") ? 1 : csvDatas[3].Contains("Y") ? 2 : int.Parse(csvDatas[3].Substring(0, 1));
						inputManagerGenerator.AddAxis(InputAxis.MouseAxisSetting(name, num));
					}
				} else {
					int num = csvDatas[2].Contains("X") ? 1 : csvDatas[2].Contains("Y") ? 2 : int.Parse(csvDatas[2].Substring(0, 1));
					inputManagerGenerator.AddAxis(InputAxis.PadAxisSetting(name, csvDatas[2].Contains("!"), num, joynum));
					if (num > 3) num--;
					if (csvDatas[3] != "axis") inputManagerGenerator.AddAxis(InputAxis.KeySetting(name, csvDatas[3], csvDatas[4]));
					else {
						num = csvDatas[4].Contains("X") ? 1 : csvDatas[4].Contains("Y") ? 2 : int.Parse(csvDatas[4].Substring(0, 1));
						inputManagerGenerator.AddAxis(InputAxis.MouseAxisSetting(name, num));
					}
				}
			}
		}
    
	}

	public enum AxisType {
		KeyOrMouseButton, MouseMovement, JoystickAxis
	}

	public class InputAxis {
		public string Name = "";
		public string DescriptiveName = "";
		public string DescriptiveNegativeName = "";
		public string NegativeButton = "";
		public string PositiveButton = "";
		public string AltNegativeButton = "";
		public string AltPositiveButton = "";
		public float Gravity = 0;
		public float Dead = 0;
		public float Sensitivity = 0;
		public bool Snap = false;
		public bool Invert = false;
		public AxisType Type = AxisType.KeyOrMouseButton;
		public int Axis = 1;
		public int joyNum = 0;
	
		public static InputAxis KeySetting(string name, string positiveButton) {
			var axis = new InputAxis();
			axis.Name = name;
			axis.PositiveButton = positiveButton;
			axis.Gravity = 3;
			axis.Sensitivity = 3;
			axis.Dead = 0.001f;
			axis.Type = AxisType.KeyOrMouseButton;
			return axis;
		}
	
		public static InputAxis KeySetting(string name, string negativeButton, string positiveButton) {
			var axis = new InputAxis();
			axis.Name = name;
			axis.NegativeButton = negativeButton;
			axis.PositiveButton = positiveButton;
			axis.Gravity = 3;
			axis.Sensitivity = 3;
			axis.Dead = 0.001f;
			axis.Snap = true;
			axis.Type = AxisType.KeyOrMouseButton;
			return axis;
		}
	
		public static InputAxis ButtonSetting(string name, string positiveButton,int joynum = 0) {
			var axis = new InputAxis();
			axis.Name = name;
			axis.PositiveButton = positiveButton;
			axis.Gravity = 1000;
			axis.Dead = 0.001f;
			axis.Sensitivity = 1000;
			axis.Type = AxisType.KeyOrMouseButton;
			axis.joyNum = joynum;
			return axis;
		}

		public static InputAxis MouseAxisSetting(string name, int axisNum) {		
			var axis = new InputAxis();
			axis.Name = name;
			axis.Sensitivity = 0.1f;
			axis.Type = AxisType.MouseMovement;
			axis.Axis = axisNum;
			return axis;
		}
	
		public static InputAxis PadAxisSetting(string name, int axisNum,int joynum = 0) {
			var axis = new InputAxis();
			axis.Name = name;
			axis.Dead = 0.2f;
			axis.Sensitivity = 1;
			axis.Type = AxisType.JoystickAxis;
			axis.Axis = axisNum;
			axis.joyNum = joynum;
			return axis;
		}
	
		public static InputAxis PadAxisSetting(string name, bool invert, int axisNum,int joynum = 0) {
			var axis = new InputAxis();
			axis.Name = name;
			axis.Dead = 0.2f;
			axis.Sensitivity = 1;
			axis.Invert = invert;
			axis.Type = AxisType.JoystickAxis;
			axis.Axis = axisNum;
			axis.joyNum = joynum;
			return axis;
		}
	}
	public class InputManagerGenerator {
		private SerializedObject serializedObject;
		private static SerializedProperty axesProperty;
	
		public InputManagerGenerator() {
			serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
			axesProperty = serializedObject.FindProperty("m_Axes");
		}
	
		public void AddAxis(InputAxis axis) {
			axesProperty.arraySize++;
			serializedObject.ApplyModifiedProperties();
	
			SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);
		
			GetChildProperty(axisProperty, "m_Name").stringValue = axis.Name;
			GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.DescriptiveName;
			GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.DescriptiveNegativeName;
			GetChildProperty(axisProperty, "negativeButton").stringValue = axis.NegativeButton;
			GetChildProperty(axisProperty, "positiveButton").stringValue = axis.PositiveButton;
			GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.AltNegativeButton;
			GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.AltPositiveButton;
			GetChildProperty(axisProperty, "gravity").floatValue = axis.Gravity;
			GetChildProperty(axisProperty, "dead").floatValue = axis.Dead;
			GetChildProperty(axisProperty, "sensitivity").floatValue = axis.Sensitivity;
			GetChildProperty(axisProperty, "snap").boolValue = axis.Snap;
			GetChildProperty(axisProperty, "invert").boolValue = axis.Invert;
			GetChildProperty(axisProperty, "type").intValue = (int)axis.Type;
			GetChildProperty(axisProperty, "axis").intValue = axis.Axis - 1;
			GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;
 
			serializedObject.ApplyModifiedProperties();
		}
	
		private SerializedProperty GetChildProperty(SerializedProperty parent, string name) {
			SerializedProperty child = parent.Copy();
			child.Next(true);
			do　if (child.name == name) return child;
			while (child.Next(false));
			return null;
		}
	
		public void Clear() {
			axesProperty.ClearArray();
			serializedObject.ApplyModifiedProperties();
		}
	}
}