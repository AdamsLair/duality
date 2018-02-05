<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">33</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="1050330856">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1379338798">
        <_items dataType="Array" type="Duality.Component[]" id="2630271824">
          <item dataType="Struct" type="Duality.Components.Transform" id="1107608074">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <gameobj dataType="ObjectRef">1050330856</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <pos dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">-500</Z>
            </pos>
            <posAbs dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">-500</Z>
            </posAbs>
            <scale dataType="Float">1</scale>
            <scaleAbs dataType="Float">1</scaleAbs>
          </item>
          <item dataType="Struct" type="Duality.Components.Camera" id="2596717333">
            <active dataType="Bool">true</active>
            <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">0</A>
              <B dataType="Byte">154</B>
              <G dataType="Byte">129</G>
              <R dataType="Byte">99</R>
            </clearColor>
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">1050330856</gameobj>
            <nearZ dataType="Float">50</nearZ>
            <priority dataType="Int">0</priority>
            <projection dataType="Enum" type="Duality.Drawing.ProjectionMode" name="Perspective" value="1" />
            <renderSetup dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderSetup]]" />
            <renderTarget dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]" />
            <shaderParameters dataType="Struct" type="Duality.Drawing.ShaderParameterCollection" id="3256541825" custom="true">
              <body />
            </shaderParameters>
            <targetRect dataType="Struct" type="Duality.Rect">
              <H dataType="Float">1</H>
              <W dataType="Float">1</W>
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
            </targetRect>
            <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="All" value="4294967295" />
          </item>
          <item dataType="Struct" type="Duality.Components.VelocityTracker" id="3121465323">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1050330856</gameobj>
          </item>
          <item dataType="Struct" type="Duality.Components.SoundListener" id="3082983383">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1050330856</gameobj>
          </item>
        </_items>
        <_size dataType="Int">4</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1262677194" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3120556716">
            <item dataType="Type" id="1096038116" value="Duality.Components.Transform" />
            <item dataType="Type" id="1326986774" value="Duality.Components.Camera" />
            <item dataType="Type" id="2541007072" value="Duality.Components.SoundListener" />
            <item dataType="Type" id="2313505378" value="Duality.Components.VelocityTracker" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="2486870454">
            <item dataType="ObjectRef">1107608074</item>
            <item dataType="ObjectRef">2596717333</item>
            <item dataType="ObjectRef">3082983383</item>
            <item dataType="ObjectRef">3121465323</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">1107608074</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1793785080">FmbIdiGcaECC4D9FoGgPSA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">MainCamera</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="2941461157">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="944333527">
        <_items dataType="Array" type="Duality.Component[]" id="4084004878" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="2998738375">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <gameobj dataType="ObjectRef">2941461157</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <pos dataType="Struct" type="Duality.Vector3" />
            <posAbs dataType="Struct" type="Duality.Vector3" />
            <scale dataType="Float">1</scale>
            <scaleAbs dataType="Float">1</scaleAbs>
          </item>
          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="115113141">
            <active dataType="Bool">true</active>
            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">128</B>
              <G dataType="Byte">188</G>
              <R dataType="Byte">255</R>
            </colorTint>
            <customMat />
            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
            <gameobj dataType="ObjectRef">2941461157</gameobj>
            <offset dataType="Float">-1</offset>
            <pixelGrid dataType="Bool">false</pixelGrid>
            <rect dataType="Struct" type="Duality.Rect">
              <H dataType="Float">64</H>
              <W dataType="Float">64</W>
              <X dataType="Float">-32</X>
              <Y dataType="Float">-32</Y>
            </rect>
            <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
            <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
              <contentPath dataType="String">Data\CameraController\Visuals\TargetCharacter.Material.res</contentPath>
            </sharedMat>
            <spriteIndex dataType="Int">-1</spriteIndex>
            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          </item>
          <item dataType="Struct" type="CameraController.PlayerCharacter" id="3986049436">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">2941461157</gameobj>
            <speed dataType="Float">5</speed>
          </item>
        </_items>
        <_size dataType="Int">3</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1692304832" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2943016797">
            <item dataType="ObjectRef">1096038116</item>
            <item dataType="Type" id="2534126950" value="Duality.Components.Renderers.SpriteRenderer" />
            <item dataType="Type" id="4063824698" value="CameraController.PlayerCharacter" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="1071022456">
            <item dataType="ObjectRef">2998738375</item>
            <item dataType="ObjectRef">115113141</item>
            <item dataType="ObjectRef">3986049436</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">2998738375</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="724993271">vytfBF4aBEqzNygTOMMzvw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">TargetCharacter</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="3963289781">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="302503879">
        <_items dataType="Array" type="Duality.GameObject[]" id="3050997966" length="4">
          <item dataType="Struct" type="Duality.GameObject" id="3570972695">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1242683879">
              <_items dataType="Array" type="Duality.Component[]" id="3806831950" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="3628249913">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">3570972695</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <pos dataType="Struct" type="Duality.Vector3" />
                  <posAbs dataType="Struct" type="Duality.Vector3" />
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="744624679">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">3570972695</gameobj>
                  <offset dataType="Float">1</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">8192</H>
                    <W dataType="Float">8192</W>
                    <X dataType="Float">-4096</X>
                    <Y dataType="Float">-4096</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="WrapBoth" value="3" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\CameraController\Visuals\BackgroundTile.Material.res</contentPath>
                  </sharedMat>
                  <spriteIndex dataType="Int">-1</spriteIndex>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3423249280" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1654578893">
                  <item dataType="ObjectRef">1096038116</item>
                  <item dataType="ObjectRef">2534126950</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="614035128">
                  <item dataType="ObjectRef">3628249913</item>
                  <item dataType="ObjectRef">744624679</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3628249913</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="920078503">+ExNizz3cEe0UfGBqwttHw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Background</name>
            <parent dataType="ObjectRef">3963289781</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="1288488042">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="676118830">
              <_items dataType="Array" type="Duality.Component[]" id="136701776" length="4">
                <item dataType="Struct" type="CameraController.ExampleSceneController" id="1090088597">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">1288488042</gameobj>
                  <mainCameraObj dataType="ObjectRef">1050330856</mainCameraObj>
                  <targetObj dataType="ObjectRef">2941461157</targetObj>
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2191466698" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3987202988">
                  <item dataType="Type" id="2906500324" value="CameraController.ExampleSceneController" />
                </keys>
                <values dataType="Array" type="System.Object[]" id="3532608438">
                  <item dataType="ObjectRef">1090088597</item>
                </values>
              </body>
            </compMap>
            <compTransform />
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1571574776">8vAve9JI4Uimq5DmExUldw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">ExampleSceneController</name>
            <parent dataType="ObjectRef">3963289781</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">2</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="747954432">
        <_items dataType="Array" type="Duality.Component[]" id="2664724589" length="0" />
        <_size dataType="Int">0</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2310451269" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3195527956" length="0" />
          <values dataType="Array" type="System.Object[]" id="2905313590" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1137903024">sXPEN5GpdUGA1RgdUkw6QA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">System</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">3570972695</item>
    <item dataType="ObjectRef">1288488042</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
