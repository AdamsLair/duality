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
        <_items dataType="Array" type="Duality.Component[]" id="1392996316">
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
          <item dataType="Struct" type="Duality.Components.Camera" id="4126102485">
            <active dataType="Bool">true</active>
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">3588826678</gameobj>
            <nearZ dataType="Float">0</nearZ>
            <passes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Camera+Pass]]" id="136409209">
              <_items dataType="Array" type="Duality.Components.Camera+Pass[]" id="2322753358" length="4">
                <item dataType="Struct" type="Duality.Components.Camera+Pass" id="3719713488">
                  <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">78</B>
                    <G dataType="Byte">67</G>
                    <R dataType="Byte">63</R>
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
                <item dataType="Struct" type="Duality.Components.Camera+Pass" id="452707950">
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
          <item dataType="Struct" type="Duality.Components.SoundListener" id="4242308049">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3588826678</gameobj>
          </item>
          <item dataType="Struct" type="BasicMenu.EventMenuController" id="1563800156">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3588826678</gameobj>
          </item>
        </_items>
        <_size dataType="Int">4</_size>
        <_version dataType="Int">4</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4076619662" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="726969330">
            <item dataType="Type" id="3939607504" value="Duality.Components.Transform" />
            <item dataType="Type" id="1077793390" value="Duality.Components.Camera" />
            <item dataType="Type" id="2908634028" value="Duality.Components.SoundListener" />
            <item dataType="Type" id="1125487122" value="BasicMenu.EventMenuController" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="3096052554">
            <item dataType="ObjectRef">1654174314</item>
            <item dataType="ObjectRef">4126102485</item>
            <item dataType="ObjectRef">4242308049</item>
            <item dataType="ObjectRef">1563800156</item>
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
    <item dataType="Struct" type="Duality.GameObject" id="1991011308">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3356329634">
        <_items dataType="Array" type="Duality.GameObject[]" id="2742974224" length="4">
          <item dataType="Struct" type="Duality.GameObject" id="2178015094">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="356019190">
              <_items dataType="Array" type="Duality.Component[]" id="73389792">
                <item dataType="Struct" type="Duality.Components.Transform" id="243362730">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">2178015094</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform dataType="Struct" type="Duality.Components.Transform" id="56358944">
                    <active dataType="Bool">true</active>
                    <angle dataType="Float">0</angle>
                    <angleAbs dataType="Float">0</angleAbs>
                    <angleVel dataType="Float">0</angleVel>
                    <angleVelAbs dataType="Float">0</angleVelAbs>
                    <deriveAngle dataType="Bool">true</deriveAngle>
                    <gameobj dataType="ObjectRef">1991011308</gameobj>
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
                  </parentTransform>
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
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3820181662">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">2178015094</gameobj>
                  <offset dataType="Int">1</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">50</H>
                    <W dataType="Float">300</W>
                    <X dataType="Float">-150</X>
                    <Y dataType="Float">-25</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                  </sharedMat>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0, AllFlags" value="2147483649" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="3920643916">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">0</B>
                    <G dataType="Byte">0</G>
                    <R dataType="Byte">0</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">2178015094</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath />
                  </iconMat>
                  <offset dataType="Int">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="3571598932">
                    <displayedText dataType="String">Quit</displayedText>
                    <elements dataType="Array" type="Duality.Drawing.FormattedText+Element[]" id="398345444">
                      <item dataType="Struct" type="Duality.Drawing.FormattedText+TextElement" id="25343940">
                        <text dataType="String">Quit</text>
                      </item>
                    </elements>
                    <flowAreas />
                    <fontGlyphCount dataType="Array" type="System.Int32[]" id="903757334">4</fontGlyphCount>
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1915816672">
                      <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                        <contentPath dataType="String">Default:Font:GenericMonospace10</contentPath>
                      </item>
                    </fonts>
                    <iconCount dataType="Int">0</iconCount>
                    <icons />
                    <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                    <maxHeight dataType="Int">0</maxHeight>
                    <maxWidth dataType="Int">0</maxWidth>
                    <sourceText dataType="String">Quit</sourceText>
                    <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                  </text>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0, AllFlags" value="2147483649" />
                </item>
                <item dataType="Struct" type="BasicMenu.MenuQuit" id="1442516381">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2178015094</gameobj>
                  <hoverTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">0</B>
                    <G dataType="Byte">0</G>
                    <R dataType="Byte">255</R>
                  </hoverTint>
                </item>
              </_items>
              <_size dataType="Int">4</_size>
              <_version dataType="Int">4</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1814164506" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="579307204">
                  <item dataType="ObjectRef">3939607504</item>
                  <item dataType="Type" id="2378641220" value="Duality.Components.Renderers.SpriteRenderer" />
                  <item dataType="Type" id="4133867158" value="Duality.Components.Renderers.TextRenderer" />
                  <item dataType="Type" id="1660909312" value="BasicMenu.MenuQuit" />
                </keys>
                <values dataType="Array" type="System.Object[]" id="2787951510">
                  <item dataType="ObjectRef">243362730</item>
                  <item dataType="ObjectRef">3820181662</item>
                  <item dataType="ObjectRef">3920643916</item>
                  <item dataType="ObjectRef">1442516381</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">243362730</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="703675264">Z5+raHo95EC9J9Txr9RI9Q==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Quit</name>
            <parent dataType="ObjectRef">1991011308</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">1</_size>
        <_version dataType="Int">3</_version>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1967733514">
        <_items dataType="Array" type="Duality.Component[]" id="560817976" length="4">
          <item dataType="ObjectRef">56358944</item>
          <item dataType="Struct" type="BasicMenu.CenterScreen" id="2507152410">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1991011308</gameobj>
          </item>
        </_items>
        <_size dataType="Int">2</_size>
        <_version dataType="Int">10</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3446879058" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2536238880">
            <item dataType="ObjectRef">3939607504</item>
            <item dataType="Type" id="442937308" value="BasicMenu.CenterScreen" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="4283008910">
            <item dataType="ObjectRef">56358944</item>
            <item dataType="ObjectRef">2507152410</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">56358944</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1963166268">qSytuAGr30epimWOTaEamg==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">#MenuMain</name>
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
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="4015162559">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">2372995991</gameobj>
                  <offset dataType="Int">10</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">8192</H>
                    <W dataType="Float">8192</W>
                    <X dataType="Float">-4096</X>
                    <Y dataType="Float">-4096</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="WrapBoth" value="3" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\BasicMenu\Visuals\BackgroundTile.Material.res</contentPath>
                  </sharedMat>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1192659896" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="852773145">
                  <item dataType="ObjectRef">3939607504</item>
                  <item dataType="ObjectRef">2378641220</item>
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
        </_items>
        <_size dataType="Int">1</_size>
        <_version dataType="Int">5</_version>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2184748730">
        <_items dataType="Array" type="Duality.Component[]" id="4289085568" length="0" />
        <_size dataType="Int">0</_size>
        <_version dataType="Int">0</_version>
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
    <item dataType="Struct" type="Duality.GameObject" id="2991214831">
      <active dataType="Bool">false</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1272227597">
        <_items dataType="Array" type="Duality.GameObject[]" id="1468055334" length="4">
          <item dataType="Struct" type="Duality.GameObject" id="3944217670">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="364252938">
              <_items dataType="Array" type="Duality.Component[]" id="3620373728">
                <item dataType="Struct" type="Duality.Components.Transform" id="2009565306">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">3944217670</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform dataType="Struct" type="Duality.Components.Transform" id="1056562467">
                    <active dataType="Bool">true</active>
                    <angle dataType="Float">0</angle>
                    <angleAbs dataType="Float">0</angleAbs>
                    <angleVel dataType="Float">0</angleVel>
                    <angleVelAbs dataType="Float">0</angleVelAbs>
                    <deriveAngle dataType="Bool">true</deriveAngle>
                    <gameobj dataType="ObjectRef">2991214831</gameobj>
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
                  </parentTransform>
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-80</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-80</X>
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
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1291416942">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">3944217670</gameobj>
                  <offset dataType="Int">1</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">50</H>
                    <W dataType="Float">150</W>
                    <X dataType="Float">-75</X>
                    <Y dataType="Float">-25</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                  </sharedMat>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0, AllFlags" value="2147483649" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="1391879196">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">0</B>
                    <G dataType="Byte">0</G>
                    <R dataType="Byte">0</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">3944217670</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath />
                  </iconMat>
                  <offset dataType="Int">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1627669252">
                    <displayedText dataType="String">Yes</displayedText>
                    <elements dataType="Array" type="Duality.Drawing.FormattedText+Element[]" id="310085956">
                      <item dataType="Struct" type="Duality.Drawing.FormattedText+TextElement" id="2259132996">
                        <text dataType="String">Yes</text>
                      </item>
                    </elements>
                    <flowAreas />
                    <fontGlyphCount dataType="Array" type="System.Int32[]" id="62005910">3</fontGlyphCount>
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1090312448">
                      <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                        <contentPath dataType="String">Default:Font:GenericMonospace10</contentPath>
                      </item>
                    </fonts>
                    <iconCount dataType="Int">0</iconCount>
                    <icons />
                    <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                    <maxHeight dataType="Int">0</maxHeight>
                    <maxWidth dataType="Int">0</maxWidth>
                    <sourceText dataType="String">Yes</sourceText>
                    <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                  </text>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0, AllFlags" value="2147483649" />
                </item>
                <item dataType="Struct" type="BasicMenu.MenuQuitConfirm" id="1674350375">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">3944217670</gameobj>
                  <hoverTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">0</B>
                    <G dataType="Byte">0</G>
                    <R dataType="Byte">255</R>
                  </hoverTint>
                </item>
              </_items>
              <_size dataType="Int">4</_size>
              <_version dataType="Int">10</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3436355098" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1948164592">
                  <item dataType="ObjectRef">3939607504</item>
                  <item dataType="ObjectRef">2378641220</item>
                  <item dataType="ObjectRef">4133867158</item>
                  <item dataType="Type" id="3161203516" value="BasicMenu.MenuQuitConfirm" />
                </keys>
                <values dataType="Array" type="System.Object[]" id="3034458350">
                  <item dataType="ObjectRef">2009565306</item>
                  <item dataType="ObjectRef">1291416942</item>
                  <item dataType="ObjectRef">1391879196</item>
                  <item dataType="ObjectRef">1674350375</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2009565306</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1148563788">6LxU90NJPUCn4lXtH+n0kw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Yes</name>
            <parent dataType="ObjectRef">2991214831</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3148492957">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3587985677">
              <_items dataType="Array" type="Duality.Component[]" id="2037847846">
                <item dataType="Struct" type="Duality.Components.Transform" id="1213840593">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">3148492957</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform dataType="ObjectRef">1056562467</parentTransform>
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">80</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">80</X>
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
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="495692229">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">3148492957</gameobj>
                  <offset dataType="Int">1</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">50</H>
                    <W dataType="Float">150</W>
                    <X dataType="Float">-75</X>
                    <Y dataType="Float">-25</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                  </sharedMat>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0, AllFlags" value="2147483649" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="596154483">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">0</B>
                    <G dataType="Byte">0</G>
                    <R dataType="Byte">0</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">3148492957</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath />
                  </iconMat>
                  <offset dataType="Int">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="3678189059">
                    <displayedText dataType="String">No</displayedText>
                    <elements dataType="Array" type="Duality.Drawing.FormattedText+Element[]" id="3110242598">
                      <item dataType="Struct" type="Duality.Drawing.FormattedText+TextElement" id="3567011072">
                        <text dataType="String">No</text>
                      </item>
                    </elements>
                    <flowAreas />
                    <fontGlyphCount dataType="Array" type="System.Int32[]" id="1330293434">2</fontGlyphCount>
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1135772198">
                      <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                        <contentPath dataType="String">Default:Font:GenericMonospace10</contentPath>
                      </item>
                    </fonts>
                    <iconCount dataType="Int">0</iconCount>
                    <icons />
                    <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                    <maxHeight dataType="Int">0</maxHeight>
                    <maxWidth dataType="Int">0</maxWidth>
                    <sourceText dataType="String">No</sourceText>
                    <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                  </text>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0, AllFlags" value="2147483649" />
                </item>
                <item dataType="Struct" type="BasicMenu.MenuBackToMain" id="2466420622">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">3148492957</gameobj>
                  <hoverTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">0</A>
                    <B dataType="Byte">0</B>
                    <G dataType="Byte">0</G>
                    <R dataType="Byte">0</R>
                  </hoverTint>
                </item>
              </_items>
              <_size dataType="Int">4</_size>
              <_version dataType="Int">10</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="567439800" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3722034791">
                  <item dataType="ObjectRef">3939607504</item>
                  <item dataType="ObjectRef">2378641220</item>
                  <item dataType="ObjectRef">4133867158</item>
                  <item dataType="Type" id="112944206" value="BasicMenu.MenuBackToMain" />
                </keys>
                <values dataType="Array" type="System.Object[]" id="404299392">
                  <item dataType="ObjectRef">1213840593</item>
                  <item dataType="ObjectRef">495692229</item>
                  <item dataType="ObjectRef">596154483</item>
                  <item dataType="ObjectRef">2466420622</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1213840593</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3715110693">s3CyBhjd/0COHWDcMho30Q==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">No</name>
            <parent dataType="ObjectRef">2991214831</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">2</_size>
        <_version dataType="Int">2</_version>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2170206648">
        <_items dataType="Array" type="Duality.Component[]" id="297953383" length="4">
          <item dataType="ObjectRef">1056562467</item>
          <item dataType="Struct" type="BasicMenu.CenterScreen" id="3507355933">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">2991214831</gameobj>
          </item>
        </_items>
        <_size dataType="Int">2</_size>
        <_version dataType="Int">2</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1368391655" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="899993876">
            <item dataType="ObjectRef">3939607504</item>
            <item dataType="ObjectRef">442937308</item>
          </keys>
          <values dataType="Array" type="System.Object[]" id="1545886006">
            <item dataType="ObjectRef">1056562467</item>
            <item dataType="ObjectRef">3507355933</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">1056562467</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1281428400">UxHty6eN2kGa9fQ9f14PZg==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">#MenuQuit</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">2178015094</item>
    <item dataType="ObjectRef">2372995991</item>
    <item dataType="ObjectRef">3944217670</item>
    <item dataType="ObjectRef">3148492957</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
