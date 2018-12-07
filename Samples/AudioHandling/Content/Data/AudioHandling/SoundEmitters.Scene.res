<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">33</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="2941461157">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="944333527">
        <_items dataType="Array" type="Duality.GameObject[]" id="4084004878" length="4">
          <item dataType="Struct" type="Duality.GameObject" id="1050330856">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1233381620">
              <_items dataType="Array" type="Duality.GameObject[]" id="3134126244" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="2070411088">
                  <active dataType="Bool">true</active>
                  <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3423286040">
                    <_items dataType="Array" type="Duality.GameObject[]" id="3089807916" length="4">
                      <item dataType="Struct" type="Duality.GameObject" id="811858158">
                        <active dataType="Bool">true</active>
                        <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2648488118">
                          <_items dataType="Array" type="Duality.GameObject[]" id="615346016" length="4" />
                          <_size dataType="Int">0</_size>
                        </children>
                        <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="44860058">
                          <_items dataType="Array" type="Duality.Component[]" id="523180420" length="4">
                            <item dataType="Struct" type="Duality.Components.Transform" id="869135376">
                              <active dataType="Bool">true</active>
                              <angle dataType="Float">0</angle>
                              <angleAbs dataType="Float">0</angleAbs>
                              <gameobj dataType="ObjectRef">811858158</gameobj>
                              <ignoreParent dataType="Bool">false</ignoreParent>
                              <pos dataType="Struct" type="Duality.Vector3" />
                              <posAbs dataType="Struct" type="Duality.Vector3" />
                              <scale dataType="Float">1</scale>
                              <scaleAbs dataType="Float">1</scaleAbs>
                            </item>
                            <item dataType="Struct" type="Duality.Components.VelocityTracker" id="2882992625">
                              <active dataType="Bool">true</active>
                              <gameobj dataType="ObjectRef">811858158</gameobj>
                            </item>
                            <item dataType="Struct" type="Duality.Components.SoundEmitter" id="3032670041">
                              <active dataType="Bool">true</active>
                              <gameobj dataType="ObjectRef">811858158</gameobj>
                              <sources dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.SoundEmitter+Source]]" id="1200872229">
                                <_items dataType="Array" type="Duality.Components.SoundEmitter+Source[]" id="3465450134" length="4">
                                  <item dataType="Struct" type="Duality.Components.SoundEmitter+Source" id="2473013792">
                                    <looped dataType="Bool">true</looped>
                                    <lowpass dataType="Float">1</lowpass>
                                    <offset dataType="Struct" type="Duality.Vector3" />
                                    <paused dataType="Bool">false</paused>
                                    <pitch dataType="Float">1</pitch>
                                    <sound dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Sound]]">
                                      <contentPath dataType="String">Data\AudioHandling\Audio\Rain.Sound.res</contentPath>
                                    </sound>
                                    <volume dataType="Float">1</volume>
                                  </item>
                                </_items>
                                <_size dataType="Int">1</_size>
                              </sources>
                            </item>
                          </_items>
                          <_size dataType="Int">3</_size>
                        </compList>
                        <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1124134742" surrogate="true">
                          <header />
                          <body>
                            <keys dataType="Array" type="System.Object[]" id="1606603328">
                              <item dataType="Type" id="1053895452" value="Duality.Components.Transform" />
                              <item dataType="Type" id="1798069782" value="Duality.Components.SoundEmitter" />
                              <item dataType="Type" id="2765248648" value="Duality.Components.VelocityTracker" />
                            </keys>
                            <values dataType="Array" type="System.Object[]" id="2928927822">
                              <item dataType="ObjectRef">869135376</item>
                              <item dataType="ObjectRef">3032670041</item>
                              <item dataType="ObjectRef">2882992625</item>
                            </values>
                          </body>
                        </compMap>
                        <compTransform dataType="ObjectRef">869135376</compTransform>
                        <identifier dataType="Struct" type="System.Guid" surrogate="true">
                          <header>
                            <data dataType="Array" type="System.Byte[]" id="2940760284">LTOYExa1eE2/G5VJQBOGvg==</data>
                          </header>
                          <body />
                        </identifier>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        <name dataType="String">Rain</name>
                        <parent dataType="ObjectRef">2070411088</parent>
                        <prefabLink />
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                  </children>
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1838744862">
                    <_items dataType="Array" type="Duality.Component[]" id="2417284314" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2127688306">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <gameobj dataType="ObjectRef">2070411088</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">0</Y>
                          <Z dataType="Float">500</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3" />
                        <scale dataType="Float">1</scale>
                        <scaleAbs dataType="Float">1</scaleAbs>
                      </item>
                      <item dataType="Struct" type="Duality.Components.VelocityTracker" id="4141545555">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2070411088</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.SoundListener" id="4103063615">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2070411088</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">3</_size>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2621811268" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="2869145832">
                        <item dataType="ObjectRef">1053895452</item>
                        <item dataType="Type" id="3430959660" value="Duality.Components.SoundListener" />
                        <item dataType="ObjectRef">2765248648</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="864810270">
                        <item dataType="ObjectRef">2127688306</item>
                        <item dataType="ObjectRef">4103063615</item>
                        <item dataType="ObjectRef">4141545555</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2127688306</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2739209812">YdDlDASn+k20XNsuHc781A==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Microphone</name>
                  <parent dataType="ObjectRef">1050330856</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2331473654">
              <_items dataType="Array" type="Duality.Component[]" id="1012425438" length="4">
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
                    <B dataType="Byte">163</B>
                    <G dataType="Byte">132</G>
                    <R dataType="Byte">98</R>
                  </clearColor>
                  <farZ dataType="Float">10000</farZ>
                  <focusDist dataType="Float">500</focusDist>
                  <gameobj dataType="ObjectRef">1050330856</gameobj>
                  <nearZ dataType="Float">50</nearZ>
                  <priority dataType="Int">0</priority>
                  <projection dataType="Enum" type="Duality.Drawing.ProjectionMode" name="Perspective" value="1" />
                  <renderSetup dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderSetup]]" />
                  <renderTarget dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]" />
                  <shaderParameters dataType="Struct" type="Duality.Drawing.ShaderParameterCollection" id="1987284565" custom="true">
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
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1052096976" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="956500616">
                  <item dataType="ObjectRef">1053895452</item>
                  <item dataType="Type" id="1551789932" value="Duality.Components.Camera" />
                </keys>
                <values dataType="Array" type="System.Object[]" id="3494726622">
                  <item dataType="ObjectRef">1107608074</item>
                  <item dataType="ObjectRef">2596717333</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1107608074</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="406106228">FmbIdiGcaECC4D9FoGgPSA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">MainCamera</name>
            <parent dataType="ObjectRef">2941461157</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1692304832">
        <_items dataType="Array" type="Duality.Component[]" id="2943016797" length="4">
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
              <contentPath dataType="String">Data\AudioHandling\Visuals\TargetCharacter.Material.res</contentPath>
            </sharedMat>
            <spriteIndex dataType="Int">-1</spriteIndex>
            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          </item>
          <item dataType="Struct" type="AudioHandling.PlayerCharacter" id="3052174018">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">2941461157</gameobj>
            <speed dataType="Float">5</speed>
          </item>
        </_items>
        <_size dataType="Int">3</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4044266741" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="4193519796">
            <item dataType="ObjectRef">1053895452</item>
            <item dataType="Type" id="3124833700" value="Duality.Components.Renderers.SpriteRenderer" />
            <item dataType="Type" id="2179462934" value="AudioHandling.PlayerCharacter" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="3728564214">
            <item dataType="ObjectRef">2998738375</item>
            <item dataType="ObjectRef">115113141</item>
            <item dataType="ObjectRef">3052174018</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">2998738375</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="4177651472">vytfBF4aBEqzNygTOMMzvw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">PlayerCharacter</name>
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
                    <B dataType="Byte">129</B>
                    <G dataType="Byte">129</G>
                    <R dataType="Byte">129</R>
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
                    <contentPath dataType="String">Data\AudioHandling\Visuals\BackgroundTile.Material.res</contentPath>
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
                  <item dataType="ObjectRef">1053895452</item>
                  <item dataType="ObjectRef">3124833700</item>
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
        </_items>
        <_size dataType="Int">1</_size>
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
    <item dataType="Struct" type="Duality.GameObject" id="1807655791">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1338602381">
        <_items dataType="Array" type="Duality.GameObject[]" id="1548294438" length="4">
          <item dataType="Struct" type="Duality.GameObject" id="1018476967">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2881187495">
              <_items dataType="Array" type="Duality.Component[]" id="1457562574" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1075754185">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">1018476967</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">18</X>
                    <Y dataType="Float">2</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">178</X>
                    <Y dataType="Float">-94</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="1901414621">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">179</G>
                    <R dataType="Byte">128</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">1018476967</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Float">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="3334435485">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1505644262">
                      <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                        <contentPath dataType="String">Default:Font:GenericMonospace10</contentPath>
                      </item>
                    </fonts>
                    <icons />
                    <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                    <maxHeight dataType="Int">0</maxHeight>
                    <maxWidth dataType="Int">0</maxWidth>
                    <sourceText dataType="String">Dogs, Barking</sourceText>
                    <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                  </text>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3132524544" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1507708045">
                  <item dataType="ObjectRef">1053895452</item>
                  <item dataType="Type" id="499089702" value="Duality.Components.Renderers.TextRenderer" />
                </keys>
                <values dataType="Array" type="System.Object[]" id="1976707000">
                  <item dataType="ObjectRef">1075754185</item>
                  <item dataType="ObjectRef">1901414621</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1075754185</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1300928359">b3tU0/GuR0OJTUgSccPwTQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">TextRenderer</name>
            <parent dataType="ObjectRef">1807655791</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2074296248">
        <_items dataType="Array" type="Duality.Component[]" id="3681472743">
          <item dataType="Struct" type="Duality.Components.Transform" id="1864933009">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <gameobj dataType="ObjectRef">1807655791</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <pos dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">160</X>
              <Y dataType="Float">-96</Y>
              <Z dataType="Float">0</Z>
            </pos>
            <posAbs dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">160</X>
              <Y dataType="Float">-96</Y>
              <Z dataType="Float">0</Z>
            </posAbs>
            <scale dataType="Float">1</scale>
            <scaleAbs dataType="Float">1</scaleAbs>
          </item>
          <item dataType="Struct" type="Duality.Components.VelocityTracker" id="3878790258">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1807655791</gameobj>
          </item>
          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3276275071">
            <active dataType="Bool">true</active>
            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">255</B>
              <G dataType="Byte">179</G>
              <R dataType="Byte">128</R>
            </colorTint>
            <customMat />
            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
            <gameobj dataType="ObjectRef">1807655791</gameobj>
            <offset dataType="Float">0</offset>
            <pixelGrid dataType="Bool">false</pixelGrid>
            <rect dataType="Struct" type="Duality.Rect">
              <H dataType="Float">32</H>
              <W dataType="Float">32</W>
              <X dataType="Float">-16</X>
              <Y dataType="Float">-16</Y>
            </rect>
            <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
            <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
              <contentPath dataType="String">Data\AudioHandling\Visuals\TargetCharacter.Material.res</contentPath>
            </sharedMat>
            <spriteIndex dataType="Int">-1</spriteIndex>
            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Components.SoundEmitter" id="4028467674">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1807655791</gameobj>
            <sources dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.SoundEmitter+Source]]" id="3547696508">
              <_items dataType="Array" type="Duality.Components.SoundEmitter+Source[]" id="3728913476" length="4">
                <item dataType="Struct" type="Duality.Components.SoundEmitter+Source" id="597286468">
                  <looped dataType="Bool">true</looped>
                  <lowpass dataType="Float">1</lowpass>
                  <offset dataType="Struct" type="Duality.Vector3" />
                  <paused dataType="Bool">false</paused>
                  <pitch dataType="Float">1</pitch>
                  <sound dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Sound]]">
                    <contentPath dataType="String">Data\AudioHandling\Audio\DogsBarking.Sound.res</contentPath>
                  </sound>
                  <volume dataType="Float">1</volume>
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </sources>
          </item>
        </_items>
        <_size dataType="Int">4</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1826798183" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2941634068">
            <item dataType="ObjectRef">1053895452</item>
            <item dataType="ObjectRef">3124833700</item>
            <item dataType="ObjectRef">1798069782</item>
            <item dataType="ObjectRef">2765248648</item>
          </keys>
          <values dataType="Array" type="System.Object[]" id="3875645238">
            <item dataType="ObjectRef">1864933009</item>
            <item dataType="ObjectRef">3276275071</item>
            <item dataType="ObjectRef">4028467674</item>
            <item dataType="ObjectRef">3878790258</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">1864933009</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="636681904">oNEUXLZYoUiwxhixTMJtZQ==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">DogsBarking</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="941830520">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1873207966">
        <_items dataType="Array" type="Duality.GameObject[]" id="3297403792" length="4">
          <item dataType="Struct" type="Duality.GameObject" id="3541146052">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3518997548">
              <_items dataType="Array" type="Duality.Component[]" id="3302036708" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="3598423270">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">3541146052</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">18</X>
                    <Y dataType="Float">2</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-334</X>
                    <Y dataType="Float">290</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="129116410">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">179</G>
                    <R dataType="Byte">128</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">3541146052</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Float">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="3311757722">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="3935138688">
                      <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                        <contentPath dataType="String">Default:Font:GenericMonospace10</contentPath>
                      </item>
                    </fonts>
                    <icons />
                    <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                    <maxHeight dataType="Int">0</maxHeight>
                    <maxWidth dataType="Int">0</maxWidth>
                    <sourceText dataType="String">Guitar</sourceText>
                    <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                  </text>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2762955702" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2571178086">
                  <item dataType="ObjectRef">1053895452</item>
                  <item dataType="ObjectRef">499089702</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="1259542330">
                  <item dataType="ObjectRef">3598423270</item>
                  <item dataType="ObjectRef">129116410</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3598423270</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3985134822">sSFmAoRuJUqtMIwyuVl3yQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">TextRenderer</name>
            <parent dataType="ObjectRef">941830520</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="977790346">
        <_items dataType="Array" type="Duality.Component[]" id="2062000572">
          <item dataType="Struct" type="Duality.Components.Transform" id="999107738">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <gameobj dataType="ObjectRef">941830520</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <pos dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">-352</X>
              <Y dataType="Float">288</Y>
              <Z dataType="Float">0</Z>
            </pos>
            <posAbs dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">-352</X>
              <Y dataType="Float">288</Y>
              <Z dataType="Float">0</Z>
            </posAbs>
            <scale dataType="Float">1</scale>
            <scaleAbs dataType="Float">1</scaleAbs>
          </item>
          <item dataType="Struct" type="Duality.Components.VelocityTracker" id="3012964987">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">941830520</gameobj>
          </item>
          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2410449800">
            <active dataType="Bool">true</active>
            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">255</B>
              <G dataType="Byte">179</G>
              <R dataType="Byte">128</R>
            </colorTint>
            <customMat />
            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
            <gameobj dataType="ObjectRef">941830520</gameobj>
            <offset dataType="Float">0</offset>
            <pixelGrid dataType="Bool">false</pixelGrid>
            <rect dataType="Struct" type="Duality.Rect">
              <H dataType="Float">32</H>
              <W dataType="Float">32</W>
              <X dataType="Float">-16</X>
              <Y dataType="Float">-16</Y>
            </rect>
            <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
            <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
              <contentPath dataType="String">Data\AudioHandling\Visuals\TargetCharacter.Material.res</contentPath>
            </sharedMat>
            <spriteIndex dataType="Int">-1</spriteIndex>
            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Components.SoundEmitter" id="3162642403">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">941830520</gameobj>
            <sources dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.SoundEmitter+Source]]" id="101413007">
              <_items dataType="Array" type="Duality.Components.SoundEmitter+Source[]" id="1480797358" length="4">
                <item dataType="Struct" type="Duality.Components.SoundEmitter+Source" id="815456592">
                  <looped dataType="Bool">true</looped>
                  <lowpass dataType="Float">1</lowpass>
                  <offset dataType="Struct" type="Duality.Vector3" />
                  <paused dataType="Bool">false</paused>
                  <pitch dataType="Float">1</pitch>
                  <sound dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Sound]]">
                    <contentPath dataType="String">Data\AudioHandling\Audio\Guitar.Sound.res</contentPath>
                  </sound>
                  <volume dataType="Float">1</volume>
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </sources>
          </item>
        </_items>
        <_size dataType="Int">4</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1820307822" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="4261964960">
            <item dataType="ObjectRef">1053895452</item>
            <item dataType="ObjectRef">3124833700</item>
            <item dataType="ObjectRef">1798069782</item>
            <item dataType="ObjectRef">2765248648</item>
          </keys>
          <values dataType="Array" type="System.Object[]" id="1605576846">
            <item dataType="ObjectRef">999107738</item>
            <item dataType="ObjectRef">2410449800</item>
            <item dataType="ObjectRef">3162642403</item>
            <item dataType="ObjectRef">3012964987</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">999107738</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1983307964">IGjcCsf6FEqCI5ckBiPBHQ==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Guitar</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="2615744653">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1399434559">
        <_items dataType="Array" type="Duality.Component[]" id="2601748142" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="2673021871">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <gameobj dataType="ObjectRef">2615744653</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <pos dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">-256</X>
              <Y dataType="Float">-96</Y>
              <Z dataType="Float">0</Z>
            </pos>
            <posAbs dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">-256</X>
              <Y dataType="Float">-96</Y>
              <Z dataType="Float">0</Z>
            </posAbs>
            <scale dataType="Float">1</scale>
            <scaleAbs dataType="Float">1</scaleAbs>
          </item>
          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="3498682307">
            <active dataType="Bool">true</active>
            <blockAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">255</B>
              <G dataType="Byte">255</G>
              <R dataType="Byte">255</R>
            </colorTint>
            <customMat />
            <gameobj dataType="ObjectRef">2615744653</gameobj>
            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
            <offset dataType="Float">0</offset>
            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2135061571">
              <flowAreas />
              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1106541606">
                <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                  <contentPath dataType="String">Default:Font:GenericMonospace10</contentPath>
                </item>
              </fonts>
              <icons />
              <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
              <maxHeight dataType="Int">0</maxHeight>
              <maxWidth dataType="Int">192</maxWidth>
              <sourceText dataType="String">Use /c44AAFFFFArrow Keys/cFFFFFFFF or /c44AAFFFFLeft Thumbstick/cFFFFFFFF to move around.</sourceText>
              <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
            </text>
            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          </item>
        </_items>
        <_size dataType="Int">2</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="723234528" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="707504373">
            <item dataType="ObjectRef">1053895452</item>
            <item dataType="ObjectRef">499089702</item>
          </keys>
          <values dataType="Array" type="System.Object[]" id="4058506440">
            <item dataType="ObjectRef">2673021871</item>
            <item dataType="ObjectRef">3498682307</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">2673021871</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="263128383">A2uPVpGLFk+Aw6RjJ2It9Q==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">TextRenderer</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">1050330856</item>
    <item dataType="ObjectRef">3570972695</item>
    <item dataType="ObjectRef">1018476967</item>
    <item dataType="ObjectRef">3541146052</item>
    <item dataType="ObjectRef">2070411088</item>
    <item dataType="ObjectRef">811858158</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
