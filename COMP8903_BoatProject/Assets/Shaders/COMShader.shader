Shader "Custom/COMShader" {
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_COMPosition("COM Pos", Vector) = (0,0,0,0) //marker position
		_CircleRadius("COM Size", Range(0, 20)) = 3
		_RingSize("Ring Size", Range(0,5)) = 1
		_ColorTint("Outside Spotlight Color", Color) = (1,1,1,0)
		_ColorTint2("Inside Spotlight Color", Color) = (1,1,1,0)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha // use alpha blending
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _COMPosition;
			float4 _MainTex_ST;
			float _CircleRadius;
			float _RingSize;
			float4 _ColorTint;
			float4 _ColorTint2;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = _ColorTint;

				float dist = distance(i.worldPos, _COMPosition.xyz);

				//update pixel based on position in COM
				if(dist < _CircleRadius){
					col = _ColorTint2;

				} else if(dist > _CircleRadius && dist < _CircleRadius + _RingSize){
					float blendStr = dist - _CircleRadius;
					col = lerp(_ColorTint2, _ColorTint, blendStr / _RingSize);
				}
				return col;
			}
			ENDCG
		}
	}
}
