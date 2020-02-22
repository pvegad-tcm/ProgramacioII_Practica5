﻿uniform vec2 offset;

void main()
{
  gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
  gl_TexCoord[0] = gl_TextureMatrix[0] * gl_MultiTexCoord0;
  gl_TexCoord[0].xy += offset;

  gl_FrontColor = gl_Color;
}

