<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">33</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="3588826678">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3432301344">
        <_items dataType="Array" type="Duality.Component[]" id="1392996316" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="1654174314">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">3588826678</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <parentTransform />
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
            <vel dataType="Struct" type="Duality.Vector3" />
            <velAbs dataType="Struct" type="Duality.Vector3" />
          </item>
          <item dataType="Struct" type="Duality.Components.Camera" id="4126102485">
            <active dataType="Bool">true</active>
            <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">0</A>
              <B dataType="Byte">154</B>
              <G dataType="Byte">129</G>
              <R dataType="Byte">99</R>
            </clearColor>
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">3588826678</gameobj>
            <nearZ dataType="Float">0</nearZ>
            <perspective dataType="Enum" type="Duality.Drawing.PerspectiveMode" name="Parallax" value="1" />
            <priority dataType="Int">0</priority>
            <renderSetup dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderSetup]]" />
            <renderTarget dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]" />
            <targetRect dataType="Struct" type="Duality.Rect">
              <H dataType="Float">1</H>
              <W dataType="Float">1</W>
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
            </targetRect>
            <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="All" value="4294967295" />
          </item>
          <item dataType="Struct" type="Duality.Components.SoundListener" id="4242308049">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3588826678</gameobj>
          </item>
        </_items>
        <_size dataType="Int">3</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4076619662" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="726969330">
            <item dataType="Type" id="3939607504" value="Duality.Components.Transform" />
            <item dataType="Type" id="1077793390" value="Duality.Components.Camera" />
            <item dataType="Type" id="2908634028" value="Duality.Components.SoundListener" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="3096052554">
            <item dataType="ObjectRef">1654174314</item>
            <item dataType="ObjectRef">4126102485</item>
            <item dataType="ObjectRef">4242308049</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">1654174314</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2922393794">FmbIdiGcaECC4D9FoGgPSA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">MainCamera</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="906060036">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3876133818">
        <_items dataType="Array" type="Duality.Component[]" id="3889524736" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="3266374968">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">906060036</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <parentTransform />
            <pos dataType="Struct" type="Duality.Vector3" />
            <posAbs dataType="Struct" type="Duality.Vector3" />
            <scale dataType="Float">1</scale>
            <scaleAbs dataType="Float">1</scaleAbs>
            <vel dataType="Struct" type="Duality.Vector3" />
            <velAbs dataType="Struct" type="Duality.Vector3" />
          </item>
          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2548226604">
            <active dataType="Bool">true</active>
            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">128</B>
              <G dataType="Byte">188</G>
              <R dataType="Byte">255</R>
            </colorTint>
            <customMat />
            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
            <gameobj dataType="ObjectRef">906060036</gameobj>
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
          <item dataType="Struct" type="CameraController.PlayerCharacter" id="3170648315">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">906060036</gameobj>
            <speed dataType="Float">5</speed>
          </item>
        </_items>
        <_size dataType="Int">3</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1212267962" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2433954304">
            <item dataType="ObjectRef">3939607504</item>
            <item dataType="Type" id="2411138204" value="Duality.Components.Renderers.SpriteRenderer" />
            <item dataType="Type" id="1131121174" value="CameraController.PlayerCharacter" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="2575663054">
            <item dataType="ObjectRef">3266374968</item>
            <item dataType="ObjectRef">2548226604</item>
            <item dataType="ObjectRef">3170648315</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">3266374968</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3307205276">vytfBF4aBEqzNygTOMMzvw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">TargetCharacter</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="2253730436">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2684401466">
        <_items dataType="Array" type="Duality.GameObject[]" id="783780096" length="4">
          <item dataType="Struct" type="Duality.GameObject" id="2372995991">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2083117683">
              <_items dataType="Array" type="Duality.Component[]" id="3552646438" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="438343627">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">2372995991</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3" />
                  <posAbs dataType="Struct" type="Duality.Vector3" />
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="4015162559">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">2372995991</gameobj>
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1192659896" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="852773145">
                  <item dataType="ObjectRef">3939607504</item>
                  <item dataType="ObjectRef">2411138204</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="1552232320">
                  <item dataType="ObjectRef">438343627</item>
                  <item dataType="ObjectRef">4015162559</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">438343627</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1848791899">+ExNizz3cEe0UfGBqwttHw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Background</name>
            <parent dataType="ObjectRef">2253730436</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2751314020">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="45343148">
              <_items dataType="Array" type="Duality.Component[]" id="1137579236" length="4">
                <item dataType="Struct" type="CameraController.ExampleSceneController" id="200078843">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2751314020</gameobj>
                  <mainCameraObj dataType="ObjectRef">3588826678</mainCameraObj>
                  <targetObj dataType="ObjectRef">906060036</targetObj>
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3187170230" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2575456230">
                  <item dataType="Type" id="1934551424" value="CameraController.ExampleSceneController" />
                </keys>
                <values dataType="Array" type="System.Object[]" id="2541096250">
                  <item dataType="ObjectRef">200078843</item>
                </values>
              </body>
            </compMap>
            <compTransform />
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3166683494">8vAve9JI4Uimq5DmExUldw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">ExampleSceneController</name>
            <parent dataType="ObjectRef">2253730436</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">2</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2184748730">
        <_items dataType="Array" type="Duality.Component[]" id="4289085568" length="0" />
        <_size dataType="Int">0</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3346030650" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2568592896" length="0" />
          <values dataType="Array" type="System.Object[]" id="1888829390" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2797805212">sXPEN5GpdUGA1RgdUkw6QA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">System</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">2372995991</item>
    <item dataType="ObjectRef">2751314020</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
