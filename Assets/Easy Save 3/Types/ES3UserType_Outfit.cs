using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("_clothings")]
	public class ES3UserType_Outfit : ES3ScriptableObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Outfit() : base(typeof(Outfit)){ Instance = this; priority = 1; }


		protected override void WriteScriptableObject(object obj, ES3Writer writer)
		{
			var instance = (Outfit)obj;
			
			writer.WritePrivateField("_clothings", instance);
		}

		protected override void ReadScriptableObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Outfit)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "_clothings":
					instance = (Outfit)reader.SetPrivateField("_clothings", reader.Read<System.Collections.Generic.List<Clothing>>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_OutfitArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_OutfitArray() : base(typeof(Outfit[]), ES3UserType_Outfit.Instance)
		{
			Instance = this;
		}
	}
}