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
        <_items dataType="Array" type="Duality.Component[]" id="2630271824" length="8">
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
              <B dataType="Byte">162</B>
              <G dataType="Byte">126</G>
              <R dataType="Byte">94</R>
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
          <item dataType="Struct" type="BasicMenu.EventMenuController" id="3588024560">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1050330856</gameobj>
            <startingMenu dataType="Struct" type="BasicMenu.MenuPage" id="1161626833">
              <active dataType="Bool">true</active>
              <gameobj dataType="Struct" type="Duality.GameObject" id="797331632">
                <active dataType="Bool">true</active>
                <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="153744459">
                  <_items dataType="Array" type="Duality.GameObject[]" id="3101054710" length="4">
                    <item dataType="Struct" type="Duality.GameObject" id="4291917305">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3152269913">
                        <_items dataType="Array" type="Duality.Component[]" id="3885202382">
                          <item dataType="Struct" type="Duality.Components.Transform" id="54227227">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <gameobj dataType="ObjectRef">4291917305</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
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
                          </item>
                          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1465569289">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">45</B>
                              <G dataType="Byte">45</G>
                              <R dataType="Byte">45</R>
                            </colorTint>
                            <customMat />
                            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                            <gameobj dataType="ObjectRef">4291917305</gameobj>
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
                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="879887663">
                            <active dataType="Bool">true</active>
                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <gameobj dataType="ObjectRef">4291917305</gameobj>
                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                            <offset dataType="Float">0</offset>
                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1201165727">
                              <flowAreas />
                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="2714448238">
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
                          <item dataType="Struct" type="BasicMenu.MenuSwitchToPage" id="420330073">
                            <active dataType="Bool">true</active>
                            <gameobj dataType="ObjectRef">4291917305</gameobj>
                            <hoverTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">38</B>
                              <G dataType="Byte">43</G>
                              <R dataType="Byte">245</R>
                            </hoverTint>
                            <target dataType="Struct" type="BasicMenu.MenuPage" id="821547922">
                              <active dataType="Bool">true</active>
                              <gameobj dataType="Struct" type="Duality.GameObject" id="457252721">
                                <active dataType="Bool">false</active>
                                <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2001892606">
                                  <_items dataType="Array" type="Duality.GameObject[]" id="2628980112" length="4">
                                    <item dataType="Struct" type="Duality.GameObject" id="3821432958">
                                      <active dataType="Bool">true</active>
                                      <children />
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4221105854">
                                        <_items dataType="Array" type="Duality.Component[]" id="1422283792">
                                          <item dataType="Struct" type="Duality.Components.Transform" id="3878710176">
                                            <active dataType="Bool">true</active>
                                            <angle dataType="Float">0</angle>
                                            <angleAbs dataType="Float">0</angleAbs>
                                            <gameobj dataType="ObjectRef">3821432958</gameobj>
                                            <ignoreParent dataType="Bool">false</ignoreParent>
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
                                          </item>
                                          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="995084942">
                                            <active dataType="Bool">true</active>
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">45</B>
                                              <G dataType="Byte">45</G>
                                              <R dataType="Byte">45</R>
                                            </colorTint>
                                            <customMat />
                                            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                                            <gameobj dataType="ObjectRef">3821432958</gameobj>
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
                                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="409403316">
                                            <active dataType="Bool">true</active>
                                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">255</B>
                                              <G dataType="Byte">255</G>
                                              <R dataType="Byte">255</R>
                                            </colorTint>
                                            <customMat />
                                            <gameobj dataType="ObjectRef">3821432958</gameobj>
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                                            <offset dataType="Float">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="4016312444">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1355470916">
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
                                          <item dataType="Struct" type="BasicMenu.MenuSwitchToPage" id="4244813022">
                                            <active dataType="Bool">true</active>
                                            <gameobj dataType="ObjectRef">3821432958</gameobj>
                                            <hoverTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">38</B>
                                              <G dataType="Byte">140</G>
                                              <R dataType="Byte">245</R>
                                            </hoverTint>
                                            <target dataType="ObjectRef">1161626833</target>
                                          </item>
                                        </_items>
                                        <_size dataType="Int">4</_size>
                                      </compList>
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2341619722" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="2315839900">
                                            <item dataType="Type" id="1995515332" value="Duality.Components.Transform" />
                                            <item dataType="Type" id="726534550" value="Duality.Components.Renderers.SpriteRenderer" />
                                            <item dataType="Type" id="3158078592" value="Duality.Components.Renderers.TextRenderer" />
                                            <item dataType="Type" id="2103117858" value="BasicMenu.MenuSwitchToPage" />
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="2931982358">
                                            <item dataType="ObjectRef">3878710176</item>
                                            <item dataType="ObjectRef">995084942</item>
                                            <item dataType="ObjectRef">409403316</item>
                                            <item dataType="ObjectRef">4244813022</item>
                                          </values>
                                        </body>
                                      </compMap>
                                      <compTransform dataType="ObjectRef">3878710176</compTransform>
                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                        <header>
                                          <data dataType="Array" type="System.Byte[]" id="2251634696">s3CyBhjd/0COHWDcMho30Q==</data>
                                        </header>
                                        <body />
                                      </identifier>
                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                      <name dataType="String">No</name>
                                      <parent dataType="ObjectRef">457252721</parent>
                                      <prefabLink />
                                    </item>
                                    <item dataType="Struct" type="Duality.GameObject" id="3977901161">
                                      <active dataType="Bool">true</active>
                                      <children />
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1969626157">
                                        <_items dataType="Array" type="Duality.Component[]" id="3362134630">
                                          <item dataType="Struct" type="Duality.Components.Transform" id="4035178379">
                                            <active dataType="Bool">true</active>
                                            <angle dataType="Float">0</angle>
                                            <angleAbs dataType="Float">0</angleAbs>
                                            <gameobj dataType="ObjectRef">3977901161</gameobj>
                                            <ignoreParent dataType="Bool">false</ignoreParent>
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
                                          </item>
                                          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1151553145">
                                            <active dataType="Bool">true</active>
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">45</B>
                                              <G dataType="Byte">45</G>
                                              <R dataType="Byte">45</R>
                                            </colorTint>
                                            <customMat />
                                            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                                            <gameobj dataType="ObjectRef">3977901161</gameobj>
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
                                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="565871519">
                                            <active dataType="Bool">true</active>
                                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">255</B>
                                              <G dataType="Byte">255</G>
                                              <R dataType="Byte">255</R>
                                            </colorTint>
                                            <customMat />
                                            <gameobj dataType="ObjectRef">3977901161</gameobj>
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                                            <offset dataType="Float">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2029345247">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="2869031534">
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
                                          <item dataType="Struct" type="BasicMenu.MenuQuitConfirm" id="3075715482">
                                            <active dataType="Bool">true</active>
                                            <gameobj dataType="ObjectRef">3977901161</gameobj>
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
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2940632696" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="1596110663">
                                            <item dataType="ObjectRef">1995515332</item>
                                            <item dataType="ObjectRef">726534550</item>
                                            <item dataType="ObjectRef">3158078592</item>
                                            <item dataType="Type" id="178804430" value="BasicMenu.MenuQuitConfirm" />
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="2973248256">
                                            <item dataType="ObjectRef">4035178379</item>
                                            <item dataType="ObjectRef">1151553145</item>
                                            <item dataType="ObjectRef">565871519</item>
                                            <item dataType="ObjectRef">3075715482</item>
                                          </values>
                                        </body>
                                      </compMap>
                                      <compTransform dataType="ObjectRef">4035178379</compTransform>
                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                        <header>
                                          <data dataType="Array" type="System.Byte[]" id="2760310469">jeCXeZBSd0OOVGLvXCI2bQ==</data>
                                        </header>
                                        <body />
                                      </identifier>
                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                      <name dataType="String">Yes</name>
                                      <parent dataType="ObjectRef">457252721</parent>
                                      <prefabLink />
                                    </item>
                                    <item dataType="Struct" type="Duality.GameObject" id="2811845245">
                                      <active dataType="Bool">true</active>
                                      <children />
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3596147241">
                                        <_items dataType="Array" type="Duality.Component[]" id="1484849678" length="4">
                                          <item dataType="Struct" type="Duality.Components.Transform" id="2869122463">
                                            <active dataType="Bool">true</active>
                                            <angle dataType="Float">0</angle>
                                            <angleAbs dataType="Float">0</angleAbs>
                                            <gameobj dataType="ObjectRef">2811845245</gameobj>
                                            <ignoreParent dataType="Bool">false</ignoreParent>
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
                                          </item>
                                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="3694782899">
                                            <active dataType="Bool">true</active>
                                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">255</B>
                                              <G dataType="Byte">255</G>
                                              <R dataType="Byte">255</R>
                                            </colorTint>
                                            <customMat />
                                            <gameobj dataType="ObjectRef">2811845245</gameobj>
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                                            <offset dataType="Float">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="255300179">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="844619110">
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
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="110402496" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="120213411">
                                            <item dataType="ObjectRef">1995515332</item>
                                            <item dataType="ObjectRef">3158078592</item>
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="2903931768">
                                            <item dataType="ObjectRef">2869122463</item>
                                            <item dataType="ObjectRef">3694782899</item>
                                          </values>
                                        </body>
                                      </compMap>
                                      <compTransform dataType="ObjectRef">2869122463</compTransform>
                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                        <header>
                                          <data dataType="Array" type="System.Byte[]" id="3745989641">HowsWsea+0+21YQa0ZqmbA==</data>
                                        </header>
                                        <body />
                                      </identifier>
                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                      <name dataType="String">Text</name>
                                      <parent dataType="ObjectRef">457252721</parent>
                                      <prefabLink />
                                    </item>
                                  </_items>
                                  <_size dataType="Int">3</_size>
                                </children>
                                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2222463882">
                                  <_items dataType="Array" type="Duality.Component[]" id="3934738140" length="4">
                                    <item dataType="Struct" type="Duality.Components.Transform" id="514529939">
                                      <active dataType="Bool">true</active>
                                      <angle dataType="Float">0</angle>
                                      <angleAbs dataType="Float">0</angleAbs>
                                      <gameobj dataType="ObjectRef">457252721</gameobj>
                                      <ignoreParent dataType="Bool">false</ignoreParent>
                                      <pos dataType="Struct" type="Duality.Vector3" />
                                      <posAbs dataType="Struct" type="Duality.Vector3" />
                                      <scale dataType="Float">1</scale>
                                      <scaleAbs dataType="Float">1</scaleAbs>
                                    </item>
                                    <item dataType="ObjectRef">821547922</item>
                                  </_items>
                                  <_size dataType="Int">2</_size>
                                </compList>
                                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1789880974" surrogate="true">
                                  <header />
                                  <body>
                                    <keys dataType="Array" type="System.Object[]" id="1551546144">
                                      <item dataType="ObjectRef">1995515332</item>
                                      <item dataType="Type" id="1515364316" value="BasicMenu.MenuPage" />
                                    </keys>
                                    <values dataType="Array" type="System.Object[]" id="2671138702">
                                      <item dataType="ObjectRef">514529939</item>
                                      <item dataType="ObjectRef">821547922</item>
                                    </values>
                                  </body>
                                </compMap>
                                <compTransform dataType="ObjectRef">514529939</compTransform>
                                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                  <header>
                                    <data dataType="Array" type="System.Byte[]" id="1128411708">UxHty6eN2kGa9fQ9f14PZg==</data>
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
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2098053120" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="3759135347">
                            <item dataType="ObjectRef">1995515332</item>
                            <item dataType="ObjectRef">726534550</item>
                            <item dataType="ObjectRef">3158078592</item>
                            <item dataType="ObjectRef">2103117858</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="616729528">
                            <item dataType="ObjectRef">54227227</item>
                            <item dataType="ObjectRef">1465569289</item>
                            <item dataType="ObjectRef">879887663</item>
                            <item dataType="ObjectRef">420330073</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">54227227</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="507480985">kS8f+DMrU0qmnrkON0HZZw==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Quit</name>
                      <parent dataType="ObjectRef">797331632</parent>
                      <prefabLink />
                    </item>
                    <item dataType="Struct" type="Duality.GameObject" id="2026669693">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3396545357">
                        <_items dataType="Array" type="Duality.Component[]" id="1098741798">
                          <item dataType="Struct" type="Duality.Components.Transform" id="2083946911">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <gameobj dataType="ObjectRef">2026669693</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
                            <pos dataType="Struct" type="Duality.Vector3" />
                            <posAbs dataType="Struct" type="Duality.Vector3" />
                            <scale dataType="Float">1</scale>
                            <scaleAbs dataType="Float">1</scaleAbs>
                          </item>
                          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3495288973">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">45</B>
                              <G dataType="Byte">45</G>
                              <R dataType="Byte">45</R>
                            </colorTint>
                            <customMat />
                            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                            <gameobj dataType="ObjectRef">2026669693</gameobj>
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
                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="2909607347">
                            <active dataType="Bool">true</active>
                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <gameobj dataType="ObjectRef">2026669693</gameobj>
                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                            <offset dataType="Float">0</offset>
                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="247028291">
                              <flowAreas />
                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1159659558">
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
                          <item dataType="Struct" type="BasicMenu.MenuSwitchToPage" id="2450049757">
                            <active dataType="Bool">true</active>
                            <gameobj dataType="ObjectRef">2026669693</gameobj>
                            <hoverTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">38</B>
                              <G dataType="Byte">140</G>
                              <R dataType="Byte">245</R>
                            </hoverTint>
                            <target dataType="Struct" type="BasicMenu.MenuPage" id="2964052133">
                              <active dataType="Bool">true</active>
                              <gameobj dataType="Struct" type="Duality.GameObject" id="2599756932">
                                <active dataType="Bool">false</active>
                                <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3288063298">
                                  <_items dataType="Array" type="Duality.GameObject[]" id="3100544016" length="4">
                                    <item dataType="Struct" type="Duality.GameObject" id="3363248187">
                                      <active dataType="Bool">true</active>
                                      <children />
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3732157359">
                                        <_items dataType="Array" type="Duality.Component[]" id="3529590766">
                                          <item dataType="Struct" type="Duality.Components.Transform" id="3420525405">
                                            <active dataType="Bool">true</active>
                                            <angle dataType="Float">0</angle>
                                            <angleAbs dataType="Float">0</angleAbs>
                                            <gameobj dataType="ObjectRef">3363248187</gameobj>
                                            <ignoreParent dataType="Bool">false</ignoreParent>
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
                                          </item>
                                          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="536900171">
                                            <active dataType="Bool">true</active>
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">45</B>
                                              <G dataType="Byte">45</G>
                                              <R dataType="Byte">45</R>
                                            </colorTint>
                                            <customMat />
                                            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                                            <gameobj dataType="ObjectRef">3363248187</gameobj>
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
                                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="4246185841">
                                            <active dataType="Bool">true</active>
                                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">255</B>
                                              <G dataType="Byte">255</G>
                                              <R dataType="Byte">255</R>
                                            </colorTint>
                                            <customMat />
                                            <gameobj dataType="ObjectRef">3363248187</gameobj>
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                                            <offset dataType="Float">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2193832385">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1339350190">
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
                                          <item dataType="Struct" type="BasicMenu.MenuSwitchToPage" id="3786628251">
                                            <active dataType="Bool">true</active>
                                            <gameobj dataType="ObjectRef">3363248187</gameobj>
                                            <hoverTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">38</B>
                                              <G dataType="Byte">140</G>
                                              <R dataType="Byte">245</R>
                                            </hoverTint>
                                            <target dataType="ObjectRef">1161626833</target>
                                          </item>
                                        </_items>
                                        <_size dataType="Int">4</_size>
                                      </compList>
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1420994720" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="485009541">
                                            <item dataType="ObjectRef">1995515332</item>
                                            <item dataType="ObjectRef">726534550</item>
                                            <item dataType="ObjectRef">3158078592</item>
                                            <item dataType="ObjectRef">2103117858</item>
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="3704356008">
                                            <item dataType="ObjectRef">3420525405</item>
                                            <item dataType="ObjectRef">536900171</item>
                                            <item dataType="ObjectRef">4246185841</item>
                                            <item dataType="ObjectRef">3786628251</item>
                                          </values>
                                        </body>
                                      </compMap>
                                      <compTransform dataType="ObjectRef">3420525405</compTransform>
                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                        <header>
                                          <data dataType="Array" type="System.Byte[]" id="3324682639">JkpTB5ZLZkOKfkQFW2fWdg==</data>
                                        </header>
                                        <body />
                                      </identifier>
                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                      <name dataType="String">Back</name>
                                      <parent dataType="ObjectRef">2599756932</parent>
                                      <prefabLink />
                                    </item>
                                    <item dataType="Struct" type="Duality.GameObject" id="1827315574">
                                      <active dataType="Bool">true</active>
                                      <children />
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1493584886">
                                        <_items dataType="Array" type="Duality.Component[]" id="1475090144">
                                          <item dataType="Struct" type="Duality.Components.Transform" id="1884592792">
                                            <active dataType="Bool">true</active>
                                            <angle dataType="Float">0</angle>
                                            <angleAbs dataType="Float">0</angleAbs>
                                            <gameobj dataType="ObjectRef">1827315574</gameobj>
                                            <ignoreParent dataType="Bool">false</ignoreParent>
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
                                          </item>
                                          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3295934854">
                                            <active dataType="Bool">true</active>
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">45</B>
                                              <G dataType="Byte">45</G>
                                              <R dataType="Byte">45</R>
                                            </colorTint>
                                            <customMat />
                                            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                                            <gameobj dataType="ObjectRef">1827315574</gameobj>
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
                                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="2710253228">
                                            <active dataType="Bool">true</active>
                                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">255</B>
                                              <G dataType="Byte">255</G>
                                              <R dataType="Byte">255</R>
                                            </colorTint>
                                            <customMat />
                                            <gameobj dataType="ObjectRef">1827315574</gameobj>
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                                            <offset dataType="Float">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1515453812">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="592562596">
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
                                          <item dataType="Struct" type="BasicMenu.MenuChangeColor" id="2181795775">
                                            <active dataType="Bool">true</active>
                                            <gameobj dataType="ObjectRef">1827315574</gameobj>
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
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1161704474" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="4096108228">
                                            <item dataType="ObjectRef">1995515332</item>
                                            <item dataType="ObjectRef">726534550</item>
                                            <item dataType="ObjectRef">3158078592</item>
                                            <item dataType="Type" id="1893707588" value="BasicMenu.MenuChangeColor" />
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="2765505430">
                                            <item dataType="ObjectRef">1884592792</item>
                                            <item dataType="ObjectRef">3295934854</item>
                                            <item dataType="ObjectRef">2710253228</item>
                                            <item dataType="ObjectRef">2181795775</item>
                                          </values>
                                        </body>
                                      </compMap>
                                      <compTransform dataType="ObjectRef">1884592792</compTransform>
                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                        <header>
                                          <data dataType="Array" type="System.Byte[]" id="2814090112">6LxU90NJPUCn4lXtH+n0kw==</data>
                                        </header>
                                        <body />
                                      </identifier>
                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                      <name dataType="String">ChangeColor</name>
                                      <parent dataType="ObjectRef">2599756932</parent>
                                      <prefabLink />
                                    </item>
                                    <item dataType="Struct" type="Duality.GameObject" id="1191922409">
                                      <active dataType="Bool">true</active>
                                      <children />
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4066278573">
                                        <_items dataType="Array" type="Duality.Component[]" id="2483426150" length="4">
                                          <item dataType="Struct" type="Duality.Components.Transform" id="1249199627">
                                            <active dataType="Bool">true</active>
                                            <angle dataType="Float">0</angle>
                                            <angleAbs dataType="Float">0</angleAbs>
                                            <gameobj dataType="ObjectRef">1191922409</gameobj>
                                            <ignoreParent dataType="Bool">false</ignoreParent>
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
                                          </item>
                                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="2074860063">
                                            <active dataType="Bool">true</active>
                                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">255</B>
                                              <G dataType="Byte">255</G>
                                              <R dataType="Byte">255</R>
                                            </colorTint>
                                            <customMat />
                                            <gameobj dataType="ObjectRef">1191922409</gameobj>
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                                            <offset dataType="Float">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="3306476895">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="2073070702">
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
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="726121336" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="1951294919">
                                            <item dataType="ObjectRef">1995515332</item>
                                            <item dataType="ObjectRef">3158078592</item>
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="752355584">
                                            <item dataType="ObjectRef">1249199627</item>
                                            <item dataType="ObjectRef">2074860063</item>
                                          </values>
                                        </body>
                                      </compMap>
                                      <compTransform dataType="ObjectRef">1249199627</compTransform>
                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                        <header>
                                          <data dataType="Array" type="System.Byte[]" id="890701381">XgOaC/d3nU6CewDCFzc6Zw==</data>
                                        </header>
                                        <body />
                                      </identifier>
                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                      <name dataType="String">Text</name>
                                      <parent dataType="ObjectRef">2599756932</parent>
                                      <prefabLink />
                                    </item>
                                  </_items>
                                  <_size dataType="Int">3</_size>
                                </children>
                                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1807245322">
                                  <_items dataType="Array" type="Duality.Component[]" id="1485095960" length="4">
                                    <item dataType="Struct" type="Duality.Components.Transform" id="2657034150">
                                      <active dataType="Bool">true</active>
                                      <angle dataType="Float">0</angle>
                                      <angleAbs dataType="Float">0</angleAbs>
                                      <gameobj dataType="ObjectRef">2599756932</gameobj>
                                      <ignoreParent dataType="Bool">false</ignoreParent>
                                      <pos dataType="Struct" type="Duality.Vector3" />
                                      <posAbs dataType="Struct" type="Duality.Vector3" />
                                      <scale dataType="Float">1</scale>
                                      <scaleAbs dataType="Float">1</scaleAbs>
                                    </item>
                                    <item dataType="ObjectRef">2964052133</item>
                                  </_items>
                                  <_size dataType="Int">2</_size>
                                </compList>
                                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1419933490" surrogate="true">
                                  <header />
                                  <body>
                                    <keys dataType="Array" type="System.Object[]" id="1432939936">
                                      <item dataType="ObjectRef">1995515332</item>
                                      <item dataType="ObjectRef">1515364316</item>
                                    </keys>
                                    <values dataType="Array" type="System.Object[]" id="2148505230">
                                      <item dataType="ObjectRef">2657034150</item>
                                      <item dataType="ObjectRef">2964052133</item>
                                    </values>
                                  </body>
                                </compMap>
                                <compTransform dataType="ObjectRef">2657034150</compTransform>
                                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                  <header>
                                    <data dataType="Array" type="System.Byte[]" id="114915772">0sUNkQwR50CeTXizJxhuVA==</data>
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
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="397589176" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="3019778343">
                            <item dataType="ObjectRef">1995515332</item>
                            <item dataType="ObjectRef">726534550</item>
                            <item dataType="ObjectRef">3158078592</item>
                            <item dataType="ObjectRef">2103117858</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="2004035072">
                            <item dataType="ObjectRef">2083946911</item>
                            <item dataType="ObjectRef">3495288973</item>
                            <item dataType="ObjectRef">2909607347</item>
                            <item dataType="ObjectRef">2450049757</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">2083946911</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="1971041381">KjE6oFKOU0+SS26p2SI4vw==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Another</name>
                      <parent dataType="ObjectRef">797331632</parent>
                      <prefabLink />
                    </item>
                    <item dataType="Struct" type="Duality.GameObject" id="1287612476">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3766970296">
                        <_items dataType="Array" type="Duality.Component[]" id="2696490092">
                          <item dataType="Struct" type="Duality.Components.Transform" id="1344889694">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <gameobj dataType="ObjectRef">1287612476</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
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
                          </item>
                          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2756231756">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">45</B>
                              <G dataType="Byte">45</G>
                              <R dataType="Byte">45</R>
                            </colorTint>
                            <customMat />
                            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                            <gameobj dataType="ObjectRef">1287612476</gameobj>
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
                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="2170550130">
                            <active dataType="Bool">true</active>
                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <gameobj dataType="ObjectRef">1287612476</gameobj>
                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                            <offset dataType="Float">0</offset>
                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2406148482">
                              <flowAreas />
                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="385338512">
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
                          <item dataType="Struct" type="BasicMenu.MenuSwitchToPage" id="1710992540">
                            <active dataType="Bool">true</active>
                            <gameobj dataType="ObjectRef">1287612476</gameobj>
                            <hoverTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">38</B>
                              <G dataType="Byte">140</G>
                              <R dataType="Byte">245</R>
                            </hoverTint>
                            <target dataType="Struct" type="BasicMenu.MenuPage" id="2236712753">
                              <active dataType="Bool">true</active>
                              <gameobj dataType="Struct" type="Duality.GameObject" id="1872417552">
                                <active dataType="Bool">false</active>
                                <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3557588183">
                                  <_items dataType="Array" type="Duality.GameObject[]" id="155283470">
                                    <item dataType="Struct" type="Duality.GameObject" id="438107799">
                                      <active dataType="Bool">true</active>
                                      <children />
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1680197479">
                                        <_items dataType="Array" type="Duality.Component[]" id="2861982286">
                                          <item dataType="Struct" type="Duality.Components.Transform" id="495385017">
                                            <active dataType="Bool">true</active>
                                            <angle dataType="Float">0</angle>
                                            <angleAbs dataType="Float">0</angleAbs>
                                            <gameobj dataType="ObjectRef">438107799</gameobj>
                                            <ignoreParent dataType="Bool">false</ignoreParent>
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
                                          </item>
                                          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1906727079">
                                            <active dataType="Bool">true</active>
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">45</B>
                                              <G dataType="Byte">45</G>
                                              <R dataType="Byte">45</R>
                                            </colorTint>
                                            <customMat />
                                            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                                            <gameobj dataType="ObjectRef">438107799</gameobj>
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
                                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="1321045453">
                                            <active dataType="Bool">true</active>
                                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">255</B>
                                              <G dataType="Byte">255</G>
                                              <R dataType="Byte">255</R>
                                            </colorTint>
                                            <customMat />
                                            <gameobj dataType="ObjectRef">438107799</gameobj>
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                                            <offset dataType="Float">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2447863597">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="3502122598">
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
                                          <item dataType="Struct" type="BasicMenu.MenuChangeVolume" id="907698475">
                                            <active dataType="Bool">true</active>
                                            <changeAmount dataType="Short">-1</changeAmount>
                                            <gameobj dataType="ObjectRef">438107799</gameobj>
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
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1326010496" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="1758679373">
                                            <item dataType="ObjectRef">1995515332</item>
                                            <item dataType="ObjectRef">726534550</item>
                                            <item dataType="ObjectRef">3158078592</item>
                                            <item dataType="Type" id="2040124454" value="BasicMenu.MenuChangeVolume" />
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="2787043000">
                                            <item dataType="ObjectRef">495385017</item>
                                            <item dataType="ObjectRef">1906727079</item>
                                            <item dataType="ObjectRef">1321045453</item>
                                            <item dataType="ObjectRef">907698475</item>
                                          </values>
                                        </body>
                                      </compMap>
                                      <compTransform dataType="ObjectRef">495385017</compTransform>
                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                        <header>
                                          <data dataType="Array" type="System.Byte[]" id="1164298535">RThwaA1XakahLG48HxAACA==</data>
                                        </header>
                                        <body />
                                      </identifier>
                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                      <name dataType="String">Minus</name>
                                      <parent dataType="ObjectRef">1872417552</parent>
                                      <prefabLink />
                                    </item>
                                    <item dataType="Struct" type="Duality.GameObject" id="3373619622">
                                      <active dataType="Bool">true</active>
                                      <children />
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="788798874">
                                        <_items dataType="Array" type="Duality.Component[]" id="3996896128">
                                          <item dataType="Struct" type="Duality.Components.Transform" id="3430896840">
                                            <active dataType="Bool">true</active>
                                            <angle dataType="Float">0</angle>
                                            <angleAbs dataType="Float">0</angleAbs>
                                            <gameobj dataType="ObjectRef">3373619622</gameobj>
                                            <ignoreParent dataType="Bool">false</ignoreParent>
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
                                          </item>
                                          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="547271606">
                                            <active dataType="Bool">true</active>
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">45</B>
                                              <G dataType="Byte">45</G>
                                              <R dataType="Byte">45</R>
                                            </colorTint>
                                            <customMat />
                                            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                                            <gameobj dataType="ObjectRef">3373619622</gameobj>
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
                                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="4256557276">
                                            <active dataType="Bool">true</active>
                                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">255</B>
                                              <G dataType="Byte">255</G>
                                              <R dataType="Byte">255</R>
                                            </colorTint>
                                            <customMat />
                                            <gameobj dataType="ObjectRef">3373619622</gameobj>
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                                            <offset dataType="Float">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1067104900">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="2846368836">
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
                                          <item dataType="Struct" type="BasicMenu.MenuChangeVolume" id="3843210298">
                                            <active dataType="Bool">true</active>
                                            <changeAmount dataType="Short">1</changeAmount>
                                            <gameobj dataType="ObjectRef">3373619622</gameobj>
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
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2784582458" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="798199776">
                                            <item dataType="ObjectRef">1995515332</item>
                                            <item dataType="ObjectRef">726534550</item>
                                            <item dataType="ObjectRef">3158078592</item>
                                            <item dataType="ObjectRef">2040124454</item>
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="1999430542">
                                            <item dataType="ObjectRef">3430896840</item>
                                            <item dataType="ObjectRef">547271606</item>
                                            <item dataType="ObjectRef">4256557276</item>
                                            <item dataType="ObjectRef">3843210298</item>
                                          </values>
                                        </body>
                                      </compMap>
                                      <compTransform dataType="ObjectRef">3430896840</compTransform>
                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                        <header>
                                          <data dataType="Array" type="System.Byte[]" id="1248149244">kJ3c4yd3nUyhgw0U7zhTNA==</data>
                                        </header>
                                        <body />
                                      </identifier>
                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                      <name dataType="String">Plus</name>
                                      <parent dataType="ObjectRef">1872417552</parent>
                                      <prefabLink />
                                    </item>
                                    <item dataType="Struct" type="Duality.GameObject" id="1667858786">
                                      <active dataType="Bool">true</active>
                                      <children />
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="813505254">
                                        <_items dataType="Array" type="Duality.Component[]" id="3358178688" length="4">
                                          <item dataType="Struct" type="Duality.Components.Transform" id="1725136004">
                                            <active dataType="Bool">true</active>
                                            <angle dataType="Float">0</angle>
                                            <angleAbs dataType="Float">0</angleAbs>
                                            <gameobj dataType="ObjectRef">1667858786</gameobj>
                                            <ignoreParent dataType="Bool">false</ignoreParent>
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
                                          </item>
                                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="2550796440">
                                            <active dataType="Bool">true</active>
                                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">255</B>
                                              <G dataType="Byte">255</G>
                                              <R dataType="Byte">255</R>
                                            </colorTint>
                                            <customMat />
                                            <gameobj dataType="ObjectRef">1667858786</gameobj>
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                                            <offset dataType="Float">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="141015560">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="662676844">
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
                                          <item dataType="Struct" type="BasicMenu.VolumeRenderer" id="1381708810">
                                            <active dataType="Bool">true</active>
                                            <gameobj dataType="ObjectRef">1667858786</gameobj>
                                          </item>
                                        </_items>
                                        <_size dataType="Int">3</_size>
                                      </compList>
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1176555834" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="12880212">
                                            <item dataType="ObjectRef">1995515332</item>
                                            <item dataType="ObjectRef">3158078592</item>
                                            <item dataType="Type" id="223594212" value="BasicMenu.VolumeRenderer" />
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="3200684470">
                                            <item dataType="ObjectRef">1725136004</item>
                                            <item dataType="ObjectRef">2550796440</item>
                                            <item dataType="ObjectRef">1381708810</item>
                                          </values>
                                        </body>
                                      </compMap>
                                      <compTransform dataType="ObjectRef">1725136004</compTransform>
                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                        <header>
                                          <data dataType="Array" type="System.Byte[]" id="596963952">IAcOZLPGHk2ALP+qZ2z55A==</data>
                                        </header>
                                        <body />
                                      </identifier>
                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                      <name dataType="String">VolumeText</name>
                                      <parent dataType="ObjectRef">1872417552</parent>
                                      <prefabLink />
                                    </item>
                                    <item dataType="Struct" type="Duality.GameObject" id="1107886670">
                                      <active dataType="Bool">true</active>
                                      <children />
                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3747628866">
                                        <_items dataType="Array" type="Duality.Component[]" id="239932432">
                                          <item dataType="Struct" type="Duality.Components.Transform" id="1165163888">
                                            <active dataType="Bool">true</active>
                                            <angle dataType="Float">0</angle>
                                            <angleAbs dataType="Float">0</angleAbs>
                                            <gameobj dataType="ObjectRef">1107886670</gameobj>
                                            <ignoreParent dataType="Bool">false</ignoreParent>
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
                                          </item>
                                          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2576505950">
                                            <active dataType="Bool">true</active>
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">45</B>
                                              <G dataType="Byte">45</G>
                                              <R dataType="Byte">45</R>
                                            </colorTint>
                                            <customMat />
                                            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                                            <gameobj dataType="ObjectRef">1107886670</gameobj>
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
                                          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="1990824324">
                                            <active dataType="Bool">true</active>
                                            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">255</B>
                                              <G dataType="Byte">255</G>
                                              <R dataType="Byte">255</R>
                                            </colorTint>
                                            <customMat />
                                            <gameobj dataType="ObjectRef">1107886670</gameobj>
                                            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                                            <offset dataType="Float">0</offset>
                                            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2670087532">
                                              <flowAreas />
                                              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1052801892">
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
                                          <item dataType="Struct" type="BasicMenu.MenuSwitchToPage" id="1531266734">
                                            <active dataType="Bool">true</active>
                                            <gameobj dataType="ObjectRef">1107886670</gameobj>
                                            <hoverTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                              <A dataType="Byte">255</A>
                                              <B dataType="Byte">38</B>
                                              <G dataType="Byte">140</G>
                                              <R dataType="Byte">245</R>
                                            </hoverTint>
                                            <target dataType="ObjectRef">1161626833</target>
                                          </item>
                                        </_items>
                                        <_size dataType="Int">4</_size>
                                      </compList>
                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2885945354" surrogate="true">
                                        <header />
                                        <body>
                                          <keys dataType="Array" type="System.Object[]" id="455225880">
                                            <item dataType="ObjectRef">1995515332</item>
                                            <item dataType="ObjectRef">726534550</item>
                                            <item dataType="ObjectRef">3158078592</item>
                                            <item dataType="ObjectRef">2103117858</item>
                                          </keys>
                                          <values dataType="Array" type="System.Object[]" id="793981726">
                                            <item dataType="ObjectRef">1165163888</item>
                                            <item dataType="ObjectRef">2576505950</item>
                                            <item dataType="ObjectRef">1990824324</item>
                                            <item dataType="ObjectRef">1531266734</item>
                                          </values>
                                        </body>
                                      </compMap>
                                      <compTransform dataType="ObjectRef">1165163888</compTransform>
                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                        <header>
                                          <data dataType="Array" type="System.Byte[]" id="753836868">O6GUJgTvAUWGs3PGuBzr7g==</data>
                                        </header>
                                        <body />
                                      </identifier>
                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                      <name dataType="String">Back</name>
                                      <parent dataType="ObjectRef">1872417552</parent>
                                      <prefabLink />
                                    </item>
                                  </_items>
                                  <_size dataType="Int">4</_size>
                                </children>
                                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="760348096">
                                  <_items dataType="Array" type="Duality.Component[]" id="2176144733" length="4">
                                    <item dataType="Struct" type="Duality.Components.Transform" id="1929694770">
                                      <active dataType="Bool">true</active>
                                      <angle dataType="Float">0</angle>
                                      <angleAbs dataType="Float">0</angleAbs>
                                      <gameobj dataType="ObjectRef">1872417552</gameobj>
                                      <ignoreParent dataType="Bool">false</ignoreParent>
                                      <pos dataType="Struct" type="Duality.Vector3" />
                                      <posAbs dataType="Struct" type="Duality.Vector3" />
                                      <scale dataType="Float">1</scale>
                                      <scaleAbs dataType="Float">1</scaleAbs>
                                    </item>
                                    <item dataType="ObjectRef">2236712753</item>
                                  </_items>
                                  <_size dataType="Int">2</_size>
                                </compList>
                                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4271378165" surrogate="true">
                                  <header />
                                  <body>
                                    <keys dataType="Array" type="System.Object[]" id="928144564">
                                      <item dataType="ObjectRef">1995515332</item>
                                      <item dataType="ObjectRef">1515364316</item>
                                    </keys>
                                    <values dataType="Array" type="System.Object[]" id="3938275318">
                                      <item dataType="ObjectRef">1929694770</item>
                                      <item dataType="ObjectRef">2236712753</item>
                                    </values>
                                  </body>
                                </compMap>
                                <compTransform dataType="ObjectRef">1929694770</compTransform>
                                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                  <header>
                                    <data dataType="Array" type="System.Byte[]" id="419212048">qSytuAGr30epimWOTaEamg==</data>
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
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3925997790" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="326661114">
                            <item dataType="ObjectRef">1995515332</item>
                            <item dataType="ObjectRef">726534550</item>
                            <item dataType="ObjectRef">3158078592</item>
                            <item dataType="ObjectRef">2103117858</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="2325760314">
                            <item dataType="ObjectRef">1344889694</item>
                            <item dataType="ObjectRef">2756231756</item>
                            <item dataType="ObjectRef">2170550130</item>
                            <item dataType="ObjectRef">1710992540</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">1344889694</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="3295553914">AIagRNFb20qRczGOdKEzqg==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Options</name>
                      <parent dataType="ObjectRef">797331632</parent>
                      <prefabLink />
                    </item>
                  </_items>
                  <_size dataType="Int">3</_size>
                </children>
                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3279721288">
                  <_items dataType="Array" type="Duality.Component[]" id="4148690529" length="4">
                    <item dataType="Struct" type="Duality.Components.Transform" id="854608850">
                      <active dataType="Bool">true</active>
                      <angle dataType="Float">0</angle>
                      <angleAbs dataType="Float">0</angleAbs>
                      <gameobj dataType="ObjectRef">797331632</gameobj>
                      <ignoreParent dataType="Bool">false</ignoreParent>
                      <pos dataType="Struct" type="Duality.Vector3" />
                      <posAbs dataType="Struct" type="Duality.Vector3" />
                      <scale dataType="Float">1</scale>
                      <scaleAbs dataType="Float">1</scaleAbs>
                    </item>
                    <item dataType="ObjectRef">1161626833</item>
                  </_items>
                  <_size dataType="Int">2</_size>
                </compList>
                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4160726273" surrogate="true">
                  <header />
                  <body>
                    <keys dataType="Array" type="System.Object[]" id="314235204">
                      <item dataType="ObjectRef">1995515332</item>
                      <item dataType="ObjectRef">1515364316</item>
                    </keys>
                    <values dataType="Array" type="System.Object[]" id="1701986966">
                      <item dataType="ObjectRef">854608850</item>
                      <item dataType="ObjectRef">1161626833</item>
                    </values>
                  </body>
                </compMap>
                <compTransform dataType="ObjectRef">854608850</compTransform>
                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                  <header>
                    <data dataType="Array" type="System.Byte[]" id="279169280">Btict1a0Q0a8rLhg1ELqGA==</data>
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
          <item dataType="Struct" type="BasicMenu.UpdateMenuController" id="1527748615">
            <active dataType="Bool">false</active>
            <gameobj dataType="ObjectRef">1050330856</gameobj>
            <startingMenu dataType="ObjectRef">1161626833</startingMenu>
          </item>
        </_items>
        <_size dataType="Int">6</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1262677194" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3120556716">
            <item dataType="ObjectRef">1995515332</item>
            <item dataType="Type" id="1096038116" value="Duality.Components.Camera" />
            <item dataType="Type" id="1326986774" value="Duality.Components.SoundListener" />
            <item dataType="Type" id="2541007072" value="BasicMenu.EventMenuController" />
            <item dataType="Type" id="2313505378" value="BasicMenu.UpdateMenuController" />
            <item dataType="Type" id="3682614972" value="Duality.Components.VelocityTracker" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="2486870454">
            <item dataType="ObjectRef">1107608074</item>
            <item dataType="ObjectRef">2596717333</item>
            <item dataType="ObjectRef">3082983383</item>
            <item dataType="ObjectRef">3588024560</item>
            <item dataType="ObjectRef">1527748615</item>
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
    <item dataType="ObjectRef">1872417552</item>
    <item dataType="ObjectRef">457252721</item>
    <item dataType="ObjectRef">2599756932</item>
    <item dataType="Struct" type="Duality.GameObject" id="3714973577">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="195829963">
        <_items dataType="Array" type="Duality.Component[]" id="757322742" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="3772250795">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <gameobj dataType="ObjectRef">3714973577</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
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
          </item>
          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="888625561">
            <active dataType="Bool">true</active>
            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">255</B>
              <G dataType="Byte">255</G>
              <R dataType="Byte">255</R>
            </colorTint>
            <customMat />
            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
            <gameobj dataType="ObjectRef">3714973577</gameobj>
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
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4150116424" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="4117077729">
            <item dataType="ObjectRef">1995515332</item>
            <item dataType="ObjectRef">726534550</item>
          </keys>
          <values dataType="Array" type="System.Object[]" id="715159072">
            <item dataType="ObjectRef">3772250795</item>
            <item dataType="ObjectRef">888625561</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">3772250795</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="875369587">AMEXEUxNi0ynXVBg8zLQjw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Background</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="536445860">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4268670234">
        <_items dataType="Array" type="Duality.Component[]" id="2587893120" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="593723078">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <gameobj dataType="ObjectRef">536445860</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
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
          </item>
          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="1419383514">
            <active dataType="Bool">true</active>
            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">255</B>
              <G dataType="Byte">255</G>
              <R dataType="Byte">255</R>
            </colorTint>
            <customMat />
            <gameobj dataType="ObjectRef">536445860</gameobj>
            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
            <offset dataType="Float">0</offset>
            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2497476530">
              <flowAreas />
              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="485705424">
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
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3776753978" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2217351264">
            <item dataType="ObjectRef">1995515332</item>
            <item dataType="ObjectRef">3158078592</item>
          </keys>
          <values dataType="Array" type="System.Object[]" id="4244446862">
            <item dataType="ObjectRef">593723078</item>
            <item dataType="ObjectRef">1419383514</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">593723078</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1859994236">7Tq2cVo2nkm6EuVYyhtBrA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">TextRenderer</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">797331632</item>
    <item dataType="Struct" type="Duality.GameObject" id="1373948707">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2197302033">
        <_items dataType="Array" type="Duality.Component[]" id="1046754030" length="4">
          <item dataType="Struct" type="BasicMenu.MouseCursorRenderer" id="3155741276">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1373948707</gameobj>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3028378528" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2803429051">
            <item dataType="Type" id="492404950" value="BasicMenu.MouseCursorRenderer" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="1550496808">
            <item dataType="ObjectRef">3155741276</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2556213041">wG8rjzLH0UWLy35KUweUlw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">MouseCursorRenderer</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">438107799</item>
    <item dataType="ObjectRef">3373619622</item>
    <item dataType="ObjectRef">1667858786</item>
    <item dataType="ObjectRef">1107886670</item>
    <item dataType="ObjectRef">3821432958</item>
    <item dataType="ObjectRef">3977901161</item>
    <item dataType="ObjectRef">2811845245</item>
    <item dataType="ObjectRef">3363248187</item>
    <item dataType="ObjectRef">1827315574</item>
    <item dataType="ObjectRef">1191922409</item>
    <item dataType="ObjectRef">4291917305</item>
    <item dataType="ObjectRef">2026669693</item>
    <item dataType="ObjectRef">1287612476</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
