uniform float time;
uniform vec2 center;
uniform float radius;
const float amplitude = 10.0;
const float frequency = 15.0;
const float speed = 150.0;

void main()
{
    vec2 pos = gl_FragCoord.xy - center;
    float dist = length(pos);

    if (dist < radius)
    {
        float amplitudeSin = amplitude * sin(time * speed + dist * frequency);
        vec4 color = gl_Color;
        color.a *= abs(amplitudeSin / radius);
        gl_FragColor = color;
    }
    else
    {
        gl_FragColor = gl_Color;
    }
}
