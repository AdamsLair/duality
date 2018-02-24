<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">33</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="1610950862">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1929410264">
        <_items dataType="Array" type="Duality.Component[]" id="311433132" length="8">
          <item dataType="Struct" type="Duality.Components.Transform" id="1668228080">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <gameobj dataType="ObjectRef">1610950862</gameobj>
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
          <item dataType="Struct" type="Duality.Components.Camera" id="3157337339">
            <active dataType="Bool">true</active>
            <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">196</B>
              <G dataType="Byte">192</G>
              <R dataType="Byte">162</R>
            </clearColor>
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">1610950862</gameobj>
            <nearZ dataType="Float">50</nearZ>
            <priority dataType="Int">0</priority>
            <projection dataType="Enum" type="Duality.Drawing.ProjectionMode" name="Perspective" value="1" />
            <renderSetup dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderSetup]]" />
            <renderTarget dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]" />
            <shaderParameters dataType="Struct" type="Duality.Drawing.ShaderParameterCollection" id="874680055" custom="true">
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
          <item dataType="Struct" type="Duality.Components.VelocityTracker" id="3682085329">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1610950862</gameobj>
          </item>
          <item dataType="Struct" type="Duality.Components.SoundListener" id="3643603389">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1610950862</gameobj>
          </item>
          <item dataType="Struct" type="FlapOrDie.Controllers.GameController" id="3273908911">
            <active dataType="Bool">true</active>
            <baseSpeed dataType="Float">200</baseSpeed>
            <bgScroller dataType="Struct" type="FlapOrDie.Components.BackgroundScroller" id="715599476">
              <active dataType="Bool">true</active>
              <back dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3002226874">
                <active dataType="Bool">true</active>
                <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                  <A dataType="Byte">255</A>
                  <B dataType="Byte">255</B>
                  <G dataType="Byte">255</G>
                  <R dataType="Byte">255</R>
                </colorTint>
                <customMat />
                <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                <gameobj dataType="Struct" type="Duality.GameObject" id="1533607594">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="856359941">
                    <_items dataType="Array" type="Duality.Component[]" id="2712121942" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1590884812">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <gameobj dataType="ObjectRef">1533607594</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">0</Y>
                          <Z dataType="Float">10</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">0</Y>
                          <Z dataType="Float">10</Z>
                        </posAbs>
                        <scale dataType="Float">1</scale>
                        <scaleAbs dataType="Float">1</scaleAbs>
                      </item>
                      <item dataType="ObjectRef">3002226874</item>
                    </_items>
                    <_size dataType="Int">2</_size>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3790913448" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="4068788207">
                        <item dataType="Type" id="3812992238" value="Duality.Components.Transform" />
                        <item dataType="Type" id="1309696458" value="Duality.Components.Renderers.SpriteRenderer" />
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="3854137760">
                        <item dataType="ObjectRef">1590884812</item>
                        <item dataType="ObjectRef">3002226874</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1590884812</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1976187517">xgbYFACqbEqsFx3CUxwdOA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Back</name>
                  <parent dataType="Struct" type="Duality.GameObject" id="291443106">
                    <active dataType="Bool">true</active>
                    <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1738984277">
                      <_items dataType="Array" type="Duality.GameObject[]" id="950546678" length="4">
                        <item dataType="ObjectRef">1533607594</item>
                        <item dataType="Struct" type="Duality.GameObject" id="467284875">
                          <active dataType="Bool">true</active>
                          <children />
                          <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="790096379">
                            <_items dataType="Array" type="Duality.Component[]" id="4024322646" length="4">
                              <item dataType="Struct" type="Duality.Components.Transform" id="524562093">
                                <active dataType="Bool">true</active>
                                <angle dataType="Float">0</angle>
                                <angleAbs dataType="Float">0</angleAbs>
                                <gameobj dataType="ObjectRef">467284875</gameobj>
                                <ignoreParent dataType="Bool">false</ignoreParent>
                                <pos dataType="Struct" type="Duality.Vector3">
                                  <X dataType="Float">0</X>
                                  <Y dataType="Float">0</Y>
                                  <Z dataType="Float">5</Z>
                                </pos>
                                <posAbs dataType="Struct" type="Duality.Vector3">
                                  <X dataType="Float">0</X>
                                  <Y dataType="Float">0</Y>
                                  <Z dataType="Float">5</Z>
                                </posAbs>
                                <scale dataType="Float">1</scale>
                                <scaleAbs dataType="Float">1</scaleAbs>
                              </item>
                              <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1935904155">
                                <active dataType="Bool">true</active>
                                <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                  <A dataType="Byte">255</A>
                                  <B dataType="Byte">255</B>
                                  <G dataType="Byte">255</G>
                                  <R dataType="Byte">255</R>
                                </colorTint>
                                <customMat />
                                <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                                <gameobj dataType="ObjectRef">467284875</gameobj>
                                <offset dataType="Float">0</offset>
                                <pixelGrid dataType="Bool">false</pixelGrid>
                                <rect dataType="Struct" type="Duality.Rect">
                                  <H dataType="Float">600</H>
                                  <W dataType="Float">4800</W>
                                  <X dataType="Float">-1200</X>
                                  <Y dataType="Float">-300</Y>
                                </rect>
                                <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="WrapHorizontal" value="1" />
                                <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                                  <contentPath dataType="String">Data\FlapOrDie\Graphics\bg-middle.Material.res</contentPath>
                                </sharedMat>
                                <spriteIndex dataType="Int">-1</spriteIndex>
                                <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                              </item>
                            </_items>
                            <_size dataType="Int">2</_size>
                          </compList>
                          <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1711716264" surrogate="true">
                            <header />
                            <body>
                              <keys dataType="Array" type="System.Object[]" id="407641617">
                                <item dataType="ObjectRef">3812992238</item>
                                <item dataType="ObjectRef">1309696458</item>
                              </keys>
                              <values dataType="Array" type="System.Object[]" id="1831328">
                                <item dataType="ObjectRef">524562093</item>
                                <item dataType="ObjectRef">1935904155</item>
                              </values>
                            </body>
                          </compMap>
                          <compTransform dataType="ObjectRef">524562093</compTransform>
                          <identifier dataType="Struct" type="System.Guid" surrogate="true">
                            <header>
                              <data dataType="Array" type="System.Byte[]" id="4219677571">5DVHFtOWaEOz2idZCToHyg==</data>
                            </header>
                            <body />
                          </identifier>
                          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                          <name dataType="String">Middle</name>
                          <parent dataType="ObjectRef">291443106</parent>
                          <prefabLink />
                        </item>
                        <item dataType="Struct" type="Duality.GameObject" id="3876724628">
                          <active dataType="Bool">true</active>
                          <children />
                          <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2729154704">
                            <_items dataType="Array" type="Duality.Component[]" id="2113008956" length="4">
                              <item dataType="Struct" type="Duality.Components.Transform" id="3934001846">
                                <active dataType="Bool">true</active>
                                <angle dataType="Float">0</angle>
                                <angleAbs dataType="Float">0</angleAbs>
                                <gameobj dataType="ObjectRef">3876724628</gameobj>
                                <ignoreParent dataType="Bool">false</ignoreParent>
                                <pos dataType="Struct" type="Duality.Vector3" />
                                <posAbs dataType="Struct" type="Duality.Vector3" />
                                <scale dataType="Float">1</scale>
                                <scaleAbs dataType="Float">1</scaleAbs>
                              </item>
                              <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1050376612">
                                <active dataType="Bool">true</active>
                                <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                  <A dataType="Byte">255</A>
                                  <B dataType="Byte">255</B>
                                  <G dataType="Byte">255</G>
                                  <R dataType="Byte">255</R>
                                </colorTint>
                                <customMat />
                                <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                                <gameobj dataType="ObjectRef">3876724628</gameobj>
                                <offset dataType="Float">5</offset>
                                <pixelGrid dataType="Bool">false</pixelGrid>
                                <rect dataType="Struct" type="Duality.Rect">
                                  <H dataType="Float">600</H>
                                  <W dataType="Float">4800</W>
                                  <X dataType="Float">-1200</X>
                                  <Y dataType="Float">-300</Y>
                                </rect>
                                <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="WrapHorizontal" value="1" />
                                <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                                  <contentPath dataType="String">Data\FlapOrDie\Graphics\bg-front.Material.res</contentPath>
                                </sharedMat>
                                <spriteIndex dataType="Int">-1</spriteIndex>
                                <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                              </item>
                            </_items>
                            <_size dataType="Int">2</_size>
                          </compList>
                          <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1686636270" surrogate="true">
                            <header />
                            <body>
                              <keys dataType="Array" type="System.Object[]" id="685247714">
                                <item dataType="ObjectRef">3812992238</item>
                                <item dataType="ObjectRef">1309696458</item>
                              </keys>
                              <values dataType="Array" type="System.Object[]" id="466472586">
                                <item dataType="ObjectRef">3934001846</item>
                                <item dataType="ObjectRef">1050376612</item>
                              </values>
                            </body>
                          </compMap>
                          <compTransform dataType="ObjectRef">3934001846</compTransform>
                          <identifier dataType="Struct" type="System.Guid" surrogate="true">
                            <header>
                              <data dataType="Array" type="System.Byte[]" id="2669395730">JyMdGLleSkKzywsyWPXWCA==</data>
                            </header>
                            <body />
                          </identifier>
                          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                          <name dataType="String">Front</name>
                          <parent dataType="ObjectRef">291443106</parent>
                          <prefabLink />
                        </item>
                      </_items>
                      <_size dataType="Int">3</_size>
                    </children>
                    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3104149832">
                      <_items dataType="Array" type="Duality.Component[]" id="4047188095" length="4">
                        <item dataType="ObjectRef">715599476</item>
                      </_items>
                      <_size dataType="Int">1</_size>
                    </compList>
                    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2812769631" surrogate="true">
                      <header />
                      <body>
                        <keys dataType="Array" type="System.Object[]" id="409680388">
                          <item dataType="Type" id="825125700" value="FlapOrDie.Components.BackgroundScroller" />
                        </keys>
                        <values dataType="Array" type="System.Object[]" id="2326949782">
                          <item dataType="ObjectRef">715599476</item>
                        </values>
                      </body>
                    </compMap>
                    <compTransform />
                    <identifier dataType="Struct" type="System.Guid" surrogate="true">
                      <header>
                        <data dataType="Array" type="System.Byte[]" id="4000520896">oblGw29aBkC6vDyEhownLQ==</data>
                      </header>
                      <body />
                    </identifier>
                    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                    <name dataType="String">Background</name>
                    <parent />
                    <prefabLink />
                  </parent>
                  <prefabLink />
                </gameobj>
                <offset dataType="Float">0</offset>
                <pixelGrid dataType="Bool">false</pixelGrid>
                <rect dataType="Struct" type="Duality.Rect">
                  <H dataType="Float">600</H>
                  <W dataType="Float">4800</W>
                  <X dataType="Float">-1200</X>
                  <Y dataType="Float">-300</Y>
                </rect>
                <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="WrapHorizontal" value="1" />
                <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                  <contentPath dataType="String">Data\FlapOrDie\Graphics\bg-back.Material.res</contentPath>
                </sharedMat>
                <spriteIndex dataType="Int">-1</spriteIndex>
                <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
              </back>
              <front dataType="ObjectRef">1050376612</front>
              <gameobj dataType="ObjectRef">291443106</gameobj>
              <middle dataType="ObjectRef">1935904155</middle>
              <speed dataType="Float">0</speed>
            </bgScroller>
            <gameobj dataType="ObjectRef">1610950862</gameobj>
            <gameOverOverlay dataType="Struct" type="Duality.GameObject" id="4097417655">
              <active dataType="Bool">false</active>
              <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3987494042">
                <_items dataType="Array" type="Duality.GameObject[]" id="3997054848" length="4">
                  <item dataType="Struct" type="Duality.GameObject" id="1364571484">
                    <active dataType="Bool">true</active>
                    <children />
                    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="460458244">
                      <_items dataType="Array" type="Duality.Component[]" id="2011723076" length="4">
                        <item dataType="Struct" type="Duality.Components.Transform" id="1421848702">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <gameobj dataType="ObjectRef">1364571484</gameobj>
                          <ignoreParent dataType="Bool">false</ignoreParent>
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
                        </item>
                        <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2833190764">
                          <active dataType="Bool">true</active>
                          <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                            <A dataType="Byte">255</A>
                            <B dataType="Byte">255</B>
                            <G dataType="Byte">255</G>
                            <R dataType="Byte">255</R>
                          </colorTint>
                          <customMat />
                          <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                          <gameobj dataType="ObjectRef">1364571484</gameobj>
                          <offset dataType="Float">0</offset>
                          <pixelGrid dataType="Bool">false</pixelGrid>
                          <rect dataType="Struct" type="Duality.Rect">
                            <H dataType="Float">72</H>
                            <W dataType="Float">356</W>
                            <X dataType="Float">-178</X>
                            <Y dataType="Float">-36</Y>
                          </rect>
                          <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                          <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                            <contentPath dataType="String">Data\FlapOrDie\Graphics\game-over.Material.res</contentPath>
                          </sharedMat>
                          <spriteIndex dataType="Int">-1</spriteIndex>
                          <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                        </item>
                      </_items>
                      <_size dataType="Int">2</_size>
                    </compList>
                    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3857612182" surrogate="true">
                      <header />
                      <body>
                        <keys dataType="Array" type="System.Object[]" id="1530886670">
                          <item dataType="ObjectRef">3812992238</item>
                          <item dataType="ObjectRef">1309696458</item>
                        </keys>
                        <values dataType="Array" type="System.Object[]" id="3538612042">
                          <item dataType="ObjectRef">1421848702</item>
                          <item dataType="ObjectRef">2833190764</item>
                        </values>
                      </body>
                    </compMap>
                    <compTransform dataType="ObjectRef">1421848702</compTransform>
                    <identifier dataType="Struct" type="System.Guid" surrogate="true">
                      <header>
                        <data dataType="Array" type="System.Byte[]" id="4017483070">/ehFYKAOQUiEGa2vHd5HCA==</data>
                      </header>
                      <body />
                    </identifier>
                    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                    <name dataType="String">TextRenderer</name>
                    <parent dataType="ObjectRef">4097417655</parent>
                    <prefabLink />
                  </item>
                </_items>
                <_size dataType="Int">1</_size>
              </children>
              <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="533919546">
                <_items dataType="Array" type="Duality.Component[]" id="95825632" length="0" />
                <_size dataType="Int">0</_size>
              </compList>
              <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3455749146" surrogate="true">
                <header />
                <body>
                  <keys dataType="Array" type="System.Object[]" id="3319320320" length="0" />
                  <values dataType="Array" type="System.Object[]" id="3054238158" length="0" />
                </body>
              </compMap>
              <compTransform />
              <identifier dataType="Struct" type="System.Guid" surrogate="true">
                <header>
                  <data dataType="Array" type="System.Byte[]" id="3432221084">LBRdsCja7kmOnGAjOqkJUA==</data>
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
              <contentPath dataType="String">Data\FlapOrDie\Prefabs\Obstacle.Prefab.res</contentPath>
            </obstaclePrefab>
            <player dataType="Struct" type="FlapOrDie.Controllers.PlayerController" id="828926909">
              <active dataType="Bool">true</active>
              <backWingRenderer dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="143000801">
                <active dataType="Bool">true</active>
                <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                  <A dataType="Byte">255</A>
                  <B dataType="Byte">255</B>
                  <G dataType="Byte">255</G>
                  <R dataType="Byte">255</R>
                </colorTint>
                <customMat />
                <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                <gameobj dataType="Struct" type="Duality.GameObject" id="2969348817">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="388370614">
                    <_items dataType="Array" type="Duality.Component[]" id="1869889376" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="3026626035">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <gameobj dataType="ObjectRef">2969348817</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
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
                      </item>
                      <item dataType="ObjectRef">143000801</item>
                    </_items>
                    <_size dataType="Int">2</_size>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3708596890" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="3146641796">
                        <item dataType="ObjectRef">3812992238</item>
                        <item dataType="ObjectRef">1309696458</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="3949735062">
                        <item dataType="ObjectRef">3026626035</item>
                        <item dataType="ObjectRef">143000801</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">3026626035</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1386184512">zdhNtoL4x0u/JJrNtrS90A==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">BackWing</name>
                  <parent dataType="Struct" type="Duality.GameObject" id="4252430015">
                    <active dataType="Bool">true</active>
                    <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1151730173">
                      <_items dataType="Array" type="Duality.GameObject[]" id="4004555046" length="4">
                        <item dataType="Struct" type="Duality.GameObject" id="4081692434">
                          <active dataType="Bool">true</active>
                          <children />
                          <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2505502566">
                            <_items dataType="Array" type="Duality.Component[]" id="278587264" length="4">
                              <item dataType="Struct" type="Duality.Components.Transform" id="4138969652">
                                <active dataType="Bool">true</active>
                                <angle dataType="Float">0</angle>
                                <angleAbs dataType="Float">0</angleAbs>
                                <gameobj dataType="ObjectRef">4081692434</gameobj>
                                <ignoreParent dataType="Bool">false</ignoreParent>
                                <pos dataType="Struct" type="Duality.Vector3" />
                                <posAbs dataType="Struct" type="Duality.Vector3" />
                                <scale dataType="Float">1</scale>
                                <scaleAbs dataType="Float">1</scaleAbs>
                              </item>
                              <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1255344418">
                                <active dataType="Bool">true</active>
                                <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                  <A dataType="Byte">255</A>
                                  <B dataType="Byte">255</B>
                                  <G dataType="Byte">255</G>
                                  <R dataType="Byte">255</R>
                                </colorTint>
                                <customMat />
                                <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                                <gameobj dataType="ObjectRef">4081692434</gameobj>
                                <offset dataType="Float">0</offset>
                                <pixelGrid dataType="Bool">false</pixelGrid>
                                <rect dataType="Struct" type="Duality.Rect">
                                  <H dataType="Float">74.7</H>
                                  <W dataType="Float">64</W>
                                  <X dataType="Float">-32</X>
                                  <Y dataType="Float">-37.4</Y>
                                </rect>
                                <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                                <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                                  <contentPath dataType="String">Data\FlapOrDie\Graphics\bat-spritesheet.Material.res</contentPath>
                                </sharedMat>
                                <spriteIndex dataType="Int">0</spriteIndex>
                                <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                              </item>
                            </_items>
                            <_size dataType="Int">2</_size>
                          </compList>
                          <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3270723386" surrogate="true">
                            <header />
                            <body>
                              <keys dataType="Array" type="System.Object[]" id="1912418004">
                                <item dataType="ObjectRef">3812992238</item>
                                <item dataType="ObjectRef">1309696458</item>
                              </keys>
                              <values dataType="Array" type="System.Object[]" id="2294684086">
                                <item dataType="ObjectRef">4138969652</item>
                                <item dataType="ObjectRef">1255344418</item>
                              </values>
                            </body>
                          </compMap>
                          <compTransform dataType="ObjectRef">4138969652</compTransform>
                          <identifier dataType="Struct" type="System.Guid" surrogate="true">
                            <header>
                              <data dataType="Array" type="System.Byte[]" id="3615487984">EghhVQXcQEmesrZO9dIx1A==</data>
                            </header>
                            <body />
                          </identifier>
                          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                          <name dataType="String">Body</name>
                          <parent dataType="ObjectRef">4252430015</parent>
                          <prefabLink />
                        </item>
                        <item dataType="Struct" type="Duality.GameObject" id="1431324502">
                          <active dataType="Bool">true</active>
                          <children />
                          <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1986561690">
                            <_items dataType="Array" type="Duality.Component[]" id="2342298496" length="4">
                              <item dataType="Struct" type="Duality.Components.Transform" id="1488601720">
                                <active dataType="Bool">true</active>
                                <angle dataType="Float">0</angle>
                                <angleAbs dataType="Float">0</angleAbs>
                                <gameobj dataType="ObjectRef">1431324502</gameobj>
                                <ignoreParent dataType="Bool">false</ignoreParent>
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
                              </item>
                              <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2899943782">
                                <active dataType="Bool">true</active>
                                <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                  <A dataType="Byte">255</A>
                                  <B dataType="Byte">255</B>
                                  <G dataType="Byte">255</G>
                                  <R dataType="Byte">255</R>
                                </colorTint>
                                <customMat />
                                <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                                <gameobj dataType="ObjectRef">1431324502</gameobj>
                                <offset dataType="Float">-1</offset>
                                <pixelGrid dataType="Bool">false</pixelGrid>
                                <rect dataType="Struct" type="Duality.Rect">
                                  <H dataType="Float">69.3</H>
                                  <W dataType="Float">83.5</W>
                                  <X dataType="Float">-45</X>
                                  <Y dataType="Float">-65</Y>
                                </rect>
                                <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                                <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                                  <contentPath dataType="String">Data\FlapOrDie\Graphics\bat-spritesheet.Material.res</contentPath>
                                </sharedMat>
                                <spriteIndex dataType="Int">2</spriteIndex>
                                <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                              </item>
                            </_items>
                            <_size dataType="Int">2</_size>
                          </compList>
                          <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1124045626" surrogate="true">
                            <header />
                            <body>
                              <keys dataType="Array" type="System.Object[]" id="4195773664">
                                <item dataType="ObjectRef">3812992238</item>
                                <item dataType="ObjectRef">1309696458</item>
                              </keys>
                              <values dataType="Array" type="System.Object[]" id="4206782350">
                                <item dataType="ObjectRef">1488601720</item>
                                <item dataType="ObjectRef">2899943782</item>
                              </values>
                            </body>
                          </compMap>
                          <compTransform dataType="ObjectRef">1488601720</compTransform>
                          <identifier dataType="Struct" type="System.Guid" surrogate="true">
                            <header>
                              <data dataType="Array" type="System.Byte[]" id="2636618748">7wbewMCLr02MUk6BmSwnww==</data>
                            </header>
                            <body />
                          </identifier>
                          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                          <name dataType="String">FrontWing</name>
                          <parent dataType="ObjectRef">4252430015</parent>
                          <prefabLink />
                        </item>
                        <item dataType="ObjectRef">2969348817</item>
                      </_items>
                      <_size dataType="Int">3</_size>
                    </children>
                    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1689776056">
                      <_items dataType="Array" type="Duality.Component[]" id="891033239" length="4">
                        <item dataType="Struct" type="Duality.Components.Transform" id="14739937">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <gameobj dataType="ObjectRef">4252430015</gameobj>
                          <ignoreParent dataType="Bool">false</ignoreParent>
                          <pos dataType="Struct" type="Duality.Vector3" />
                          <posAbs dataType="Struct" type="Duality.Vector3" />
                          <scale dataType="Float">1</scale>
                          <scaleAbs dataType="Float">1</scaleAbs>
                        </item>
                        <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3787359503">
                          <active dataType="Bool">true</active>
                          <allowParent dataType="Bool">false</allowParent>
                          <angularDamp dataType="Float">0.3</angularDamp>
                          <angularVel dataType="Float">0</angularVel>
                          <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                          <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                          <colFilter />
                          <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                          <explicitInertia dataType="Float">0</explicitInertia>
                          <explicitMass dataType="Float">50</explicitMass>
                          <fixedAngle dataType="Bool">false</fixedAngle>
                          <gameobj dataType="ObjectRef">4252430015</gameobj>
                          <ignoreGravity dataType="Bool">false</ignoreGravity>
                          <joints />
                          <linearDamp dataType="Float">0.3</linearDamp>
                          <linearVel dataType="Struct" type="Duality.Vector2" />
                          <revolutions dataType="Float">0</revolutions>
                          <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="835948325">
                            <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2592049814" length="4">
                              <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="3202642464">
                                <density dataType="Float">1</density>
                                <friction dataType="Float">0.3</friction>
                                <parent dataType="ObjectRef">3787359503</parent>
                                <position dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">0</X>
                                  <Y dataType="Float">5.35</Y>
                                </position>
                                <radius dataType="Float">32</radius>
                                <restitution dataType="Float">0.3</restitution>
                                <sensor dataType="Bool">false</sensor>
                                <userTag dataType="Int">0</userTag>
                              </item>
                            </_items>
                            <_size dataType="Int">1</_size>
                          </shapes>
                          <useCCD dataType="Bool">false</useCCD>
                        </item>
                        <item dataType="ObjectRef">828926909</item>
                      </_items>
                      <_size dataType="Int">3</_size>
                    </compList>
                    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2730403031" surrogate="true">
                      <header />
                      <body>
                        <keys dataType="Array" type="System.Object[]" id="4201096660">
                          <item dataType="ObjectRef">3812992238</item>
                          <item dataType="Type" id="3884414180" value="Duality.Components.Physics.RigidBody" />
                          <item dataType="Type" id="737205782" value="FlapOrDie.Controllers.PlayerController" />
                        </keys>
                        <values dataType="Array" type="System.Object[]" id="2663070646">
                          <item dataType="ObjectRef">14739937</item>
                          <item dataType="ObjectRef">3787359503</item>
                          <item dataType="ObjectRef">828926909</item>
                        </values>
                      </body>
                    </compMap>
                    <compTransform dataType="ObjectRef">14739937</compTransform>
                    <identifier dataType="Struct" type="System.Guid" surrogate="true">
                      <header>
                        <data dataType="Array" type="System.Byte[]" id="1135194352">qVPdSS7l4EWxApeL87Drsg==</data>
                      </header>
                      <body />
                    </identifier>
                    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                    <name dataType="String">Bat</name>
                    <parent />
                    <prefabLink />
                  </parent>
                  <prefabLink />
                </gameobj>
                <offset dataType="Float">1</offset>
                <pixelGrid dataType="Bool">false</pixelGrid>
                <rect dataType="Struct" type="Duality.Rect">
                  <H dataType="Float">67.3</H>
                  <W dataType="Float">61</W>
                  <X dataType="Float">-10</X>
                  <Y dataType="Float">-50</Y>
                </rect>
                <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                  <contentPath dataType="String">Data\FlapOrDie\Graphics\bat-spritesheet.Material.res</contentPath>
                </sharedMat>
                <spriteIndex dataType="Int">3</spriteIndex>
                <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
              </backWingRenderer>
              <bodyRenderer dataType="ObjectRef">1255344418</bodyRenderer>
              <frontWingRenderer dataType="ObjectRef">2899943782</frontWingRenderer>
              <gameobj dataType="ObjectRef">4252430015</gameobj>
              <impulseStrength dataType="Float">400</impulseStrength>
              <menuScene dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Scene]]">
                <contentPath dataType="String">Data\FlapOrDie\MainMenu.Scene.res</contentPath>
              </menuScene>
            </player>
            <pointsGapVariance dataType="Float">10</pointsGapVariance>
            <pointsMultiplier dataType="Float">10</pointsMultiplier>
            <scoreText dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="1663207719">
              <active dataType="Bool">true</active>
              <blockAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
              <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                <A dataType="Byte">255</A>
                <B dataType="Byte">255</B>
                <G dataType="Byte">255</G>
                <R dataType="Byte">255</R>
              </colorTint>
              <customMat />
              <gameobj dataType="Struct" type="Duality.GameObject" id="780270065">
                <active dataType="Bool">true</active>
                <children />
                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3563135935">
                  <_items dataType="Array" type="Duality.Component[]" id="638199214" length="4">
                    <item dataType="Struct" type="Duality.Components.Transform" id="837547283">
                      <active dataType="Bool">true</active>
                      <angle dataType="Float">0</angle>
                      <angleAbs dataType="Float">0</angleAbs>
                      <gameobj dataType="ObjectRef">780270065</gameobj>
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
                    <item dataType="ObjectRef">1663207719</item>
                  </_items>
                  <_size dataType="Int">2</_size>
                </compList>
                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2979292640" surrogate="true">
                  <header />
                  <body>
                    <keys dataType="Array" type="System.Object[]" id="3235905141">
                      <item dataType="ObjectRef">3812992238</item>
                      <item dataType="Type" id="2654251126" value="Duality.Components.Renderers.TextRenderer" />
                    </keys>
                    <values dataType="Array" type="System.Object[]" id="3677502152">
                      <item dataType="ObjectRef">837547283</item>
                      <item dataType="ObjectRef">1663207719</item>
                    </values>
                  </body>
                </compMap>
                <compTransform dataType="ObjectRef">837547283</compTransform>
                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                  <header>
                    <data dataType="Array" type="System.Byte[]" id="1611074751">NlKxPKf6aUKaKmrYrXrbbA==</data>
                  </header>
                  <body />
                </identifier>
                <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                <name dataType="String">TextRenderer</name>
                <parent />
                <prefabLink />
              </gameobj>
              <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
              <offset dataType="Float">0</offset>
              <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2687638154">
                <flowAreas />
                <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="3394618336">
                  <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                    <contentPath dataType="String">Data\FlapOrDie\Font\Condiment-Small.Font.res</contentPath>
                  </item>
                </fonts>
                <icons />
                <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                <maxHeight dataType="Int">0</maxHeight>
                <maxWidth dataType="Int">500</maxWidth>
                <sourceText dataType="String">Score: 0</sourceText>
                <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
              </text>
              <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0, AllFlags" value="2147483649" />
            </scoreText>
          </item>
          <item dataType="Struct" type="FlapOrDie.Components.MusicManager" id="2933965806">
            <active dataType="Bool">true</active>
            <bgm dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Sound]]">
              <contentPath dataType="String">Data\FlapOrDie\Audio\Attack-of-the-8-Bit-Hyper-Cranks.Sound.res</contentPath>
            </bgm>
            <gameobj dataType="ObjectRef">1610950862</gameobj>
          </item>
        </_items>
        <_size dataType="Int">6</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2346936478" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2261541530">
            <item dataType="ObjectRef">3812992238</item>
            <item dataType="Type" id="2907044736" value="Duality.Components.Camera" />
            <item dataType="Type" id="1064408270" value="Duality.Components.SoundListener" />
            <item dataType="Type" id="1859333404" value="FlapOrDie.Controllers.GameController" />
            <item dataType="Type" id="1135128018" value="FlapOrDie.Components.MusicManager" />
            <item dataType="Type" id="3161349944" value="Duality.Components.VelocityTracker" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="1167469370">
            <item dataType="ObjectRef">1668228080</item>
            <item dataType="ObjectRef">3157337339</item>
            <item dataType="ObjectRef">3643603389</item>
            <item dataType="ObjectRef">3273908911</item>
            <item dataType="ObjectRef">2933965806</item>
            <item dataType="ObjectRef">3682085329</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">1668228080</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1323045402">Qa2lQ5IjvUKsNsxNzIt5AQ==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">MainCamera</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">4097417655</item>
    <item dataType="ObjectRef">780270065</item>
    <item dataType="ObjectRef">4252430015</item>
    <item dataType="ObjectRef">291443106</item>
    <item dataType="Struct" type="Duality.GameObject" id="3498702872">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="334928894">
        <_items dataType="Array" type="Duality.GameObject[]" id="238814608" length="4">
          <item dataType="Struct" type="Duality.GameObject" id="909775398">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3188520774">
              <_items dataType="Array" type="Duality.Component[]" id="2288786944" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="967052616">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">909775398</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <pos dataType="Struct" type="Duality.Vector3" />
                  <posAbs dataType="Struct" type="Duality.Vector3" />
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="444704886">
                  <active dataType="Bool">true</active>
                  <allowParent dataType="Bool">false</allowParent>
                  <angularDamp dataType="Float">0.3</angularDamp>
                  <angularVel dataType="Float">0</angularVel>
                  <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Static" value="0" />
                  <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                  <colFilter />
                  <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                  <explicitInertia dataType="Float">0</explicitInertia>
                  <explicitMass dataType="Float">0</explicitMass>
                  <fixedAngle dataType="Bool">false</fixedAngle>
                  <gameobj dataType="ObjectRef">909775398</gameobj>
                  <ignoreGravity dataType="Bool">true</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.3</linearDamp>
                  <linearVel dataType="Struct" type="Duality.Vector2" />
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2221702486">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="4114804256" length="4">
                      <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="288361436">
                        <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="269081284">
                          <_items dataType="Array" type="Duality.Vector2[][]" id="1533878084" length="4">
                            <item dataType="Array" type="Duality.Vector2[]" id="1852587588">
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">-600</X>
                                <Y dataType="Float">-310</Y>
                              </item>
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">600</X>
                                <Y dataType="Float">-310</Y>
                              </item>
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">600</X>
                                <Y dataType="Float">-300</Y>
                              </item>
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">-600</X>
                                <Y dataType="Float">-300</Y>
                              </item>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </convexPolygons>
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">444704886</parent>
                        <restitution dataType="Float">0.3</restitution>
                        <sensor dataType="Bool">false</sensor>
                        <userTag dataType="Int">0</userTag>
                        <vertices dataType="Array" type="Duality.Vector2[]" id="2138346390">
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">-600</X>
                            <Y dataType="Float">-300</Y>
                          </item>
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">600</X>
                            <Y dataType="Float">-300</Y>
                          </item>
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">600</X>
                            <Y dataType="Float">-310</Y>
                          </item>
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">-600</X>
                            <Y dataType="Float">-310</Y>
                          </item>
                        </vertices>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                  </shapes>
                  <useCCD dataType="Bool">false</useCCD>
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2664777658" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="621258932">
                  <item dataType="ObjectRef">3812992238</item>
                  <item dataType="ObjectRef">3884414180</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="386441206">
                  <item dataType="ObjectRef">967052616</item>
                  <item dataType="ObjectRef">444704886</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">967052616</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3058534160">7Xh8dMYba0yx2iOkH2DzJQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Top</name>
            <parent dataType="ObjectRef">3498702872</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="199946578">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2746751434">
              <_items dataType="Array" type="Duality.Component[]" id="3055484256" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="257223796">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">199946578</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0.001</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0.001</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="4029843362">
                  <active dataType="Bool">true</active>
                  <allowParent dataType="Bool">false</allowParent>
                  <angularDamp dataType="Float">0.3</angularDamp>
                  <angularVel dataType="Float">0</angularVel>
                  <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Static" value="0" />
                  <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                  <colFilter />
                  <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                  <explicitInertia dataType="Float">0</explicitInertia>
                  <explicitMass dataType="Float">0</explicitMass>
                  <fixedAngle dataType="Bool">false</fixedAngle>
                  <gameobj dataType="ObjectRef">199946578</gameobj>
                  <ignoreGravity dataType="Bool">true</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.3</linearDamp>
                  <linearVel dataType="Struct" type="Duality.Vector2" />
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2389276890">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3049915648" length="4">
                      <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="1431116444">
                        <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="3327716292">
                          <_items dataType="Array" type="Duality.Vector2[][]" id="1019399492" length="4">
                            <item dataType="Array" type="Duality.Vector2[]" id="3577893444">
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">-600</X>
                                <Y dataType="Float">300</Y>
                              </item>
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">600</X>
                                <Y dataType="Float">300</Y>
                              </item>
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">600</X>
                                <Y dataType="Float">310</Y>
                              </item>
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">-600</X>
                                <Y dataType="Float">310</Y>
                              </item>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </convexPolygons>
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">4029843362</parent>
                        <restitution dataType="Float">0.3</restitution>
                        <sensor dataType="Bool">false</sensor>
                        <userTag dataType="Int">0</userTag>
                        <vertices dataType="Array" type="Duality.Vector2[]" id="3058906518">
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">-600</X>
                            <Y dataType="Float">300</Y>
                          </item>
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">600</X>
                            <Y dataType="Float">300</Y>
                          </item>
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">600</X>
                            <Y dataType="Float">310</Y>
                          </item>
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">-600</X>
                            <Y dataType="Float">310</Y>
                          </item>
                        </vertices>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                  </shapes>
                  <useCCD dataType="Bool">false</useCCD>
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3240854682" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2697983664">
                  <item dataType="ObjectRef">3812992238</item>
                  <item dataType="ObjectRef">3884414180</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3421796206">
                  <item dataType="ObjectRef">257223796</item>
                  <item dataType="ObjectRef">4029843362</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">257223796</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2850282764">eottU7Z5XkagJ8d4EpuyLQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Bottom</name>
            <parent dataType="ObjectRef">3498702872</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">2</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3291149194">
        <_items dataType="Array" type="Duality.Component[]" id="2376378844" length="0" />
        <_size dataType="Int">0</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2321163662" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="1091698464" length="0" />
          <values dataType="Array" type="System.Object[]" id="2179032974" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="59144764">0DDB2IhKpUyU/e8gxU0wOg==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">VerticalLimits</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">1364571484</item>
    <item dataType="ObjectRef">4081692434</item>
    <item dataType="ObjectRef">1431324502</item>
    <item dataType="ObjectRef">2969348817</item>
    <item dataType="ObjectRef">1533607594</item>
    <item dataType="ObjectRef">467284875</item>
    <item dataType="ObjectRef">3876724628</item>
    <item dataType="ObjectRef">909775398</item>
    <item dataType="ObjectRef">199946578</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
