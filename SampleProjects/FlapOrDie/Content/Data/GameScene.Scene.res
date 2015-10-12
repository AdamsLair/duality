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
            <player dataType="Struct" type="FlapOrDie.Controllers.PlayerController" id="2159651102">
              <active dataType="Bool">true</active>
              <gameobj dataType="Struct" type="Duality.GameObject" id="1510041122">
                <active dataType="Bool">true</active>
                <children />
                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3628695662">
                  <_items dataType="Array" type="Duality.Component[]" id="876090960">
                    <item dataType="Struct" type="Duality.Components.Transform" id="3870356054">
                      <active dataType="Bool">true</active>
                      <angle dataType="Float">0</angle>
                      <angleAbs dataType="Float">0</angleAbs>
                      <angleVel dataType="Float">0</angleVel>
                      <angleVelAbs dataType="Float">0</angleVelAbs>
                      <deriveAngle dataType="Bool">true</deriveAngle>
                      <gameobj dataType="ObjectRef">1510041122</gameobj>
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
                    <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3152207690">
                      <active dataType="Bool">true</active>
                      <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                        <A dataType="Byte">255</A>
                        <B dataType="Byte">255</B>
                        <G dataType="Byte">255</G>
                        <R dataType="Byte">255</R>
                      </colorTint>
                      <customMat />
                      <gameobj dataType="ObjectRef">1510041122</gameobj>
                      <offset dataType="Int">0</offset>
                      <pixelGrid dataType="Bool">false</pixelGrid>
                      <rect dataType="Struct" type="Duality.Rect">
                        <H dataType="Float">64</H>
                        <W dataType="Float">64</W>
                        <X dataType="Float">-32</X>
                        <Y dataType="Float">-32</Y>
                      </rect>
                      <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                      <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                        <contentPath dataType="String">Default:Material:DualityIcon</contentPath>
                      </sharedMat>
                      <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                    </item>
                    <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="277850350">
                      <active dataType="Bool">true</active>
                      <angularDamp dataType="Float">0.3</angularDamp>
                      <angularVel dataType="Float">0</angularVel>
                      <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                      <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                      <colFilter />
                      <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                      <continous dataType="Bool">false</continous>
                      <explicitMass dataType="Float">0</explicitMass>
                      <fixedAngle dataType="Bool">false</fixedAngle>
                      <gameobj dataType="ObjectRef">1510041122</gameobj>
                      <ignoreGravity dataType="Bool">false</ignoreGravity>
                      <joints />
                      <linearDamp dataType="Float">0.3</linearDamp>
                      <linearVel dataType="Struct" type="Duality.Vector2">
                        <X dataType="Float">0</X>
                        <Y dataType="Float">0</Y>
                      </linearVel>
                      <revolutions dataType="Float">0</revolutions>
                      <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3772774990">
                        <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3424947920" length="4">
                          <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="3313944252">
                            <density dataType="Float">1</density>
                            <friction dataType="Float">0.3</friction>
                            <parent dataType="ObjectRef">277850350</parent>
                            <position dataType="Struct" type="Duality.Vector2">
                              <X dataType="Float">0</X>
                              <Y dataType="Float">0</Y>
                            </position>
                            <radius dataType="Float">32</radius>
                            <restitution dataType="Float">0.3</restitution>
                            <sensor dataType="Bool">false</sensor>
                          </item>
                        </_items>
                        <_size dataType="Int">1</_size>
                        <_version dataType="Int">1</_version>
                      </shapes>
                    </item>
                    <item dataType="ObjectRef">2159651102</item>
                  </_items>
                  <_size dataType="Int">4</_size>
                  <_version dataType="Int">6</_version>
                </compList>
                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2870587338" surrogate="true">
                  <header />
                  <body>
                    <keys dataType="Array" type="System.Object[]" id="4244772332">
                      <item dataType="ObjectRef">3719522576</item>
                      <item dataType="Type" id="1043538532" value="Duality.Components.Renderers.SpriteRenderer" />
                      <item dataType="Type" id="2512869398" value="Duality.Components.Physics.RigidBody" />
                      <item dataType="Type" id="103098208" value="FlapOrDie.Controllers.PlayerController" />
                    </keys>
                    <values dataType="Array" type="System.Object[]" id="3769831222">
                      <item dataType="ObjectRef">3870356054</item>
                      <item dataType="ObjectRef">3152207690</item>
                      <item dataType="ObjectRef">277850350</item>
                      <item dataType="ObjectRef">2159651102</item>
                    </values>
                  </body>
                </compMap>
                <compTransform dataType="ObjectRef">3870356054</compTransform>
                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                  <header>
                    <data dataType="Array" type="System.Byte[]" id="872765624">ZyEjfiKU10e9qrD5LCC/kg==</data>
                  </header>
                  <body />
                </identifier>
                <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                <name dataType="String">Player</name>
                <parent />
                <prefabLink />
              </gameobj>
              <impulseStrength dataType="Float">250</impulseStrength>
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
    <item dataType="ObjectRef">1510041122</item>
    <item dataType="ObjectRef">175463826</item>
    <item dataType="ObjectRef">2887127091</item>
    <item dataType="ObjectRef">3286606004</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
