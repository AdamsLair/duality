<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">33</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="2889347069">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1545539151">
        <_items dataType="Array" type="Duality.Component[]" id="1754464814">
          <item dataType="Struct" type="Duality.Components.Transform" id="954694705">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">2889347069</gameobj>
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
          <item dataType="Struct" type="Duality.Components.Camera" id="3426622876">
            <active dataType="Bool">true</active>
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">2889347069</gameobj>
            <nearZ dataType="Float">0</nearZ>
            <passes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Camera+Pass]]" id="1239103752">
              <_items dataType="Array" type="Duality.Components.Camera+Pass[]" id="659784556" length="4">
                <item dataType="Struct" type="Duality.Components.Camera+Pass" id="3419570020">
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
                <item dataType="Struct" type="Duality.Components.Camera+Pass" id="1106319894">
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
          <item dataType="Struct" type="Duality.Components.SoundListener" id="3542828440">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">2889347069</gameobj>
          </item>
          <item dataType="Struct" type="FlapOrDie.Controllers.GameController" id="4146559154">
            <active dataType="Bool">true</active>
            <baseSpeed dataType="Float">200</baseSpeed>
            <gameobj dataType="ObjectRef">2889347069</gameobj>
            <gameOverOverlay dataType="Struct" type="Duality.GameObject" id="175463826">
              <active dataType="Bool">false</active>
              <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2786300252">
                <_items dataType="Array" type="Duality.GameObject[]" id="814063812" length="4">
                  <item dataType="Struct" type="Duality.GameObject" id="3286606004">
                    <active dataType="Bool">true</active>
                    <children />
                    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2965659508">
                      <_items dataType="Array" type="Duality.Component[]" id="2094650788" length="4">
                        <item dataType="Struct" type="Duality.Components.Transform" id="1351953640">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">3286606004</gameobj>
                          <ignoreParent dataType="Bool">false</ignoreParent>
                          <parentTransform />
                          <pos dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">0</X>
                            <Y dataType="Float">-150</Y>
                            <Z dataType="Float">0</Z>
                          </pos>
                          <posAbs dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">0</X>
                            <Y dataType="Float">-150</Y>
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
                        <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="734267530">
                          <active dataType="Bool">true</active>
                          <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                          <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                            <A dataType="Byte">255</A>
                            <B dataType="Byte">255</B>
                            <G dataType="Byte">255</G>
                            <R dataType="Byte">255</R>
                          </colorTint>
                          <customMat />
                          <gameobj dataType="ObjectRef">3286606004</gameobj>
                          <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                            <contentPath />
                          </iconMat>
                          <offset dataType="Int">0</offset>
                          <text dataType="Struct" type="Duality.Drawing.FormattedText" id="3790226794">
                            <flowAreas />
                            <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1527026208">
                              <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                                <contentPath dataType="String">Default:Font:GenericMonospace10</contentPath>
                              </item>
                            </fonts>
                            <icons />
                            <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                            <maxHeight dataType="Int">0</maxHeight>
                            <maxWidth dataType="Int">0</maxWidth>
                            <sourceText dataType="String">Game Over</sourceText>
                            <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                          </text>
                          <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                        </item>
                      </_items>
                      <_size dataType="Int">2</_size>
                      <_version dataType="Int">2</_version>
                    </compList>
                    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2670233590" surrogate="true">
                      <header />
                      <body>
                        <keys dataType="Array" type="System.Object[]" id="1914784350">
                          <item dataType="Type" id="3719522576" value="Duality.Components.Transform" />
                          <item dataType="Type" id="3641979118" value="Duality.Components.Renderers.TextRenderer" />
                        </keys>
                        <values dataType="Array" type="System.Object[]" id="1056157962">
                          <item dataType="ObjectRef">1351953640</item>
                          <item dataType="ObjectRef">734267530</item>
                        </values>
                      </body>
                    </compMap>
                    <compTransform dataType="ObjectRef">1351953640</compTransform>
                    <identifier dataType="Struct" type="System.Guid" surrogate="true">
                      <header>
                        <data dataType="Array" type="System.Byte[]" id="2535330734">/ehFYKAOQUiEGa2vHd5HCA==</data>
                      </header>
                      <body />
                    </identifier>
                    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                    <name dataType="String">TextRenderer</name>
                    <parent dataType="ObjectRef">175463826</parent>
                    <prefabLink />
                  </item>
                </_items>
                <_size dataType="Int">1</_size>
                <_version dataType="Int">1</_version>
              </children>
              <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1823935254">
                <_items dataType="Array" type="Duality.Component[]" id="3928282998" length="0" />
                <_size dataType="Int">0</_size>
                <_version dataType="Int">0</_version>
              </compList>
              <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1087021512" surrogate="true">
                <header />
                <body>
                  <keys dataType="Array" type="System.Object[]" id="3085402264" length="0" />
                  <values dataType="Array" type="System.Object[]" id="3217442078" length="0" />
                </body>
              </compMap>
              <compTransform />
              <identifier dataType="Struct" type="System.Guid" surrogate="true">
                <header>
                  <data dataType="Array" type="System.Byte[]" id="1957333700">LBRdsCja7kmOnGAjOqkJUA==</data>
                </header>
                <body />
              </identifier>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
              <name dataType="String">GameOver</name>
              <parent />
              <prefabLink />
            </gameOverOverlay>
            <lastFramePoints dataType="UShort">0</lastFramePoints>
            <obstaclePrefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
              <contentPath dataType="String">Data\Prefabs\Obstacle.Prefab.res</contentPath>
            </obstaclePrefab>
            <player dataType="Struct" type="FlapOrDie.Controllers.PlayerController" id="1655396855">
              <active dataType="Bool">true</active>
              <backWingRenderer dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="3277808975">
                <active dataType="Bool">true</active>
                <animDuration dataType="Float">5</animDuration>
                <animFirstFrame dataType="Int">4</animFirstFrame>
                <animFrameCount dataType="Int">0</animFrameCount>
                <animLoopMode dataType="Enum" type="Duality.Components.Renderers.AnimSpriteRenderer+LoopMode" name="FixedSingle" value="4" />
                <animPaused dataType="Bool">false</animPaused>
                <animTime dataType="Float">0</animTime>
                <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                  <A dataType="Byte">255</A>
                  <B dataType="Byte">255</B>
                  <G dataType="Byte">255</G>
                  <R dataType="Byte">255</R>
                </colorTint>
                <customFrameSequence />
                <customMat />
                <gameobj dataType="Struct" type="Duality.GameObject" id="3570373594">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1079170186">
                    <_items dataType="Array" type="Duality.Component[]" id="4040991712" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1635721230">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">3570373594</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="3366101807">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="Struct" type="Duality.GameObject" id="1005786875">
                            <active dataType="Bool">true</active>
                            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2642241112">
                              <_items dataType="Array" type="Duality.GameObject[]" id="3351539372" length="4">
                                <item dataType="Struct" type="Duality.GameObject" id="1063534982">
                                  <active dataType="Bool">true</active>
                                  <children />
                                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1775518750">
                                    <_items dataType="Array" type="Duality.Component[]" id="602676880" length="4">
                                      <item dataType="Struct" type="Duality.Components.Transform" id="3423849914">
                                        <active dataType="Bool">true</active>
                                        <angle dataType="Float">0</angle>
                                        <angleAbs dataType="Float">0</angleAbs>
                                        <angleVel dataType="Float">0</angleVel>
                                        <angleVelAbs dataType="Float">0</angleVelAbs>
                                        <deriveAngle dataType="Bool">true</deriveAngle>
                                        <gameobj dataType="ObjectRef">1063534982</gameobj>
                                        <ignoreParent dataType="Bool">false</ignoreParent>
                                        <parentTransform dataType="ObjectRef">3366101807</parentTransform>
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
                                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="770970363">
                                        <active dataType="Bool">true</active>
                                        <animDuration dataType="Float">5</animDuration>
                                        <animFirstFrame dataType="Int">0</animFirstFrame>
                                        <animFrameCount dataType="Int">0</animFrameCount>
                                        <animLoopMode dataType="Enum" type="Duality.Components.Renderers.AnimSpriteRenderer+LoopMode" name="Loop" value="1" />
                                        <animPaused dataType="Bool">false</animPaused>
                                        <animTime dataType="Float">0</animTime>
                                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                          <A dataType="Byte">255</A>
                                          <B dataType="Byte">255</B>
                                          <G dataType="Byte">255</G>
                                          <R dataType="Byte">255</R>
                                        </colorTint>
                                        <customFrameSequence />
                                        <customMat />
                                        <gameobj dataType="ObjectRef">1063534982</gameobj>
                                        <offset dataType="Int">0</offset>
                                        <pixelGrid dataType="Bool">false</pixelGrid>
                                        <rect dataType="Struct" type="Duality.Rect">
                                          <H dataType="Float">74.7</H>
                                          <W dataType="Float">64</W>
                                          <X dataType="Float">-32</X>
                                          <Y dataType="Float">-37.4</Y>
                                        </rect>
                                        <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                                        <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                                          <contentPath dataType="String">Data\Graphics\bat-spritesheet.Material.res</contentPath>
                                        </sharedMat>
                                        <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                                      </item>
                                    </_items>
                                    <_size dataType="Int">2</_size>
                                    <_version dataType="Int">6</_version>
                                  </compList>
                                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3765415050" surrogate="true">
                                    <header />
                                    <body>
                                      <keys dataType="Array" type="System.Object[]" id="871838780">
                                        <item dataType="ObjectRef">3719522576</item>
                                        <item dataType="Type" id="2121682244" value="Duality.Components.Renderers.AnimSpriteRenderer" />
                                      </keys>
                                      <values dataType="Array" type="System.Object[]" id="539866518">
                                        <item dataType="ObjectRef">3423849914</item>
                                        <item dataType="ObjectRef">770970363</item>
                                      </values>
                                    </body>
                                  </compMap>
                                  <compTransform dataType="ObjectRef">3423849914</compTransform>
                                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                    <header>
                                      <data dataType="Array" type="System.Byte[]" id="2209354984">EghhVQXcQEmesrZO9dIx1A==</data>
                                    </header>
                                    <body />
                                  </identifier>
                                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                  <name dataType="String">Body</name>
                                  <parent dataType="ObjectRef">1005786875</parent>
                                  <prefabLink />
                                </item>
                                <item dataType="Struct" type="Duality.GameObject" id="85871491">
                                  <active dataType="Bool">true</active>
                                  <children />
                                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1806299983">
                                    <_items dataType="Array" type="Duality.Component[]" id="1587484206" length="4">
                                      <item dataType="Struct" type="Duality.Components.Transform" id="2446186423">
                                        <active dataType="Bool">true</active>
                                        <angle dataType="Float">0</angle>
                                        <angleAbs dataType="Float">0</angleAbs>
                                        <angleVel dataType="Float">0</angleVel>
                                        <angleVelAbs dataType="Float">0</angleVelAbs>
                                        <deriveAngle dataType="Bool">true</deriveAngle>
                                        <gameobj dataType="ObjectRef">85871491</gameobj>
                                        <ignoreParent dataType="Bool">false</ignoreParent>
                                        <parentTransform dataType="ObjectRef">3366101807</parentTransform>
                                        <pos dataType="Struct" type="Duality.Vector3">
                                          <X dataType="Float">-25</X>
                                          <Y dataType="Float">0</Y>
                                          <Z dataType="Float">0</Z>
                                        </pos>
                                        <posAbs dataType="Struct" type="Duality.Vector3">
                                          <X dataType="Float">-25</X>
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
                                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="4088274168">
                                        <active dataType="Bool">true</active>
                                        <animDuration dataType="Float">5</animDuration>
                                        <animFirstFrame dataType="Int">3</animFirstFrame>
                                        <animFrameCount dataType="Int">0</animFrameCount>
                                        <animLoopMode dataType="Enum" type="Duality.Components.Renderers.AnimSpriteRenderer+LoopMode" name="FixedSingle" value="4" />
                                        <animPaused dataType="Bool">false</animPaused>
                                        <animTime dataType="Float">0</animTime>
                                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                          <A dataType="Byte">255</A>
                                          <B dataType="Byte">255</B>
                                          <G dataType="Byte">255</G>
                                          <R dataType="Byte">255</R>
                                        </colorTint>
                                        <customFrameSequence />
                                        <customMat />
                                        <gameobj dataType="ObjectRef">85871491</gameobj>
                                        <offset dataType="Int">-1</offset>
                                        <pixelGrid dataType="Bool">false</pixelGrid>
                                        <rect dataType="Struct" type="Duality.Rect">
                                          <H dataType="Float">69.3</H>
                                          <W dataType="Float">83.5</W>
                                          <X dataType="Float">-45</X>
                                          <Y dataType="Float">-65</Y>
                                        </rect>
                                        <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                                        <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                                          <contentPath dataType="String">Data\Graphics\bat-spritesheet.Material.res</contentPath>
                                        </sharedMat>
                                        <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                                      </item>
                                    </_items>
                                    <_size dataType="Int">2</_size>
                                    <_version dataType="Int">2</_version>
                                  </compList>
                                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2441463904" surrogate="true">
                                    <header />
                                    <body>
                                      <keys dataType="Array" type="System.Object[]" id="1880316517">
                                        <item dataType="ObjectRef">3719522576</item>
                                        <item dataType="ObjectRef">2121682244</item>
                                      </keys>
                                      <values dataType="Array" type="System.Object[]" id="449654376">
                                        <item dataType="ObjectRef">2446186423</item>
                                        <item dataType="ObjectRef">4088274168</item>
                                      </values>
                                    </body>
                                  </compMap>
                                  <compTransform dataType="ObjectRef">2446186423</compTransform>
                                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                    <header>
                                      <data dataType="Array" type="System.Byte[]" id="2597496239">7wbewMCLr02MUk6BmSwnww==</data>
                                    </header>
                                    <body />
                                  </identifier>
                                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                  <name dataType="String">FrontWing</name>
                                  <parent dataType="ObjectRef">1005786875</parent>
                                  <prefabLink />
                                </item>
                                <item dataType="ObjectRef">3570373594</item>
                              </_items>
                              <_size dataType="Int">3</_size>
                              <_version dataType="Int">3</_version>
                            </children>
                            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="698343326">
                              <_items dataType="Array" type="Duality.Component[]" id="2519349018" length="4">
                                <item dataType="ObjectRef">3366101807</item>
                                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="4068563399">
                                  <active dataType="Bool">true</active>
                                  <angularDamp dataType="Float">0.3</angularDamp>
                                  <angularVel dataType="Float">0</angularVel>
                                  <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                                  <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                                  <colFilter />
                                  <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                                  <continous dataType="Bool">false</continous>
                                  <explicitMass dataType="Float">50</explicitMass>
                                  <fixedAngle dataType="Bool">false</fixedAngle>
                                  <gameobj dataType="ObjectRef">1005786875</gameobj>
                                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                                  <joints />
                                  <linearDamp dataType="Float">0.3</linearDamp>
                                  <linearVel dataType="Struct" type="Duality.Vector2">
                                    <X dataType="Float">0</X>
                                    <Y dataType="Float">0</Y>
                                  </linearVel>
                                  <revolutions dataType="Float">0</revolutions>
                                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3368523591">
                                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1974326990" length="4">
                                      <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="120064976">
                                        <density dataType="Float">1</density>
                                        <friction dataType="Float">0.3</friction>
                                        <parent dataType="ObjectRef">4068563399</parent>
                                        <position dataType="Struct" type="Duality.Vector2">
                                          <X dataType="Float">0</X>
                                          <Y dataType="Float">5.35</Y>
                                        </position>
                                        <radius dataType="Float">32</radius>
                                        <restitution dataType="Float">0.3</restitution>
                                        <sensor dataType="Bool">false</sensor>
                                      </item>
                                    </_items>
                                    <_size dataType="Int">1</_size>
                                    <_version dataType="Int">3</_version>
                                  </shapes>
                                </item>
                                <item dataType="ObjectRef">1655396855</item>
                              </_items>
                              <_size dataType="Int">3</_size>
                              <_version dataType="Int">3</_version>
                            </compList>
                            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="531704068" surrogate="true">
                              <header />
                              <body>
                                <keys dataType="Array" type="System.Object[]" id="2062621032">
                                  <item dataType="ObjectRef">3719522576</item>
                                  <item dataType="Type" id="1410733612" value="Duality.Components.Physics.RigidBody" />
                                  <item dataType="Type" id="3434902454" value="FlapOrDie.Controllers.PlayerController" />
                                </keys>
                                <values dataType="Array" type="System.Object[]" id="1638428958">
                                  <item dataType="ObjectRef">3366101807</item>
                                  <item dataType="ObjectRef">4068563399</item>
                                  <item dataType="ObjectRef">1655396855</item>
                                </values>
                              </body>
                            </compMap>
                            <compTransform dataType="ObjectRef">3366101807</compTransform>
                            <identifier dataType="Struct" type="System.Guid" surrogate="true">
                              <header>
                                <data dataType="Array" type="System.Byte[]" id="844707796">qVPdSS7l4EWxApeL87Drsg==</data>
                              </header>
                              <body />
                            </identifier>
                            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                            <name dataType="String">Bat</name>
                            <parent />
                            <prefabLink />
                          </gameobj>
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
                          <X dataType="Float">22</X>
                          <Y dataType="Float">-5</Y>
                          <Z dataType="Float">0</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">22</X>
                          <Y dataType="Float">-5</Y>
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
                      <item dataType="ObjectRef">3277808975</item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2854606106" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="126517104">
                        <item dataType="ObjectRef">3719522576</item>
                        <item dataType="ObjectRef">2121682244</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="128497390">
                        <item dataType="ObjectRef">1635721230</item>
                        <item dataType="ObjectRef">3277808975</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1635721230</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2784368844">zdhNtoL4x0u/JJrNtrS90A==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">BackWing</name>
                  <parent dataType="ObjectRef">1005786875</parent>
                  <prefabLink />
                </gameobj>
                <offset dataType="Int">1</offset>
                <pixelGrid dataType="Bool">false</pixelGrid>
                <rect dataType="Struct" type="Duality.Rect">
                  <H dataType="Float">67.3</H>
                  <W dataType="Float">61</W>
                  <X dataType="Float">-10</X>
                  <Y dataType="Float">-50</Y>
                </rect>
                <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                  <contentPath dataType="String">Data\Graphics\bat-spritesheet.Material.res</contentPath>
                </sharedMat>
                <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
              </backWingRenderer>
              <bodyRenderer dataType="ObjectRef">770970363</bodyRenderer>
              <frontWingRenderer dataType="ObjectRef">4088274168</frontWingRenderer>
              <gameobj dataType="ObjectRef">1005786875</gameobj>
              <impulseStrength dataType="Float">400</impulseStrength>
            </player>
            <pointsGapVariance dataType="Float">5</pointsGapVariance>
            <pointsMultiplier dataType="Float">10</pointsMultiplier>
            <scoreText dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="334788617">
              <active dataType="Bool">true</active>
              <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
              <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                <A dataType="Byte">255</A>
                <B dataType="Byte">255</B>
                <G dataType="Byte">255</G>
                <R dataType="Byte">255</R>
              </colorTint>
              <customMat />
              <gameobj dataType="Struct" type="Duality.GameObject" id="2887127091">
                <active dataType="Bool">true</active>
                <children />
                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3752773070">
                  <_items dataType="Array" type="Duality.Component[]" id="2790422992" length="4">
                    <item dataType="Struct" type="Duality.Components.Transform" id="952474727">
                      <active dataType="Bool">true</active>
                      <angle dataType="Float">0</angle>
                      <angleAbs dataType="Float">0</angleAbs>
                      <angleVel dataType="Float">0</angleVel>
                      <angleVelAbs dataType="Float">0</angleVelAbs>
                      <deriveAngle dataType="Bool">true</deriveAngle>
                      <gameobj dataType="ObjectRef">2887127091</gameobj>
                      <ignoreParent dataType="Bool">false</ignoreParent>
                      <parentTransform />
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
                    <item dataType="ObjectRef">334788617</item>
                  </_items>
                  <_size dataType="Int">2</_size>
                  <_version dataType="Int">2</_version>
                </compList>
                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3769658698" surrogate="true">
                  <header />
                  <body>
                    <keys dataType="Array" type="System.Object[]" id="2848595084">
                      <item dataType="ObjectRef">3719522576</item>
                      <item dataType="ObjectRef">3641979118</item>
                    </keys>
                    <values dataType="Array" type="System.Object[]" id="2508105718">
                      <item dataType="ObjectRef">952474727</item>
                      <item dataType="ObjectRef">334788617</item>
                    </values>
                  </body>
                </compMap>
                <compTransform dataType="ObjectRef">952474727</compTransform>
                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                  <header>
                    <data dataType="Array" type="System.Byte[]" id="4163636248">NlKxPKf6aUKaKmrYrXrbbA==</data>
                  </header>
                  <body />
                </identifier>
                <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                <name dataType="String">TextRenderer</name>
                <parent />
                <prefabLink />
              </gameobj>
              <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                <contentPath />
              </iconMat>
              <offset dataType="Int">0</offset>
              <text dataType="Struct" type="Duality.Drawing.FormattedText" id="227529899">
                <flowAreas />
                <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="175878390">
                  <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                    <contentPath dataType="String">Default:Font:GenericMonospace10</contentPath>
                  </item>
                </fonts>
                <icons />
                <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                <maxHeight dataType="Int">0</maxHeight>
                <maxWidth dataType="Int">100</maxWidth>
                <sourceText dataType="String">Score: 0</sourceText>
                <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
              </text>
              <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0, AllFlags" value="2147483649" />
            </scoreText>
          </item>
        </_items>
        <_size dataType="Int">4</_size>
        <_version dataType="Int">6</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="807789664" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="445595493">
            <item dataType="ObjectRef">3719522576</item>
            <item dataType="Type" id="201365398" value="Duality.Components.Camera" />
            <item dataType="Type" id="697601754" value="Duality.Components.SoundListener" />
            <item dataType="Type" id="230444086" value="FlapOrDie.Controllers.GameController" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="3720147560">
            <item dataType="ObjectRef">954694705</item>
            <item dataType="ObjectRef">3426622876</item>
            <item dataType="ObjectRef">3542828440</item>
            <item dataType="ObjectRef">4146559154</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">954694705</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3748039855">Qa2lQ5IjvUKsNsxNzIt5AQ==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">MainCamera</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">175463826</item>
    <item dataType="ObjectRef">2887127091</item>
    <item dataType="ObjectRef">1005786875</item>
    <item dataType="ObjectRef">3286606004</item>
    <item dataType="ObjectRef">1063534982</item>
    <item dataType="ObjectRef">85871491</item>
    <item dataType="ObjectRef">3570373594</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
