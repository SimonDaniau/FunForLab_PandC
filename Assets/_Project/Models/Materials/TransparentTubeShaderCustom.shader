// Upgrade NOTE: upgraded instancing buffer 'TransparentTubeShaderCustom' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TransparentTubeShaderCustom"
{
	Properties
	{
		_ThirdLevel("ThirdLevel", Float) = 0.55
		_SecondLevel("SecondLevel", Float) = 0.24
		_FirstLevel("FirstLevel", Float) = 0.13
		_MidColor("MidColor", Color) = (0.2431372,0.8392157,0.7753482,1)
		_TopColor("TopColor", Color) = (0.8392157,0.6191606,0.2431372,1)
		_BotColor("BotColor", Color) = (0.8396226,0.2415895,0.2415895,1)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		struct Input
		{
			float3 worldPos;
			float3 viewDir;
		};

		UNITY_INSTANCING_BUFFER_START(TransparentTubeShaderCustom)
			UNITY_DEFINE_INSTANCED_PROP(float4, _TopColor)
#define _TopColor_arr TransparentTubeShaderCustom
			UNITY_DEFINE_INSTANCED_PROP(float, _SecondLevel)
#define _SecondLevel_arr TransparentTubeShaderCustom
			UNITY_DEFINE_INSTANCED_PROP(float, _ThirdLevel)
#define _ThirdLevel_arr TransparentTubeShaderCustom
			UNITY_DEFINE_INSTANCED_PROP(float, _FirstLevel)
#define _FirstLevel_arr TransparentTubeShaderCustom
			UNITY_DEFINE_INSTANCED_PROP(float4, _MidColor)
#define _MidColor_arr TransparentTubeShaderCustom
			UNITY_DEFINE_INSTANCED_PROP(float4, _BotColor)
#define _BotColor_arr TransparentTubeShaderCustom
		UNITY_INSTANCING_BUFFER_END(TransparentTubeShaderCustom)


		float MyCustomExpression70( float3 n , float3 p0 , float radius , float3 l0 , float3 l )
		{
			float denom = dot(n,l);
			float3 p0l0 = p0-l0;
			float t = dot(p0l0,n)/denom;
			float3 p = l0 + l*t;
			float3 v = p-p0;
			float d2 = dot(v,v);
			return -sign( sqrt(d2)-radius);
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 _TopColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_TopColor_arr, _TopColor);
			float4 transform11 = mul(unity_ObjectToWorld,float4( 0,0,0,1 ));
			float3 ase_worldPos = i.worldPos;
			float3 ase_objectScale = float3( length( unity_ObjectToWorld[ 0 ].xyz ), length( unity_ObjectToWorld[ 1 ].xyz ), length( unity_ObjectToWorld[ 2 ].xyz ) );
			float temp_output_9_0 = (1 + (( ( transform11 - float4( ase_worldPos , 0.0 ) ).y * ase_objectScale.y ) - -0.021) * (0 - 1) / (0.05205 - -0.021));
			float _SecondLevel_Instance = UNITY_ACCESS_INSTANCED_PROP(_SecondLevel_arr, _SecondLevel);
			float _ThirdLevel_Instance = UNITY_ACCESS_INSTANCED_PROP(_ThirdLevel_arr, _ThirdLevel);
			float _FirstLevel_Instance = UNITY_ACCESS_INSTANCED_PROP(_FirstLevel_arr, _FirstLevel);
			float temp_output_26_0 = saturate( -sign( ( (0 + (temp_output_9_0 - _FirstLevel_Instance) * (1 - 0) / (_SecondLevel_Instance - _FirstLevel_Instance)) - 1 ) ) );
			float temp_output_48_0 = ( saturate( -sign( ( (0 + (temp_output_9_0 - _SecondLevel_Instance) * (1 - 0) / (_ThirdLevel_Instance - _SecondLevel_Instance)) - 1 ) ) ) - temp_output_26_0 );
			float4 _MidColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_MidColor_arr, _MidColor);
			float temp_output_8_0 = saturate( -sign( ( (0 + (temp_output_9_0 - 0) * (1 - 0) / (_FirstLevel_Instance - 0)) - 1 ) ) );
			float temp_output_28_0 = ( temp_output_26_0 - temp_output_8_0 );
			float4 _BotColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_BotColor_arr, _BotColor);
			float3 normalizeResult73 = normalize( float3(0,1,0) );
			float3 n70 = normalizeResult73;
			float lerpResult61 = lerp( -0.021 , 0.05205 , _ThirdLevel_Instance);
			float4 transform66 = mul(unity_ObjectToWorld,float4( 0,0,0,1 ));
			float3 p070 = ( float4( float3(0,-0.02831,0) , 0.0 ) + float4( ( float3(0,1,0) * ( lerpResult61 / ase_objectScale.y ) ) , 0.0 ) + transform66 ).xyz;
			float radius70 = 0.0066;
			float3 l070 = _WorldSpaceCameraPos;
			float3 normalizeResult69 = normalize( i.viewDir );
			float3 l70 = normalizeResult69;
			float localMyCustomExpression70 = MyCustomExpression70( n70 , p070 , radius70 , l070 , l70 );
			float temp_output_74_0 = saturate( localMyCustomExpression70 );
			float4 temp_output_21_0 = ( ( _TopColor_Instance * saturate( temp_output_48_0 ) ) + ( _MidColor_Instance * saturate( temp_output_28_0 ) ) + ( _BotColor_Instance * saturate( temp_output_8_0 ) ) + ( saturate( ( 1.0 - ( temp_output_48_0 + temp_output_28_0 + temp_output_8_0 + temp_output_74_0 ) ) ) * float4(1,1,1,0.1294118) ) + ( _TopColor_Instance * temp_output_74_0 * (( _WorldSpaceCameraPos.y > ase_worldPos.y ) ? 1 :  0 ) ) );
			o.Albedo = temp_output_21_0.rgb;
			o.Alpha = temp_output_21_0.a;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = worldViewDir;
				surfIN.worldPos = worldPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14501
