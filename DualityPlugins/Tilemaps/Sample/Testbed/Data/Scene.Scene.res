<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">33</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="3761029062">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="664753936">
        <_items dataType="Array" type="Duality.Component[]" id="83719996" length="4">
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
        </_items>
        <_size dataType="Int">3</_size>
        <_version dataType="Int">3</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1778928878" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="1373440354">
            <item dataType="Type" id="1128403856" value="Duality.Components.Transform" />
            <item dataType="Type" id="3457661678" value="Duality.Components.Camera" />
            <item dataType="Type" id="3352197740" value="Duality.Components.SoundListener" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="105870730">
            <item dataType="ObjectRef">1826376698</item>
            <item dataType="ObjectRef">3337573</item>
            <item dataType="ObjectRef">119543137</item>
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
    <item dataType="Struct" type="Duality.GameObject" id="1204324351">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2004972541">
        <_items dataType="Array" type="Duality.Component[]" id="1072938278" length="4">
          <item dataType="Struct" type="Duality.Plugins.Tilemaps.Tilemap" id="3206176368">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1204324351</gameobj>
            <tileData dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapData" id="2854444812" custom="true">
              <body>
                <version dataType="Int">2</version>
                <data dataType="Array" type="System.Byte[]" id="2024138916">H4sIAAAAAAAEAO3W07IYBhSG0SSNnQaNnSaN09h20tg8QWM7tW3btt2eU9u2bbudWZk+Qu/+fbFmP8CePd+iYsWKLSr23xRnCe7EkizF0izDsizH8qzAiqzEyqzCqqzGnVmdNViTtbgLa7MO67Ie67MBG7IRG7MJm7IZm7MFd2VLtuJubM02bMt2bM8O7MhO3J2d2YVd2Y3d2YM92Yu92Yd92Y/9OYADOYiDOYRDOYzDOYIjOYqjOYZjuQfHcTwncCIncTKncCqncTpncCZncTbncC7ncT4LuIALueO2FnNPLuFSLuNyruBKruJqruFaruN6buBGbuJmbuFWbuN27sW9uQ/35X7cnwfwQB7Eg3kID+VhPJxH8EgexaN5DI/lcTyeJ/BEnsSTeQpP5Wk8nWfwTJ7Fs3kOz+V5PJ8X8EJexIt5CS/lZbycV/BKXsWreQ2v5XW8njfwRt7Em3kLb+VtvJ138E7exbt5D+/lfSxkEe/nA3yQD/FhPsJH+Rgf5xN8kk/xaT7DZ/kcn+cLfJEv8WW+wlf5Gl/nG3yTb/FtvsN3+R7f5wf8kB/xY37CT/kZP+cX/JJf8Wt+w2/5Hb/nD/yRP/Fn/sJf+Rt/5x/8k3/xb/7DHa+/OEtwJ5ZkKZZmGZZlOZZnBVZkJVZmFVZlNe7M6qzBmqzFXVibdViX9VifDdiQjdiYTdiUzdicLbgrW7IVd2NrtmFbtmN7dmBHduLu7Mwu7Mpu7M4e7Mle7M0+7Mt+7M8BHMhBHMwhHMphHM4RHMlRHM0xHMs9OI7jOYETOYmTOYVTOY3TOYMzOYuzOYdzOY/zWcAFXMhFXMw9uYRLuYzLuYIruYqruYZruY7ruYEbuYmbuYVbuY3buWN2rCmcFE4KJ4WTwknhpHBSOCmcFE4KZz4LmMJJ4aRwUjgpnBROCieFk8JJ4aRwClnEFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4BUzgpnBROCieFk8JJ4aRwUjgpnBROIYuYwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwilgCieFk8JJ4aRwUjgpnBROCieFk8IpZBFTOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOAVM4aRwUjgpnBROCieFk8JJ4aRwUjiFLGIKJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4Kp4ApnBROCieFk8JJ4aRwUjgpnBROCqeQRUzhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknh/B+F8y+SuH50aOoAAA==</data>
              </body>
            </tileData>
            <tileset dataType="Struct" type="Duality.ContentRef`1[[Duality.Plugins.Tilemaps.Tileset]]">
              <contentPath dataType="String">Data\TestTiles.Tileset.res</contentPath>
            </tileset>
          </item>
          <item dataType="Struct" type="Duality.Components.Transform" id="3564639283">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">1204324351</gameobj>
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
          <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapRenderer" id="4197399799">
            <active dataType="Bool">true</active>
            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">255</B>
              <G dataType="Byte">255</G>
              <R dataType="Byte">255</R>
            </colorTint>
            <externalTilemap />
            <gameobj dataType="ObjectRef">1204324351</gameobj>
            <offset dataType="Float">0</offset>
            <origin dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
            <tileDepthMode dataType="Enum" type="Duality.Plugins.Tilemaps.TileDepthOffsetMode" name="Flat" value="0" />
            <tileDepthScale dataType="Float">0.01</tileDepthScale>
            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          </item>
        </_items>
        <_size dataType="Int">3</_size>
        <_version dataType="Int">3</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2420306872" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3748154519">
            <item dataType="Type" id="3831934222" value="Duality.Plugins.Tilemaps.Tilemap" />
            <item dataType="ObjectRef">1128403856</item>
            <item dataType="Type" id="3045257546" value="Duality.Plugins.Tilemaps.TilemapRenderer" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="3977319104">
            <item dataType="ObjectRef">3206176368</item>
            <item dataType="ObjectRef">3564639283</item>
            <item dataType="ObjectRef">4197399799</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">3564639283</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3439226933">eMLIjgB3YUWg7L6MR7U35g==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">TilemapA</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="4093171636">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3769923210">
        <_items dataType="Array" type="Duality.Component[]" id="3334896608" length="4">
          <item dataType="Struct" type="Duality.Plugins.Tilemaps.Tilemap" id="1800056357">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">4093171636</gameobj>
            <tileData dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapData" id="3496668625" custom="true">
              <body>
                <version dataType="Int">2</version>
                <data dataType="Array" type="System.Byte[]" id="1570235118">H4sIAAAAAAAEAO3W07IYBhSG0SSNnQaNnSaN09h20tg8QWM7tW3btt2eU9u2bbudWZk+Qu/+fbFmP8CePd+iYsWKLSr23xRnCe7EkizF0izDsizH8qzAiqzEyqzCqqzGnVmdNViTtbgLa7MO67Ie67MBG7IRG7MJm7IZm7MFd2VLtuJubM02bMt2bM8O7MhO3J2d2YVd2Y3d2YM92Yu92Yd92Y/9OYADOYiDOYRDOYzDOYIjOYqjOYZjuQfHcTwncCIncTKncCqncTpncCZncTbncC7ncT4LuIALueO2FnNPLuFSLuNyruBKruJqruFaruN6buBGbuJmbuFWbuN27sW9uQ/35X7cnwfwQB7Eg3kID+VhPJxH8EgexaN5DI/lcTyeJ/BEnsSTeQpP5Wk8nWfwTJ7Fs3kOz+V5PJ8X8EJexIt5CS/lZbycV/BKXsWreQ2v5XW8njfwRt7Em3kLb+VtvJ138E7exbt5D+/lfSxkEe/nA3yQD/FhPsJH+Rgf5xN8kk/xaT7DZ/kcn+cLfJEv8WW+wlf5Gl/nG3yTb/FtvsN3+R7f5wf8kB/xY37CT/kZP+cX/JJf8Wt+w2/5Hb/nD/yRP/Fn/sJf+Rt/5x/8k3/xb/7DHa+/OEtwJ5ZkKZZmGZZlOZZnBVZkJVZmFVZlNe7M6qzBmqzFXVibdViX9VifDdiQjdiYTdiUzdicLbgrW7IVd2NrtmFbtmN7dmBHduLu7Mwu7Mpu7M4e7Mle7M0+7Mt+7M8BHMhBHMwhHMphHM4RHMlRHM0xHMs9OI7jOYETOYmTOYVTOY3TOYMzOYuzOYdzOY/zWcAFXMhFXMw9uYRLuYzLuYIruYqruYZruY7ruYEbuYmbuYVbuY3buWN2rCmcFE4KJ4WTwknhpHBSOCmcFE4KZz4LmMJJ4aRwUjgpnBROCieFk8JJ4aRwClnEFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4BUzgpnBROCieFk8JJ4aRwUjgpnBROIYuYwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwilgCieFk8JJ4aRwUjgpnBROCieFk8IpZBFTOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOAVM4aRwUjgpnBROCieFk8JJ4aRwUjiFLGIKJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4Kp4ApnBROCieFk8JJ4aRwUjgpnBROCqeQRUzhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknhpHBSOCmcFE4KJ4WTwknh/B+F8y+SuH50aOoAAA==</data>
              </body>
            </tileData>
            <tileset dataType="Struct" type="Duality.ContentRef`1[[Duality.Plugins.Tilemaps.Tileset]]">
              <contentPath dataType="String">Data\TestTiles.Tileset.res</contentPath>
            </tileset>
          </item>
          <item dataType="Struct" type="Duality.Components.Transform" id="2158519272">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">4093171636</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <parentTransform />
            <pos dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">150</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">-150</Z>
            </pos>
            <posAbs dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">150</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">-150</Z>
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
          <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapRenderer" id="2791279788">
            <active dataType="Bool">true</active>
            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">128</B>
              <G dataType="Byte">128</G>
              <R dataType="Byte">255</R>
            </colorTint>
            <externalTilemap />
            <gameobj dataType="ObjectRef">4093171636</gameobj>
            <offset dataType="Float">0</offset>
            <origin dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
            <tileDepthMode dataType="Enum" type="Duality.Plugins.Tilemaps.TileDepthOffsetMode" name="Flat" value="0" />
            <tileDepthScale dataType="Float">0.01</tileDepthScale>
            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          </item>
        </_items>
        <_size dataType="Int">3</_size>
        <_version dataType="Int">3</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3895455002" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="39189872">
            <item dataType="ObjectRef">3831934222</item>
            <item dataType="ObjectRef">1128403856</item>
            <item dataType="ObjectRef">3045257546</item>
          </keys>
          <values dataType="Array" type="System.Object[]" id="2124623598">
            <item dataType="ObjectRef">1800056357</item>
            <item dataType="ObjectRef">2158519272</item>
            <item dataType="ObjectRef">2791279788</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">2158519272</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="4051743436">e5PS8SobcESTwRnZuQDTvw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">TilemapB</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="4225538166">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="622956000">
        <_items dataType="Array" type="Duality.Component[]" id="2342384604" length="4">
          <item dataType="Struct" type="Duality.Plugins.Tilemaps.Tilemap" id="1932422887">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">4225538166</gameobj>
            <tileData dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapData" id="550722715" custom="true">
              <body>
                <version dataType="Int">2</version>
                <data dataType="Array" type="System.Byte[]" id="3295367062">H4sIAAAAAAAEAOzdXWrDMBCF0fRn/+vMMppHg4m41siNJZ8DMYXG0fvHMHr+PB7P1wcAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAC4sq9BTwAAAAAAuKdRpV17BwAAAADgnvaF/PvgU3UHAAAAAIB6bz/a3hV4AAAAAADWM7a3t6v7uxMBAAAAAGAN58235+dq7wAAAAAAzG7UlHtfOVfdAQAAAAC4glE7W0bdmtpHdQcAAAAA4FP2jb39d/Jr+az70UK+fTc/HQAAAAAAzpbcSVr/zeTZ9q7YJ6cDAAAAAMBK+np7soumfRYAAAAAAJztjPn2xG/zuaW3AwAAAABwfWP3t+fO6+35Da0AAAAAADBWe759bG8/utFdbwcAAAAA4A4qNVtvBwAAAACApGnnt6P23Zq6f0tvBwAAAABgLkd7e7t7J1trkrf0dgAAAAAA5pL39vybR+ntAAAAAACsoTLZ3rd5ZktvBwAAAABgDe07T/Pevt8Skz+39HYAAAAAAGZXKe3vevtRejsAAAAAALOr9/b6thm9HQAAAACA2dX3yeSFvD39nu+fAQAAAACAq6n09kfw3+QsXR0AAAAAgNm1u3e9pSenAwAAAADA7NqdPJ9I35dzLR0AAAAAgPtI5tL7JtXbU+42yQAAAAAAMK++204rm2H0dgAAAAAA1lPp7X23nertAAAAAACsp6+3V+jtAAAAAACs51O9XV0HAAAAAGAl/9/bt/R2AAAAAADWUOntlf3tSjsAAAAAACs5Y769r8MDAAAAAMC8PrtPBgAAAAAA1qC3AwAAAABAnd4OAAAAAAB1ejsAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAH/twQEJAAAAgKD/r/sRKgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD8BhR/t3IiNWwA=</data>
              </body>
            </tileData>
            <tileset dataType="Struct" type="Duality.ContentRef`1[[Duality.Plugins.Tilemaps.Tileset]]">
              <contentPath dataType="String">Data\TestTiles.Tileset.res</contentPath>
            </tileset>
          </item>
          <item dataType="Struct" type="Duality.Components.Transform" id="2290885802">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">4225538166</gameobj>
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
          <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapRenderer" id="2923646318">
            <active dataType="Bool">true</active>
            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">255</B>
              <G dataType="Byte">255</G>
              <R dataType="Byte">255</R>
            </colorTint>
            <externalTilemap />
            <gameobj dataType="ObjectRef">4225538166</gameobj>
            <offset dataType="Float">0</offset>
            <origin dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
            <tileDepthMode dataType="Enum" type="Duality.Plugins.Tilemaps.TileDepthOffsetMode" name="Flat" value="0" />
            <tileDepthScale dataType="Float">0.01</tileDepthScale>
            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          </item>
        </_items>
        <_size dataType="Int">3</_size>
        <_version dataType="Int">3</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2764209038" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="1639660850">
            <item dataType="ObjectRef">3831934222</item>
            <item dataType="ObjectRef">1128403856</item>
            <item dataType="ObjectRef">3045257546</item>
          </keys>
          <values dataType="Array" type="System.Object[]" id="3812500298">
            <item dataType="ObjectRef">1932422887</item>
            <item dataType="ObjectRef">2290885802</item>
            <item dataType="ObjectRef">2923646318</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">2290885802</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3875643266">19aZc0Vmlk6UX+4+8/+wNw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">TilemapX</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="109330599">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1438854149">
        <_items dataType="Array" type="Duality.Component[]" id="3791045206" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="2469645531">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0.08726646</angle>
            <angleAbs dataType="Float">0.08726646</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">109330599</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <parentTransform />
            <pos dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">300</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">-300</Z>
            </pos>
            <posAbs dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">300</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">-300</Z>
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
          <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapRenderer" id="3102406047">
            <active dataType="Bool">true</active>
            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">128</B>
              <G dataType="Byte">128</G>
              <R dataType="Byte">255</R>
            </colorTint>
            <externalTilemap dataType="ObjectRef">1800056357</externalTilemap>
            <gameobj dataType="ObjectRef">109330599</gameobj>
            <offset dataType="Float">0</offset>
            <origin dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
            <tileDepthMode dataType="Enum" type="Duality.Plugins.Tilemaps.TileDepthOffsetMode" name="Flat" value="0" />
            <tileDepthScale dataType="Float">0.01</tileDepthScale>
            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          </item>
        </_items>
        <_size dataType="Int">2</_size>
        <_version dataType="Int">4</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2446716840" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3738009583">
            <item dataType="ObjectRef">1128403856</item>
            <item dataType="ObjectRef">3045257546</item>
          </keys>
          <values dataType="Array" type="System.Object[]" id="2184034720">
            <item dataType="ObjectRef">2469645531</item>
            <item dataType="ObjectRef">3102406047</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">2469645531</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1583501949">VgJJZ80iykK+zUN5mgwxOw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">TilemapB2</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="2961408854">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1023990784">
        <_items dataType="Array" type="Duality.Component[]" id="3240911004" length="4">
          <item dataType="Struct" type="Duality.Plugins.Tilemaps.Tilemap" id="668293575">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">2961408854</gameobj>
            <tileData dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapData" id="3135594555" custom="true">
              <body>
                <version dataType="Int">2</version>
                <data dataType="Array" type="System.Byte[]" id="3454070486">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</data>
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
            <tileDepthMode dataType="Enum" type="Duality.Plugins.Tilemaps.TileDepthOffsetMode" name="Flat" value="0" />
            <tileDepthScale dataType="Float">0.01</tileDepthScale>
            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          </item>
        </_items>
        <_size dataType="Int">3</_size>
        <_version dataType="Int">3</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3979117518" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3656509138">
            <item dataType="ObjectRef">3831934222</item>
            <item dataType="ObjectRef">1128403856</item>
            <item dataType="ObjectRef">3045257546</item>
          </keys>
          <values dataType="Array" type="System.Object[]" id="3223298250">
            <item dataType="ObjectRef">668293575</item>
            <item dataType="ObjectRef">1026756490</item>
            <item dataType="ObjectRef">1659517006</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">1026756490</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3428422498">KMXuZW3ZOEaxigxAvZjLRg==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">TilemapC_Lower</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="3085774208">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3929111238">
        <_items dataType="Array" type="Duality.Component[]" id="1185542400" length="4">
          <item dataType="Struct" type="Duality.Plugins.Tilemaps.Tilemap" id="792658929">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3085774208</gameobj>
            <tileData dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapData" id="2875785653" custom="true">
              <body>
                <version dataType="Int">2</version>
                <data dataType="Array" type="System.Byte[]" id="2152370934">H4sIAAAAAAAEAO3duW7UUBQG4ElmACkJhE3sgheADkQTSBNqOuigSUOJxNKwNAEaloalpAJeAahIngxxDNKVDJZnlGGOPV8xn6xje+Yvrjz32le+m4PBYPP354+L4TAcFfXKfbXKUrgcroT7wwPhaniwdhZJ9sOv4bfwe4I8JEmS3fVTggzsq3eK7UPh4QSp2C33hHuLyt1i+2R4qqgcC48nSM7MlvdR2xx/NjyXIDn7rSsYJ/PWwi9vL/x9rysYyd1yJUEGdtFqxsJygiScB8cd65GVo9ZHrk70/eZucRqeSJCB8+D1BBn4/70X3g8fhA8TpGLXfRQ+Dp+ETxOkYtfdCp+Fz8MXCVKxu16N5yzr4auovA7fJMjGrvs2fBe+Dz8kSMX+uf6P58VkHo03OQ2NN7vu5ZT/X1uh8WZfXZrR774MjTe77qzaT7PleDNDHpKzdS36V1dqvazy/mdZz9kfY043orVcG7PN5Pz3JEmSJEmSJEmSJEly2parqDRrjRW290e43cKd2rkbZgoxHBbbR1uf9bFWGXceEefHM62P/JIgLftt+fbUxQR5mM3JVuf5nCA5c1qfw38+Khf0mrhLXoq2dDE8UttbVcrVoCZ7byrn01F4ulavKvXVoMj2Nr+R/mZc04aNx3A+XRuzB1Wt1HOjcb0ekiRJkiRJkiRJkiRJkiRJkiRJkiRJkiRJkiRJkiRJkiRJkiRJkiQ5PX8CVD9RJ2jqAAA=</data>
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
            <tileDepthMode dataType="Enum" type="Duality.Plugins.Tilemaps.TileDepthOffsetMode" name="Flat" value="0" />
            <tileDepthScale dataType="Float">0.01</tileDepthScale>
            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          </item>
        </_items>
        <_size dataType="Int">3</_size>
        <_version dataType="Int">3</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3478357690" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="547646260">
            <item dataType="ObjectRef">3831934222</item>
            <item dataType="ObjectRef">1128403856</item>
            <item dataType="ObjectRef">3045257546</item>
          </keys>
          <values dataType="Array" type="System.Object[]" id="3711407862">
            <item dataType="ObjectRef">792658929</item>
            <item dataType="ObjectRef">1151121844</item>
            <item dataType="ObjectRef">1783882360</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">1151121844</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="4072129680">OGFE/6l2ukykXD788OcpAg==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">TilemapC_Upper</name>
      <parent />
      <prefabLink />
    </item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
