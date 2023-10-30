Shader "Unlit/TestZone"
{
    Properties{
      _InnerRadius("Inner Radius", Range(0, 1)) = 0.5
      _OuterColor("Outer Color", Color) = (1, 0, 0, 1)
    }

        SubShader{
            Tags { "RenderType" = "Opaque" }

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                };

                struct v2f {
                    float4 vertex : SV_POSITION;
                };

                float _InnerRadius;
                float4 _OuterColor;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    float2 uv = i.vertex.xy / i.vertex.w;
                    float2 center = float2(0.5, 0.5);

                    // Check if the pixel is within the inner radius
                    if (distance(uv, center) <= _InnerRadius) {
                        // Transparent inside the zone
                        return fixed4(0, 0, 0, 0);
                    }
                    else {
                        // Solid color outside the zone
                        return _OuterColor;
                    }
                }
                ENDCG
            }
    }
        FallBack "Diffuse"
}

