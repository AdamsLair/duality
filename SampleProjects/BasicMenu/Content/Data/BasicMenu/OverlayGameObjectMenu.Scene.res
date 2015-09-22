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
        <_items dataType="Array" type="Duality.Component[]" id="1392996316" length="8">
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
            <startingMenu dataType="Struct" type="BasicMenu.MenuPage" id="3799790557">
              <active dataType="Bool">true</active>
              <gameobj dataType="Struct" type="Duality.GameObject" id="1991011308">
                <active dataType="Bool">true</active>
                <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2276070543">
                  <_items dataType="Array" type="Duality.GameObject[]" id="79688878" length="4">
                    <item dataType="Struct" type="Duality.GameObject" id="2178015094">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="896792522">
                        <_items dataType="Array" type="Duality.Component[]" id="1656338784">
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
                              <Y dataType="Float">30</Y>
                              <Z dataType="Float">0</Z>
                            </pos>
                            <posAbs dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">0</X>
                              <Y dataType="Float">30</Y>
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
                              <B dataType="Byte">45</B>
                              <G dataType="Byte">45</G>
                              <R dataType="Byte">45</R>
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
                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                          </item>
                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="3920643916">
                            <active dataType="Bool">true</active>
                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <gameobj dataType="ObjectRef">2178015094</gameobj>
                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                              <contentPath />
                            </iconMat>
                            <offset dataType="Int">0</offset>
                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="872099668">
                              <flowAreas />
                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="784925412">
                                <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                                  <contentPath dataType="String">Data\BasicMenu\Visuals\DuruSans-Regular.Font.res</contentPath>
                                </item>
                              </fonts>
                              <icons />
                              <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                              <maxHeight dataType="Int">0</maxHeight>
                              <maxWidth dataType="Int">0</maxWidth>
                              <sourceText dataType="String">Quit</sourceText>
                              <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                            </text>
                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                          </item>
                          <item dataType="Struct" type="BasicMenu.MenuSwitchToPage" id="2494082424">
                            <active dataType="Bool">true</active>
                            <gameobj dataType="ObjectRef">2178015094</gameobj>
                            <hoverTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">38</B>
                              <G dataType="Byte">43</G>
                              <R dataType="Byte">245</R>
                            </hoverTint>
                            <target dataType="Struct" type="BasicMenu.MenuPage" id="505026784">
                              <active dataType="Bool">true</active>
                              <gameobj dataType="Struct" type="Duality.GameObject" id="2991214831">
                                <active dataType="Bool">false</active>
                                <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1840739811">
                                  <_items dataType="Array" type="Duality.GameObject[]" id="1235827942" length="4">
                                    <item dataType="Struct" type="Duality.GameObject" id="3148492957">
                                      <active dataType="Bool">true</active>
                                      <children />
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2686198157">
                                        <_items dataType="Array" type="Duality.Component[]" id="2464015654">
                                          <item dataType="Struct" type="Duality.Components.Transform" id="1213840593">
                                            <active dataType="Bool">true</active>
                                            <angle dataType="Float">0</angle>
                                            <angleAbs dataType="Float">0</angleAbs>
                                            <angleVel dataType="Float">0</angleVel>
                                            <angleVelAbs dataType="Float">0</angleVelAbs>
                                            <deriveAngle dataType="Bool">true</deriveAngle>
                                            <gameobj dataType="ObjectRef">3148492957</gameobj>
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
                                              <B dataType="Byte">45</B>
                                              <G dataType="Byte">45</G>
                                              <R dataType="Byte">45</R>
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
                                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                                          </item>
                                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="596154483">
                                            <active dataType="Bool">true</active>
                                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">255</B>
                                              <G dataType="Byte">255</G>
                                              <R dataType="Byte">255</R>
                                            </colorTint>
                                            <customMat />
                                            <gameobj dataType="ObjectRef">3148492957</gameobj>
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                                              <contentPath />
                                            </iconMat>
                                            <offset dataType="Int">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1042193923">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="3083413798">
                                                <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                                                  <contentPath dataType="String">Data\BasicMenu\Visuals\DuruSans-Regular.Font.res</contentPath>
                                                </item>
                                              </fonts>
                                              <icons />
                                              <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                                              <maxHeight dataType="Int">0</maxHeight>
                                              <maxWidth dataType="Int">0</maxWidth>
                                              <sourceText dataType="String">No</sourceText>
                                              <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                                            </text>
                                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                                          </item>
                                          <item dataType="Struct" type="BasicMenu.MenuSwitchToPage" id="3464560287">
                                            <active dataType="Bool">true</active>
                                            <gameobj dataType="ObjectRef">3148492957</gameobj>
                                            <hoverTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">38</B>
                                              <G dataType="Byte">140</G>
                                              <R dataType="Byte">245</R>
                                            </hoverTint>
                                            <target dataType="ObjectRef">3799790557</target>
                                          </item>
                                        </_items>
                                        <_size dataType="Int">4</_size>
                                        <_version dataType="Int">12</_version>
                                      </compList>
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1508162488" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="3293428455">
                                            <item dataType="Type" id="3562508622" value="Duality.Components.Transform" />
                                            <item dataType="Type" id="1827081802" value="Duality.Components.Renderers.SpriteRenderer" />
                                            <item dataType="Type" id="868565246" value="Duality.Components.Renderers.TextRenderer" />
                                            <item dataType="Type" id="2973504346" value="BasicMenu.MenuSwitchToPage" />
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="2191449984">
                                            <item dataType="ObjectRef">1213840593</item>
                                            <item dataType="ObjectRef">495692229</item>
                                            <item dataType="ObjectRef">596154483</item>
                                            <item dataType="ObjectRef">3464560287</item>
                                          </values>
                                        </body>
                                      </compMap>
                                      <compTransform dataType="ObjectRef">1213840593</compTransform>
                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                        <header>
                                          <data dataType="Array" type="System.Byte[]" id="4281073829">s3CyBhjd/0COHWDcMho30Q==</data>
                                        </header>
                                        <body />
                                      </identifier>
                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                      <name dataType="String">No</name>
                                      <parent dataType="ObjectRef">2991214831</parent>
                                      <prefabLink />
                                    </item>
                                    <item dataType="Struct" type="Duality.GameObject" id="1225237399">
                                      <active dataType="Bool">true</active>
                                      <children />
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3679163927">
                                        <_items dataType="Array" type="Duality.Component[]" id="4020955918">
                                          <item dataType="Struct" type="Duality.Components.Transform" id="3585552331">
                                            <active dataType="Bool">true</active>
                                            <angle dataType="Float">0</angle>
                                            <angleAbs dataType="Float">0</angleAbs>
                                            <angleVel dataType="Float">0</angleVel>
                                            <angleVelAbs dataType="Float">0</angleVelAbs>
                                            <deriveAngle dataType="Bool">true</deriveAngle>
                                            <gameobj dataType="ObjectRef">1225237399</gameobj>
                                            <ignoreParent dataType="Bool">false</ignoreParent>
                                            <parentTransform dataType="ObjectRef">1056562467</parentTransform>
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
                                          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2867403967">
                                            <active dataType="Bool">true</active>
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">45</B>
                                              <G dataType="Byte">45</G>
                                              <R dataType="Byte">45</R>
                                            </colorTint>
                                            <customMat />
                                            <gameobj dataType="ObjectRef">1225237399</gameobj>
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
                                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                                          </item>
                                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="2967866221">
                                            <active dataType="Bool">true</active>
                                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">255</B>
                                              <G dataType="Byte">255</G>
                                              <R dataType="Byte">255</R>
                                            </colorTint>
                                            <customMat />
                                            <gameobj dataType="ObjectRef">1225237399</gameobj>
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                                              <contentPath />
                                            </iconMat>
                                            <offset dataType="Int">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="250618125">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1710331686">
                                                <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                                                  <contentPath dataType="String">Data\BasicMenu\Visuals\DuruSans-Regular.Font.res</contentPath>
                                                </item>
                                              </fonts>
                                              <icons />
                                              <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                                              <maxHeight dataType="Int">0</maxHeight>
                                              <maxWidth dataType="Int">0</maxWidth>
                                              <sourceText dataType="String">Yes</sourceText>
                                              <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                                            </text>
                                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                                          </item>
                                          <item dataType="Struct" type="BasicMenu.MenuQuitConfirm" id="3250337400">
                                            <active dataType="Bool">true</active>
                                            <gameobj dataType="ObjectRef">1225237399</gameobj>
                                            <hoverTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">38</B>
                                              <G dataType="Byte">43</G>
                                              <R dataType="Byte">245</R>
                                            </hoverTint>
                                          </item>
                                        </_items>
                                        <_size dataType="Int">4</_size>
                                        <_version dataType="Int">6</_version>
                                      </compList>
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2731500736" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="3636502941">
                                            <item dataType="ObjectRef">3562508622</item>
                                            <item dataType="ObjectRef">1827081802</item>
                                            <item dataType="ObjectRef">868565246</item>
                                            <item dataType="Type" id="4108079846" value="BasicMenu.MenuQuitConfirm" />
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="3995608312">
                                            <item dataType="ObjectRef">3585552331</item>
                                            <item dataType="ObjectRef">2867403967</item>
                                            <item dataType="ObjectRef">2967866221</item>
                                            <item dataType="ObjectRef">3250337400</item>
                                          </values>
                                        </body>
                                      </compMap>
                                      <compTransform dataType="ObjectRef">3585552331</compTransform>
                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                        <header>
                                          <data dataType="Array" type="System.Byte[]" id="1303494199">jeCXeZBSd0OOVGLvXCI2bQ==</data>
                                        </header>
                                        <body />
                                      </identifier>
                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                      <name dataType="String">Yes</name>
                                      <parent dataType="ObjectRef">2991214831</parent>
                                      <prefabLink />
                                    </item>
                                    <item dataType="Struct" type="Duality.GameObject" id="1156301705">
                                      <active dataType="Bool">true</active>
                                      <children />
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3371404041">
                                        <_items dataType="Array" type="Duality.Component[]" id="2206035598" length="4">
                                          <item dataType="Struct" type="Duality.Components.Transform" id="3516616637">
                                            <active dataType="Bool">true</active>
                                            <angle dataType="Float">0</angle>
                                            <angleAbs dataType="Float">0</angleAbs>
                                            <angleVel dataType="Float">0</angleVel>
                                            <angleVelAbs dataType="Float">0</angleVelAbs>
                                            <deriveAngle dataType="Bool">true</deriveAngle>
                                            <gameobj dataType="ObjectRef">1156301705</gameobj>
                                            <ignoreParent dataType="Bool">false</ignoreParent>
                                            <parentTransform dataType="ObjectRef">1056562467</parentTransform>
                                            <pos dataType="Struct" type="Duality.Vector3">
                                              <X dataType="Float">0</X>
                                              <Y dataType="Float">-50</Y>
                                              <Z dataType="Float">0</Z>
                                            </pos>
                                            <posAbs dataType="Struct" type="Duality.Vector3">
                                              <X dataType="Float">0</X>
                                              <Y dataType="Float">-50</Y>
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
                                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="2898930527">
                                            <active dataType="Bool">true</active>
                                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">255</B>
                                              <G dataType="Byte">255</G>
                                              <R dataType="Byte">255</R>
                                            </colorTint>
                                            <customMat />
                                            <gameobj dataType="ObjectRef">1156301705</gameobj>
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                                              <contentPath />
                                            </iconMat>
                                            <offset dataType="Int">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1307621871">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1903152366">
                                                <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                                                  <contentPath dataType="String">Data\BasicMenu\Visuals\DuruSans-Regular.Font.res</contentPath>
                                                </item>
                                              </fonts>
                                              <icons />
                                              <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                                              <maxHeight dataType="Int">0</maxHeight>
                                              <maxWidth dataType="Int">0</maxWidth>
                                              <sourceText dataType="String">Do you really want to quit?</sourceText>
                                              <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                                            </text>
                                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                                          </item>
                                        </_items>
                                        <_size dataType="Int">2</_size>
                                        <_version dataType="Int">2</_version>
                                      </compList>
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3043805760" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="2956512195">
                                            <item dataType="ObjectRef">3562508622</item>
                                            <item dataType="ObjectRef">868565246</item>
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="899324600">
                                            <item dataType="ObjectRef">3516616637</item>
                                            <item dataType="ObjectRef">2898930527</item>
                                          </values>
                                        </body>
                                      </compMap>
                                      <compTransform dataType="ObjectRef">3516616637</compTransform>
                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                        <header>
                                          <data dataType="Array" type="System.Byte[]" id="3994841577">HowsWsea+0+21YQa0ZqmbA==</data>
                                        </header>
                                        <body />
                                      </identifier>
                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                      <name dataType="String">Text</name>
                                      <parent dataType="ObjectRef">2991214831</parent>
                                      <prefabLink />
                                    </item>
                                  </_items>
                                  <_size dataType="Int">3</_size>
                                  <_version dataType="Int">9</_version>
                                </children>
                                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1081580280">
                                  <_items dataType="Array" type="Duality.Component[]" id="1564357257" length="4">
                                    <item dataType="ObjectRef">1056562467</item>
                                    <item dataType="ObjectRef">505026784</item>
                                  </_items>
                                  <_size dataType="Int">2</_size>
                                  <_version dataType="Int">6</_version>
                                </compList>
                                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="623612233" surrogate="true">
                                  <header />
                                  <body>
                                    <keys dataType="Array" type="System.Object[]" id="3053465364">
                                      <item dataType="ObjectRef">3562508622</item>
                                      <item dataType="Type" id="3459735652" value="BasicMenu.MenuPage" />
                                    </keys>
                                    <values dataType="Array" type="System.Object[]" id="95060278">
                                      <item dataType="ObjectRef">1056562467</item>
                                      <item dataType="ObjectRef">505026784</item>
                                    </values>
                                  </body>
                                </compMap>
                                <compTransform dataType="ObjectRef">1056562467</compTransform>
                                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                  <header>
                                    <data dataType="Array" type="System.Byte[]" id="448063920">UxHty6eN2kGa9fQ9f14PZg==</data>
                                  </header>
                                  <body />
                                </identifier>
                                <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                <name dataType="String">#MenuQuit</name>
                                <parent />
                                <prefabLink />
                              </gameobj>
                            </target>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                        <_version dataType="Int">6</_version>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4119580826" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="904924336">
                            <item dataType="ObjectRef">3562508622</item>
                            <item dataType="ObjectRef">1827081802</item>
                            <item dataType="ObjectRef">868565246</item>
                            <item dataType="ObjectRef">2973504346</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="4020211566">
                            <item dataType="ObjectRef">243362730</item>
                            <item dataType="ObjectRef">3820181662</item>
                            <item dataType="ObjectRef">3920643916</item>
                            <item dataType="ObjectRef">2494082424</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">243362730</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="3916615436">Z5+raHo95EC9J9Txr9RI9Q==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Quit</name>
                      <parent dataType="ObjectRef">1991011308</parent>
                      <prefabLink />
                    </item>
                    <item dataType="Struct" type="Duality.GameObject" id="1367178791">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2051579991">
                        <_items dataType="Array" type="Duality.Component[]" id="1523610126">
                          <item dataType="Struct" type="Duality.Components.Transform" id="3727493723">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <angleVel dataType="Float">0</angleVel>
                            <angleVelAbs dataType="Float">0</angleVelAbs>
                            <deriveAngle dataType="Bool">true</deriveAngle>
                            <gameobj dataType="ObjectRef">1367178791</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
                            <parentTransform dataType="ObjectRef">56358944</parentTransform>
                            <pos dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">0</X>
                              <Y dataType="Float">-30</Y>
                              <Z dataType="Float">0</Z>
                            </pos>
                            <posAbs dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">0</X>
                              <Y dataType="Float">-30</Y>
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
                          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3009345359">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">45</B>
                              <G dataType="Byte">45</G>
                              <R dataType="Byte">45</R>
                            </colorTint>
                            <customMat />
                            <gameobj dataType="ObjectRef">1367178791</gameobj>
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
                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                          </item>
                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="3109807613">
                            <active dataType="Bool">true</active>
                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <gameobj dataType="ObjectRef">1367178791</gameobj>
                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                              <contentPath />
                            </iconMat>
                            <offset dataType="Int">0</offset>
                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1618922621">
                              <flowAreas />
                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="4105342758">
                                <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                                  <contentPath dataType="String">Data\BasicMenu\Visuals\DuruSans-Regular.Font.res</contentPath>
                                </item>
                              </fonts>
                              <icons />
                              <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                              <maxHeight dataType="Int">0</maxHeight>
                              <maxWidth dataType="Int">0</maxWidth>
                              <sourceText dataType="String">Another menu</sourceText>
                              <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                            </text>
                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                          </item>
                          <item dataType="Struct" type="BasicMenu.MenuSwitchToPage" id="1683246121">
                            <active dataType="Bool">true</active>
                            <gameobj dataType="ObjectRef">1367178791</gameobj>
                            <hoverTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">38</B>
                              <G dataType="Byte">140</G>
                              <R dataType="Byte">245</R>
                            </hoverTint>
                            <target dataType="Struct" type="BasicMenu.MenuPage" id="1674661202">
                              <active dataType="Bool">true</active>
                              <gameobj dataType="Struct" type="Duality.GameObject" id="4160849249">
                                <active dataType="Bool">false</active>
                                <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1249555358">
                                  <_items dataType="Array" type="Duality.GameObject[]" id="3372310416" length="4">
                                    <item dataType="Struct" type="Duality.GameObject" id="147130678">
                                      <active dataType="Bool">true</active>
                                      <children />
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1145126966">
                                        <_items dataType="Array" type="Duality.Component[]" id="2522995040">
                                          <item dataType="Struct" type="Duality.Components.Transform" id="2507445610">
                                            <active dataType="Bool">true</active>
                                            <angle dataType="Float">0</angle>
                                            <angleAbs dataType="Float">0</angleAbs>
                                            <angleVel dataType="Float">0</angleVel>
                                            <angleVelAbs dataType="Float">0</angleVelAbs>
                                            <deriveAngle dataType="Bool">true</deriveAngle>
                                            <gameobj dataType="ObjectRef">147130678</gameobj>
                                            <ignoreParent dataType="Bool">false</ignoreParent>
                                            <parentTransform dataType="Struct" type="Duality.Components.Transform" id="2226196885">
                                              <active dataType="Bool">true</active>
                                              <angle dataType="Float">0</angle>
                                              <angleAbs dataType="Float">0</angleAbs>
                                              <angleVel dataType="Float">0</angleVel>
                                              <angleVelAbs dataType="Float">0</angleVelAbs>
                                              <deriveAngle dataType="Bool">true</deriveAngle>
                                              <gameobj dataType="ObjectRef">4160849249</gameobj>
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
                                          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1789297246">
                                            <active dataType="Bool">true</active>
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">45</B>
                                              <G dataType="Byte">45</G>
                                              <R dataType="Byte">45</R>
                                            </colorTint>
                                            <customMat />
                                            <gameobj dataType="ObjectRef">147130678</gameobj>
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
                                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                                          </item>
                                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="1889759500">
                                            <active dataType="Bool">true</active>
                                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">255</B>
                                              <G dataType="Byte">255</G>
                                              <R dataType="Byte">255</R>
                                            </colorTint>
                                            <customMat />
                                            <gameobj dataType="ObjectRef">147130678</gameobj>
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                                              <contentPath />
                                            </iconMat>
                                            <offset dataType="Int">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="3198418452">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="3414604388">
                                                <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                                                  <contentPath dataType="String">Data\BasicMenu\Visuals\DuruSans-Regular.Font.res</contentPath>
                                                </item>
                                              </fonts>
                                              <icons />
                                              <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                                              <maxHeight dataType="Int">0</maxHeight>
                                              <maxWidth dataType="Int">0</maxWidth>
                                              <sourceText dataType="String">Back</sourceText>
                                              <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                                            </text>
                                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                                          </item>
                                          <item dataType="Struct" type="BasicMenu.MenuSwitchToPage" id="463198008">
                                            <active dataType="Bool">true</active>
                                            <gameobj dataType="ObjectRef">147130678</gameobj>
                                            <hoverTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">38</B>
                                              <G dataType="Byte">140</G>
                                              <R dataType="Byte">245</R>
                                            </hoverTint>
                                            <target dataType="ObjectRef">3799790557</target>
                                          </item>
                                        </_items>
                                        <_size dataType="Int">4</_size>
                                        <_version dataType="Int">6</_version>
                                      </compList>
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1724346522" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="1374131460">
                                            <item dataType="ObjectRef">3562508622</item>
                                            <item dataType="ObjectRef">1827081802</item>
                                            <item dataType="ObjectRef">868565246</item>
                                            <item dataType="ObjectRef">2973504346</item>
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="1530391958">
                                            <item dataType="ObjectRef">2507445610</item>
                                            <item dataType="ObjectRef">1789297246</item>
                                            <item dataType="ObjectRef">1889759500</item>
                                            <item dataType="ObjectRef">463198008</item>
                                          </values>
                                        </body>
                                      </compMap>
                                      <compTransform dataType="ObjectRef">2507445610</compTransform>
                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                        <header>
                                          <data dataType="Array" type="System.Byte[]" id="2303512512">JkpTB5ZLZkOKfkQFW2fWdg==</data>
                                        </header>
                                        <body />
                                      </identifier>
                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                      <name dataType="String">Back</name>
                                      <parent dataType="ObjectRef">4160849249</parent>
                                      <prefabLink />
                                    </item>
                                    <item dataType="Struct" type="Duality.GameObject" id="3944217670">
                                      <active dataType="Bool">true</active>
                                      <children />
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="226344550">
                                        <_items dataType="Array" type="Duality.Component[]" id="1319281536">
                                          <item dataType="Struct" type="Duality.Components.Transform" id="2009565306">
                                            <active dataType="Bool">true</active>
                                            <angle dataType="Float">0</angle>
                                            <angleAbs dataType="Float">0</angleAbs>
                                            <angleVel dataType="Float">0</angleVel>
                                            <angleVelAbs dataType="Float">0</angleVelAbs>
                                            <deriveAngle dataType="Bool">true</deriveAngle>
                                            <gameobj dataType="ObjectRef">3944217670</gameobj>
                                            <ignoreParent dataType="Bool">false</ignoreParent>
                                            <parentTransform dataType="ObjectRef">2226196885</parentTransform>
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
                                              <B dataType="Byte">45</B>
                                              <G dataType="Byte">45</G>
                                              <R dataType="Byte">45</R>
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
                                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                                          </item>
                                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="1391879196">
                                            <active dataType="Bool">true</active>
                                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">255</B>
                                              <G dataType="Byte">255</G>
                                              <R dataType="Byte">255</R>
                                            </colorTint>
                                            <customMat />
                                            <gameobj dataType="ObjectRef">3944217670</gameobj>
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                                              <contentPath />
                                            </iconMat>
                                            <offset dataType="Int">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2072820292">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="13806148">
                                                <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                                                  <contentPath dataType="String">Data\BasicMenu\Visuals\DuruSans-Regular.Font.res</contentPath>
                                                </item>
                                              </fonts>
                                              <icons />
                                              <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                                              <maxHeight dataType="Int">0</maxHeight>
                                              <maxWidth dataType="Int">0</maxWidth>
                                              <sourceText dataType="String">Change</sourceText>
                                              <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                                            </text>
                                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                                          </item>
                                          <item dataType="Struct" type="BasicMenu.MenuChangeColor" id="2040529331">
                                            <active dataType="Bool">true</active>
                                            <gameobj dataType="ObjectRef">3944217670</gameobj>
                                            <hoverTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">38</B>
                                              <G dataType="Byte">140</G>
                                              <R dataType="Byte">245</R>
                                            </hoverTint>
                                          </item>
                                        </_items>
                                        <_size dataType="Int">4</_size>
                                        <_version dataType="Int">12</_version>
                                      </compList>
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4133327674" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="3223299540">
                                            <item dataType="ObjectRef">3562508622</item>
                                            <item dataType="ObjectRef">1827081802</item>
                                            <item dataType="ObjectRef">868565246</item>
                                            <item dataType="Type" id="4209472740" value="BasicMenu.MenuChangeColor" />
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="3550165942">
                                            <item dataType="ObjectRef">2009565306</item>
                                            <item dataType="ObjectRef">1291416942</item>
                                            <item dataType="ObjectRef">1391879196</item>
                                            <item dataType="ObjectRef">2040529331</item>
                                          </values>
                                        </body>
                                      </compMap>
                                      <compTransform dataType="ObjectRef">2009565306</compTransform>
                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                        <header>
                                          <data dataType="Array" type="System.Byte[]" id="2785128688">6LxU90NJPUCn4lXtH+n0kw==</data>
                                        </header>
                                        <body />
                                      </identifier>
                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                      <name dataType="String">ChangeColor</name>
                                      <parent dataType="ObjectRef">4160849249</parent>
                                      <prefabLink />
                                    </item>
                                    <item dataType="Struct" type="Duality.GameObject" id="2087538084">
                                      <active dataType="Bool">true</active>
                                      <children />
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4269717644">
                                        <_items dataType="Array" type="Duality.Component[]" id="3749914020" length="4">
                                          <item dataType="Struct" type="Duality.Components.Transform" id="152885720">
                                            <active dataType="Bool">true</active>
                                            <angle dataType="Float">0</angle>
                                            <angleAbs dataType="Float">0</angleAbs>
                                            <angleVel dataType="Float">0</angleVel>
                                            <angleVelAbs dataType="Float">0</angleVelAbs>
                                            <deriveAngle dataType="Bool">true</deriveAngle>
                                            <gameobj dataType="ObjectRef">2087538084</gameobj>
                                            <ignoreParent dataType="Bool">false</ignoreParent>
                                            <parentTransform dataType="ObjectRef">2226196885</parentTransform>
                                            <pos dataType="Struct" type="Duality.Vector3">
                                              <X dataType="Float">0</X>
                                              <Y dataType="Float">-50</Y>
                                              <Z dataType="Float">0</Z>
                                            </pos>
                                            <posAbs dataType="Struct" type="Duality.Vector3">
                                              <X dataType="Float">0</X>
                                              <Y dataType="Float">-50</Y>
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
                                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="3830166906">
                                            <active dataType="Bool">true</active>
                                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">255</B>
                                              <G dataType="Byte">255</G>
                                              <R dataType="Byte">255</R>
                                            </colorTint>
                                            <customMat />
                                            <gameobj dataType="ObjectRef">2087538084</gameobj>
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                                              <contentPath />
                                            </iconMat>
                                            <offset dataType="Int">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="4152971802">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="517886336">
                                                <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                                                  <contentPath dataType="String">Data\BasicMenu\Visuals\DuruSans-Regular.Font.res</contentPath>
                                                </item>
                                              </fonts>
                                              <icons />
                                              <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                                              <maxHeight dataType="Int">0</maxHeight>
                                              <maxWidth dataType="Int">0</maxWidth>
                                              <sourceText dataType="String">Click Change to change its color</sourceText>
                                              <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                                            </text>
                                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                                          </item>
                                        </_items>
                                        <_size dataType="Int">2</_size>
                                        <_version dataType="Int">2</_version>
                                      </compList>
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="166475766" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="208104966">
                                            <item dataType="ObjectRef">3562508622</item>
                                            <item dataType="ObjectRef">868565246</item>
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="2059153722">
                                            <item dataType="ObjectRef">152885720</item>
                                            <item dataType="ObjectRef">3830166906</item>
                                          </values>
                                        </body>
                                      </compMap>
                                      <compTransform dataType="ObjectRef">152885720</compTransform>
                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                        <header>
                                          <data dataType="Array" type="System.Byte[]" id="496468102">XgOaC/d3nU6CewDCFzc6Zw==</data>
                                        </header>
                                        <body />
                                      </identifier>
                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                      <name dataType="String">Text</name>
                                      <parent dataType="ObjectRef">4160849249</parent>
                                      <prefabLink />
                                    </item>
                                  </_items>
                                  <_size dataType="Int">3</_size>
                                  <_version dataType="Int">3</_version>
                                </children>
                                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="457994634">
                                  <_items dataType="Array" type="Duality.Component[]" id="1074222780" length="4">
                                    <item dataType="ObjectRef">2226196885</item>
                                    <item dataType="ObjectRef">1674661202</item>
                                  </_items>
                                  <_size dataType="Int">2</_size>
                                  <_version dataType="Int">4</_version>
                                </compList>
                                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2041585262" surrogate="true">
                                  <header />
                                  <body>
                                    <keys dataType="Array" type="System.Object[]" id="3431116448">
                                      <item dataType="ObjectRef">3562508622</item>
                                      <item dataType="ObjectRef">3459735652</item>
                                    </keys>
                                    <values dataType="Array" type="System.Object[]" id="1617593486">
                                      <item dataType="ObjectRef">2226196885</item>
                                      <item dataType="ObjectRef">1674661202</item>
                                    </values>
                                  </body>
                                </compMap>
                                <compTransform dataType="ObjectRef">2226196885</compTransform>
                                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                  <header>
                                    <data dataType="Array" type="System.Byte[]" id="3178657468">0sUNkQwR50CeTXizJxhuVA==</data>
                                  </header>
                                  <body />
                                </identifier>
                                <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                <name dataType="String">#MenuAnother</name>
                                <parent />
                                <prefabLink />
                              </gameobj>
                            </target>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                        <_version dataType="Int">8</_version>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3333663680" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="1998733533">
                            <item dataType="ObjectRef">3562508622</item>
                            <item dataType="ObjectRef">1827081802</item>
                            <item dataType="ObjectRef">868565246</item>
                            <item dataType="ObjectRef">2973504346</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="4204577912">
                            <item dataType="ObjectRef">3727493723</item>
                            <item dataType="ObjectRef">3009345359</item>
                            <item dataType="ObjectRef">3109807613</item>
                            <item dataType="ObjectRef">1683246121</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">3727493723</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="3393848439">RThwaA1XakahLG48HxAACA==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Another</name>
                      <parent dataType="ObjectRef">1991011308</parent>
                      <prefabLink />
                    </item>
                  </_items>
                  <_size dataType="Int">2</_size>
                  <_version dataType="Int">4</_version>
                </children>
                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4202967264">
                  <_items dataType="Array" type="Duality.Component[]" id="1965095717" length="4">
                    <item dataType="ObjectRef">56358944</item>
                    <item dataType="ObjectRef">3799790557</item>
                  </_items>
                  <_size dataType="Int">2</_size>
                  <_version dataType="Int">12</_version>
                </compList>
                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1719832285" surrogate="true">
                  <header />
                  <body>
                    <keys dataType="Array" type="System.Object[]" id="606515620">
                      <item dataType="ObjectRef">3562508622</item>
                      <item dataType="ObjectRef">3459735652</item>
                    </keys>
                    <values dataType="Array" type="System.Object[]" id="862054166">
                      <item dataType="ObjectRef">56358944</item>
                      <item dataType="ObjectRef">3799790557</item>
                    </values>
                  </body>
                </compMap>
                <compTransform dataType="ObjectRef">56358944</compTransform>
                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                  <header>
                    <data dataType="Array" type="System.Byte[]" id="1205820576">qSytuAGr30epimWOTaEamg==</data>
                  </header>
                  <body />
                </identifier>
                <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                <name dataType="String">#MenuMain</name>
                <parent />
                <prefabLink />
              </gameobj>
            </startingMenu>
          </item>
          <item dataType="Struct" type="BasicMenu.UpdateMenuController" id="743110769">
            <active dataType="Bool">false</active>
            <gameobj dataType="ObjectRef">3588826678</gameobj>
            <startingMenu dataType="ObjectRef">3799790557</startingMenu>
          </item>
        </_items>
        <_size dataType="Int">5</_size>
        <_version dataType="Int">5</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4076619662" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="726969330">
            <item dataType="ObjectRef">3562508622</item>
            <item dataType="Type" id="3939607504" value="Duality.Components.Camera" />
            <item dataType="Type" id="1077793390" value="Duality.Components.SoundListener" />
            <item dataType="Type" id="2908634028" value="BasicMenu.EventMenuController" />
            <item dataType="Type" id="1125487122" value="BasicMenu.UpdateMenuController" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="3096052554">
            <item dataType="ObjectRef">1654174314</item>
            <item dataType="ObjectRef">4126102485</item>
            <item dataType="ObjectRef">4242308049</item>
            <item dataType="ObjectRef">1563800156</item>
            <item dataType="ObjectRef">743110769</item>
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
    <item dataType="ObjectRef">1991011308</item>
    <item dataType="ObjectRef">2991214831</item>
    <item dataType="ObjectRef">4160849249</item>
    <item dataType="Struct" type="Duality.GameObject" id="207326468">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="469589946">
        <_items dataType="Array" type="Duality.Component[]" id="1470629888" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="2567641400">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">207326468</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <parentTransform />
            <pos dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">-200</Y>
              <Z dataType="Float">0</Z>
            </pos>
            <posAbs dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">-200</Y>
              <Z dataType="Float">0</Z>
            </posAbs>
            <scale dataType="Float">1.25</scale>
            <scaleAbs dataType="Float">1.25</scaleAbs>
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
          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1849493036">
            <active dataType="Bool">true</active>
            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">192</A>
              <B dataType="Byte">64</B>
              <G dataType="Byte">64</G>
              <R dataType="Byte">64</R>
            </colorTint>
            <customMat />
            <gameobj dataType="ObjectRef">207326468</gameobj>
            <offset dataType="Int">0</offset>
            <pixelGrid dataType="Bool">false</pixelGrid>
            <rect dataType="Struct" type="Duality.Rect">
              <H dataType="Float">256</H>
              <W dataType="Float">512</W>
              <X dataType="Float">-256</X>
              <Y dataType="Float">-128</Y>
            </rect>
            <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
            <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
              <contentPath dataType="String">Default:Material:DualityLogoBig</contentPath>
            </sharedMat>
            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          </item>
        </_items>
        <_size dataType="Int">2</_size>
        <_version dataType="Int">2</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2473047482" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="1583631872">
            <item dataType="ObjectRef">3562508622</item>
            <item dataType="ObjectRef">1827081802</item>
          </keys>
          <values dataType="Array" type="System.Object[]" id="2263646158">
            <item dataType="ObjectRef">2567641400</item>
            <item dataType="ObjectRef">1849493036</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">2567641400</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1122741916">AMEXEUxNi0ynXVBg8zLQjw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Background</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="2733405151">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2935515421">
        <_items dataType="Array" type="Duality.Component[]" id="2120373478" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="798752787">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">2733405151</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <parentTransform />
            <pos dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">160</Y>
              <Z dataType="Float">0</Z>
            </pos>
            <posAbs dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">160</Y>
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
          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="181066677">
            <active dataType="Bool">true</active>
            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">255</B>
              <G dataType="Byte">255</G>
              <R dataType="Byte">255</R>
            </colorTint>
            <customMat />
            <gameobj dataType="ObjectRef">2733405151</gameobj>
            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
              <contentPath />
            </iconMat>
            <offset dataType="Int">0</offset>
            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1658023141">
              <flowAreas />
              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="24092822">
                <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                  <contentPath dataType="String">Default:Font:GenericMonospace10</contentPath>
                </item>
              </fonts>
              <icons />
              <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
              <maxHeight dataType="Int">0</maxHeight>
              <maxWidth dataType="Int">350</maxWidth>
              <sourceText dataType="String">To start using the menu, enable either /cFFAA44FFEventMenuController/cFFFFFFFF or /cFFAA44FFUpdateMenuController/cFFFFFFFF in MainCamera's GameObject./n/nHaving both enabled at the same time might create issues./n/nAlso, make sure to switch to the /c44AAFFFFGame View/cFFFFFFFF when running this sample in the editor.</sourceText>
              <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
            </text>
            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          </item>
        </_items>
        <_size dataType="Int">2</_size>
        <_version dataType="Int">2</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4186247928" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3822850679">
            <item dataType="ObjectRef">3562508622</item>
            <item dataType="ObjectRef">868565246</item>
          </keys>
          <values dataType="Array" type="System.Object[]" id="2715098432">
            <item dataType="ObjectRef">798752787</item>
            <item dataType="ObjectRef">181066677</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">798752787</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1689408213">7Tq2cVo2nkm6EuVYyhtBrA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">TextRenderer</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">2178015094</item>
    <item dataType="ObjectRef">1367178791</item>
    <item dataType="ObjectRef">3148492957</item>
    <item dataType="ObjectRef">1225237399</item>
    <item dataType="ObjectRef">1156301705</item>
    <item dataType="ObjectRef">147130678</item>
    <item dataType="ObjectRef">3944217670</item>
    <item dataType="ObjectRef">2087538084</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
