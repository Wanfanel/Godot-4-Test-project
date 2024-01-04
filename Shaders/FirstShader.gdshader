shader_type canvas_item;

void fragment() {
// COLOR = vec4(0.4, 0.6, 0.9, 1.0);
//  COLOR = vec4(UV, 0.5, 1.0);
COLOR = texture(TEXTURE, UV);
COLOR.b = 1.0;
}

void vertex() {
  VERTEX += vec2(float((int(TIME*10.0)) % 3)-1.0, 0.0);
}
