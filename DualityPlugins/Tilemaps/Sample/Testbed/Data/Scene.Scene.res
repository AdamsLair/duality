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
                      <data dataType="Array" type="System.Byte[]" id="455732262">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</data>
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
                  <item dataType="ObjectRef">1128403856</item>
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
                      <data dataType="Array" type="System.Byte[]" id="1707160614">H4sIAAAAAAAEAO3duW4TURQG4GyAlATCJnbBC0AHogmkCTUddNCkoURikVglMCCFTWItqYBXACrgyRBnQLrS4GhsEvvM5Cv8aXRmxv5tjcb32nfmrkxMTKz8efx1KpwOZ4p65bZaZTacC+fD7eGOcCHcWduLJLvhl/Br+C1BHpIkyfb6MUEGdtUrxfKucHeCVGyXW8KtReVqsXwwPFRU9oX7EyRnZsvfUZtsfzQ8liA5u60zGIfz0uRvL0/+e60zGMn1cj5BBrbRasTCXK1e7+uRzT3Qpz5oX4+snGm85cJQz2/sFjfCfmdCcn09nyADR++18Hp4I7yZIBXb7p3wbngvvJ8gFdtuL3wUPg6fJEjF9no2/mdZCp9H5UX4MkE2tt034dvwXfg+QSp2z6U+/xeTedTf5Eaov9l2T6f8/uqF+ptddXZMr/ss1N9su+M6fta27G9myENyvC5G++pMrZVV/v5Z1nO2x5jT5Thazg14zOT89iRJkiRJkiRJkiRJcqMtZ1FZW3OssLnfwx8N/Fnbd9lIIYbTxfLexnt9qFUGHUfEzeORxlt+TpCW3ba8e+pUgjwcjc2vYx1udp5PCd4jR2+T61jrY/iPR+WEVhP72AubX8d6Ko6lk+Ge2tqqUs4GNdx9U9l2h7uOdSY8XKtXlfpsUNxs/s99k+p3pC+9GOe06T5rb4W3E3wCHL2LA7agqpl6Lqw5X0/lg/BhgvfILrkaPk2QhF3yVfg6QRKSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmyib8AGd+2ZWjqAAA=</data>
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
                  <item dataType="ObjectRef">1128403856</item>
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
        </_items>
        <_size dataType="Int">2</_size>
        <_version dataType="Int">2</_version>
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
    <item dataType="Struct" type="Duality.GameObject" id="1593919710">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="158003912">
        <_items dataType="Array" type="Duality.Component[]" id="3716853356" length="4">
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
              <Y dataType="Float">240</Y>
              <Z dataType="Float">0</Z>
            </pos>
            <posAbs dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">240</Y>
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
        </_items>
        <_size dataType="Int">2</_size>
        <_version dataType="Int">2</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2561091294" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3769218442">
            <item dataType="ObjectRef">1128403856</item>
            <item dataType="Type" id="499419104" value="Duality.Plugins.Tilemaps.Sample.RpgLike.ActorRenderer" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="1630447898">
            <item dataType="ObjectRef">3954234642</item>
            <item dataType="ObjectRef">3907431145</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">3954234642</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2941495914">6QaaXpvsP06njO2/8LNlCA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">TestActor</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">2961408854</item>
    <item dataType="ObjectRef">3085774208</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
