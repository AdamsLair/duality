<?xml version="1.0" encoding="utf-8"?>
<root>
  <object dataType="Class" type="Duality.Resources.VertexShader" id="1">
    <source dataType="String">uniform float _camRefDist;
uniform vec3 _camWorldPos;

varying vec3 worldSpacePos;
varying mat2 objTransform;

attribute vec4 objTrAttrib;

void main()
{
	gl_Position = ftransform();
	gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_FrontColor = gl_Color;
	
	float camDistScaleInv = gl_Vertex.z / _camRefDist;
	worldSpacePos = _camWorldPos + vec3(gl_Vertex.xy * camDistScaleInv, gl_Vertex.z);
	
	objTransform = mat2(objTrAttrib.x, objTrAttrib.y, objTrAttrib.z, objTrAttrib.w);
}</source>
    <sourcePath dataType="String">Source\Media\Light.vert</sourcePath>
  </object>
</root>