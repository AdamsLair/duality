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
            <vel dataType="Struct" type="Duality.Vector3" />
            <velAbs dataType="Struct" type="Duality.Vector3" />
          </item>
          <item dataType="Struct" type="Duality.Components.Camera" id="4126102485">
            <active dataType="Bool">true</active>
            <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">0</A>
              <B dataType="Byte">162</B>
              <G dataType="Byte">126</G>
              <R dataType="Byte">94</R>
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
          <item dataType="Struct" type="BasicMenu.EventMenuController" id="1563800156">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3588826678</gameobj>
            <startingMenu dataType="Struct" type="BasicMenu.MenuPage" id="664430158">
              <active dataType="Bool">true</active>
              <gameobj dataType="Struct" type="Duality.GameObject" id="3150618205">
                <active dataType="Bool">true</active>
                <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2072697719">
                  <_items dataType="Array" type="Duality.GameObject[]" id="300031886" length="4">
                    <item dataType="Struct" type="Duality.GameObject" id="1781294501">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1932011141">
                        <_items dataType="Array" type="Duality.Component[]" id="3257920342">
                          <item dataType="Struct" type="Duality.Components.Transform" id="4141609433">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <angleVel dataType="Float">0</angleVel>
                            <angleVelAbs dataType="Float">0</angleVelAbs>
                            <deriveAngle dataType="Bool">true</deriveAngle>
                            <gameobj dataType="ObjectRef">1781294501</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
                            <parentTransform dataType="Struct" type="Duality.Components.Transform" id="1215965841">
                              <active dataType="Bool">true</active>
                              <angle dataType="Float">0</angle>
                              <angleAbs dataType="Float">0</angleAbs>
                              <angleVel dataType="Float">0</angleVel>
                              <angleVelAbs dataType="Float">0</angleVelAbs>
                              <deriveAngle dataType="Bool">true</deriveAngle>
                              <gameobj dataType="ObjectRef">3150618205</gameobj>
                              <ignoreParent dataType="Bool">false</ignoreParent>
                              <parentTransform />
                              <pos dataType="Struct" type="Duality.Vector3" />
                              <posAbs dataType="Struct" type="Duality.Vector3" />
                              <scale dataType="Float">1</scale>
                              <scaleAbs dataType="Float">1</scaleAbs>
                              <vel dataType="Struct" type="Duality.Vector3" />
                              <velAbs dataType="Struct" type="Duality.Vector3" />
                            </parentTransform>
                            <pos dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">0</X>
                              <Y dataType="Float">60</Y>
                              <Z dataType="Float">0</Z>
                            </pos>
                            <posAbs dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">0</X>
                              <Y dataType="Float">60</Y>
                              <Z dataType="Float">0</Z>
                            </posAbs>
                            <scale dataType="Float">1</scale>
                            <scaleAbs dataType="Float">1</scaleAbs>
                            <vel dataType="Struct" type="Duality.Vector3" />
                            <velAbs dataType="Struct" type="Duality.Vector3" />
                          </item>
                          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3423461069">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">45</B>
                              <G dataType="Byte">45</G>
                              <R dataType="Byte">45</R>
                            </colorTint>
                            <customMat />
                            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                            <gameobj dataType="ObjectRef">1781294501</gameobj>
                            <offset dataType="Float">1</offset>
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
                            <spriteIndex dataType="Int">-1</spriteIndex>
                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                          </item>
                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="3523923323">
                            <active dataType="Bool">true</active>
                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <gameobj dataType="ObjectRef">1781294501</gameobj>
                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                            <offset dataType="Float">0</offset>
                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2766966635">
                              <flowAreas />
                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1507712118">
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
                          <item dataType="Struct" type="BasicMenu.MenuSwitchToPage" id="2097361831">
                            <active dataType="Bool">true</active>
                            <gameobj dataType="ObjectRef">1781294501</gameobj>
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
                                <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2981349324">
                                  <_items dataType="Array" type="Duality.GameObject[]" id="3577730212" length="4">
                                    <item dataType="Struct" type="Duality.GameObject" id="3148492957">
                                      <active dataType="Bool">true</active>
                                      <children />
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="111850097">
                                        <_items dataType="Array" type="Duality.Component[]" id="2466838190">
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
                                              <pos dataType="Struct" type="Duality.Vector3" />
                                              <posAbs dataType="Struct" type="Duality.Vector3" />
                                              <scale dataType="Float">1</scale>
                                              <scaleAbs dataType="Float">1</scaleAbs>
                                              <vel dataType="Struct" type="Duality.Vector3" />
                                              <velAbs dataType="Struct" type="Duality.Vector3" />
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
                                            <vel dataType="Struct" type="Duality.Vector3" />
                                            <velAbs dataType="Struct" type="Duality.Vector3" />
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
                                            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                                            <gameobj dataType="ObjectRef">3148492957</gameobj>
                                            <offset dataType="Float">1</offset>
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
                                            <spriteIndex dataType="Int">-1</spriteIndex>
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
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                                            <offset dataType="Float">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1704046355">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="91612902">
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
                                            <target dataType="ObjectRef">664430158</target>
                                          </item>
                                        </_items>
                                        <_size dataType="Int">4</_size>
                                      </compList>
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3091267296" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="4094512091">
                                            <item dataType="Type" id="4260545686" value="Duality.Components.Transform" />
                                            <item dataType="Type" id="1695278298" value="Duality.Components.Renderers.SpriteRenderer" />
                                            <item dataType="Type" id="651038006" value="Duality.Components.Renderers.TextRenderer" />
                                            <item dataType="Type" id="3489050106" value="BasicMenu.MenuSwitchToPage" />
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="271037288">
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
                                          <data dataType="Array" type="System.Byte[]" id="974830353">s3CyBhjd/0COHWDcMho30Q==</data>
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
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1333459979">
                                        <_items dataType="Array" type="Duality.Component[]" id="3025160310">
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
                                            <vel dataType="Struct" type="Duality.Vector3" />
                                            <velAbs dataType="Struct" type="Duality.Vector3" />
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
                                            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                                            <gameobj dataType="ObjectRef">1225237399</gameobj>
                                            <offset dataType="Float">1</offset>
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
                                            <spriteIndex dataType="Int">-1</spriteIndex>
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
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                                            <offset dataType="Float">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2978313277">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="4071413798">
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
                                      </compList>
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="309877448" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="51045281">
                                            <item dataType="ObjectRef">4260545686</item>
                                            <item dataType="ObjectRef">1695278298</item>
                                            <item dataType="ObjectRef">651038006</item>
                                            <item dataType="Type" id="233023598" value="BasicMenu.MenuQuitConfirm" />
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="1294425888">
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
                                          <data dataType="Array" type="System.Byte[]" id="4152725043">jeCXeZBSd0OOVGLvXCI2bQ==</data>
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
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3182377429">
                                        <_items dataType="Array" type="Duality.Component[]" id="1435425782" length="4">
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
                                              <Y dataType="Float">-75</Y>
                                              <Z dataType="Float">0</Z>
                                            </pos>
                                            <posAbs dataType="Struct" type="Duality.Vector3">
                                              <X dataType="Float">0</X>
                                              <Y dataType="Float">-75</Y>
                                              <Z dataType="Float">0</Z>
                                            </posAbs>
                                            <scale dataType="Float">1</scale>
                                            <scaleAbs dataType="Float">1</scaleAbs>
                                            <vel dataType="Struct" type="Duality.Vector3" />
                                            <velAbs dataType="Struct" type="Duality.Vector3" />
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
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                                            <offset dataType="Float">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2530555455">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="4266294446">
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
                                      </compList>
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2960123976" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="3034918655">
                                            <item dataType="ObjectRef">4260545686</item>
                                            <item dataType="ObjectRef">651038006</item>
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="1090272096">
                                            <item dataType="ObjectRef">3516616637</item>
                                            <item dataType="ObjectRef">2898930527</item>
                                          </values>
                                        </body>
                                      </compMap>
                                      <compTransform dataType="ObjectRef">3516616637</compTransform>
                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                        <header>
                                          <data dataType="Array" type="System.Byte[]" id="4237147821">HowsWsea+0+21YQa0ZqmbA==</data>
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
                                </children>
                                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2645172982">
                                  <_items dataType="Array" type="Duality.Component[]" id="2200064070" length="4">
                                    <item dataType="ObjectRef">1056562467</item>
                                    <item dataType="ObjectRef">505026784</item>
                                  </_items>
                                  <_size dataType="Int">2</_size>
                                </compList>
                                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3967442904" surrogate="true">
                                  <header />
                                  <body>
                                    <keys dataType="Array" type="System.Object[]" id="2438100856">
                                      <item dataType="ObjectRef">4260545686</item>
                                      <item dataType="Type" id="72148844" value="BasicMenu.MenuPage" />
                                    </keys>
                                    <values dataType="Array" type="System.Object[]" id="1852023774">
                                      <item dataType="ObjectRef">1056562467</item>
                                      <item dataType="ObjectRef">505026784</item>
                                    </values>
                                  </body>
                                </compMap>
                                <compTransform dataType="ObjectRef">1056562467</compTransform>
                                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                  <header>
                                    <data dataType="Array" type="System.Byte[]" id="2313314852">UxHty6eN2kGa9fQ9f14PZg==</data>
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
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2220177576" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="3582168431">
                            <item dataType="ObjectRef">4260545686</item>
                            <item dataType="ObjectRef">1695278298</item>
                            <item dataType="ObjectRef">651038006</item>
                            <item dataType="ObjectRef">3489050106</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="1484602016">
                            <item dataType="ObjectRef">4141609433</item>
                            <item dataType="ObjectRef">3423461069</item>
                            <item dataType="ObjectRef">3523923323</item>
                            <item dataType="ObjectRef">2097361831</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">4141609433</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="1600206077">kS8f+DMrU0qmnrkON0HZZw==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Quit</name>
                      <parent dataType="ObjectRef">3150618205</parent>
                      <prefabLink />
                    </item>
                    <item dataType="Struct" type="Duality.GameObject" id="1080590022">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2846753402">
                        <_items dataType="Array" type="Duality.Component[]" id="365160320">
                          <item dataType="Struct" type="Duality.Components.Transform" id="3440904954">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <angleVel dataType="Float">0</angleVel>
                            <angleVelAbs dataType="Float">0</angleVelAbs>
                            <deriveAngle dataType="Bool">true</deriveAngle>
                            <gameobj dataType="ObjectRef">1080590022</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
                            <parentTransform dataType="ObjectRef">1215965841</parentTransform>
                            <pos dataType="Struct" type="Duality.Vector3" />
                            <posAbs dataType="Struct" type="Duality.Vector3" />
                            <scale dataType="Float">1</scale>
                            <scaleAbs dataType="Float">1</scaleAbs>
                            <vel dataType="Struct" type="Duality.Vector3" />
                            <velAbs dataType="Struct" type="Duality.Vector3" />
                          </item>
                          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2722756590">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">45</B>
                              <G dataType="Byte">45</G>
                              <R dataType="Byte">45</R>
                            </colorTint>
                            <customMat />
                            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                            <gameobj dataType="ObjectRef">1080590022</gameobj>
                            <offset dataType="Float">1</offset>
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
                            <spriteIndex dataType="Int">-1</spriteIndex>
                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                          </item>
                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="2823218844">
                            <active dataType="Bool">true</active>
                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <gameobj dataType="ObjectRef">1080590022</gameobj>
                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                            <offset dataType="Float">0</offset>
                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2881498308">
                              <flowAreas />
                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="2009802564">
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
                          <item dataType="Struct" type="BasicMenu.MenuSwitchToPage" id="1396657352">
                            <active dataType="Bool">true</active>
                            <gameobj dataType="ObjectRef">1080590022</gameobj>
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
                                <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3697665419">
                                  <_items dataType="Array" type="Duality.GameObject[]" id="1131466870" length="4">
                                    <item dataType="Struct" type="Duality.GameObject" id="147130678">
                                      <active dataType="Bool">true</active>
                                      <children />
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3271081946">
                                        <_items dataType="Array" type="Duality.Component[]" id="2327988480">
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
                                              <pos dataType="Struct" type="Duality.Vector3" />
                                              <posAbs dataType="Struct" type="Duality.Vector3" />
                                              <scale dataType="Float">1</scale>
                                              <scaleAbs dataType="Float">1</scaleAbs>
                                              <vel dataType="Struct" type="Duality.Vector3" />
                                              <velAbs dataType="Struct" type="Duality.Vector3" />
                                            </parentTransform>
                                            <pos dataType="Struct" type="Duality.Vector3">
                                              <X dataType="Float">75</X>
                                              <Y dataType="Float">30</Y>
                                              <Z dataType="Float">0</Z>
                                            </pos>
                                            <posAbs dataType="Struct" type="Duality.Vector3">
                                              <X dataType="Float">75</X>
                                              <Y dataType="Float">30</Y>
                                              <Z dataType="Float">0</Z>
                                            </posAbs>
                                            <scale dataType="Float">1</scale>
                                            <scaleAbs dataType="Float">1</scaleAbs>
                                            <vel dataType="Struct" type="Duality.Vector3" />
                                            <velAbs dataType="Struct" type="Duality.Vector3" />
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
                                            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                                            <gameobj dataType="ObjectRef">147130678</gameobj>
                                            <offset dataType="Float">1</offset>
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
                                            <spriteIndex dataType="Int">-1</spriteIndex>
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
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                                            <offset dataType="Float">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="3488900692">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="3438658788">
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
                                            <target dataType="ObjectRef">664430158</target>
                                          </item>
                                        </_items>
                                        <_size dataType="Int">4</_size>
                                      </compList>
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="482172602" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="1505474080">
                                            <item dataType="ObjectRef">4260545686</item>
                                            <item dataType="ObjectRef">1695278298</item>
                                            <item dataType="ObjectRef">651038006</item>
                                            <item dataType="ObjectRef">3489050106</item>
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="2763375502">
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
                                          <data dataType="Array" type="System.Byte[]" id="3914593596">JkpTB5ZLZkOKfkQFW2fWdg==</data>
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
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3019332394">
                                        <_items dataType="Array" type="Duality.Component[]" id="1447653664">
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
                                            <vel dataType="Struct" type="Duality.Vector3" />
                                            <velAbs dataType="Struct" type="Duality.Vector3" />
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
                                            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                                            <gameobj dataType="ObjectRef">3944217670</gameobj>
                                            <offset dataType="Float">1</offset>
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
                                            <spriteIndex dataType="Int">-1</spriteIndex>
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
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                                            <offset dataType="Float">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1435159300">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="3992361284">
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
                                      </compList>
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1471714266" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="1719013904">
                                            <item dataType="ObjectRef">4260545686</item>
                                            <item dataType="ObjectRef">1695278298</item>
                                            <item dataType="ObjectRef">651038006</item>
                                            <item dataType="Type" id="2187047740" value="BasicMenu.MenuChangeColor" />
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="3926530286">
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
                                          <data dataType="Array" type="System.Byte[]" id="3577156844">6LxU90NJPUCn4lXtH+n0kw==</data>
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
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3905790432">
                                        <_items dataType="Array" type="Duality.Component[]" id="264301532" length="4">
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
                                              <Y dataType="Float">-75</Y>
                                              <Z dataType="Float">0</Z>
                                            </pos>
                                            <posAbs dataType="Struct" type="Duality.Vector3">
                                              <X dataType="Float">0</X>
                                              <Y dataType="Float">-75</Y>
                                              <Z dataType="Float">0</Z>
                                            </posAbs>
                                            <scale dataType="Float">1</scale>
                                            <scaleAbs dataType="Float">1</scaleAbs>
                                            <vel dataType="Struct" type="Duality.Vector3" />
                                            <velAbs dataType="Struct" type="Duality.Vector3" />
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
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                                            <offset dataType="Float">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1100207130">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="2233100672">
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
                                      </compList>
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="155079566" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="3263893810">
                                            <item dataType="ObjectRef">4260545686</item>
                                            <item dataType="ObjectRef">651038006</item>
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="1613232970">
                                            <item dataType="ObjectRef">152885720</item>
                                            <item dataType="ObjectRef">3830166906</item>
                                          </values>
                                        </body>
                                      </compMap>
                                      <compTransform dataType="ObjectRef">152885720</compTransform>
                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                        <header>
                                          <data dataType="Array" type="System.Byte[]" id="3927868290">XgOaC/d3nU6CewDCFzc6Zw==</data>
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
                                </children>
                                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1160700616">
                                  <_items dataType="Array" type="Duality.Component[]" id="382576929" length="4">
                                    <item dataType="ObjectRef">2226196885</item>
                                    <item dataType="ObjectRef">1674661202</item>
                                  </_items>
                                  <_size dataType="Int">2</_size>
                                </compList>
                                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1396000577" surrogate="true">
                                  <header />
                                  <body>
                                    <keys dataType="Array" type="System.Object[]" id="1296517444">
                                      <item dataType="ObjectRef">4260545686</item>
                                      <item dataType="ObjectRef">72148844</item>
                                    </keys>
                                    <values dataType="Array" type="System.Object[]" id="201040534">
                                      <item dataType="ObjectRef">2226196885</item>
                                      <item dataType="ObjectRef">1674661202</item>
                                    </values>
                                  </body>
                                </compMap>
                                <compTransform dataType="ObjectRef">2226196885</compTransform>
                                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                  <header>
                                    <data dataType="Array" type="System.Byte[]" id="1784518912">0sUNkQwR50CeTXizJxhuVA==</data>
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
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3466610490" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="1256573376">
                            <item dataType="ObjectRef">4260545686</item>
                            <item dataType="ObjectRef">1695278298</item>
                            <item dataType="ObjectRef">651038006</item>
                            <item dataType="ObjectRef">3489050106</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="2665518158">
                            <item dataType="ObjectRef">3440904954</item>
                            <item dataType="ObjectRef">2722756590</item>
                            <item dataType="ObjectRef">2823218844</item>
                            <item dataType="ObjectRef">1396657352</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">3440904954</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="1488426332">KjE6oFKOU0+SS26p2SI4vw==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Another</name>
                      <parent dataType="ObjectRef">3150618205</parent>
                      <prefabLink />
                    </item>
                    <item dataType="Struct" type="Duality.GameObject" id="779209841">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1257998913">
                        <_items dataType="Array" type="Duality.Component[]" id="1469257134">
                          <item dataType="Struct" type="Duality.Components.Transform" id="3139524773">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <angleVel dataType="Float">0</angleVel>
                            <angleVelAbs dataType="Float">0</angleVelAbs>
                            <deriveAngle dataType="Bool">true</deriveAngle>
                            <gameobj dataType="ObjectRef">779209841</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
                            <parentTransform dataType="ObjectRef">1215965841</parentTransform>
                            <pos dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">0</X>
                              <Y dataType="Float">-60</Y>
                              <Z dataType="Float">0</Z>
                            </pos>
                            <posAbs dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">0</X>
                              <Y dataType="Float">-60</Y>
                              <Z dataType="Float">0</Z>
                            </posAbs>
                            <scale dataType="Float">1</scale>
                            <scaleAbs dataType="Float">1</scaleAbs>
                            <vel dataType="Struct" type="Duality.Vector3" />
                            <velAbs dataType="Struct" type="Duality.Vector3" />
                          </item>
                          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2421376409">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">45</B>
                              <G dataType="Byte">45</G>
                              <R dataType="Byte">45</R>
                            </colorTint>
                            <customMat />
                            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                            <gameobj dataType="ObjectRef">779209841</gameobj>
                            <offset dataType="Float">1</offset>
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
                            <spriteIndex dataType="Int">-1</spriteIndex>
                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                          </item>
                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="2521838663">
                            <active dataType="Bool">true</active>
                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <gameobj dataType="ObjectRef">779209841</gameobj>
                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                            <offset dataType="Float">0</offset>
                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="477288247">
                              <flowAreas />
                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="4255879822">
                                <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                                  <contentPath dataType="String">Data\BasicMenu\Visuals\DuruSans-Regular.Font.res</contentPath>
                                </item>
                              </fonts>
                              <icons />
                              <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                              <maxHeight dataType="Int">0</maxHeight>
                              <maxWidth dataType="Int">0</maxWidth>
                              <sourceText dataType="String">Options</sourceText>
                              <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                            </text>
                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                          </item>
                          <item dataType="Struct" type="BasicMenu.MenuSwitchToPage" id="1095277171">
                            <active dataType="Bool">true</active>
                            <gameobj dataType="ObjectRef">779209841</gameobj>
                            <hoverTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">38</B>
                              <G dataType="Byte">140</G>
                              <R dataType="Byte">245</R>
                            </hoverTint>
                            <target dataType="Struct" type="BasicMenu.MenuPage" id="3799790557">
                              <active dataType="Bool">true</active>
                              <gameobj dataType="Struct" type="Duality.GameObject" id="1991011308">
                                <active dataType="Bool">false</active>
                                <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1498293280">
                                  <_items dataType="Array" type="Duality.GameObject[]" id="1329497052">
                                    <item dataType="Struct" type="Duality.GameObject" id="1367178791">
                                      <active dataType="Bool">true</active>
                                      <children />
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2935528923">
                                        <_items dataType="Array" type="Duality.Component[]" id="79746198">
                                          <item dataType="Struct" type="Duality.Components.Transform" id="3727493723">
                                            <active dataType="Bool">true</active>
                                            <angle dataType="Float">0</angle>
                                            <angleAbs dataType="Float">0</angleAbs>
                                            <angleVel dataType="Float">0</angleVel>
                                            <angleVelAbs dataType="Float">0</angleVelAbs>
                                            <deriveAngle dataType="Bool">true</deriveAngle>
                                            <gameobj dataType="ObjectRef">1367178791</gameobj>
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
                                              <pos dataType="Struct" type="Duality.Vector3" />
                                              <posAbs dataType="Struct" type="Duality.Vector3" />
                                              <scale dataType="Float">1</scale>
                                              <scaleAbs dataType="Float">1</scaleAbs>
                                              <vel dataType="Struct" type="Duality.Vector3" />
                                              <velAbs dataType="Struct" type="Duality.Vector3" />
                                            </parentTransform>
                                            <pos dataType="Struct" type="Duality.Vector3">
                                              <X dataType="Float">-125</X>
                                              <Y dataType="Float">-30</Y>
                                              <Z dataType="Float">0</Z>
                                            </pos>
                                            <posAbs dataType="Struct" type="Duality.Vector3">
                                              <X dataType="Float">-125</X>
                                              <Y dataType="Float">-30</Y>
                                              <Z dataType="Float">0</Z>
                                            </posAbs>
                                            <scale dataType="Float">1</scale>
                                            <scaleAbs dataType="Float">1</scaleAbs>
                                            <vel dataType="Struct" type="Duality.Vector3" />
                                            <velAbs dataType="Struct" type="Duality.Vector3" />
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
                                            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                                            <gameobj dataType="ObjectRef">1367178791</gameobj>
                                            <offset dataType="Float">1</offset>
                                            <pixelGrid dataType="Bool">false</pixelGrid>
                                            <rect dataType="Struct" type="Duality.Rect">
                                              <H dataType="Float">50</H>
                                              <W dataType="Float">50</W>
                                              <X dataType="Float">-25</X>
                                              <Y dataType="Float">-25</Y>
                                            </rect>
                                            <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                                            <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                                              <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                                            </sharedMat>
                                            <spriteIndex dataType="Int">-1</spriteIndex>
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
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                                            <offset dataType="Float">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="4196214285">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="266459942">
                                                <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                                                  <contentPath dataType="String">Data\BasicMenu\Visuals\DuruSans-Regular.Font.res</contentPath>
                                                </item>
                                              </fonts>
                                              <icons />
                                              <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                                              <maxHeight dataType="Int">0</maxHeight>
                                              <maxWidth dataType="Int">0</maxWidth>
                                              <sourceText dataType="String">-</sourceText>
                                              <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                                            </text>
                                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                                          </item>
                                          <item dataType="Struct" type="BasicMenu.MenuChangeVolume" id="2810582917">
                                            <active dataType="Bool">true</active>
                                            <changeAmount dataType="Short">-1</changeAmount>
                                            <gameobj dataType="ObjectRef">1367178791</gameobj>
                                            <hoverTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">38</B>
                                              <G dataType="Byte">140</G>
                                              <R dataType="Byte">245</R>
                                            </hoverTint>
                                          </item>
                                        </_items>
                                        <_size dataType="Int">4</_size>
                                      </compList>
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3038503784" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="1706699057">
                                            <item dataType="ObjectRef">4260545686</item>
                                            <item dataType="ObjectRef">1695278298</item>
                                            <item dataType="ObjectRef">651038006</item>
                                            <item dataType="Type" id="3041612846" value="BasicMenu.MenuChangeVolume" />
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="2663072352">
                                            <item dataType="ObjectRef">3727493723</item>
                                            <item dataType="ObjectRef">3009345359</item>
                                            <item dataType="ObjectRef">3109807613</item>
                                            <item dataType="ObjectRef">2810582917</item>
                                          </values>
                                        </body>
                                      </compMap>
                                      <compTransform dataType="ObjectRef">3727493723</compTransform>
                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                        <header>
                                          <data dataType="Array" type="System.Byte[]" id="2962387683">RThwaA1XakahLG48HxAACA==</data>
                                        </header>
                                        <body />
                                      </identifier>
                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                      <name dataType="String">Minus</name>
                                      <parent dataType="ObjectRef">1991011308</parent>
                                      <prefabLink />
                                    </item>
                                    <item dataType="Struct" type="Duality.GameObject" id="2583417145">
                                      <active dataType="Bool">true</active>
                                      <children />
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4216173509">
                                        <_items dataType="Array" type="Duality.Component[]" id="3104571094">
                                          <item dataType="Struct" type="Duality.Components.Transform" id="648764781">
                                            <active dataType="Bool">true</active>
                                            <angle dataType="Float">0</angle>
                                            <angleAbs dataType="Float">0</angleAbs>
                                            <angleVel dataType="Float">0</angleVel>
                                            <angleVelAbs dataType="Float">0</angleVelAbs>
                                            <deriveAngle dataType="Bool">true</deriveAngle>
                                            <gameobj dataType="ObjectRef">2583417145</gameobj>
                                            <ignoreParent dataType="Bool">false</ignoreParent>
                                            <parentTransform dataType="ObjectRef">56358944</parentTransform>
                                            <pos dataType="Struct" type="Duality.Vector3">
                                              <X dataType="Float">125</X>
                                              <Y dataType="Float">-30</Y>
                                              <Z dataType="Float">0</Z>
                                            </pos>
                                            <posAbs dataType="Struct" type="Duality.Vector3">
                                              <X dataType="Float">125</X>
                                              <Y dataType="Float">-30</Y>
                                              <Z dataType="Float">0</Z>
                                            </posAbs>
                                            <scale dataType="Float">1</scale>
                                            <scaleAbs dataType="Float">1</scaleAbs>
                                            <vel dataType="Struct" type="Duality.Vector3" />
                                            <velAbs dataType="Struct" type="Duality.Vector3" />
                                          </item>
                                          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="4225583713">
                                            <active dataType="Bool">true</active>
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">45</B>
                                              <G dataType="Byte">45</G>
                                              <R dataType="Byte">45</R>
                                            </colorTint>
                                            <customMat />
                                            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                                            <gameobj dataType="ObjectRef">2583417145</gameobj>
                                            <offset dataType="Float">1</offset>
                                            <pixelGrid dataType="Bool">false</pixelGrid>
                                            <rect dataType="Struct" type="Duality.Rect">
                                              <H dataType="Float">50</H>
                                              <W dataType="Float">50</W>
                                              <X dataType="Float">-25</X>
                                              <Y dataType="Float">-25</Y>
                                            </rect>
                                            <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                                            <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                                              <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                                            </sharedMat>
                                            <spriteIndex dataType="Int">-1</spriteIndex>
                                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                                          </item>
                                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="31078671">
                                            <active dataType="Bool">true</active>
                                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">255</B>
                                              <G dataType="Byte">255</G>
                                              <R dataType="Byte">255</R>
                                            </colorTint>
                                            <customMat />
                                            <gameobj dataType="ObjectRef">2583417145</gameobj>
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                                            <offset dataType="Float">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="3540624815">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="2327641070">
                                                <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                                                  <contentPath dataType="String">Data\BasicMenu\Visuals\DuruSans-Regular.Font.res</contentPath>
                                                </item>
                                              </fonts>
                                              <icons />
                                              <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                                              <maxHeight dataType="Int">0</maxHeight>
                                              <maxWidth dataType="Int">0</maxWidth>
                                              <sourceText dataType="String">+</sourceText>
                                              <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                                            </text>
                                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                                          </item>
                                          <item dataType="Struct" type="BasicMenu.MenuChangeVolume" id="4026821271">
                                            <active dataType="Bool">true</active>
                                            <changeAmount dataType="Short">1</changeAmount>
                                            <gameobj dataType="ObjectRef">2583417145</gameobj>
                                            <hoverTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">38</B>
                                              <G dataType="Byte">140</G>
                                              <R dataType="Byte">245</R>
                                            </hoverTint>
                                          </item>
                                        </_items>
                                        <_size dataType="Int">4</_size>
                                      </compList>
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3588705832" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="2992215215">
                                            <item dataType="ObjectRef">4260545686</item>
                                            <item dataType="ObjectRef">1695278298</item>
                                            <item dataType="ObjectRef">651038006</item>
                                            <item dataType="ObjectRef">3041612846</item>
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="38095008">
                                            <item dataType="ObjectRef">648764781</item>
                                            <item dataType="ObjectRef">4225583713</item>
                                            <item dataType="ObjectRef">31078671</item>
                                            <item dataType="ObjectRef">4026821271</item>
                                          </values>
                                        </body>
                                      </compMap>
                                      <compTransform dataType="ObjectRef">648764781</compTransform>
                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                        <header>
                                          <data dataType="Array" type="System.Byte[]" id="896967741">kJ3c4yd3nUyhgw0U7zhTNA==</data>
                                        </header>
                                        <body />
                                      </identifier>
                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                      <name dataType="String">Plus</name>
                                      <parent dataType="ObjectRef">1991011308</parent>
                                      <prefabLink />
                                    </item>
                                    <item dataType="Struct" type="Duality.GameObject" id="1572686554">
                                      <active dataType="Bool">true</active>
                                      <children />
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3832094138">
                                        <_items dataType="Array" type="Duality.Component[]" id="2741944320" length="4">
                                          <item dataType="Struct" type="Duality.Components.Transform" id="3933001486">
                                            <active dataType="Bool">true</active>
                                            <angle dataType="Float">0</angle>
                                            <angleAbs dataType="Float">0</angleAbs>
                                            <angleVel dataType="Float">0</angleVel>
                                            <angleVelAbs dataType="Float">0</angleVelAbs>
                                            <deriveAngle dataType="Bool">true</deriveAngle>
                                            <gameobj dataType="ObjectRef">1572686554</gameobj>
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
                                            <vel dataType="Struct" type="Duality.Vector3" />
                                            <velAbs dataType="Struct" type="Duality.Vector3" />
                                          </item>
                                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="3315315376">
                                            <active dataType="Bool">true</active>
                                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">255</B>
                                              <G dataType="Byte">255</G>
                                              <R dataType="Byte">255</R>
                                            </colorTint>
                                            <customMat />
                                            <gameobj dataType="ObjectRef">1572686554</gameobj>
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                                            <offset dataType="Float">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="3415372960">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="2877305052">
                                                <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                                                  <contentPath dataType="String">Data\BasicMenu\Visuals\DuruSans-Regular.Font.res</contentPath>
                                                </item>
                                              </fonts>
                                              <icons />
                                              <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                                              <maxHeight dataType="Int">0</maxHeight>
                                              <maxWidth dataType="Int">0</maxWidth>
                                              <sourceText dataType="String">Volume</sourceText>
                                              <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                                            </text>
                                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                                          </item>
                                          <item dataType="Struct" type="BasicMenu.VolumeRenderer" id="576859912">
                                            <active dataType="Bool">true</active>
                                            <gameobj dataType="ObjectRef">1572686554</gameobj>
                                          </item>
                                        </_items>
                                        <_size dataType="Int">3</_size>
                                      </compList>
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1488268730" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="533106688">
                                            <item dataType="ObjectRef">4260545686</item>
                                            <item dataType="ObjectRef">651038006</item>
                                            <item dataType="Type" id="771438748" value="BasicMenu.VolumeRenderer" />
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="1519377358">
                                            <item dataType="ObjectRef">3933001486</item>
                                            <item dataType="ObjectRef">3315315376</item>
                                            <item dataType="ObjectRef">576859912</item>
                                          </values>
                                        </body>
                                      </compMap>
                                      <compTransform dataType="ObjectRef">3933001486</compTransform>
                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                        <header>
                                          <data dataType="Array" type="System.Byte[]" id="698524">IAcOZLPGHk2ALP+qZ2z55A==</data>
                                        </header>
                                        <body />
                                      </identifier>
                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                      <name dataType="String">VolumeText</name>
                                      <parent dataType="ObjectRef">1991011308</parent>
                                      <prefabLink />
                                    </item>
                                    <item dataType="Struct" type="Duality.GameObject" id="3130057940">
                                      <active dataType="Bool">true</active>
                                      <children />
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2174110036">
                                        <_items dataType="Array" type="Duality.Component[]" id="3673474788">
                                          <item dataType="Struct" type="Duality.Components.Transform" id="1195405576">
                                            <active dataType="Bool">true</active>
                                            <angle dataType="Float">0</angle>
                                            <angleAbs dataType="Float">0</angleAbs>
                                            <angleVel dataType="Float">0</angleVel>
                                            <angleVelAbs dataType="Float">0</angleVelAbs>
                                            <deriveAngle dataType="Bool">true</deriveAngle>
                                            <gameobj dataType="ObjectRef">3130057940</gameobj>
                                            <ignoreParent dataType="Bool">false</ignoreParent>
                                            <parentTransform dataType="ObjectRef">56358944</parentTransform>
                                            <pos dataType="Struct" type="Duality.Vector3">
                                              <X dataType="Float">75</X>
                                              <Y dataType="Float">30</Y>
                                              <Z dataType="Float">0</Z>
                                            </pos>
                                            <posAbs dataType="Struct" type="Duality.Vector3">
                                              <X dataType="Float">75</X>
                                              <Y dataType="Float">30</Y>
                                              <Z dataType="Float">0</Z>
                                            </posAbs>
                                            <scale dataType="Float">1</scale>
                                            <scaleAbs dataType="Float">1</scaleAbs>
                                            <vel dataType="Struct" type="Duality.Vector3" />
                                            <velAbs dataType="Struct" type="Duality.Vector3" />
                                          </item>
                                          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="477257212">
                                            <active dataType="Bool">true</active>
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">45</B>
                                              <G dataType="Byte">45</G>
                                              <R dataType="Byte">45</R>
                                            </colorTint>
                                            <customMat />
                                            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                                            <gameobj dataType="ObjectRef">3130057940</gameobj>
                                            <offset dataType="Float">1</offset>
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
                                            <spriteIndex dataType="Int">-1</spriteIndex>
                                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                                          </item>
                                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="577719466">
                                            <active dataType="Bool">true</active>
                                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">255</B>
                                              <G dataType="Byte">255</G>
                                              <R dataType="Byte">255</R>
                                            </colorTint>
                                            <customMat />
                                            <gameobj dataType="ObjectRef">3130057940</gameobj>
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                                            <offset dataType="Float">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1416774346">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="376845152">
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
                                          <item dataType="Struct" type="BasicMenu.MenuSwitchToPage" id="3446125270">
                                            <active dataType="Bool">true</active>
                                            <gameobj dataType="ObjectRef">3130057940</gameobj>
                                            <hoverTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">38</B>
                                              <G dataType="Byte">140</G>
                                              <R dataType="Byte">245</R>
                                            </hoverTint>
                                            <target dataType="ObjectRef">664430158</target>
                                          </item>
                                        </_items>
                                        <_size dataType="Int">4</_size>
                                      </compList>
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4198732214" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="61065982">
                                            <item dataType="ObjectRef">4260545686</item>
                                            <item dataType="ObjectRef">1695278298</item>
                                            <item dataType="ObjectRef">651038006</item>
                                            <item dataType="ObjectRef">3489050106</item>
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="1456228234">
                                            <item dataType="ObjectRef">1195405576</item>
                                            <item dataType="ObjectRef">477257212</item>
                                            <item dataType="ObjectRef">577719466</item>
                                            <item dataType="ObjectRef">3446125270</item>
                                          </values>
                                        </body>
                                      </compMap>
                                      <compTransform dataType="ObjectRef">1195405576</compTransform>
                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                        <header>
                                          <data dataType="Array" type="System.Byte[]" id="2150742158">O6GUJgTvAUWGs3PGuBzr7g==</data>
                                        </header>
                                        <body />
                                      </identifier>
                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                      <name dataType="String">Back</name>
                                      <parent dataType="ObjectRef">1991011308</parent>
                                      <prefabLink />
                                    </item>
                                  </_items>
                                  <_size dataType="Int">4</_size>
                                </children>
                                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2085301134">
                                  <_items dataType="Array" type="Duality.Component[]" id="886376690" length="4">
                                    <item dataType="ObjectRef">56358944</item>
                                    <item dataType="ObjectRef">3799790557</item>
                                  </_items>
                                  <_size dataType="Int">2</_size>
                                </compList>
                                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="661643068" surrogate="true">
                                  <header />
                                  <body>
                                    <keys dataType="Array" type="System.Object[]" id="2876738680">
                                      <item dataType="ObjectRef">4260545686</item>
                                      <item dataType="ObjectRef">72148844</item>
                                    </keys>
                                    <values dataType="Array" type="System.Object[]" id="347872734">
                                      <item dataType="ObjectRef">56358944</item>
                                      <item dataType="ObjectRef">3799790557</item>
                                    </values>
                                  </body>
                                </compMap>
                                <compTransform dataType="ObjectRef">56358944</compTransform>
                                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                  <header>
                                    <data dataType="Array" type="System.Byte[]" id="1306055972">qSytuAGr30epimWOTaEamg==</data>
                                  </header>
                                  <body />
                                </identifier>
                                <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                <name dataType="String">#MenuOptions</name>
                                <parent />
                                <prefabLink />
                              </gameobj>
                            </target>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1415788000" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="421567371">
                            <item dataType="ObjectRef">4260545686</item>
                            <item dataType="ObjectRef">1695278298</item>
                            <item dataType="ObjectRef">651038006</item>
                            <item dataType="ObjectRef">3489050106</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="2264949448">
                            <item dataType="ObjectRef">3139524773</item>
                            <item dataType="ObjectRef">2421376409</item>
                            <item dataType="ObjectRef">2521838663</item>
                            <item dataType="ObjectRef">1095277171</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">3139524773</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="1426796865">AIagRNFb20qRczGOdKEzqg==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Options</name>
                      <parent dataType="ObjectRef">3150618205</parent>
                      <prefabLink />
                    </item>
                  </_items>
                  <_size dataType="Int">3</_size>
                </children>
                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2204745536">
                  <_items dataType="Array" type="Duality.Component[]" id="2430907069" length="4">
                    <item dataType="ObjectRef">1215965841</item>
                    <item dataType="ObjectRef">664430158</item>
                  </_items>
                  <_size dataType="Int">2</_size>
                </compList>
                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2175074773" surrogate="true">
                  <header />
                  <body>
                    <keys dataType="Array" type="System.Object[]" id="1172905140">
                      <item dataType="ObjectRef">4260545686</item>
                      <item dataType="ObjectRef">72148844</item>
                    </keys>
                    <values dataType="Array" type="System.Object[]" id="2805583862">
                      <item dataType="ObjectRef">1215965841</item>
                      <item dataType="ObjectRef">664430158</item>
                    </values>
                  </body>
                </compMap>
                <compTransform dataType="ObjectRef">1215965841</compTransform>
                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                  <header>
                    <data dataType="Array" type="System.Byte[]" id="2804476688">Btict1a0Q0a8rLhg1ELqGA==</data>
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
            <startingMenu dataType="ObjectRef">664430158</startingMenu>
          </item>
        </_items>
        <_size dataType="Int">5</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4076619662" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="726969330">
            <item dataType="ObjectRef">4260545686</item>
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
            <vel dataType="Struct" type="Duality.Vector3" />
            <velAbs dataType="Struct" type="Duality.Vector3" />
          </item>
          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1849493036">
            <active dataType="Bool">true</active>
            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">255</B>
              <G dataType="Byte">255</G>
              <R dataType="Byte">255</R>
            </colorTint>
            <customMat />
            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
            <gameobj dataType="ObjectRef">207326468</gameobj>
            <offset dataType="Float">0</offset>
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
            <spriteIndex dataType="Int">-1</spriteIndex>
            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          </item>
        </_items>
        <_size dataType="Int">2</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2473047482" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="1583631872">
            <item dataType="ObjectRef">4260545686</item>
            <item dataType="ObjectRef">1695278298</item>
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
            <vel dataType="Struct" type="Duality.Vector3" />
            <velAbs dataType="Struct" type="Duality.Vector3" />
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
            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
            <offset dataType="Float">0</offset>
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
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4186247928" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3822850679">
            <item dataType="ObjectRef">4260545686</item>
            <item dataType="ObjectRef">651038006</item>
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
    <item dataType="ObjectRef">3150618205</item>
    <item dataType="Struct" type="Duality.GameObject" id="907618950">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="43005776">
        <_items dataType="Array" type="Duality.Component[]" id="2079251388" length="4">
          <item dataType="Struct" type="BasicMenu.MouseCursorRenderer" id="1313891129">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">907618950</gameobj>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1228380014" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="740264482">
            <item dataType="Type" id="1961653520" value="BasicMenu.MouseCursorRenderer" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="1203260682">
            <item dataType="ObjectRef">1313891129</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="4252989906">wG8rjzLH0UWLy35KUweUlw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">MouseCursorRenderer</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">1367178791</item>
    <item dataType="ObjectRef">2583417145</item>
    <item dataType="ObjectRef">1572686554</item>
    <item dataType="ObjectRef">3130057940</item>
    <item dataType="ObjectRef">3148492957</item>
    <item dataType="ObjectRef">1225237399</item>
    <item dataType="ObjectRef">1156301705</item>
    <item dataType="ObjectRef">147130678</item>
    <item dataType="ObjectRef">3944217670</item>
    <item dataType="ObjectRef">2087538084</item>
    <item dataType="ObjectRef">1781294501</item>
    <item dataType="ObjectRef">1080590022</item>
    <item dataType="ObjectRef">779209841</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
