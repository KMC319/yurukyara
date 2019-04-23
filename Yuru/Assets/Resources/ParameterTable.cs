using System.Collections.Generic;
using CharSelects;
using UnityEngine;


[CreateAssetMenu( menuName = "Resource/Create ParameterTable", fileName = "ParameterTable" )]
	public class ParameterTable : ScriptableObject
	{
	
		private static readonly string RESOURCE_PATH  = "ParameterTable";
	
		private static  ParameterTable  s_instance    = null;
		public static   ParameterTable  Instance
		{
			get
			{
				if( s_instance == null )
				{
					var asset   = UnityEngine.Resources.Load( RESOURCE_PATH ,typeof(ParameterTable)) as ParameterTable; 
					if( asset == null )
					{
						Debug.AssertFormat( false, "Missing ParameterTable! path={0}", RESOURCE_PATH );
						asset   = CreateInstance<ParameterTable>();
					}

					s_instance  = asset;
				}
            
				return s_instance;
			}
		}


		[SerializeField] public List<CharIconInformation> CharIconInformations;
	}
