Shader "Custom/ZoneTest"
{
    Properties
    {
        _Center("Center", Vector) = (0, 0, 0, 0)
        _Radius("Radius", Range(0, 1)) = 0.5
        _Color("Color", Color) = (1, 1, 1, 1)
    }

        SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 200

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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            float4 _Center;
            float _Radius;
            float4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.vertex.xy * 0.5 + 0.5;

                float2 center = _Center.xy;
                float radius = _Radius;

                // Calculate the distance from the center
                float dist = length(uv - center);

                // Check if the pixel is inside the circle
                if (dist <= radius)
                {
                    // Inside the circle, return transparent color
                    return float4(0, 0, 0, 0);
                }
                else
                {
                    // Outside the circle, return the specified color
                    return _Color;
                }
            }

            ENDCG
        }
    }
}