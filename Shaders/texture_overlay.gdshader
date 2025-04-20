shader_type canvas_item;

uniform sampler2D overlay_tex : repeat_enable, filter_nearest;
uniform float    scale        = 0.006944444;                     // 1.0 / texture_size

uniform vec4     key_color    : source_color = vec4(1.0, 0.0, 0.0, 1.0);

uniform float    tolerance    : hint_range(0.0, 1.0) = 0.05;

uniform bool     use_overlay_alpha = true;

varying vec2 world_position;

void vertex() {
    world_position = (MODEL_MATRIX * vec4(VERTEX, 0.0, 1.0)).xy;
}

void fragment() {
    float dist = distance(COLOR.rgb, key_color.rgb);

    float match = step(dist, tolerance);

    vec4 overlay_color = texture(overlay_tex, world_position * scale);

    float overlay_alpha = use_overlay_alpha ? overlay_color.a : 1.0;

    float mix_amount = match * overlay_alpha;        // 0‑1
    COLOR = mix(COLOR, overlay_color, mix_amount);
}
