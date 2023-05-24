#version 450
uniform float wave_phase;
uniform vec2 wave_amplitude;
in vec2 in_vertex;
out gl_PerVertex
{
    vec4 gl_Position;
};
void main()
{
    vec4 vertex = vec4(in_vertex, 0.0, 1.0);

    vertex.x += cos(in_vertex.y * 0.02 + wave_phase * 3.8) * wave_amplitude.x
        + sin(in_vertex.y * 0.02 + wave_phase * 6.3) * wave_amplitude.x * 0.3;

    vertex.y += cos(in_vertex.x * 0.02 + wave_phase * 3.8) * wave_amplitude.y
        + sin(in_vertex.x * 0.02 + wave_phase * 6.3) * wave_amplitude.y * 0.3;
    
    gl_Position = vertex;
}