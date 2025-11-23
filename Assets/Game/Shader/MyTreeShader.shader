Shader "Unlit/MyTreeShader"
{
    Properties
    {
        _MainTex("纹理图片", 2D) = "white" {}
        _Strength("摇摆幅度", Float) = 1
        _Speed("摇摆速度", Float) = 3
        _AoColor("基础色", Color) = (1,1,1)
        _ShadowColor("阴影色", Color) = (1,1,1)
        _Specular("高光色", Color) = (1,1,1)
        _Gloss("光泽度", Float) = 1
    }
        SubShader
        {
            Pass
            {
                //指明该Pass的光照模式
                Tags {"LightMode" = "ForwardBase"}
                Cull Off
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
            //包含进Unity的内置变量
            #include "UnityCG.cginc" 
            #include "Lighting.cginc"
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR0;
                float3 normal: NORMAL;
            };
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float4 color: TEXCOORD1;
                float3 worldNormal: TEXCOORD2;
                float3 worldPos : TEXCOORD3;
            };
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed3 _AoColor;
            float _Speed;
            float _Strength;
            fixed3 _ShadowColor;
            fixed3 _Specular;
            float _Gloss;

            v2f vert(appdata v)
            {
                v2f o;
                float3 worldPos = UnityObjectToWorldDir(v.vertex);

                float stage1 = dot(v.vertex, float3(0, 1, 0)) * _Strength;
                float stage2 = sin(dot(v.vertex, float3(1, 0, 0)) * _Strength + _Time.y * _Speed);
                float3 stage3 = stage1 * stage2 * float3(0.001, 0, 0.001) * v.color.a;
                o.pos = UnityObjectToClipPos(v.vertex + stage3);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                o.worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }
            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                clip(col.a - 0.5);
                // 世界空间下的法线
                fixed3 worldNormal = i.worldNormal;
                // 世界空间下光源方向
                fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
                // 漫反射
                fixed3 diffuse = _LightColor0.rgb * col.rgb * _AoColor
                * lerp(_ShadowColor, float3(1,1,1), i.color.rgb)
                * (dot(worldNormal, worldLightDir) * 0.5 + 0.5);
                // 环境光
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * col.rgb;
                // 世界空间下的视角方向
                fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
                // 世界空间下的half方向
                fixed3 halfDir = normalize(worldLightDir + viewDir);
                // 高光
                fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(saturate(dot(halfDir, worldNormal)), _Gloss);

                return fixed4(diffuse + ambient + specular, col.a);
            }
            ENDCG
        }
        }
}
