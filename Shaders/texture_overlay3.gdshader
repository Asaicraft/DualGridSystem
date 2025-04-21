shader_type canvas_item;

uniform sampler2D overlay_tex_a : repeat_enable, filter_nearest;
uniform vec3      key_color_a   : source_color = vec3(1.0, 0.0, 0.0);
uniform float     tolerance_a   : hint_range(0.0, 1.0) = 0.05;

uniform sampler2D overlay_tex_b : repeat_enable, filter_nearest;
uniform vec3      key_color_b   : source_color = vec3(0.0, 1.0, 0.0);
uniform float     tolerance_b   : hint_range(0.0, 1.0) = 0.05;

uniform sampler2D overlay_tex_c : repeat_enable, filter_nearest;
uniform vec3      key_color_c   : source_color = vec3(0.0, 0.0, 1.0);
uniform float     tolerance_c   : hint_range(0.0, 1.0) = 0.05;

uniform float    scale             = 0.006944444; // 1.0 / texture_size
uniform bool     use_overlay_alpha = true;

varying vec2 world_position;

void vertex() {
    world_position = (MODEL_MATRIX * vec4(VERTEX, 0.0, 1.0)).xy;
}

void fragment() {
    vec3 current = COLOR.rgb;

    float dist_a   = distance(current, key_color_a);
    float match_a  = step(dist_a, tolerance_a);
    vec4 overlay_a = texture(overlay_tex_a, world_position * scale);
    float alpha_a  = use_overlay_alpha ? overlay_a.a : 1.0;
    current        = mix(current, overlay_a.rgb, match_a * alpha_a);

    float dist_b   = distance(COLOR.rgb, key_color_b);
    float match_b  = step(dist_b, tolerance_b);
    vec4 overlay_b = texture(overlay_tex_b, world_position * scale);
    float alpha_b  = use_overlay_alpha ? overlay_b.a : 1.0;
    current        = mix(current, overlay_b.rgb, match_b * alpha_b);

    float dist_c   = distance(COLOR.rgb, key_color_c);
    float match_c  = step(dist_c, tolerance_c);
    vec4 overlay_c = texture(overlay_tex_c, world_position * scale);
    float alpha_c  = use_overlay_alpha ? overlay_c.a : 1.0;
    current        = mix(current, overlay_c.rgb, match_c * alpha_c);

    COLOR = vec4(current, COLOR.a);
}
