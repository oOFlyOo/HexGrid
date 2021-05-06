Shader "Hex/HexRenderer"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MinSize ("Min Size", Range(0,1)) = 0
        _MaxSize ("Max Size", Range(0,1)) = 1
        _Internal ("Internal", Range(1, 10)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                half2 oVertex : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            half4 _Color;
            half _MaxSize; 
            half _MinSize; 
            half _Internal;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.oVertex = half2(v.vertex.x, v.vertex.z);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _Color;
                
                half maxSize = max(_MaxSize, _MinSize);
                maxSize = saturate(maxSize);
                half minSize = min(_MaxSize, _MinSize);
                minSize = saturate(minSize);
                minSize = min(minSize, maxSize - 0.01);
                minSize = max(0, minSize);
                maxSize = max(maxSize, minSize + 0.01);
                clip(min(maxSize - i.uv.x, i.uv.x - minSize));
                
                half2 left = half2(-1, 0);
                half2 right = half2(1, 0);
                half angle = 0;
                if (i.oVertex.y <= 0)
                {
                    angle = acos(dot(normalize(i.oVertex), right));
                }
                else
                {
                    angle = acos(dot(normalize(i.oVertex), left)) + 3.14;
                }
                angle = angle / 3.14 / 2;
                angle = angle * 100;
                clip(1 - angle % _Internal);
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