82;173;1481;724;3535.068;954.7482;4.171679;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;12;-2402.775,1133.249;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;11;-2410.077,950.6906;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;13;-2207.435,1058.401;Float;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ObjectScaleNode;15;-2013.921,1230.007;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BreakToComponentsNode;14;-2070.516,1067.528;Float;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-1829.537,1140.553;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1756.013,1416.422;Float;False;Constant;_top;top;3;0;Create;True;0;-0.021;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1801.315,1536.146;Float;False;Constant;_base;base;3;0;Create;True;0;0.05205;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1;-2110.045,-266.9932;Float;False;InstancedProperty;_ThirdLevel;ThirdLevel;0;0;Create;True;0;0.55;0.42;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;9;-1472.049,1086.706;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-2105.896,242.5916;Float;False;InstancedProperty;_FirstLevel;FirstLevel;2;0;Create;True;0;0.13;0.13;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-2269.079,-64.11209;Float;False;InstancedProperty;_SecondLevel;SecondLevel;1;0;Create;True;0;0.24;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;61;-628.5652,1157.759;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;22;-1325.062,-113.3662;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;4;-1315.167,175.9748;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectScaleNode;65;-648.3401,1320.656;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TFHCRemapNode;37;-1339.965,-385.0247;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;39;-1137.159,-381.7067;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;5;-1110.386,179.2928;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;23;-1122.255,-110.0481;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;64;-427.978,1225.24;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;62;-564.2842,898.1058;Float;False;Constant;_Vector0;Vector 0;6;0;Create;True;0;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;75;-973.2051,1254.773;Float;False;Constant;_Vector2;Vector 2;6;0;Create;True;0;0,-0.02831,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;68;-441.6104,1761.379;Float;False;World;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;72;176.3116,945.8125;Float;False;Constant;_Vector1;Vector 1;6;0;Create;True;0;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-212.1599,1229.785;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SignOpNode;24;-987.7468,-109.0265;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SignOpNode;40;-1002.651,-380.685;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;66;-248.5083,1391.081;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SignOpNode;6;-977.8524,180.3145;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;7;-843.4763,186.8648;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;25;-853.3707,-102.4762;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceCameraPos;67;-493.8609,1579.638;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;71;24.10303,1266.132;Float;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.NormalizeNode;69;-223.5202,1768.194;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NegateNode;43;-868.2745,-374.1347;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-957.3024,1447.875;Float;False;Constant;_Float0;Float 0;6;0;Create;True;0;0.0066;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;73;255.8238,1188.893;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;8;-696.865,181.7089;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;70;205.8447,1441.059;Float;False;float denom = dot(n,l)@$$float3 p0l0 = p0-l0@$float t = dot(p0l0,n)/denom@$float3 p = l0 + l*t@$float3 v = p-p0@$float d2 = dot(v,v)@$return -sign( sqrt(d2)-radius)@$;1;False;5;True;n;FLOAT3;0,0,0;In;True;p0;FLOAT3;0,0,0;In;True;radius;FLOAT;0;In;True;l0;FLOAT3;0,0,0;In;True;l;FLOAT3;0,0,0;In;My Custom Expression;True;5;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;26;-706.7593,-107.632;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;47;-721.6632,-379.2906;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;28;-474.8629,49.8511;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;48;-528.9475,-266.0993;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;74;560.2409,1293.394;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;56;382.2384,485.5283;Float;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;57;517.6983,487.1223;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;19;-318.1691,205.799;Float;False;InstancedProperty;_BotColor;BotColor;5;0;Create;True;0;0.8396226,0.2415895,0.2415895,1;0.8396226,0.2415893,0.2415893,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCCompareGreater;78;573.0502,1480.722;Float;False;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;55;-289.2744,-188.2524;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;59;637.2204,644.8921;Float;False;Constant;_Color2;Color 2;3;0;Create;True;0;1,1,1,0.1294118;0.8396226,0.2415895,0.2415895,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;30;-326.8241,-114.4698;Float;False;InstancedProperty;_MidColor;MidColor;3;0;Create;True;0;0.2431372,0.8392157,0.7753482,1;0.4626487,0.7924528,0.4590244,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;53;-279.188,405.1853;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;54;-306.0857,100.9014;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;50;-316.0215,-368.8816;Float;False;InstancedProperty;_TopColor;TopColor;4;0;Create;True;0;0.8392157,0.6191606,0.2431372,1;0.8392157,0.6191606,0.2431371,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;58;708.9348,482.3412;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-10.90545,-31.19244;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;925.6687,573.1783;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-9.294001,305.3183;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-1.89028,-287.3916;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;716.4482,1224.455;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;21;226.9286,18.83847;Float;False;5;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;31;420.4974,57.52682;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;899.8605,-157.7273;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;TransparentTubeShaderCustom;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;Back;0;0;False;0;0;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;13;0;11;0
WireConnection;13;1;12;0
WireConnection;14;0;13;0
WireConnection;16;0;14;1
WireConnection;16;1;15;2
WireConnection;9;0;16;0
WireConnection;9;1;17;0
WireConnection;9;2;18;0
WireConnection;61;0;17;0
WireConnection;61;1;18;0
WireConnection;61;2;1;0
WireConnection;22;0;9;0
WireConnection;22;1;3;0
WireConnection;22;2;2;0
WireConnection;4;0;9;0
WireConnection;4;2;3;0
WireConnection;37;0;9;0
WireConnection;37;1;2;0
WireConnection;37;2;1;0
WireConnection;39;0;37;0
WireConnection;5;0;4;0
WireConnection;23;0;22;0
WireConnection;64;0;61;0
WireConnection;64;1;65;2
WireConnection;63;0;62;0
WireConnection;63;1;64;0
WireConnection;24;0;23;0
WireConnection;40;0;39;0
WireConnection;6;0;5;0
WireConnection;7;0;6;0
WireConnection;25;0;24;0
WireConnection;71;0;75;0
WireConnection;71;1;63;0
WireConnection;71;2;66;0
WireConnection;69;0;68;0
WireConnection;43;0;40;0
WireConnection;73;0;72;0
WireConnection;8;0;7;0
WireConnection;70;0;73;0
WireConnection;70;1;71;0
WireConnection;70;2;76;0
WireConnection;70;3;67;0
WireConnection;70;4;69;0
WireConnection;26;0;25;0
WireConnection;47;0;43;0
WireConnection;28;0;26;0
WireConnection;28;1;8;0
WireConnection;48;0;47;0
WireConnection;48;1;26;0
WireConnection;74;0;70;0
WireConnection;56;0;48;0
WireConnection;56;1;28;0
WireConnection;56;2;8;0
WireConnection;56;3;74;0
WireConnection;57;0;56;0
WireConnection;78;0;67;2
WireConnection;78;1;12;2
WireConnection;55;0;48;0
WireConnection;53;0;8;0
WireConnection;54;0;28;0
WireConnection;58;0;57;0
WireConnection;29;0;30;0
WireConnection;29;1;54;0
WireConnection;60;0;58;0
WireConnection;60;1;59;0
WireConnection;20;0;19;0
WireConnection;20;1;53;0
WireConnection;49;0;50;0
WireConnection;49;1;55;0
WireConnection;77;0;50;0
WireConnection;77;1;74;0
WireConnection;77;2;78;0
WireConnection;21;0;49;0
WireConnection;21;1;29;0
WireConnection;21;2;20;0
WireConnection;21;3;60;0
WireConnection;21;4;77;0
WireConnection;31;0;21;0
WireConnection;0;0;21;0
WireConnection;0;9;31;3
ASEEND*/
//CHKSM=3BFD6E37E43DBBF8D20A37D0092000C0AF3C2F62