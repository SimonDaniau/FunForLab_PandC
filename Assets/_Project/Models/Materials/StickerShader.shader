// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Sticker"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[PerRendererData]_BarCode("BarCode", Int) = 0
		_Resolution("Resolution", Int) = 0
		_Border("Border", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			fixed ASEVFace : VFACE;
		};

		uniform int _BarCode;
		uniform int _Resolution;
		uniform float2 _Border;
		uniform float _Cutoff = 0.5;


		float MyCustomExpression4( int n , int k )
		{
			return ((n&(1<<(k-1)))>>(k-1));
		}


		float MyCustomExpression24( float2 Size , float2 UV , float Roundness )
		{
			float p1,p2;
			p1 = max(abs(UV.x),Size.x)-Size.x;
			p2 = max(abs(UV.y),Size.y)-Size.y;
			return (p1*p1 + p2*p2 )>Roundness*Roundness ;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			int n4 = _BarCode;
			float2 uv_TexCoord9 = i.uv_texcoord * float2( 1,1 ) + float2( -0.1,0 );
			float temp_output_7_0 = ( uv_TexCoord9.x * _Resolution );
			int k4 = (int)( temp_output_7_0 - frac( temp_output_7_0 ) );
			float localMyCustomExpression4 = MyCustomExpression4( n4 , k4 );
			float2 uv_TexCoord43 = i.uv_texcoord * float2( 1,1 ) + float2( 0.047,-0.12 );
			float temp_output_37_0 = saturate( ( ( 1.0 - localMyCustomExpression4 ) + step( uv_TexCoord43.x , _Border.x ) + step( ( 1.0 - _Border.x ) , uv_TexCoord43.x ) + step( uv_TexCoord43.y , _Border.y ) + step( ( 1.0 - _Border.y ) , uv_TexCoord43.y ) ) );
			float switchResult32 = (((i.ASEVFace>0)?(temp_output_37_0):(saturate( ( temp_output_37_0 + 0.8 ) ))));
			float3 temp_cast_1 = (switchResult32).xxx;
			o.Albedo = temp_cast_1;
			o.Alpha = 1;
			float2 Size24 = float2( 0.62,0.33 );
			float2 uv_TexCoord17 = i.uv_texcoord * ( float2( 2,2 ) * float2( 1,0.75 ) ) + ( float2( -0.92,-1.25 ) * float2( 1,0.75 ) );
			float2 UV24 = uv_TexCoord17;
			float Roundness24 = 0.09;
			float localMyCustomExpression24 = MyCustomExpression24( Size24 , UV24 , Roundness24 );
			clip( ( 1.0 - localMyCustomExpression24 ) - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14501
-1642;481.2;1422;676;3583.104;1042.982;3.493584;True;True
Node;AmplifyShaderEditor.IntNode;15;-1548.531,315.2285;Float;False;Property;_Resolution;Resolution;2;0;Create;True;0;0;32;0;1;INT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-1827.123,64.7022;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.1,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-1392.314,95.4711;Float;True;2;2;0;FLOAT;0;False;1;INT;8;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;11;-1159.113,257.2711;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;12;-974.2244,192.5974;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;5;-1082.913,-7.22879;Float;False;Property;_BarCode;BarCode;1;1;[PerRendererData];Create;True;0;0;782411;0;1;INT;0
Node;AmplifyShaderEditor.Vector2Node;49;-1317.294,708.9455;Float;False;Property;_Border;Border;3;0;Create;True;0;0,0;0.17,0.3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;44;-2064.104,471.498;Float;False;Constant;_BorderOffset;BorderOffset;4;0;Create;True;0;0.047,-0.12;0.047,-0.12;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.OneMinusNode;41;-1005.498,644.6074;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;45;-1002.785,1079.09;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;4;-773.4174,8.460602;Float;False;return ((n&(1<<(k-1)))>>(k-1))@;1;False;2;True;n;INT;0;In;True;k;INT;0;In;My Custom Expression;True;2;0;INT;0;False;1;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;43;-1861.42,476.8255;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;38;-821.1308,441.4952;Float;True;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;46;-816.0035,1089.885;Float;True;2;0;FLOAT;0.1;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;50;-572.8651,-4.461121;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;47;-818.4182,875.9784;Float;True;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;39;-818.7162,655.4031;Float;True;2;0;FLOAT;0.1;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;36;-579.6323,124.8046;Float;False;5;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;53;-1945.797,-196.5385;Float;False;Constant;_Vector0;Vector 0;6;0;Create;True;0;-0.92,-1.25;-0.92,-1.25;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;27;-1971.137,-416.0989;Float;False;Constant;_factor;factor;5;0;Create;True;0;2,2;2,2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-1734.713,-417.1746;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;1,0.75;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1716.713,-214.1746;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;1,0.75;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;37;-430.8461,-113.7721;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;54;-1362.983,-534.1735;Float;False;Constant;_Size;Size;4;0;Create;True;0;0.62,0.33;0.62,0.33;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;17;-1536.358,-369.837;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;26;-1329.378,-254.3058;Float;False;Constant;_round;round;5;0;Create;True;0;0.09;0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;51;-266.2582,-67.75778;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.8;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;24;-1147.423,-381.7397;Float;False;float p1,p2@$p1 = max(abs(UV.x),Size.x)-Size.x@$p2 = max(abs(UV.y),Size.y)-Size.y@$$return (p1*p1 + p2*p2 )>Roundness*Roundness @;1;False;3;True;Size;FLOAT2;0,0;In;True;UV;FLOAT2;0,0;In;True;Roundness;FLOAT;0;In;My Custom Expression;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;52;-141.2582,-66.75778;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwitchByFaceNode;32;23.64542,-141.2599;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;29;-909.137,-336.0989;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;211.417,-18.33095;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Sticker;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;0;False;0;0;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;7;0;9;1
WireConnection;7;1;15;0
WireConnection;11;0;7;0
WireConnection;12;0;7;0
WireConnection;12;1;11;0
WireConnection;41;0;49;1
WireConnection;45;0;49;2
WireConnection;4;0;5;0
WireConnection;4;1;12;0
WireConnection;43;1;44;0
WireConnection;38;0;43;1
WireConnection;38;1;49;1
WireConnection;46;0;45;0
WireConnection;46;1;43;2
WireConnection;50;0;4;0
WireConnection;47;0;43;2
WireConnection;47;1;49;2
WireConnection;39;0;41;0
WireConnection;39;1;43;1
WireConnection;36;0;50;0
WireConnection;36;1;38;0
WireConnection;36;2;39;0
WireConnection;36;3;47;0
WireConnection;36;4;46;0
WireConnection;30;0;27;0
WireConnection;31;0;53;0
WireConnection;37;0;36;0
WireConnection;17;0;30;0
WireConnection;17;1;31;0
WireConnection;51;0;37;0
WireConnection;24;0;54;0
WireConnection;24;1;17;0
WireConnection;24;2;26;0
WireConnection;52;0;51;0
WireConnection;32;0;37;0
WireConnection;32;1;52;0
WireConnection;29;0;24;0
WireConnection;0;0;32;0
WireConnection;0;10;29;0
ASEEND*/
//CHKSM=5D3A8404D728D289CBA06477E116F121D46AF235