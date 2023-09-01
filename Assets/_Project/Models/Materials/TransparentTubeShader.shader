// Upgrade NOTE: upgraded instancing buffer 'TubeShader' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TubeShader"
{
	Properties
	{
		_TopColor("TopColor", Color) = (0,0,0,0)
		_MidColor("MidColor", Color) = (0,0,0,0)
		_BotColor("BotColor", Color) = (0,0,0,0)
		_FirstLevel("FirstLevel", Float) = 0
		_SecondLevel("SecondLevel", Float) = 0
		_ThirdLevel("ThirdLevel", Float) = 0
		_radius("radius", Float) = 0.0066
		_top("top", Float) = -0.021
		_offset("offset", Vector) = (0,-0.02831,0,0)
		_Float0("Float 0", Float) = 0.09
		_base("base", Float) = 0.05205
		_Float7("Float 7", Float) = 0.09
		_Float5("Float 5", Float) = 0.09
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "ForceNoShadowCasting" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma multi_compile_instancing
		#pragma surface surf Standard alpha:fade keepalpha noshadow 
		struct Input
		{
			float3 worldPos;
			float3 viewDir;
		};

		uniform float _top;
		uniform float _base;
		uniform float3 _offset;
		uniform float _Float0;
		uniform float _radius;
		uniform float _Float5;
		uniform float _Float7;

		UNITY_INSTANCING_BUFFER_START(TubeShader)
			UNITY_DEFINE_INSTANCED_PROP(float, _SecondLevel)
#define _SecondLevel_arr TubeShader
			UNITY_DEFINE_INSTANCED_PROP(float, _ThirdLevel)
#define _ThirdLevel_arr TubeShader
			UNITY_DEFINE_INSTANCED_PROP(float, _FirstLevel)
#define _FirstLevel_arr TubeShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _TopColor)
#define _TopColor_arr TubeShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _MidColor)
#define _MidColor_arr TubeShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _BotColor)
#define _BotColor_arr TubeShader
		UNITY_INSTANCING_BUFFER_END(TubeShader)


		float intersectDisc( float3 n , float3 p0 , float radius , float3 l0 , float3 l )
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
			float4 transform505 = mul(unity_ObjectToWorld,float4( 0,0,0,1 ));
			float3 ase_worldPos = i.worldPos;
			float3 ase_objectScale = float3( length( unity_ObjectToWorld[ 0 ].xyz ), length( unity_ObjectToWorld[ 1 ].xyz ), length( unity_ObjectToWorld[ 2 ].xyz ) );
			float temp_output_149_0 = (1 + (( ( transform505 - float4( ase_worldPos , 0.0 ) ).y * ase_objectScale.y ) - _top) * (0 - 1) / (_base - _top));
			float _SecondLevel_Instance = UNITY_ACCESS_INSTANCED_PROP(_SecondLevel_arr, _SecondLevel);
			float _ThirdLevel_Instance = UNITY_ACCESS_INSTANCED_PROP(_ThirdLevel_arr, _ThirdLevel);
			float _FirstLevel_Instance = UNITY_ACCESS_INSTANCED_PROP(_FirstLevel_arr, _FirstLevel);
			float temp_output_180_0 = saturate( -sign( ( (0 + (temp_output_149_0 - _FirstLevel_Instance) * (1 - 0) / (_SecondLevel_Instance - _FirstLevel_Instance)) - 1 ) ) );
			float ifLocalVar190 = 0;
			if( _SecondLevel_Instance >= _ThirdLevel_Instance )
				ifLocalVar190 = 0.0;
			else
				ifLocalVar190 = 1.0;
			float3 normalizeResult414 = normalize( float3(0,1,0) );
			float3 n415 = normalizeResult414;
			float lerpResult635 = lerp( _top , _base , _SecondLevel_Instance);
			float4 transform633 = mul(unity_ObjectToWorld,float4( 0,0,0,1 ));
			float3 p0415 = ( float4( _offset , 0.0 ) + float4( ( float3(0,1,0) * ( lerpResult635 / ase_objectScale.y ) ) , 0.0 ) + transform633 ).xyz;
			float temp_output_389_0 = ( _SecondLevel_Instance + 0 );
			float lerpResult402 = lerp( sin( radians( (0 + (temp_output_389_0 - 0) * (140 - 0) / (_Float0 - 0)) ) ) , sin( radians( (0 + (temp_output_389_0 - 0) * (90 - 0) / (_Float0 - 0)) ) ) , (0 + (temp_output_389_0 - 0) * (1 - 0) / (_Float0 - 0)));
			float radius415 = (( temp_output_389_0 > _Float0 ) ? _radius :  ( _radius * lerpResult402 ) );
			float3 l0415 = _WorldSpaceCameraPos;
			float3 normalizeResult412 = normalize( i.viewDir );
			float3 l415 = normalizeResult412;
			float localintersectDisc415 = intersectDisc( n415 , p0415 , radius415 , l0415 , l415 );
			float midtopMask440 = saturate( localintersectDisc415 );
			float3 normalizeResult250 = normalize( float3(0,1,0) );
			float3 n231 = normalizeResult250;
			float lerpResult618 = lerp( _top , _base , _ThirdLevel_Instance);
			float4 transform629 = mul(unity_ObjectToWorld,float4( 0,0,0,1 ));
			float3 p0231 = ( float4( _offset , 0.0 ) + float4( ( float3(0,1,0) * ( lerpResult618 / ase_objectScale.y ) ) , 0.0 ) + transform629 ).xyz;
			float temp_output_304_0 = ( _ThirdLevel_Instance + 0 );
			float lerpResult266 = lerp( sin( radians( (0 + (temp_output_304_0 - 0) * (140 - 0) / (_Float5 - 0)) ) ) , sin( radians( (0 + (temp_output_304_0 - 0) * (90 - 0) / (_Float5 - 0)) ) ) , (0 + (temp_output_304_0 - 0) * (1 - 0) / (_Float5 - 0)));
			float radius231 = (( temp_output_304_0 > _Float5 ) ? _radius :  ( _radius * lerpResult266 ) );
			float3 l0231 = _WorldSpaceCameraPos;
			float3 normalizeResult249 = normalize( i.viewDir );
			float3 l231 = normalizeResult249;
			float localintersectDisc231 = intersectDisc( n231 , p0231 , radius231 , l0231 , l231 );
			float topMask438 = saturate( localintersectDisc231 );
			float4 _TopColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_TopColor_arr, _TopColor);
			float temp_output_179_0 = saturate( -sign( ( (0 + (temp_output_149_0 - 0) * (1 - 0) / (_FirstLevel_Instance - 0)) - 1 ) ) );
			float ifLocalVar186 = 0;
			if( _FirstLevel_Instance >= _SecondLevel_Instance )
				ifLocalVar186 = 0.0;
			else
				ifLocalVar186 = 1.0;
			float3 normalizeResult464 = normalize( float3(0,1,0) );
			float3 n457 = normalizeResult464;
			float lerpResult638 = lerp( _top , _base , _FirstLevel_Instance);
			float4 transform643 = mul(unity_ObjectToWorld,float4( 0,0,0,1 ));
			float3 p0457 = ( float4( _offset , 0.0 ) + float4( ( float3(0,1,0) * ( lerpResult638 / ase_objectScale.y ) ) , 0.0 ) + transform643 ).xyz;
			float temp_output_466_0 = ( _FirstLevel_Instance + 0 );
			float lerpResult453 = lerp( sin( radians( (0 + (temp_output_466_0 - 0) * (140 - 0) / (_Float7 - 0)) ) ) , sin( radians( (0 + (temp_output_466_0 - 0) * (90 - 0) / (_Float7 - 0)) ) ) , (0 + (temp_output_466_0 - 0) * (1 - 0) / (_Float7 - 0)));
			float radius457 = (( temp_output_466_0 > _Float7 ) ? _radius :  ( _radius * lerpResult453 ) );
			float3 l0457 = _WorldSpaceCameraPos;
			float3 normalizeResult467 = normalize( i.viewDir );
			float3 l457 = normalizeResult467;
			float localintersectDisc457 = intersectDisc( n457 , p0457 , radius457 , l0457 , l457 );
			float botmidMask471 = saturate( localintersectDisc457 );
			float4 _MidColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_MidColor_arr, _MidColor);
			float4 _BotColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_BotColor_arr, _BotColor);
			float4 transform435 = mul(unity_ObjectToWorld,float4( float3(0,1,0) , 0.0 ));
			float4 normalizeResult433 = normalize( transform435 );
			float3 normalizeResult437 = normalize( i.viewDir );
			float dotResult434 = dot( normalizeResult433 , float4( normalizeResult437 , 0.0 ) );
			float temp_output_518_0 = saturate( ( ( topMask438 - midtopMask440 ) - botmidMask471 ) );
			float ifLocalVar431 = 0;
			if( dotResult434 <= 0 )
				ifLocalVar431 = temp_output_518_0;
			else
				ifLocalVar431 = topMask438;
			float4 temp_cast_14 = (1.0).xxxx;
			float temp_output_564_0 = ( midtopMask440 - botmidMask471 );
			float ifLocalVar537 = 0;
			if( dotResult434 <= 0 )
				ifLocalVar537 = temp_output_564_0;
			else
				ifLocalVar537 = ( midtopMask440 - topMask438 );
			float ifLocalVar596 = 0;
			if( dotResult434 <= 0 )
				ifLocalVar596 = _MidColor_Instance.a;
			else
				ifLocalVar596 = _TopColor_Instance.a;
			float4 lerpResult597 = lerp( (( dotResult434 > 0 ) ? _MidColor_Instance :  _TopColor_Instance ) , (( dotResult434 > 0 ) ? _TopColor_Instance :  _MidColor_Instance ) , ifLocalVar596);
			float4 appendResult593 = (float4(lerpResult597.r , lerpResult597.g , lerpResult597.b , max( _MidColor_Instance.a , _TopColor_Instance.a )));
			float ifLocalVar559 = 0;
			if( dotResult434 <= 0 )
				ifLocalVar559 = botmidMask471;
			else
				ifLocalVar559 = ( botmidMask471 - midtopMask440 );
			float ifLocalVar586 = 0;
			if( dotResult434 <= 0 )
				ifLocalVar586 = _BotColor_Instance.a;
			else
				ifLocalVar586 = _MidColor_Instance.a;
			float4 lerpResult557 = lerp( (( dotResult434 > 0 ) ? _BotColor_Instance :  _MidColor_Instance ) , (( dotResult434 > 0 ) ? _MidColor_Instance :  _BotColor_Instance ) , ifLocalVar586);
			float4 appendResult569 = (float4(lerpResult557.r , lerpResult557.g , lerpResult557.b , max( _BotColor_Instance.a , _MidColor_Instance.a )));
			float4 temp_output_194_0 = ( ( ( saturate( ( ( ( ( saturate( -sign( ( (0 + (temp_output_149_0 - _SecondLevel_Instance) * (1 - 0) / (_ThirdLevel_Instance - _SecondLevel_Instance)) - 1 ) ) ) - temp_output_180_0 ) * ifLocalVar190 ) - midtopMask440 ) - topMask438 ) ) * _TopColor_Instance ) + ( saturate( ( ( ( ( temp_output_180_0 - temp_output_179_0 ) * ifLocalVar186 ) - botmidMask471 ) - midtopMask440 ) ) * _MidColor_Instance ) + ( saturate( ( temp_output_179_0 - botmidMask471 ) ) * _BotColor_Instance ) + ( saturate( ifLocalVar431 ) * (( _FirstLevel_Instance > max( _SecondLevel_Instance , _ThirdLevel_Instance ) ) ? _BotColor_Instance :  (( _SecondLevel_Instance > max( _FirstLevel_Instance , _ThirdLevel_Instance ) ) ? _MidColor_Instance :  _TopColor_Instance ) ) * (( dotResult434 > 0 ) ? float4(0.9,0.9,0.9,1) :  temp_cast_14 ) ) + ( saturate( ifLocalVar537 ) * appendResult593 ) + ( saturate( ifLocalVar559 ) * appendResult569 ) ) + float4( 0,0,0,0 ) );
			float4 temp_cast_19 = (1.0).xxxx;
			float4 temp_cast_24 = (1.0).xxxx;
			float3 appendResult380 = (float3(temp_output_194_0.r , temp_output_194_0.g , temp_output_194_0.b));
			float4 temp_cast_29 = (1.0).xxxx;
			float4 temp_output_381_0 = (( length( appendResult380 ) < 0.1 ) ? float4(1,1,1,0) :  temp_output_194_0 );
			o.Albedo = temp_output_381_0.rgb;
			float4 temp_cast_35 = (1.0).xxxx;
			float clampResult377 = clamp( temp_output_381_0.a , 0.2 , 1 );
			o.Alpha = clampResult377;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14501
