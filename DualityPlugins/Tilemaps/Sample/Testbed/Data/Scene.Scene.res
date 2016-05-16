<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">0</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="3761029062">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="664753936">
        <_items dataType="Array" type="Duality.Component[]" id="83719996">
          <item dataType="Struct" type="Duality.Components.Transform" id="1826376698">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">3761029062</gameobj>
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
            <vel dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">0</Z>
            </vel>
            <velAbs dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">0</Z>
            </velAbs>
          </item>
          <item dataType="Struct" type="Duality.Components.Camera" id="3337573">
            <active dataType="Bool">true</active>
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">3761029062</gameobj>
            <nearZ dataType="Float">0</nearZ>
            <passes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Camera+Pass]]" id="293189449">
              <_items dataType="Array" type="Duality.Components.Camera+Pass[]" id="79299470" length="4">
                <item dataType="Struct" type="Duality.Components.Camera+Pass" id="2606380240">
                  <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">0</A>
                    <B dataType="Byte">0</B>
                    <G dataType="Byte">0</G>
                    <R dataType="Byte">0</R>
                  </clearColor>
                  <clearDepth dataType="Float">1</clearDepth>
                  <clearFlags dataType="Enum" type="Duality.Drawing.ClearFlag" name="All" value="3" />
                  <input />
                  <matrixMode dataType="Enum" type="Duality.Drawing.RenderMatrix" name="PerspectiveWorld" value="0" />
                  <output dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]">
                    <contentPath />
                  </output>
                  <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                </item>
                <item dataType="Struct" type="Duality.Components.Camera+Pass" id="1919160942">
                  <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">0</A>
                    <B dataType="Byte">0</B>
                    <G dataType="Byte">0</G>
                    <R dataType="Byte">0</R>
                  </clearColor>
                  <clearDepth dataType="Float">1</clearDepth>
                  <clearFlags dataType="Enum" type="Duality.Drawing.ClearFlag" name="None" value="0" />
                  <input />
                  <matrixMode dataType="Enum" type="Duality.Drawing.RenderMatrix" name="OrthoScreen" value="1" />
                  <output dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]">
                    <contentPath />
                  </output>
                  <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="All" value="4294967295" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">2</_version>
            </passes>
            <perspective dataType="Enum" type="Duality.Drawing.PerspectiveMode" name="Parallax" value="1" />
            <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="All" value="4294967295" />
          </item>
          <item dataType="Struct" type="Duality.Components.SoundListener" id="119543137">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3761029062</gameobj>
          </item>
          <item dataType="Struct" type="Duality.Plugins.Tilemaps.Sample.RpgLike.CameraController" id="1844544486">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3761029062</gameobj>
            <smoothness dataType="Float">1</smoothness>
            <targetObj dataType="Struct" type="Duality.GameObject" id="1593919710">
              <active dataType="Bool">true</active>
              <children />
              <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3195412880">
                <_items dataType="Array" type="Duality.Component[]" id="420700476" length="8">
                  <item dataType="Struct" type="Duality.Components.Transform" id="3954234642">
                    <active dataType="Bool">true</active>
                    <angle dataType="Float">0</angle>
                    <angleAbs dataType="Float">0</angleAbs>
                    <angleVel dataType="Float">0</angleVel>
                    <angleVelAbs dataType="Float">0</angleVelAbs>
                    <deriveAngle dataType="Bool">true</deriveAngle>
                    <gameobj dataType="ObjectRef">1593919710</gameobj>
                    <ignoreParent dataType="Bool">false</ignoreParent>
                    <parentTransform />
                    <pos dataType="Struct" type="Duality.Vector3">
                      <X dataType="Float">0</X>
                      <Y dataType="Float">0</Y>
                      <Z dataType="Float">0</Z>
                    </pos>
                    <posAbs dataType="Struct" type="Duality.Vector3">
                      <X dataType="Float">0</X>
                      <Y dataType="Float">0</Y>
                      <Z dataType="Float">0</Z>
                    </posAbs>
                    <scale dataType="Float">1</scale>
                    <scaleAbs dataType="Float">1</scaleAbs>
                    <vel dataType="Struct" type="Duality.Vector3">
                      <X dataType="Float">0</X>
                      <Y dataType="Float">0</Y>
                      <Z dataType="Float">0</Z>
                    </vel>
                    <velAbs dataType="Struct" type="Duality.Vector3">
                      <X dataType="Float">0</X>
                      <Y dataType="Float">0</Y>
                      <Z dataType="Float">0</Z>
                    </velAbs>
                  </item>
                  <item dataType="Struct" type="Duality.Plugins.Tilemaps.Sample.RpgLike.ActorRenderer" id="3907431145">
                    <active dataType="Bool">true</active>
                    <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                      <A dataType="Byte">255</A>
                      <B dataType="Byte">255</B>
                      <G dataType="Byte">255</G>
                      <R dataType="Byte">255</R>
                    </colorTint>
                    <customMat />
                    <depthScale dataType="Float">0.01</depthScale>
                    <gameobj dataType="ObjectRef">1593919710</gameobj>
                    <height dataType="Float">0</height>
                    <isVertical dataType="Bool">true</isVertical>
                    <offset dataType="Float">-0.08</offset>
                    <rect dataType="Struct" type="Duality.Rect">
                      <H dataType="Float">48</H>
                      <W dataType="Float">32</W>
                      <X dataType="Float">-16</X>
                      <Y dataType="Float">-40</Y>
                    </rect>
                    <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                      <contentPath dataType="String">Data\Cylinder.Material.res</contentPath>
                    </sharedMat>
                    <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                  </item>
                  <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="361728938">
                    <active dataType="Bool">true</active>
                    <angularDamp dataType="Float">0.3</angularDamp>
                    <angularVel dataType="Float">0</angularVel>
                    <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                    <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                    <colFilter />
                    <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                    <continous dataType="Bool">false</continous>
                    <explicitInertia dataType="Float">0</explicitInertia>
                    <explicitMass dataType="Float">80</explicitMass>
                    <fixedAngle dataType="Bool">false</fixedAngle>
                    <gameobj dataType="ObjectRef">1593919710</gameobj>
                    <ignoreGravity dataType="Bool">false</ignoreGravity>
                    <joints />
                    <linearDamp dataType="Float">0.3</linearDamp>
                    <linearVel dataType="Struct" type="Duality.Vector2">
                      <X dataType="Float">0</X>
                      <Y dataType="Float">0</Y>
                    </linearVel>
                    <revolutions dataType="Float">0</revolutions>
                    <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3790767434">
                      <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2806249312" length="4">
                        <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="4276353244">
                          <density dataType="Float">1</density>
                          <friction dataType="Float">0.3</friction>
                          <parent dataType="ObjectRef">361728938</parent>
                          <position dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">0</X>
                            <Y dataType="Float">0</Y>
                          </position>
                          <radius dataType="Float">16</radius>
                          <restitution dataType="Float">0.3</restitution>
                          <sensor dataType="Bool">false</sensor>
                        </item>
                      </_items>
                      <_size dataType="Int">1</_size>
                      <_version dataType="Int">1</_version>
                    </shapes>
                  </item>
                  <item dataType="Struct" type="Duality.Plugins.Tilemaps.Sample.RpgLike.CharacterController" id="479935388">
                    <acceleration dataType="Float">0.15</acceleration>
                    <active dataType="Bool">true</active>
                    <gameobj dataType="ObjectRef">1593919710</gameobj>
                    <speed dataType="Float">5</speed>
                    <targetMovement dataType="Struct" type="Duality.Vector2">
                      <X dataType="Float">0</X>
                      <Y dataType="Float">0</Y>
                    </targetMovement>
                  </item>
                </_items>
                <_size dataType="Int">4</_size>
                <_version dataType="Int">6</_version>
              </compList>
              <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="111452910" surrogate="true">
                <header />
                <body>
                  <keys dataType="Array" type="System.Object[]" id="3864501218">
                    <item dataType="Type" id="4219969168" value="Duality.Components.Transform" />
                    <item dataType="Type" id="3361562350" value="Duality.Plugins.Tilemaps.Sample.RpgLike.ActorRenderer" />
                    <item dataType="Type" id="794497900" value="Duality.Components.Physics.RigidBody" />
                    <item dataType="Type" id="1223005970" value="Duality.Plugins.Tilemaps.Sample.RpgLike.CharacterController" />
                  </keys>
                  <values dataType="Array" type="System.Object[]" id="3342871690">
                    <item dataType="ObjectRef">3954234642</item>
                    <item dataType="ObjectRef">3907431145</item>
                    <item dataType="ObjectRef">361728938</item>
                    <item dataType="ObjectRef">479935388</item>
                  </values>
                </body>
              </compMap>
              <compTransform dataType="ObjectRef">3954234642</compTransform>
              <identifier dataType="Struct" type="System.Guid" surrogate="true">
                <header>
                  <data dataType="Array" type="System.Byte[]" id="967860754">6QaaXpvsP06njO2/8LNlCA==</data>
                </header>
                <body />
              </identifier>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
              <name dataType="String">MainChar</name>
              <parent />
              <prefabLink />
            </targetObj>
          </item>
        </_items>
        <_size dataType="Int">4</_size>
        <_version dataType="Int">4</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1778928878" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="1373440354">
            <item dataType="ObjectRef">4219969168</item>
            <item dataType="Type" id="1128403856" value="Duality.Components.Camera" />
            <item dataType="Type" id="3457661678" value="Duality.Components.SoundListener" />
            <item dataType="Type" id="3352197740" value="Duality.Plugins.Tilemaps.Sample.RpgLike.CameraController" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="105870730">
            <item dataType="ObjectRef">1826376698</item>
            <item dataType="ObjectRef">3337573</item>
            <item dataType="ObjectRef">119543137</item>
            <item dataType="ObjectRef">1844544486</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">1826376698</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1324124818">G/E04Bh2zE6yFHaIss4iEQ==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">MainCamera</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="10461599">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1388021341">
        <_items dataType="Array" type="Duality.GameObject[]" id="2170589030" length="4">
          <item dataType="Struct" type="Duality.GameObject" id="2961408854">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="965536794">
              <_items dataType="Array" type="Duality.Component[]" id="282188160" length="4">
                <item dataType="Struct" type="Duality.Plugins.Tilemaps.Tilemap" id="668293575">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2961408854</gameobj>
                  <tileData dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapData" id="2703622211" custom="true">
                    <body>
                      <version dataType="Int">2</version>
                      <data dataType="Array" type="System.Byte[]" id="455732262">H4sIAAAAAAAEAJ1Yy05UQRC9dyfqwufGB7nOMIioUXe6MNENEUwMD02UPzUacmcERRJ1gdEFCxcYZyGPX1DOWXCcQ1UaK+Gk09PVdbq66lY1TVVVzd+/fnUgA+AQeKY+wLNAl3OYPw+8AGwx3w+QO3+3faj1OsBIy633030oXTnLJYwvA68U8/8R8C/X6tRHW4920H1c/KR9YTWQmcgzx9WK5GOKG7a+FSuOzqQES7TUkz2Jh4kUe8BhMf914MYx+ZdoRZFwLcWO8actvaN3YmUKOH1M/q61alY2hMmE+P9GitxzXNZ3uQOQt9Om3JyJnzry9t161JZ6Ur8q5dIA9XbWAz5kTr/dEybdYq379agt5a/Cs/AbdQI4JqfTfOFd6+2QT2tMBsL8AXAG2BRrPapHbY2bzz0vtH5NFNwR+Xhe0Idk/hj4DDhVrBV5m9Ia/27KuRf8Sj6aF7e5vjpk/rAaRdWaFF3Vcm9rzJO/5rWvcc4aS7qyAd4C3gQyYusUG1nPOL9ufDqBP8nf85oScY5E80Ij1jmPydi17gDd83vAfeOv5/XMpZzEzCng6eKv1p7wnKtHT1Ebn5J9iFvAFpj3JxTv+i7KKfx+3WKOegrdbRO4jJldjLdlJXM/r4zMbu/6rhpn2t0MGKr1nfQUvwX3RNelW8w//45VZtG5bclKP2MUXX4uSkfqSAn/qIPNOWue7hesdy2NZI805n7ekwxsXBVw+D/MMzHKTe2W24C/VkZn7refR7uvd7t9mY/eBZxn1WAFiao5+8CeMYnylJJntKLa1Rjwd4F2s5zX2sGMZvXXas4+aloYRr7VPFWJ1u+KXY8BfxcMhGHPrDRA3gg7gbxrdd8q/xXBfL3aVf55zLDyap8Q9QBezSnMBX4xNHLWgG+A74Ef5Fe9hX2z6zHs8aydhktJz5x7fk2YK/+h6GoWqy5jibHBiJq0+Sgeov7HRb9CXh+V/1vgivGPzt4AGbd87+irx+NZe0v/z9sT4KzgHNG8p32L+19vwfl7VWLc8r2jrx6PZ/aTGjM6ngcuCC6K3agz+QT8bPgFSO/5t8hrtNZivaPZ+hC1n+wJ5vyVub9ovmL8DThjM0NhGFU0r3fkP4/xC+BTYNRPUl4Cl4DPgctHL/xHfgJ/AXn7OsPbj7rQvCY6fxWPIvqWt7BoK/N3TUmEKMMdGW8LqpD/EnAhiHl/LTp/SvSu0R288mpUewatGLq8qg/xD4/MCEEIGAAA</data>
                    </body>
                  </tileData>
                  <tileset dataType="Struct" type="Duality.ContentRef`1[[Duality.Plugins.Tilemaps.Tileset]]">
                    <contentPath dataType="String">Data\TestTiles.Tileset.res</contentPath>
                  </tileset>
                </item>
                <item dataType="Struct" type="Duality.Components.Transform" id="1026756490">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">2961408854</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapRenderer" id="1659517006">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <externalTilemap />
                  <gameobj dataType="ObjectRef">2961408854</gameobj>
                  <offset dataType="Float">0</offset>
                  <origin dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <tileDepthMode dataType="Enum" type="Duality.Plugins.Tilemaps.TileDepthOffsetMode" name="World" value="2" />
                  <tileDepthOffset dataType="Int">0</tileDepthOffset>
                  <tileDepthScale dataType="Float">0.01</tileDepthScale>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">3</_size>
              <_version dataType="Int">3</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4050640186" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="552656736">
                  <item dataType="Type" id="3198937308" value="Duality.Plugins.Tilemaps.Tilemap" />
                  <item dataType="ObjectRef">4219969168</item>
                  <item dataType="Type" id="27113750" value="Duality.Plugins.Tilemaps.TilemapRenderer" />
                </keys>
                <values dataType="Array" type="System.Object[]" id="2310733966">
                  <item dataType="ObjectRef">668293575</item>
                  <item dataType="ObjectRef">1026756490</item>
                  <item dataType="ObjectRef">1659517006</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1026756490</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="588166524">KMXuZW3ZOEaxigxAvZjLRg==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">BaseLayer</name>
            <parent dataType="ObjectRef">10461599</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3085774208">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="153351068">
              <_items dataType="Array" type="Duality.Component[]" id="2220440004" length="4">
                <item dataType="Struct" type="Duality.Plugins.Tilemaps.Tilemap" id="792658929">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">3085774208</gameobj>
                  <tileData dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapData" id="3306088781" custom="true">
                    <body>
                      <version dataType="Int">2</version>
                      <data dataType="Array" type="System.Byte[]" id="1707160614">H4sIAAAAAAAEAN2Yy05UQRCGh4uQIDARuURF5QEMK8aFC0kQZckrzMKFSxciidcEVMJFBYkmyALFiBC8QFAxJsZXOLyFj6H8tfmTSlWqZ85MVBK+VGqqumu6q7q7ZqBQKAz8+b9aOPw7+I+4RvL3RN9vdfFqdD99F5ir/quauib/FleJf0M8EbaR/IYomhvgBHgzceS1RPtqWFJyqeGQdyDfBe+RzSS47Y7pV1Aq/bksPgAfgo/oW9wH98BzDdHRNkh+mxjJHskXMGM35HbQqus+WI6BU/QteKeGAvG/BDPSZKSP5AZnyAhm7Id8yZ19EJ8WwTlo5gMzaq5TzMz1ikaLV/R5RN4KPoNmmfTs1U3yihozUxrWZ66vjqoyyl5cBIdV/P0kv1a+o+BlxStkI3X0KhwPV5CcjXKq+PeysBNztRi5xxnbQ3qdPxnZSB3pbGf2gn1Kz6eKFXMreARsIY1lL1Gddm0OyKYnYHkGPGt8at04fDp1gJ3hqLR+MdE+kj/C+I1TJLkc9pLMnE2MU9hUkVeE1xLtl2oWSYT6prvu2pcNfWQ99Stdz25Vev13x+op/L5G9B8TozoatvR3Jy9+zXW0yA2SL/Wt8R78QJoNZZMXuRY+hb18y8/glyqi+qE0PwNe++HxLUu9F7Wu8cpm4Th1nzhpeFnvt7yY2teMg7pPlLcQv5NvgbfJV5/h/itX3ieRLEql7hNnlI30X9NVzBJ/nxwHdUZZr9MF8DH4BHwKjtH6W/1XLV4mpwy9vneaQcmT55BfgIMqw7n/Ek0j+eq5NsGtxMjLOa1AhFbV/wJ3wF2y93sTofXe8HfZOnmKam15ta2q76VfLeKrIbvpvzeOgV1Kr2MQPWdRB8nS0ftVrzMwQn3rCeXUOgGeBPkXg9+jWz4rCBgAAA==</data>
                    </body>
                  </tileData>
                  <tileset dataType="Struct" type="Duality.ContentRef`1[[Duality.Plugins.Tilemaps.Tileset]]">
                    <contentPath dataType="String">Data\TestTiles.Tileset.res</contentPath>
                  </tileset>
                </item>
                <item dataType="Struct" type="Duality.Components.Transform" id="1151121844">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">3085774208</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapRenderer" id="1783882360">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <externalTilemap />
                  <gameobj dataType="ObjectRef">3085774208</gameobj>
                  <offset dataType="Float">-0.01</offset>
                  <origin dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <tileDepthMode dataType="Enum" type="Duality.Plugins.Tilemaps.TileDepthOffsetMode" name="World" value="2" />
                  <tileDepthOffset dataType="Int">0</tileDepthOffset>
                  <tileDepthScale dataType="Float">0.01</tileDepthScale>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">3</_size>
              <_version dataType="Int">3</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2608161814" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3739445558">
                  <item dataType="ObjectRef">3198937308</item>
                  <item dataType="ObjectRef">4219969168</item>
                  <item dataType="ObjectRef">27113750</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2000748186">
                  <item dataType="ObjectRef">792658929</item>
                  <item dataType="ObjectRef">1151121844</item>
                  <item dataType="ObjectRef">1783882360</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1151121844</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="624963030">OGFE/6l2ukykXD788OcpAg==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">UpperLayer</name>
            <parent dataType="ObjectRef">10461599</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="1404051932">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3492662136">
              <_items dataType="Array" type="Duality.Component[]" id="2212659052" length="4">
                <item dataType="Struct" type="Duality.Plugins.Tilemaps.Tilemap" id="3405903949">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">1404051932</gameobj>
                  <tileData dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapData" id="2980221569" custom="true">
                    <body>
                      <version dataType="Int">2</version>
                      <data dataType="Array" type="System.Byte[]" id="3362391342">H4sIAAAAAAAEAO2YMU7DQBBFd4H0gCgCosgBQhcqJFIgajookKDhEKGDW3EMcxMSsAQGS5A/SIwYrRmDbX7hSHnarEbJm/VsvONRCGH08b4Ky9d9z549wQdwDi7UzILAzcNn8AUs1IyMH8GcwFO4AW6CW4oxLrkS5cPX+BUTJYG5cBvcAXcVM1BW+wnM1Xz2r84H8eeYoFb7DSxp/I/gdueIXEXkGjhwZN092/5XWa85X5dFy/5D8Nhw2ND32x3aRhZZgjpm/1f1aXfouSJDhXvqP2XLn0vh9ufMotqQ2X8cv1t5smAwF07c/sxZ2AphM/xL/dtIBmfP+lvOwGsC57r+EnkD3hI4a06xi23PyFwzmnLmsT0jg5uHcuaRbkX3jAxumnK32jNuA1U/PE8Y7Fl0Urmq+glDN4YXBFe5uncYd2KYO0xStL3D59UH9RXX9XCG8UlD2ZWVJinq/VK3u79E5GlD/vOEySE4TfyK3i+pe0c3fAfKgV1QCBgAAA==</data>
                    </body>
                  </tileData>
                  <tileset dataType="Struct" type="Duality.ContentRef`1[[Duality.Plugins.Tilemaps.Tileset]]">
                    <contentPath dataType="String">Data\TestTiles.Tileset.res</contentPath>
                  </tileset>
                </item>
                <item dataType="Struct" type="Duality.Components.Transform" id="3764366864">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">1404051932</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapRenderer" id="102160084">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <externalTilemap />
                  <gameobj dataType="ObjectRef">1404051932</gameobj>
                  <offset dataType="Float">-0.02</offset>
                  <origin dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <tileDepthMode dataType="Enum" type="Duality.Plugins.Tilemaps.TileDepthOffsetMode" name="World" value="2" />
                  <tileDepthOffset dataType="Int">0</tileDepthOffset>
                  <tileDepthScale dataType="Float">0.01</tileDepthScale>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">3</_size>
              <_version dataType="Int">3</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="40942558" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="756083130">
                  <item dataType="ObjectRef">3198937308</item>
                  <item dataType="ObjectRef">4219969168</item>
                  <item dataType="ObjectRef">27113750</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="876787130">
                  <item dataType="ObjectRef">3405903949</item>
                  <item dataType="ObjectRef">3764366864</item>
                  <item dataType="ObjectRef">102160084</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3764366864</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2147107258">/2KPfz/Jj0O7HOXC4QkFrA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">TopLayer</name>
            <parent dataType="ObjectRef">10461599</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">3</_size>
        <_version dataType="Int">5</_version>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2081326968">
        <_items dataType="Array" type="Duality.Component[]" id="1387570487" length="0" />
        <_size dataType="Int">0</_size>
        <_version dataType="Int">0</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="410896887" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="232855956" length="0" />
          <values dataType="Array" type="System.Object[]" id="1978646582" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1700514864">HunZh0b630iicX0zyp3aXg==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Map</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">1593919710</item>
    <item dataType="Struct" type="Duality.GameObject" id="3851204924">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="367255378">
        <_items dataType="Array" type="Duality.Component[]" id="2959980880" length="4">
          <item dataType="Struct" type="Duality.Plugins.Tilemaps.Sample.RpgLike.Player" id="1378947066">
            <active dataType="Bool">true</active>
            <character dataType="ObjectRef">479935388</character>
            <gameobj dataType="ObjectRef">3851204924</gameobj>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
        <_version dataType="Int">1</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1943215818" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="4114983048">
            <item dataType="Type" id="1898588012" value="Duality.Plugins.Tilemaps.Sample.RpgLike.Player" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="527819742">
            <item dataType="ObjectRef">1378947066</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="4191128180">IffF10lNek6ct3+h8WBmZw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Player</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">2961408854</item>
    <item dataType="ObjectRef">3085774208</item>
    <item dataType="ObjectRef">1404051932</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
