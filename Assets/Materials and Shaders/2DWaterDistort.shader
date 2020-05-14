Shader "Custom/2DWaterDistort"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_DisplacementTex("Displacement Texture", 2D) = "white" {}
		_Magnitude("Magnitude", Range(0,0.1)) = 1
		_DistortionSpeed("Distortion Speed", Float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

	

        Pass
        {
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
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
			sampler2D _DisplacementTex;
			float _Magnitude;
			float _DistortionSpeed;

			fixed4 frag(v2f i) : SV_Target
			{
				float s = _DistortionSpeed * _Time;
				float2 distuv = float2(i.uv.x + s * 2, i.uv.y + s * 2);
				float2 disp = tex2D(_DisplacementTex, distuv).xy;
				disp = ((disp * 2) - 1) * _Magnitude;
				float4 col = tex2D(_MainTex, i.uv + disp);
				//col *= float4(i.uv.x, i.uv.y, 1, 1);
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                return col;
            }
            ENDCG
        }
    }
}
