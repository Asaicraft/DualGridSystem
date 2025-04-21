shader_type canvas_item;

uniform sampler2D overlay_tex_a : repeat_enable, filter_nearest;
uniform vec3      key_color_a   : source_color = vec3(1.0, 0.0, 0.0);
uniform float     tolerance_a   : hint_range(0.0, 1.0) = 0.05;

uniform sampler2D overlay_tex_b : repeat_enable, filter_nearest;
uniform vec3      key_color_b   : source_color = vec3(0.0, 1.0, 0.0);
uniform float     tolerance_b   : hint_range(0.0, 1.0) = 0.05;

uniform float    scale             = 0.006944444; // 1.0 / texture_size
uniform bool     use_overlay_alpha = true;

varying vec2 world_position;

void vertex() {
    world_position = (MODEL_MATRIX * vec4(VERTEX, 0.0, 1.0)).xy;
}

void fragment() {
    vec3 src = COLOR.rgb;

    float dist_a    = distance(src, key_color_a);
    float match_a   = step(dist_a, tolerance_a);
    vec4 overlay_a  = texture(overlay_tex_a, world_position * scale);
    float alpha_a   = use_overlay_alpha ? overlay_a.a : 1.0;
    float mix_a     = match_a * alpha_a;
    vec3 result_col = mix(src, overlay_a.rgb, mix_a);

    float dist_b    = distance(src, key_color_b);
    float match_b   = step(dist_b, tolerance_b);
    vec4 overlay_b  = texture(overlay_tex_b, world_position * scale);
    float alpha_b   = use_overlay_alpha ? overlay_b.a : 1.0;
    float mix_b     = match_b * alpha_b;
    result_col      = mix(result_col, overlay_b.rgb, mix_b);

    COLOR = vec4(result_col, COLOR.a);
}