-1650;343.2;1422;621;692.4899;953.9175;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;385;-1043.603,5358.195;Float;False;2253.806;2344.406;;5;440;410;394;389;387;Mid Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;305;-1108.55,2488.111;Float;False;2291.022;2337.601;;5;438;257;272;304;271;Top Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;441;-1055.529,8152.362;Float;False;2229.79;2336.401;;5;471;469;442;466;443;Bot Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;164;-1907.562,220.5475;Float;False;InstancedProperty;_ThirdLevel;ThirdLevel;5;0;Create;True;0;0;0.65;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;163;-2565.071,888.288;Float;False;InstancedProperty;_SecondLevel;SecondLevel;4;0;Create;True;0;0;0.13;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;387;-615.1277,6677.96;Float;False;1070.247;958.9185;mid tube radius scaling;10;405;402;401;400;397;395;393;392;391;390;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;271;-680.0735,3807.874;Float;False;1070.247;958.9185;top tube radius scaling;10;260;267;261;269;263;262;270;265;266;259;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;580;-2082.356,-962.7415;Float;False;926.8912;475.3982;;6;507;499;498;505;495;506;Y Gradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;304;-1058.55,3976;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;505;-2032.356,-912.7413;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;162;-3010.469,1642.502;Float;False;InstancedProperty;_FirstLevel;FirstLevel;3;0;Create;True;0;0;0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;389;-993.6028,6846.086;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;260;-630.0739,4362.742;Float;False;Property;_Float5;Float 5;13;0;Create;True;0;0.09;0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;443;-627.0543,9472.13;Float;False;1070.247;958.9185;Bottom tube radius scaling;10;468;465;461;459;454;453;450;449;447;446;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;390;-565.1281,7232.832;Float;False;Property;_Float0;Float 0;10;0;Create;True;0;0.09;0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;495;-2016.627,-714.415;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;454;-577.0546,10025;Float;False;Property;_Float7;Float 7;12;0;Create;True;0;0.09;0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;466;-1005.529,9640.256;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;267;-292.9039,4327.338;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;90;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;391;-300.2469,6947.372;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;140;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;261;-365.1929,4077.286;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;140;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;498;-1762.289,-826.8638;Float;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TFHCRemapNode;392;-227.9578,7197.428;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;90;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;581;-160.882,-837.7958;Float;False;509.2932;364.5297;;3;157;158;149;Mapped to Tube;1,1,1,1;0;0
Node;AmplifyShaderEditor.ObjectScaleNode;506;-1531.472,-666.343;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;157;-103.1901,-695.715;Float;False;Property;_top;top;8;0;Create;True;0;-0.021;-0.021;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RadiansOpNode;395;-75.45055,6991.532;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;461;-312.1736,9741.542;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;140;False;1;FLOAT;0
Node;AmplifyShaderEditor.RadiansOpNode;269;-104.1915,4317.362;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;158;-110.882,-588.2661;Float;False;Property;_base;base;11;0;Create;True;0;0.05205;0.05205;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;465;-239.8845,9991.594;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;90;False;1;FLOAT;0
Node;AmplifyShaderEditor.RadiansOpNode;393;-39.24493,7187.451;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;499;-1585.043,-827.0876;Float;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.CommentaryNode;272;-913.7653,2600.777;Float;False;1680.8;980.6938;;14;228;227;249;231;253;250;226;619;620;623;622;621;618;629;Top Disc;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;394;-863.6845,5467.655;Float;False;1680.8;980.6938;;15;416;415;414;413;412;408;407;632;630;631;633;634;635;636;637;Mid Disc;1,1,1,1;0;0
Node;AmplifyShaderEditor.RadiansOpNode;263;-140.3973,4121.446;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;397;101.3968,7002.192;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RadiansOpNode;468;-51.17161,9981.618;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectScaleNode;619;-654.6783,2992.516;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;442;-865.2389,8310.198;Float;False;1680.8;980.6938;;14;470;467;464;463;457;452;448;638;639;640;641;642;643;645;Bot Disc;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;635;-739.2457,5717.498;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;265;-358.9778,4564.792;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectScaleNode;636;-719.1488,5851.885;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SinOpNode;262;36.44949,4132.106;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;400;104.0658,7157.282;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RadiansOpNode;446;-87.37767,9785.702;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;507;-1324.465,-794.7408;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;270;39.11843,4287.192;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;401;-294.0319,7434.881;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;618;-674.775,2858.129;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;449;-305.9587,10229.05;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;447;89.47073,9796.362;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;266;202.1696,4278.263;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;402;267.1165,7148.353;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;630;-602.2955,5539.338;Float;False;Constant;_Vector3;Vector 3;11;0;Create;True;0;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;582;918.8837,-437.3625;Float;False;1226.985;770.373;;17;167;168;185;180;195;177;178;176;181;182;183;179;193;166;171;173;172;Side Raw Masks;1,1,1,1;0;0
Node;AmplifyShaderEditor.SinOpNode;459;92.13968,9951.448;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;232;-4556.029,5993.141;Float;False;Property;_radius;radius;7;0;Create;True;0;0.0066;0.0066;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;638;-761.1133,8540.543;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;149;133.411,-785.7956;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.1;False;3;FLOAT;1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectScaleNode;639;-741.0167,8674.93;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;623;-537.8249,2679.969;Float;False;Constant;_Vector2;Vector 2;11;0;Create;True;0;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleDivideOpNode;620;-471.0862,2902.157;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;634;-535.5569,5761.526;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;631;-399.21,5726.72;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;167;970.5644,-147.2205;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;629;-219.5254,3014.813;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;627;-4552.566,5762.881;Float;False;Property;_offset;offset;9;0;Create;True;0;0,-0.02831,0;0,-0.02831,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TFHCRemapNode;166;977.6556,85.71125;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;408;297.3206,5546.625;Float;False;Constant;_Vector4;Vector 4;16;0;Create;True;0;0,1,0;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;633;-283.9971,5874.182;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;259;3.348408,3986.811;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;405;68.29535,6856.897;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;168;971.2311,-379.6924;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;622;-334.7385,2867.351;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;226;247.2387,2679.747;Float;False;Constant;_Vector0;Vector 0;16;0;Create;True;0;0,1,0;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;228;-193.8895,3397.47;Float;False;World;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;641;-624.1634,8362.384;Float;False;Constant;_Vector7;Vector 7;11;0;Create;True;0;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleDivideOpNode;640;-557.4249,8584.571;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;453;255.1895,9942.519;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;407;-143.808,6264.351;Float;False;World;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;463;295.7667,8389.17;Float;False;Constant;_Vector8;Vector 8;16;0;Create;True;0;0,1,0;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;172;1181.386,91.09528;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareGreater;410;80.39886,6533.482;Float;False;4;0;FLOAT;0;False;1;FLOAT;0.09;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;412;86.54938,6232.382;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;250;355.0873,2898.026;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;572;3755.358,1868.252;Float;False;798.1021;448.2432;;6;432;436;435;433;437;434;Topside commmuter;1,1,1,1;0;0
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;643;-305.8641,8697.227;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceCameraPos;227;-374.1399,3222.832;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;642;-421.0772,8549.765;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;414;237.5073,5884.337;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldSpaceCameraPos;413;-324.059,6089.713;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;176;1189.472,-144.4093;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;632;-26.27961,5751.179;Float;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TFHCCompareGreater;257;15.45144,3663.397;Float;False;4;0;FLOAT;0;False;1;FLOAT;0.09;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;470;-145.3624,9106.896;Float;False;World;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;249;36.46755,3365.5;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;450;56.36807,9651.067;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;621;38.19166,2891.81;Float;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;181;1176.238,-382.3089;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SignOpNode;177;1332.528,-146.6803;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareGreater;469;68.47256,9327.651;Float;False;4;0;FLOAT;0;False;1;FLOAT;0.09;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;415;403.0266,6067.936;Float;False;float denom = dot(n,l)@$$float3 p0l0 = p0-l0@$float t = dot(p0l0,n)/denom@$float3 p = l0 + l*t@$float3 v = p-p0@$float d2 = dot(v,v)@$return -sign( sqrt(d2)-radius)@$;1;False;5;True;n;FLOAT3;0,0,0;In;True;p0;FLOAT3;0,0,0;In;True;radius;FLOAT;0;In;True;l0;FLOAT3;0,0,0;In;True;l;FLOAT3;0,0,0;In;intersectDisc;False;5;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;432;3805.357,1918.252;Float;False;Constant;_Vector5;Vector 5;16;0;Create;True;0;0,1,0;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SignOpNode;182;1319.293,-384.5799;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;467;84.99454,9074.926;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CustomExpressionNode;231;352.9448,3201.055;Float;False;float denom = dot(n,l)@$$float3 p0l0 = p0-l0@$float t = dot(p0l0,n)/denom@$float3 p = l0 + l*t@$float3 v = p-p0@$float d2 = dot(v,v)@$return -sign( sqrt(d2)-radius)@$;1;False;5;True;n;FLOAT3;0,0,0;In;True;p0;FLOAT3;0,0,0;In;True;radius;FLOAT;0;In;True;l0;FLOAT3;0,0,0;In;True;l;FLOAT3;0,0,0;In;intersectDisc;False;5;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceCameraPos;452;-325.6133,8932.258;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;645;-48.14695,8574.224;Float;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SignOpNode;171;1324.441,88.82429;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;464;235.9527,8726.882;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;253;592.0323,3209.817;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;416;642.1141,6076.698;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;583;920.9186,511.5906;Float;False;568.439;511.3879;;4;186;190;187;188;Height Cut;1,1,1,1;0;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;436;4011.366,2132.495;Float;False;World;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NegateNode;178;1450.784,-147.2163;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;457;401.4724,8910.48;Float;False;float denom = dot(n,l)@$$float3 p0l0 = p0-l0@$float t = dot(p0l0,n)/denom@$float3 p = l0 + l*t@$float3 v = p-p0@$float d2 = dot(v,v)@$return -sign( sqrt(d2)-radius)@$;1;False;5;True;n;FLOAT3;0,0,0;In;True;p0;FLOAT3;0,0,0;In;True;radius;FLOAT;0;In;True;l0;FLOAT3;0,0,0;In;True;l;FLOAT3;0,0,0;In;intersectDisc;False;5;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;173;1442.699,88.2884;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;183;1437.547,-385.1158;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;435;3982.534,1943.839;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;573;3926.53,3157.959;Float;False;1157.942;479.3782;;8;482;519;517;521;522;518;431;579;Top Full Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;482;3976.53,3207.959;Float;False;438;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;440;942.3028,6060.885;Float;False;midtopMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;180;1689.136,-147.3727;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;437;4220.718,2070.525;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;448;640.5598,8919.242;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;187;970.9186,645.3664;Float;False;Constant;_Float1;Float 1;16;0;Create;True;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;519;4005.268,3419.182;Float;False;440;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;188;974.4149,785.97;Float;False;Constant;_Float4;Float 4;16;0;Create;True;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;185;1692.489,-382.2171;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;179;1697.46,84.23139;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;438;909.6465,3205.939;Float;False;topMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;433;4213.469,1929.582;Float;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ConditionalIfNode;190;1284.689,820.9792;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;160;2679.802,4354.799;Float;False;InstancedProperty;_MidColor;MidColor;1;0;Create;True;0;0,0,0,0;0.7734375,0.6523438,0.2499999,0.4705882;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;193;1956.906,-56.07968;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;159;2560.059,1962.205;Float;False;InstancedProperty;_TopColor;TopColor;0;0;Create;True;0;0,0,0,0;0.8301887,0.5599858,0.5599858,0.7882353;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ConditionalIfNode;186;1291.358,561.5908;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;576;3596.753,5482.174;Float;False;1433.943;472.4248;;7;596;595;594;597;593;592;591;Mid Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;575;3997.309,4852.842;Float;False;1022.247;508.8582;;8;537;541;564;525;560;542;561;543;Mid Full Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;161;2644.394,6435.089;Float;False;InstancedProperty;_BotColor;BotColor;2;0;Create;True;0;0,0,0,0;0.3018867,0,0.01857772,0.8941177;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;481;3108.064,-69.74021;Float;False;1068.326;772.4816;;13;515;514;189;475;509;473;513;476;508;516;512;474;191;Side Full Masks;1,1,1,1;0;0
Node;AmplifyShaderEditor.DotProductOpNode;434;4399.451,1941.734;Float;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;471;924.4963,8901.261;Float;False;botmidMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;195;1967.078,-259.7567;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;521;4058.166,3522.337;Float;False;471;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;578;3465.384,7251.19;Float;False;1435.443;463.3413;;7;557;586;588;589;569;571;570;Bot Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;517;4207.038,3326.517;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;577;3935.96,6613.769;Float;False;946.6804;512.8926;;6;559;555;562;552;553;556;Bot Full Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.TFHCCompareGreater;589;3522.05,7534.803;Float;False;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;522;4360.692,3379.861;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;586;3692.836,7393.467;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;542;4084.773,5017.024;Float;False;438;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;553;3989.087,6839.352;Float;False;440;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;473;3346.502,379.5114;Float;False;440;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareGreater;594;3657.662,5756.917;Float;False;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ConditionalIfNode;596;3828.447,5615.581;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;191;3191.611,47.93892;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;189;3179.406,309.9794;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareGreater;595;3655.08,5521.098;Float;False;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCCompareGreater;588;3519.468,7298.984;Float;False;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;552;3985.96,6727.274;Float;False;471;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;475;3354.487,599.9567;Float;False;471;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;525;4080.646,4903.948;Float;False;440;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;561;4050.545,5246.699;Float;False;471;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;574;3931.042,3759.74;Float;False;558.0027;437.4875;;4;489;493;485;492;Top Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;560;4047.309,5155.672;Float;False;440;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;541;4306.395,4902.842;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;557;4152.074,7339.865;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;474;3582.116,52.31926;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;518;4524.94,3373.695;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;508;3339.208,128.7596;Float;False;438;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;476;3584.656,315.4827;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;555;4227.31,6663.769;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;564;4299.962,5148.9;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;562;4013.868,7021.646;Float;False;471;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;489;3981.042,4032.403;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;597;4294.519,5584.589;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;485;4092.999,3827.934;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;513;3779.401,344.9604;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;512;3754.275,85.53943;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;548;5029.561,3758.793;Float;False;Constant;_Vector9;Vector 9;11;0;Create;True;0;0.9,0.9,0.9,1;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ConditionalIfNode;537;4633.433,4939.019;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;591;4541.458,5575.562;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ConditionalIfNode;431;4721.204,3292.99;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareGreater;493;4177.449,4018.227;Float;False;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ConditionalIfNode;559;4498.25,6755.744;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;592;4237.881,5774.419;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;571;4095.436,7529.696;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;602;5078.451,3982.067;Float;False;Constant;_Float2;Float 2;6;0;Create;True;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;570;4399.012,7330.838;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleSubtractOpNode;509;3778.507,525.0272;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;515;3993.343,338.2025;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;593;4876.27,5605.611;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;569;4733.824,7360.887;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TFHCCompareGreater;600;5343.785,3648.563;Float;False;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT4;0,0,0,0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TFHCCompareGreater;492;4296.042,3809.74;Float;False;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;516;3995.615,82.51044;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;556;4696.829,6758.148;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;514;3995.253,527.9272;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;579;4902.278,3296.366;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;543;4831.923,4936.055;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;478;4575.99,496.6095;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;494;5631.188,3539.558;Float;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;558;5207.806,7043.6;Float;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;531;5359.326,5394.65;Float;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;480;4578.272,50.07952;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;479;4582.105,281.3589;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;585;6726.141,705.0873;Float;False;1191.869;520.4186;;6;194;376;380;382;379;381;Transparent Fill;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;477;6457.899,1077.64;Float;False;6;6;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;FLOAT4;0,0,0,0;False;5;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;194;6776.141,1092.505;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;376;6997.147,757.1089;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;380;7308.579,755.0873;Float;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LengthOpNode;379;7469.479,773.3867;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;382;7382.364,899.9849;Float;False;Constant;_Color0;Color 0;7;0;Create;True;0;1,1,1,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCCompareLower;381;7725.009,1035.666;Float;False;4;0;FLOAT;0;False;1;FLOAT;0.1;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;383;8113.558,1272.464;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.Vector3Node;637;-272.5415,5549.62;Float;False;Property;_Vector6;Vector 6;6;0;Create;True;0;0,-0.023,0;0,-0.023,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;617;496.4072,1185.673;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;377;8457.429,1294.9;Float;False;3;0;FLOAT;0;False;1;FLOAT;0.2;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;517.0927,-1477.171;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;TubeShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;False;False;False;Back;0;0;False;0;0;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;False;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;304;0;164;0
WireConnection;389;0;163;0
WireConnection;466;0;162;0
WireConnection;267;0;304;0
WireConnection;267;2;260;0
WireConnection;391;0;389;0
WireConnection;391;2;390;0
WireConnection;261;0;304;0
WireConnection;261;2;260;0
WireConnection;498;0;505;0
WireConnection;498;1;495;0
WireConnection;392;0;389;0
WireConnection;392;2;390;0
WireConnection;395;0;391;0
WireConnection;461;0;466;0
WireConnection;461;2;454;0
WireConnection;269;0;267;0
WireConnection;465;0;466;0
WireConnection;465;2;454;0
WireConnection;393;0;392;0
WireConnection;499;0;498;0
WireConnection;263;0;261;0
WireConnection;397;0;395;0
WireConnection;468;0;465;0
WireConnection;635;0;157;0
WireConnection;635;1;158;0
WireConnection;635;2;163;0
WireConnection;265;0;304;0
WireConnection;265;2;260;0
WireConnection;262;0;263;0
WireConnection;400;0;393;0
WireConnection;446;0;461;0
WireConnection;507;0;499;1
WireConnection;507;1;506;2
WireConnection;270;0;269;0
WireConnection;401;0;389;0
WireConnection;401;2;390;0
WireConnection;618;0;157;0
WireConnection;618;1;158;0
WireConnection;618;2;164;0
WireConnection;449;0;466;0
WireConnection;449;2;454;0
WireConnection;447;0;446;0
WireConnection;266;0;262;0
WireConnection;266;1;270;0
WireConnection;266;2;265;0
WireConnection;402;0;397;0
WireConnection;402;1;400;0
WireConnection;402;2;401;0
WireConnection;459;0;468;0
WireConnection;638;0;157;0
WireConnection;638;1;158;0
WireConnection;638;2;162;0
WireConnection;149;0;507;0
WireConnection;149;1;157;0
WireConnection;149;2;158;0
WireConnection;620;0;618;0
WireConnection;620;1;619;2
WireConnection;634;0;635;0
WireConnection;634;1;636;2
WireConnection;631;0;630;0
WireConnection;631;1;634;0
WireConnection;167;0;149;0
WireConnection;167;1;162;0
WireConnection;167;2;163;0
WireConnection;166;0;149;0
WireConnection;166;2;162;0
WireConnection;259;0;232;0
WireConnection;259;1;266;0
WireConnection;405;0;232;0
WireConnection;405;1;402;0
WireConnection;168;0;149;0
WireConnection;168;1;163;0
WireConnection;168;2;164;0
WireConnection;622;0;623;0
WireConnection;622;1;620;0
WireConnection;640;0;638;0
WireConnection;640;1;639;2
WireConnection;453;0;447;0
WireConnection;453;1;459;0
WireConnection;453;2;449;0
WireConnection;172;0;166;0
WireConnection;410;0;389;0
WireConnection;410;1;390;0
WireConnection;410;2;232;0
WireConnection;410;3;405;0
WireConnection;412;0;407;0
WireConnection;250;0;226;0
WireConnection;642;0;641;0
WireConnection;642;1;640;0
WireConnection;414;0;408;0
WireConnection;176;0;167;0
WireConnection;632;0;627;0
WireConnection;632;1;631;0
WireConnection;632;2;633;0
WireConnection;257;0;304;0
WireConnection;257;1;260;0
WireConnection;257;2;232;0
WireConnection;257;3;259;0
WireConnection;249;0;228;0
WireConnection;450;0;232;0
WireConnection;450;1;453;0
WireConnection;621;0;627;0
WireConnection;621;1;622;0
WireConnection;621;2;629;0
WireConnection;181;0;168;0
WireConnection;177;0;176;0
WireConnection;469;0;466;0
WireConnection;469;1;454;0
WireConnection;469;2;232;0
WireConnection;469;3;450;0
WireConnection;415;0;414;0
WireConnection;415;1;632;0
WireConnection;415;2;410;0
WireConnection;415;3;413;0
WireConnection;415;4;412;0
WireConnection;182;0;181;0
WireConnection;467;0;470;0
WireConnection;231;0;250;0
WireConnection;231;1;621;0
WireConnection;231;2;257;0
WireConnection;231;3;227;0
WireConnection;231;4;249;0
WireConnection;645;0;627;0
WireConnection;645;1;642;0
WireConnection;645;2;643;0
WireConnection;171;0;172;0
WireConnection;464;0;463;0
WireConnection;253;0;231;0
WireConnection;416;0;415;0
WireConnection;178;0;177;0
WireConnection;457;0;464;0
WireConnection;457;1;645;0
WireConnection;457;2;469;0
WireConnection;457;3;452;0
WireConnection;457;4;467;0
WireConnection;173;0;171;0
WireConnection;183;0;182;0
WireConnection;435;0;432;0
WireConnection;440;0;416;0
WireConnection;180;0;178;0
WireConnection;437;0;436;0
WireConnection;448;0;457;0
WireConnection;185;0;183;0
WireConnection;179;0;173;0
WireConnection;438;0;253;0
WireConnection;433;0;435;0
WireConnection;190;0;163;0
WireConnection;190;1;164;0
WireConnection;190;2;187;0
WireConnection;190;3;187;0
WireConnection;190;4;188;0
WireConnection;193;0;180;0
WireConnection;193;1;179;0
WireConnection;186;0;162;0
WireConnection;186;1;163;0
WireConnection;186;2;187;0
WireConnection;186;3;187;0
WireConnection;186;4;188;0
WireConnection;434;0;433;0
WireConnection;434;1;437;0
WireConnection;471;0;448;0
WireConnection;195;0;185;0
WireConnection;195;1;180;0
WireConnection;517;0;482;0
WireConnection;517;1;519;0
WireConnection;589;0;434;0
WireConnection;589;2;160;0
WireConnection;589;3;161;0
WireConnection;522;0;517;0
WireConnection;522;1;521;0
WireConnection;586;0;434;0
WireConnection;586;2;160;4
WireConnection;586;3;161;4
WireConnection;586;4;161;4
WireConnection;594;0;434;0
WireConnection;594;2;159;0
WireConnection;594;3;160;0
WireConnection;596;0;434;0
WireConnection;596;2;159;4
WireConnection;596;3;160;4
WireConnection;596;4;160;4
WireConnection;191;0;195;0
WireConnection;191;1;190;0
WireConnection;189;0;193;0
WireConnection;189;1;186;0
WireConnection;595;0;434;0
WireConnection;595;2;160;0
WireConnection;595;3;159;0
WireConnection;588;0;434;0
WireConnection;588;2;161;0
WireConnection;588;3;160;0
WireConnection;541;0;525;0
WireConnection;541;1;542;0
WireConnection;557;0;588;0
WireConnection;557;1;589;0
WireConnection;557;2;586;0
WireConnection;474;0;191;0
WireConnection;474;1;473;0
WireConnection;518;0;522;0
WireConnection;476;0;189;0
WireConnection;476;1;475;0
WireConnection;555;0;552;0
WireConnection;555;1;553;0
WireConnection;564;0;560;0
WireConnection;564;1;561;0
WireConnection;489;0;162;0
WireConnection;489;1;164;0
WireConnection;597;0;595;0
WireConnection;597;1;594;0
WireConnection;597;2;596;0
WireConnection;485;0;163;0
WireConnection;485;1;164;0
WireConnection;513;0;476;0
WireConnection;513;1;473;0
WireConnection;512;0;474;0
WireConnection;512;1;508;0
WireConnection;537;0;434;0
WireConnection;537;2;541;0
WireConnection;537;3;564;0
WireConnection;537;4;564;0
WireConnection;591;0;597;0
WireConnection;431;0;434;0
WireConnection;431;2;482;0
WireConnection;431;3;518;0
WireConnection;431;4;518;0
WireConnection;493;0;163;0
WireConnection;493;1;489;0
WireConnection;493;2;160;0
WireConnection;493;3;159;0
WireConnection;559;0;434;0
WireConnection;559;2;555;0
WireConnection;559;3;562;0
WireConnection;559;4;562;0
WireConnection;592;0;160;4
WireConnection;592;1;159;4
WireConnection;571;0;161;4
WireConnection;571;1;160;4
WireConnection;570;0;557;0
WireConnection;509;0;179;0
WireConnection;509;1;475;0
WireConnection;515;0;513;0
WireConnection;593;0;591;0
WireConnection;593;1;591;1
WireConnection;593;2;591;2
WireConnection;593;3;592;0
WireConnection;569;0;570;0
WireConnection;569;1;570;1
WireConnection;569;2;570;2
WireConnection;569;3;571;0
WireConnection;600;0;434;0
WireConnection;600;2;548;0
WireConnection;600;3;602;0
WireConnection;492;0;162;0
WireConnection;492;1;485;0
WireConnection;492;2;161;0
WireConnection;492;3;493;0
WireConnection;516;0;512;0
WireConnection;556;0;559;0
WireConnection;514;0;509;0
WireConnection;579;0;431;0
WireConnection;543;0;537;0
WireConnection;478;0;514;0
WireConnection;478;1;161;0
WireConnection;494;0;579;0
WireConnection;494;1;492;0
WireConnection;494;2;600;0
WireConnection;558;0;556;0
WireConnection;558;1;569;0
WireConnection;531;0;543;0
WireConnection;531;1;593;0
WireConnection;480;0;516;0
WireConnection;480;1;159;0
WireConnection;479;0;515;0
WireConnection;479;1;160;0
WireConnection;477;0;480;0
WireConnection;477;1;479;0
WireConnection;477;2;478;0
WireConnection;477;3;494;0
WireConnection;477;4;531;0
WireConnection;477;5;558;0
WireConnection;194;0;477;0
WireConnection;376;0;194;0
WireConnection;380;0;376;0
WireConnection;380;1;376;1
WireConnection;380;2;376;2
WireConnection;379;0;380;0
WireConnection;381;0;379;0
WireConnection;381;2;382;0
WireConnection;381;3;194;0
WireConnection;383;0;381;0
WireConnection;377;0;383;3
WireConnection;0;0;381;0
WireConnection;0;9;377;0
ASEEND*/
//CHKSM=F92512098882253ADCFD3DBA0B9183D963B79A84