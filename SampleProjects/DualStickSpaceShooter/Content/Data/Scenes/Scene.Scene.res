<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <globalGravity dataType="Struct" type="OpenTK.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">0</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="1163277037">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1531404383">
        <_items dataType="Array" type="Duality.GameObject[]" id="1422731374" length="4">
          <item dataType="Struct" type="Duality.GameObject" id="687383225">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="387288265">
              <_items dataType="Array" type="Duality.Component[]" id="969851534" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="3047698157">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">687383225</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform dataType="Struct" type="Duality.Components.Transform" id="3523591969">
                    <active dataType="Bool">true</active>
                    <angle dataType="Float">0</angle>
                    <angleAbs dataType="Float">0</angleAbs>
                    <angleVel dataType="Float">0</angleVel>
                    <angleVelAbs dataType="Float">0</angleVelAbs>
                    <deriveAngle dataType="Bool">true</deriveAngle>
                    <gameobj dataType="ObjectRef">1163277037</gameobj>
                    <ignoreParent dataType="Bool">false</ignoreParent>
                    <parentTransform />
                    <pos dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">0</X>
                      <Y dataType="Float">0</Y>
                      <Z dataType="Float">-500</Z>
                    </pos>
                    <posAbs dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">0</X>
                      <Y dataType="Float">0</Y>
                      <Z dataType="Float">-500</Z>
                    </posAbs>
                    <scale dataType="Float">1</scale>
                    <scaleAbs dataType="Float">1</scaleAbs>
                    <vel dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">0</X>
                      <Y dataType="Float">0</Y>
                      <Z dataType="Float">0</Z>
                    </vel>
                    <velAbs dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">0</X>
                      <Y dataType="Float">0</Y>
                      <Z dataType="Float">0</Z>
                    </velAbs>
                  </parentTransform>
                  <pos dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">500</Z>
                  </pos>
                  <posAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Components.SoundListener" id="1340864596">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">687383225</gameobj>
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3890146880" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="1932332675">
                  <item dataType="Type" id="4020116262" value="Duality.Components.Transform" />
                  <item dataType="Type" id="2392314554" value="Duality.Components.SoundListener" />
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="2057561528">
                  <item dataType="ObjectRef">3047698157</item>
                  <item dataType="ObjectRef">1340864596</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3047698157</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1054353833">HeHPGzc2DkOv5QXtB28cqg==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Microphone</name>
            <parent dataType="ObjectRef">1163277037</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">1</_size>
        <_version dataType="Int">1</_version>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="944234272">
        <_items dataType="Array" type="Duality.Component[]" id="217129813" length="4">
          <item dataType="ObjectRef">3523591969</item>
          <item dataType="Struct" type="Duality.Components.Camera" id="1700552844">
            <active dataType="Bool">true</active>
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">1163277037</gameobj>
            <nearZ dataType="Float">0</nearZ>
            <passes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Camera+Pass]]" id="3349398274">
              <_items dataType="Array" type="Duality.Components.Camera+Pass[]" id="1739664784" length="4">
                <item dataType="Struct" type="Duality.Components.Camera+Pass" id="2631985468">
                  <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">0</A>
                    <B dataType="Byte">0</B>
                    <G dataType="Byte">0</G>
                    <R dataType="Byte">0</R>
                  </clearColor>
                  <clearDepth dataType="Float">1</clearDepth>
                  <clearFlags dataType="Enum" type="Duality.Drawing.ClearFlag" name="All" value="3" />
                  <CollectDrawcalls />
                  <input />
                  <matrixMode dataType="Enum" type="Duality.Drawing.RenderMatrix" name="PerspectiveWorld" value="0" />
                  <output dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]">
                    <contentPath />
                  </output>
                  <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                </item>
                <item dataType="Struct" type="Duality.Components.Camera+Pass" id="710927254">
                  <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">0</A>
                    <B dataType="Byte">0</B>
                    <G dataType="Byte">0</G>
                    <R dataType="Byte">0</R>
                  </clearColor>
                  <clearDepth dataType="Float">1</clearDepth>
                  <clearFlags dataType="Enum" type="Duality.Drawing.ClearFlag" name="None" value="0" />
                  <CollectDrawcalls />
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
          <item dataType="Struct" type="DualStickSpaceShooter.CameraController" id="1908534968">
            <active dataType="Bool">true</active>
            <followObjects dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Transform]]" id="2691946462">
              <_items dataType="Array" type="Duality.Components.Transform[]" id="1475703056" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2466297876">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="Struct" type="Duality.GameObject" id="105982944">
                    <active dataType="Bool">false</active>
                    <children />
                    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="94225584">
                      <_items dataType="Array" type="Duality.Component[]" id="600343484">
                        <item dataType="ObjectRef">2466297876</item>
                        <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1748149512">
                          <active dataType="Bool">true</active>
                          <gameobj dataType="ObjectRef">105982944</gameobj>
                        </item>
                        <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3168759468">
                          <active dataType="Bool">true</active>
                          <gameobj dataType="ObjectRef">105982944</gameobj>
                        </item>
                        <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="1328494310">
                          <active dataType="Bool">true</active>
                          <gameobj dataType="ObjectRef">105982944</gameobj>
                        </item>
                      </_items>
                      <_size dataType="Int">4</_size>
                      <_version dataType="Int">4</_version>
                    </compList>
                    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="407822190" surrogate="true">
                      <header />
                      <body>
                        <keys dataType="Array" type="System.Type[]" id="2777083010">
                          <item dataType="ObjectRef">4020116262</item>
                          <item dataType="Type" id="2838274704" value="Duality.Components.Renderers.SpriteRenderer" />
                          <item dataType="Type" id="2826137326" value="Duality.Components.Physics.RigidBody" />
                          <item dataType="Type" id="1216776044" value="DualStickSpaceShooter.Ship" />
                        </keys>
                        <values dataType="Array" type="Duality.Component[]" id="793755786">
                          <item dataType="ObjectRef">2466297876</item>
                          <item dataType="ObjectRef">1748149512</item>
                          <item dataType="ObjectRef">3168759468</item>
                          <item dataType="ObjectRef">1328494310</item>
                        </values>
                      </body>
                    </compMap>
                    <compTransform dataType="ObjectRef">2466297876</compTransform>
                    <identifier dataType="Struct" type="System.Guid" surrogate="true">
                      <header>
                        <data dataType="Array" type="System.Byte[]" id="2457995250">kmxmnTfjNEev6HiiGEn5Rg==</data>
                      </header>
                      <body />
                    </identifier>
                    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                    <name dataType="String">PlayerShip</name>
                    <parent dataType="Struct" type="Duality.GameObject" id="1924155130">
                      <active dataType="Bool">true</active>
                      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2887489278">
                        <_items dataType="Array" type="Duality.GameObject[]" id="1883130256" length="4">
                          <item dataType="ObjectRef">105982944</item>
                        </_items>
                        <_size dataType="Int">1</_size>
                        <_version dataType="Int">1</_version>
                      </children>
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="582723466">
                        <_items dataType="Array" type="Duality.Component[]" id="2288176348" length="4">
                          <item dataType="Struct" type="DualStickSpaceShooter.Player" id="2837740131">
                            <active dataType="Bool">true</active>
                            <gameobj dataType="ObjectRef">1924155130</gameobj>
                          </item>
                        </_items>
                        <_size dataType="Int">1</_size>
                        <_version dataType="Int">1</_version>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="871830670" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Type[]" id="729827104">
                            <item dataType="Type" id="2425790428" value="DualStickSpaceShooter.Player" />
                          </keys>
                          <values dataType="Array" type="Duality.Component[]" id="2214745998">
                            <item dataType="ObjectRef">2837740131</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform />
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="965616188">bQCmfxY3+kaGNzoAOVkdbQ==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Player</name>
                      <parent />
                      <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="1723137498">
                        <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="2338173868">
                          <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="2165640420" length="4" />
                          <_size dataType="Int">0</_size>
                          <_version dataType="Int">132</_version>
                        </changes>
                        <obj dataType="ObjectRef">1924155130</obj>
                        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                          <contentPath dataType="String">Data\Prefabs\Player.Prefab.res</contentPath>
                        </prefab>
                      </prefabLink>
                    </parent>
                    <prefabLink />
                  </gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Transform" id="1966433478">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="Struct" type="Duality.GameObject" id="3901085842">
                    <active dataType="Bool">false</active>
                    <children />
                    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3292717164">
                      <_items dataType="Array" type="Duality.Component[]" id="733884260">
                        <item dataType="ObjectRef">1966433478</item>
                        <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1248285114">
                          <active dataType="Bool">true</active>
                          <gameobj dataType="ObjectRef">3901085842</gameobj>
                        </item>
                        <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2668895070">
                          <active dataType="Bool">true</active>
                          <gameobj dataType="ObjectRef">3901085842</gameobj>
                        </item>
                        <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="828629912">
                          <active dataType="Bool">true</active>
                          <gameobj dataType="ObjectRef">3901085842</gameobj>
                        </item>
                      </_items>
                      <_size dataType="Int">4</_size>
                      <_version dataType="Int">4</_version>
                    </compList>
                    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2450926646" surrogate="true">
                      <header />
                      <body>
                        <keys dataType="Array" type="System.Type[]" id="3147291814">
                          <item dataType="ObjectRef">4020116262</item>
                          <item dataType="ObjectRef">2838274704</item>
                          <item dataType="ObjectRef">2826137326</item>
                          <item dataType="ObjectRef">1216776044</item>
                        </keys>
                        <values dataType="Array" type="Duality.Component[]" id="3060829114">
                          <item dataType="ObjectRef">1966433478</item>
                          <item dataType="ObjectRef">1248285114</item>
                          <item dataType="ObjectRef">2668895070</item>
                          <item dataType="ObjectRef">828629912</item>
                        </values>
                      </body>
                    </compMap>
                    <compTransform dataType="ObjectRef">1966433478</compTransform>
                    <identifier dataType="Struct" type="System.Guid" surrogate="true">
                      <header>
                        <data dataType="Array" type="System.Byte[]" id="3830394534">MJJheeRXBkq4J8aJ26nrpA==</data>
                      </header>
                      <body />
                    </identifier>
                    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                    <name dataType="String">PlayerShip</name>
                    <parent dataType="Struct" type="Duality.GameObject" id="3691642060">
                      <active dataType="Bool">true</active>
                      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="268195460">
                        <_items dataType="Array" type="Duality.GameObject[]" id="3248620612" length="4">
                          <item dataType="ObjectRef">3901085842</item>
                        </_items>
                        <_size dataType="Int">1</_size>
                        <_version dataType="Int">1</_version>
                      </children>
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4094806166">
                        <_items dataType="Array" type="Duality.Component[]" id="2645298830" length="4">
                          <item dataType="Struct" type="DualStickSpaceShooter.Player" id="310259765">
                            <active dataType="Bool">true</active>
                            <gameobj dataType="ObjectRef">3691642060</gameobj>
                          </item>
                        </_items>
                        <_size dataType="Int">1</_size>
                        <_version dataType="Int">1</_version>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1747577408" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Type[]" id="2353352904">
                            <item dataType="ObjectRef">2425790428</item>
                          </keys>
                          <values dataType="Array" type="Duality.Component[]" id="4196106974">
                            <item dataType="ObjectRef">310259765</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform />
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="1048696116">f7P3Gf6IbE+ZoU0W66W73w==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Player2</name>
                      <parent />
                      <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="4277560482">
                        <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="2460083282">
                          <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="2514385232" length="8">
                            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="871579568">
                                <_items dataType="Array" type="System.Int32[]" id="2697357756" />
                                <_size dataType="Int">0</_size>
                                <_version dataType="Int">1</_version>
                              </childIndex>
                              <componentType />
                              <prop dataType="PropertyInfo" id="1635555694" value="P:Duality.GameObject:Name" />
                              <val dataType="String">Player2</val>
                            </item>
                            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1679150092">
                                <_items dataType="Array" type="System.Int32[]" id="3518610360" length="4" />
                                <_size dataType="Int">1</_size>
                                <_version dataType="Int">2</_version>
                              </childIndex>
                              <componentType dataType="ObjectRef">4020116262</componentType>
                              <prop dataType="PropertyInfo" id="3402294738" value="P:Duality.Components.Transform:RelativePos" />
                              <val dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">-97</X>
                                <Y dataType="Float">66</Y>
                                <Z dataType="Float">0</Z>
                              </val>
                            </item>
                            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="619957096">
                                <_items dataType="ObjectRef">2697357756</_items>
                                <_size dataType="Int">0</_size>
                                <_version dataType="Int">1</_version>
                              </childIndex>
                              <componentType dataType="ObjectRef">2425790428</componentType>
                              <prop dataType="PropertyInfo" id="2814194406" value="P:DualStickSpaceShooter.Player:Color" />
                              <val dataType="Struct" type="Duality.Drawing.ColorRgba">
                                <A dataType="Byte">255</A>
                                <B dataType="Byte">89</B>
                                <G dataType="Byte">176</G>
                                <R dataType="Byte">255</R>
                              </val>
                            </item>
                            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1387688228">
                                <_items dataType="Array" type="System.Int32[]" id="889694992" />
                                <_size dataType="Int">0</_size>
                                <_version dataType="Int">1</_version>
                              </childIndex>
                              <componentType />
                              <prop dataType="PropertyInfo" id="3043999498" value="P:Duality.GameObject:ActiveSingle" />
                              <val dataType="Bool">true</val>
                            </item>
                            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3964229984">
                                <_items dataType="Array" type="System.Int32[]" id="3266911372" />
                                <_size dataType="Int">0</_size>
                                <_version dataType="Int">1</_version>
                              </childIndex>
                              <componentType dataType="ObjectRef">2425790428</componentType>
                              <prop dataType="PropertyInfo" id="1967265150" value="P:DualStickSpaceShooter.Player:Id" />
                              <val dataType="Enum" type="DualStickSpaceShooter.PlayerId" name="PlayerTwo" value="2" />
                            </item>
                            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3039303996">
                                <_items dataType="Array" type="System.Int32[]" id="392056776" length="4" />
                                <_size dataType="Int">1</_size>
                                <_version dataType="Int">2</_version>
                              </childIndex>
                              <componentType />
                              <prop dataType="ObjectRef">3043999498</prop>
                              <val dataType="Bool">false</val>
                            </item>
                          </_items>
                          <_size dataType="Int">6</_size>
                          <_version dataType="Int">120</_version>
                        </changes>
                        <obj dataType="ObjectRef">3691642060</obj>
                        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                          <contentPath dataType="String">Data\Prefabs\Player.Prefab.res</contentPath>
                        </prefab>
                      </prefabLink>
                    </parent>
                    <prefabLink />
                  </gameobj>
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">5</_version>
            </followObjects>
            <gameobj dataType="ObjectRef">1163277037</gameobj>
            <maxZoomOutDist dataType="Float">350</maxZoomOutDist>
            <microphone dataType="ObjectRef">3047698157</microphone>
            <screenShakeOffset dataType="Struct" type="OpenTK.Vector2">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
            </screenShakeOffset>
            <screenShakeStrength dataType="Float">0</screenShakeStrength>
            <softness dataType="Float">1</softness>
            <zoomFactor dataType="Float">1</zoomFactor>
            <zoomOutScale dataType="Float">1</zoomOutScale>
          </item>
        </_items>
        <_size dataType="Int">3</_size>
        <_version dataType="Int">5</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="60134861" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="2225525156">
            <item dataType="ObjectRef">4020116262</item>
            <item dataType="Type" id="4086516932" value="Duality.Components.Camera" />
            <item dataType="Type" id="1019181974" value="DualStickSpaceShooter.CameraController" />
          </keys>
          <values dataType="Array" type="Duality.Component[]" id="4055426838">
            <item dataType="ObjectRef">3523591969</item>
            <item dataType="ObjectRef">1700552844</item>
            <item dataType="ObjectRef">1908534968</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">3523591969</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3894049952">+now62bvREivU0EqlajnUw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">MainCamera</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="1042201066">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="629316980">
        <_items dataType="Array" type="Duality.GameObject[]" id="586008996">
          <item dataType="Struct" type="Duality.GameObject" id="2383024647">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1754868347">
              <_items dataType="Array" type="Duality.Component[]" id="1299124566" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="448372283">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">2.42700338</angle>
                  <angleAbs dataType="Float">2.42700338</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">2383024647</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">1209.74658</X>
                    <Y dataType="Float">1376.97534</Y>
                    <Z dataType="Float">88.01538</Z>
                  </pos>
                  <posAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">1209.74658</X>
                    <Y dataType="Float">1376.97534</Y>
                    <Z dataType="Float">88.01538</Z>
                  </posAbs>
                  <scale dataType="Float">14.7019606</scale>
                  <scaleAbs dataType="Float">14.7019606</scaleAbs>
                  <vel dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="4025191215">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">109</B>
                    <G dataType="Byte">109</G>
                    <R dataType="Byte">109</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">2383024647</gameobj>
                  <offset dataType="Int">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">383</H>
                    <W dataType="Float">385</W>
                    <X dataType="Float">-192.5</X>
                    <Y dataType="Float">-191.5</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\Materials\Background\Backdrop4.Material.res</contentPath>
                  </sharedMat>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.SpriteDepthColor" id="131499598">
                  <active dataType="Bool">true</active>
                  <baseColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">128</B>
                    <G dataType="Byte">128</G>
                    <R dataType="Byte">128</R>
                  </baseColor>
                  <gameobj dataType="ObjectRef">2383024647</gameobj>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
              <_version dataType="Int">3</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3338222248" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="1639240593">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2838274704</item>
                  <item dataType="Type" id="3049585134" value="DualStickSpaceShooter.SpriteDepthColor" />
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="4095076000">
                  <item dataType="ObjectRef">448372283</item>
                  <item dataType="ObjectRef">4025191215</item>
                  <item dataType="ObjectRef">131499598</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">448372283</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3595664899">6VAuoyRLMEK7sWb9YyNs3g==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Backdrop4</name>
            <parent dataType="ObjectRef">1042201066</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3003406700">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1252388956">
              <_items dataType="Array" type="Duality.Component[]" id="2887632068" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1068754336">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">4.452103</angle>
                  <angleAbs dataType="Float">4.452103</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">3003406700</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">-1818.75928</X>
                    <Y dataType="Float">101.118591</Y>
                    <Z dataType="Float">392.126465</Z>
                  </pos>
                  <posAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">-1818.75928</X>
                    <Y dataType="Float">101.118591</Y>
                    <Z dataType="Float">392.126465</Z>
                  </posAbs>
                  <scale dataType="Float">15.4454136</scale>
                  <scaleAbs dataType="Float">15.4454136</scaleAbs>
                  <vel dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="350605972">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">72</B>
                    <G dataType="Byte">72</G>
                    <R dataType="Byte">72</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">3003406700</gameobj>
                  <offset dataType="Int">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">383</H>
                    <W dataType="Float">385</W>
                    <X dataType="Float">-192.5</X>
                    <Y dataType="Float">-191.5</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\Materials\Background\Backdrop4.Material.res</contentPath>
                  </sharedMat>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.SpriteDepthColor" id="751881651">
                  <active dataType="Bool">true</active>
                  <baseColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">128</B>
                    <G dataType="Byte">128</G>
                    <R dataType="Byte">128</R>
                  </baseColor>
                  <gameobj dataType="ObjectRef">3003406700</gameobj>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
              <_version dataType="Int">3</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2650890006" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="189677174">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2838274704</item>
                  <item dataType="ObjectRef">3049585134</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="1457469722">
                  <item dataType="ObjectRef">1068754336</item>
                  <item dataType="ObjectRef">350605972</item>
                  <item dataType="ObjectRef">751881651</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1068754336</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1445400982">URnxZ/V9FkCljalvlCjxKw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Backdrop4</name>
            <parent dataType="ObjectRef">1042201066</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="283960702">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4054249254">
              <_items dataType="Array" type="Duality.Component[]" id="4071924992" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2644275634">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">5.94627142</angle>
                  <angleAbs dataType="Float">5.94627142</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">283960702</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">658.1311</X>
                    <Y dataType="Float">-3240.236</Y>
                    <Z dataType="Float">1038.24341</Z>
                  </pos>
                  <posAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">658.1311</X>
                    <Y dataType="Float">-3240.236</Y>
                    <Z dataType="Float">1038.24341</Z>
                  </posAbs>
                  <scale dataType="Float">20.467577</scale>
                  <scaleAbs dataType="Float">20.467577</scaleAbs>
                  <vel dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1926127270">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">42</B>
                    <G dataType="Byte">42</G>
                    <R dataType="Byte">42</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">283960702</gameobj>
                  <offset dataType="Int">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">383</H>
                    <W dataType="Float">385</W>
                    <X dataType="Float">-192.5</X>
                    <Y dataType="Float">-191.5</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\Materials\Background\Backdrop4.Material.res</contentPath>
                  </sharedMat>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.SpriteDepthColor" id="2327402949">
                  <active dataType="Bool">true</active>
                  <baseColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">128</B>
                    <G dataType="Byte">128</G>
                    <R dataType="Byte">128</R>
                  </baseColor>
                  <gameobj dataType="ObjectRef">283960702</gameobj>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
              <_version dataType="Int">3</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2089323194" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="2219571348">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2838274704</item>
                  <item dataType="ObjectRef">3049585134</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="1763583030">
                  <item dataType="ObjectRef">2644275634</item>
                  <item dataType="ObjectRef">1926127270</item>
                  <item dataType="ObjectRef">2327402949</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2644275634</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2421711152">6KtZ87VAck6R3KPFJYBx5A==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Backdrop4</name>
            <parent dataType="ObjectRef">1042201066</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2198381846">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1051866574">
              <_items dataType="Array" type="Duality.Component[]" id="1667381712" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="263729482">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">3.372962</angle>
                  <angleAbs dataType="Float">3.372962</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">2198381846</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">310.66925</X>
                    <Y dataType="Float">3372.38086</Y>
                    <Z dataType="Float">2380.279</Z>
                  </pos>
                  <posAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">310.66925</X>
                    <Y dataType="Float">3372.38086</Y>
                    <Z dataType="Float">2380.279</Z>
                  </posAbs>
                  <scale dataType="Float">29.4827614</scale>
                  <scaleAbs dataType="Float">29.4827614</scaleAbs>
                  <vel dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3840548414">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">22</B>
                    <G dataType="Byte">22</G>
                    <R dataType="Byte">22</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">2198381846</gameobj>
                  <offset dataType="Int">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">383</H>
                    <W dataType="Float">385</W>
                    <X dataType="Float">-192.5</X>
                    <Y dataType="Float">-191.5</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\Materials\Background\Backdrop4.Material.res</contentPath>
                  </sharedMat>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.SpriteDepthColor" id="4241824093">
                  <active dataType="Bool">true</active>
                  <baseColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">128</B>
                    <G dataType="Byte">128</G>
                    <R dataType="Byte">128</R>
                  </baseColor>
                  <gameobj dataType="ObjectRef">2198381846</gameobj>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
              <_version dataType="Int">3</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2301570378" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="2947722380">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2838274704</item>
                  <item dataType="ObjectRef">3049585134</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="1884604406">
                  <item dataType="ObjectRef">263729482</item>
                  <item dataType="ObjectRef">3840548414</item>
                  <item dataType="ObjectRef">4241824093</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">263729482</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3438803992">hOP9DvVurUaWR+Oml58oPw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Backdrop4</name>
            <parent dataType="ObjectRef">1042201066</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">4</_size>
        <_version dataType="Int">12</_version>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="616811510">
        <_items dataType="Array" type="Duality.Component[]" id="2267036766" />
        <_size dataType="Int">0</_size>
        <_version dataType="Int">0</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2557405520" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="3835458696" />
          <values dataType="Array" type="Duality.Component[]" id="3123428318" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1190610548">J/60VKoplkeBj0Ms+1VSWw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Background</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">1924155130</item>
    <item dataType="Struct" type="Duality.GameObject" id="2538874541">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3418839967">
        <_items dataType="Array" type="Duality.GameObject[]" id="3723983214" length="8">
          <item dataType="Struct" type="Duality.GameObject" id="367989753">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2110999433">
              <_items dataType="Array" type="Duality.Component[]" id="1262048654" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2728304685">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">367989753</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">36</X>
                    <Y dataType="Float">-209</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">36</X>
                    <Y dataType="Float">-209</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3430766277">
                  <active dataType="Bool">true</active>
                  <angularDamp dataType="Float">0.3</angularDamp>
                  <angularVel dataType="Float">0</angularVel>
                  <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                  <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                  <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                  <continous dataType="Bool">false</continous>
                  <explicitMass dataType="Float">0</explicitMass>
                  <fixedAngle dataType="Bool">false</fixedAngle>
                  <gameobj dataType="ObjectRef">367989753</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.3</linearDamp>
                  <linearVel dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                  </linearVel>
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2102827237">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3875860630" length="4">
                      <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="1790360096">
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">3430766277</parent>
                        <position dataType="Struct" type="OpenTK.Vector2">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">0</Y>
                        </position>
                        <radius dataType="Float">128</radius>
                        <restitution dataType="Float">0.3</restitution>
                        <sensor dataType="Bool">false</sensor>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                    <_version dataType="Int">1</_version>
                  </shapes>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="966682943">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                  </areaMaterial>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customAreaMaterial />
                  <customOutlineMaterial />
                  <gameobj dataType="ObjectRef">367989753</gameobj>
                  <offset dataType="Int">0</offset>
                  <outlineMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                  </outlineMaterial>
                  <outlineWidth dataType="Float">3</outlineWidth>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                  <wrapTexture dataType="Bool">true</wrapTexture>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
              <_version dataType="Int">3</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1229187392" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="1170025027">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2826137326</item>
                  <item dataType="Type" id="4016316454" value="Duality.Components.Renderers.RigidBodyRenderer" />
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="4176143032">
                  <item dataType="ObjectRef">2728304685</item>
                  <item dataType="ObjectRef">3430766277</item>
                  <item dataType="ObjectRef">966682943</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2728304685</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1924857449">ZleB5m5EtESzdbf3v9tXKw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">RigidBodyRenderer</name>
            <parent dataType="ObjectRef">2538874541</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3837180042">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1037447694">
              <_items dataType="Array" type="Duality.Component[]" id="1302933456" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1902527678">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">5.7769413</angle>
                  <angleAbs dataType="Float">5.7769413</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">3837180042</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">-236.999</X>
                    <Y dataType="Float">-45</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">-236.999</X>
                    <Y dataType="Float">-45</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">0.4754751</scale>
                  <scaleAbs dataType="Float">0.4754751</scaleAbs>
                  <vel dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2604989270">
                  <active dataType="Bool">true</active>
                  <angularDamp dataType="Float">0.3</angularDamp>
                  <angularVel dataType="Float">0</angularVel>
                  <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                  <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                  <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                  <continous dataType="Bool">false</continous>
                  <explicitMass dataType="Float">0</explicitMass>
                  <fixedAngle dataType="Bool">false</fixedAngle>
                  <gameobj dataType="ObjectRef">3837180042</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.3</linearDamp>
                  <linearVel dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                  </linearVel>
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3171467222">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1482666272" length="4">
                      <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="2659669980">
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">2604989270</parent>
                        <position dataType="Struct" type="OpenTK.Vector2">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">0</Y>
                        </position>
                        <radius dataType="Float">128</radius>
                        <restitution dataType="Float">0.3</restitution>
                        <sensor dataType="Bool">false</sensor>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                    <_version dataType="Int">2</_version>
                  </shapes>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="140905936">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                  </areaMaterial>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customAreaMaterial />
                  <customOutlineMaterial />
                  <gameobj dataType="ObjectRef">3837180042</gameobj>
                  <offset dataType="Int">0</offset>
                  <outlineMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                  </outlineMaterial>
                  <outlineWidth dataType="Float">3</outlineWidth>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                  <wrapTexture dataType="Bool">true</wrapTexture>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
              <_version dataType="Int">3</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="794320714" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="3279476684">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2826137326</item>
                  <item dataType="ObjectRef">4016316454</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="4214801142">
                  <item dataType="ObjectRef">1902527678</item>
                  <item dataType="ObjectRef">2604989270</item>
                  <item dataType="ObjectRef">140905936</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1902527678</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1847168984">Vf0Uo6LddUWvMJoDzq8Rqg==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">RigidBodyRenderer</name>
            <parent dataType="ObjectRef">2538874541</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3508990515">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1625753939">
              <_items dataType="Array" type="Duality.Component[]" id="47808358" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1574338151">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">3508990515</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">182.001</X>
                    <Y dataType="Float">147</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">182.001</X>
                    <Y dataType="Float">147</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2276799743">
                  <active dataType="Bool">true</active>
                  <angularDamp dataType="Float">0.3</angularDamp>
                  <angularVel dataType="Float">0</angularVel>
                  <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                  <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                  <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                  <continous dataType="Bool">false</continous>
                  <explicitMass dataType="Float">0</explicitMass>
                  <fixedAngle dataType="Bool">false</fixedAngle>
                  <gameobj dataType="ObjectRef">3508990515</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.3</linearDamp>
                  <linearVel dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                  </linearVel>
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="4140661183">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1727147438" length="4">
                      <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="3199354192">
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">2276799743</parent>
                        <restitution dataType="Float">0.3</restitution>
                        <sensor dataType="Bool">false</sensor>
                        <vertices dataType="Array" type="OpenTK.Vector2[]" id="3027887036">
                          <item dataType="Struct" type="OpenTK.Vector2">
                            <X dataType="Float">16.9989929</X>
                            <Y dataType="Float">-50</Y>
                          </item>
                          <item dataType="Struct" type="OpenTK.Vector2">
                            <X dataType="Float">-106.001007</X>
                            <Y dataType="Float">18</Y>
                          </item>
                          <item dataType="Struct" type="OpenTK.Vector2">
                            <X dataType="Float">55.9989929</X>
                            <Y dataType="Float">51</Y>
                          </item>
                          <item dataType="Struct" type="OpenTK.Vector2">
                            <X dataType="Float">115.998993</X>
                            <Y dataType="Float">-4</Y>
                          </item>
                        </vertices>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                    <_version dataType="Int">4</_version>
                  </shapes>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="4107683705">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                  </areaMaterial>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customAreaMaterial />
                  <customOutlineMaterial />
                  <gameobj dataType="ObjectRef">3508990515</gameobj>
                  <offset dataType="Int">0</offset>
                  <outlineMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                  </outlineMaterial>
                  <outlineWidth dataType="Float">3</outlineWidth>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                  <wrapTexture dataType="Bool">true</wrapTexture>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
              <_version dataType="Int">3</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="940857208" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="2728838201">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2826137326</item>
                  <item dataType="ObjectRef">4016316454</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="2111389952">
                  <item dataType="ObjectRef">1574338151</item>
                  <item dataType="ObjectRef">2276799743</item>
                  <item dataType="ObjectRef">4107683705</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1574338151</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="620811195">FLvNyOs/ekmfkSZtuKgVXQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">RigidBodyRenderer</name>
            <parent dataType="ObjectRef">2538874541</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2373794822">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2540354490">
              <_items dataType="Array" type="Duality.Component[]" id="3364155392" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="439142458">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">1.628406</angle>
                  <angleAbs dataType="Float">1.628406</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">2373794822</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">270.002</X>
                    <Y dataType="Float">-85</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">270.002</X>
                    <Y dataType="Float">-85</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">0.639224648</scale>
                  <scaleAbs dataType="Float">0.639224648</scaleAbs>
                  <vel dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1141604050">
                  <active dataType="Bool">true</active>
                  <angularDamp dataType="Float">0.3</angularDamp>
                  <angularVel dataType="Float">0</angularVel>
                  <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                  <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                  <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                  <continous dataType="Bool">false</continous>
                  <explicitMass dataType="Float">0</explicitMass>
                  <fixedAngle dataType="Bool">false</fixedAngle>
                  <gameobj dataType="ObjectRef">2373794822</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.3</linearDamp>
                  <linearVel dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                  </linearVel>
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="483001322">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="4213191968" length="4">
                      <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="1422174172">
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">1141604050</parent>
                        <restitution dataType="Float">0.3</restitution>
                        <sensor dataType="Bool">false</sensor>
                        <vertices dataType="Array" type="OpenTK.Vector2[]" id="2381183684">
                          <item dataType="Struct" type="OpenTK.Vector2">
                            <X dataType="Float">16.9989929</X>
                            <Y dataType="Float">-50</Y>
                          </item>
                          <item dataType="Struct" type="OpenTK.Vector2">
                            <X dataType="Float">-106.001007</X>
                            <Y dataType="Float">18</Y>
                          </item>
                          <item dataType="Struct" type="OpenTK.Vector2">
                            <X dataType="Float">55.9989929</X>
                            <Y dataType="Float">51</Y>
                          </item>
                          <item dataType="Struct" type="OpenTK.Vector2">
                            <X dataType="Float">115.998993</X>
                            <Y dataType="Float">-4</Y>
                          </item>
                        </vertices>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                    <_version dataType="Int">2</_version>
                  </shapes>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="2972488012">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                  </areaMaterial>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customAreaMaterial />
                  <customOutlineMaterial />
                  <gameobj dataType="ObjectRef">2373794822</gameobj>
                  <offset dataType="Int">0</offset>
                  <outlineMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                  </outlineMaterial>
                  <outlineWidth dataType="Float">3</outlineWidth>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                  <wrapTexture dataType="Bool">true</wrapTexture>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
              <_version dataType="Int">3</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4286987706" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="3605516800">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2826137326</item>
                  <item dataType="ObjectRef">4016316454</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="416939982">
                  <item dataType="ObjectRef">439142458</item>
                  <item dataType="ObjectRef">1141604050</item>
                  <item dataType="ObjectRef">2972488012</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">439142458</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3528331932">4nTsfpFXEEiJ2KSOH2Xe9A==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">RigidBodyRenderer</name>
            <parent dataType="ObjectRef">2538874541</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="196886364">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1154376776">
              <_items dataType="Array" type="Duality.Component[]" id="2608125036" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2557201296">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">1.13036573</angle>
                  <angleAbs dataType="Float">1.13036573</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">196886364</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">-323.998</X>
                    <Y dataType="Float">216</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">-323.998</X>
                    <Y dataType="Float">216</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1.28693759</scale>
                  <scaleAbs dataType="Float">1.28693759</scaleAbs>
                  <vel dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3259662888">
                  <active dataType="Bool">true</active>
                  <angularDamp dataType="Float">0.3</angularDamp>
                  <angularVel dataType="Float">0</angularVel>
                  <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                  <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                  <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                  <continous dataType="Bool">false</continous>
                  <explicitMass dataType="Float">0</explicitMass>
                  <fixedAngle dataType="Bool">false</fixedAngle>
                  <gameobj dataType="ObjectRef">196886364</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.3</linearDamp>
                  <linearVel dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                  </linearVel>
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2870025600">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="576066972" length="4">
                      <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="3262303684">
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">3259662888</parent>
                        <restitution dataType="Float">0.3</restitution>
                        <sensor dataType="Bool">false</sensor>
                        <vertices dataType="Array" type="OpenTK.Vector2[]" id="2408993092">
                          <item dataType="Struct" type="OpenTK.Vector2">
                            <X dataType="Float">16.9989929</X>
                            <Y dataType="Float">-50</Y>
                          </item>
                          <item dataType="Struct" type="OpenTK.Vector2">
                            <X dataType="Float">-106.001007</X>
                            <Y dataType="Float">18</Y>
                          </item>
                          <item dataType="Struct" type="OpenTK.Vector2">
                            <X dataType="Float">55.9989929</X>
                            <Y dataType="Float">51</Y>
                          </item>
                          <item dataType="Struct" type="OpenTK.Vector2">
                            <X dataType="Float">115.998993</X>
                            <Y dataType="Float">-4</Y>
                          </item>
                        </vertices>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                    <_version dataType="Int">2</_version>
                  </shapes>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="795579554">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                  </areaMaterial>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customAreaMaterial />
                  <customOutlineMaterial />
                  <gameobj dataType="ObjectRef">196886364</gameobj>
                  <offset dataType="Int">0</offset>
                  <outlineMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                  </outlineMaterial>
                  <outlineWidth dataType="Float">3</outlineWidth>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                  <wrapTexture dataType="Bool">true</wrapTexture>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
              <_version dataType="Int">3</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4064420062" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="46517002">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2826137326</item>
                  <item dataType="ObjectRef">4016316454</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="1322274330">
                  <item dataType="ObjectRef">2557201296</item>
                  <item dataType="ObjectRef">3259662888</item>
                  <item dataType="ObjectRef">795579554</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2557201296</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="766864874">l3a1CwkMq0mG8wzpHgirYA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">RigidBodyRenderer</name>
            <parent dataType="ObjectRef">2538874541</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="4124357391">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="600232639">
              <_items dataType="Array" type="Duality.Component[]" id="1456171438" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2189705027">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">5.7769413</angle>
                  <angleAbs dataType="Float">5.7769413</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">4124357391</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">-31.9979858</X>
                    <Y dataType="Float">338</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">-31.9979858</X>
                    <Y dataType="Float">338</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">0.343205839</scale>
                  <scaleAbs dataType="Float">0.343205839</scaleAbs>
                  <vel dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2892166619">
                  <active dataType="Bool">true</active>
                  <angularDamp dataType="Float">0.3</angularDamp>
                  <angularVel dataType="Float">0</angularVel>
                  <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                  <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                  <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                  <continous dataType="Bool">false</continous>
                  <explicitMass dataType="Float">0</explicitMass>
                  <fixedAngle dataType="Bool">false</fixedAngle>
                  <gameobj dataType="ObjectRef">4124357391</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.3</linearDamp>
                  <linearVel dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                  </linearVel>
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3294108795">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2589489494" length="4">
                      <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="2000952864">
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">2892166619</parent>
                        <position dataType="Struct" type="OpenTK.Vector2">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">0</Y>
                        </position>
                        <radius dataType="Float">128</radius>
                        <restitution dataType="Float">0.3</restitution>
                        <sensor dataType="Bool">false</sensor>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                    <_version dataType="Int">2</_version>
                  </shapes>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="428083285">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                  </areaMaterial>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customAreaMaterial />
                  <customOutlineMaterial />
                  <gameobj dataType="ObjectRef">4124357391</gameobj>
                  <offset dataType="Int">0</offset>
                  <outlineMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                  </outlineMaterial>
                  <outlineWidth dataType="Float">3</outlineWidth>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                  <wrapTexture dataType="Bool">true</wrapTexture>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
              <_version dataType="Int">3</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="116939232" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="2415517045">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2826137326</item>
                  <item dataType="ObjectRef">4016316454</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="2282684616">
                  <item dataType="ObjectRef">2189705027</item>
                  <item dataType="ObjectRef">2892166619</item>
                  <item dataType="ObjectRef">428083285</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2189705027</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="666898367">SPVMkJiliUa1C8AvzOfOzg==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">RigidBodyRenderer</name>
            <parent dataType="ObjectRef">2538874541</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">6</_size>
        <_version dataType="Int">6</_version>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3785674784">
        <_items dataType="Array" type="Duality.Component[]" id="3817115157" />
        <_size dataType="Int">0</_size>
        <_version dataType="Int">0</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3815831309" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="2973954980" />
          <values dataType="Array" type="Duality.Component[]" id="665960214" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3055162016">8G1dvgnJqUuA7OCAA2pt9A==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">RigidStuff</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">3691642060</item>
    <item dataType="Struct" type="Duality.GameObject" id="4022368584">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="879246926">
        <_items dataType="Array" type="Duality.Component[]" id="916348624" length="4">
          <item dataType="Struct" type="Duality.Components.Diagnostics.ProfileRenderer" id="4289203476">
            <active dataType="Bool">true</active>
            <counterGraphs dataType="Struct" type="System.Collections.Generic.List`1[[System.String]]" id="4051744604">
              <_items dataType="Array" type="System.String[]" id="3679289540">
                <item dataType="String">Duality\Frame</item>
                <item dataType="String">Duality\Frame\Render</item>
                <item dataType="String">Duality\Frame\Update</item>
                <item dataType="String">Duality\Stats\Memory\TotalUsage</item>
              </_items>
              <_size dataType="Int">4</_size>
              <_version dataType="Int">4</_version>
            </counterGraphs>
            <drawGraphs dataType="Bool">false</drawGraphs>
            <gameobj dataType="ObjectRef">4022368584</gameobj>
            <keyToggleGraph dataType="Enum" type="OpenTK.Input.Key" name="F4" value="13" />
            <keyToggleTextPerf dataType="Enum" type="OpenTK.Input.Key" name="F2" value="11" />
            <keyToggleTextStat dataType="Enum" type="OpenTK.Input.Key" name="F3" value="12" />
            <textReportOptions dataType="Enum" type="Duality.ProfileReportOptions" name="LastValue" value="1" />
            <textReportPerf dataType="Bool">false</textReportPerf>
            <textReportStat dataType="Bool">false</textReportStat>
            <updateInterval dataType="Int">250</updateInterval>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
        <_version dataType="Int">1</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4268345930" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="3953058060">
            <item dataType="Type" id="315054244" value="Duality.Components.Diagnostics.ProfileRenderer" />
          </keys>
          <values dataType="Array" type="Duality.Component[]" id="1301632758">
            <item dataType="ObjectRef">4289203476</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2895640472">mDjM+AzTvUG5scSFR7ZRbQ==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">ProfileRenderer</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="1385290432">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1561715334">
        <_items dataType="Array" type="Duality.GameObject[]" id="680943488" length="16">
          <item dataType="Struct" type="Duality.GameObject" id="3034479095">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2479421587">
              <_items dataType="Array" type="Duality.GameObject[]" id="461756646" length="8">
                <item dataType="Struct" type="Duality.GameObject" id="1043620744">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1382186340">
                    <_items dataType="Array" type="Duality.Component[]" id="1073961924" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="3403935676">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1043620744</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="751056125">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1043620744</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="576975382" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1955749934">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="Type" id="2684742480" value="Duality.Components.Renderers.AnimSpriteRenderer" />
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="1835914442">
                        <item dataType="ObjectRef">3403935676</item>
                        <item dataType="ObjectRef">751056125</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">3403935676</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="84216222">ViIBKjV3TUGQsGPOSLxAkw==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Eye</name>
                  <parent dataType="ObjectRef">3034479095</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="3885667949">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3958539357">
                    <_items dataType="Array" type="Duality.Component[]" id="2156094310" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1951015585">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3885667949</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1232867221">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3885667949</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1638345592" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="2815035191">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="3800489536">
                        <item dataType="ObjectRef">1951015585</item>
                        <item dataType="ObjectRef">1232867221</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1951015585</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3384478485">xd1Ep96ZxkubYNnhBcLjbg==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopRight</name>
                  <parent dataType="ObjectRef">3034479095</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="840789122">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1896215382">
                    <_items dataType="Array" type="Duality.Component[]" id="3501786656" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="3201104054">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">840789122</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2482955690">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">840789122</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3869006042" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="4202226468">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="1112262934">
                        <item dataType="ObjectRef">3201104054</item>
                        <item dataType="ObjectRef">2482955690</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">3201104054</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="563989792">zSWm9rs0i0mZ8fDCeIaqxA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomLeft</name>
                  <parent dataType="ObjectRef">3034479095</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="3795653702">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1698448522">
                    <_items dataType="Array" type="Duality.Component[]" id="2570748896" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1861001338">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3795653702</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1142852974">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3795653702</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="172897562" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="2136330096">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="1667872494">
                        <item dataType="ObjectRef">1861001338</item>
                        <item dataType="ObjectRef">1142852974</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1861001338</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1113786572">3VsLGzbjlk++3hrIAHwEDQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopLeft</name>
                  <parent dataType="ObjectRef">3034479095</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="2546580953">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="430819673">
                    <_items dataType="Array" type="Duality.Component[]" id="3923555278" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="611928589">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2546580953</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="4188747521">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2546580953</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1750136832" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="415447923">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="3318478776">
                        <item dataType="ObjectRef">611928589</item>
                        <item dataType="ObjectRef">4188747521</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">611928589</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="838021273">vPaXN15OvkuE6+5PcUIP2w==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomRight</name>
                  <parent dataType="ObjectRef">3034479095</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="108267256">
              <_items dataType="Array" type="Duality.Component[]" id="1523111161" length="8">
                <item dataType="Struct" type="Duality.Components.Transform" id="1099826731">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">3034479095</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="381678367">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">3034479095</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1802288323">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">3034479095</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="4256990461">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">3034479095</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="4110587027">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">3034479095</gameobj>
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2740747129" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="4194494292">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2838274704</item>
                  <item dataType="ObjectRef">2826137326</item>
                  <item dataType="ObjectRef">1216776044</item>
                  <item dataType="Type" id="1852891876" value="DualStickSpaceShooter.EnemyClaymore" />
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="1446419894">
                  <item dataType="ObjectRef">1099826731</item>
                  <item dataType="ObjectRef">381678367</item>
                  <item dataType="ObjectRef">1802288323</item>
                  <item dataType="ObjectRef">4256990461</item>
                  <item dataType="ObjectRef">4110587027</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1099826731</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1121961072">wz6/12G8X0effd8XS9Sgaw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">EnemyClaymore</name>
            <parent dataType="ObjectRef">1385290432</parent>
            <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="2279551590">
              <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="1204889191">
                <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="162006094" length="4">
                  <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                    <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3907885452">
                      <_items dataType="Array" type="System.Int32[]" id="1034047396" />
                      <_size dataType="Int">0</_size>
                      <_version dataType="Int">1</_version>
                    </childIndex>
                    <componentType dataType="ObjectRef">4020116262</componentType>
                    <prop dataType="ObjectRef">3402294738</prop>
                    <val dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">152.897186</X>
                      <Y dataType="Float">22.8858814</Y>
                      <Z dataType="Float">0</Z>
                    </val>
                  </item>
                </_items>
                <_size dataType="Int">1</_size>
                <_version dataType="Int">332</_version>
              </changes>
              <obj dataType="ObjectRef">3034479095</obj>
              <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                <contentPath dataType="String">Data\Prefabs\EnemyClaymore.Prefab.res</contentPath>
              </prefab>
            </prefabLink>
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="143430740">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3298847420">
              <_items dataType="Array" type="Duality.GameObject[]" id="856351300" length="8">
                <item dataType="Struct" type="Duality.GameObject" id="3013370658">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1422490386">
                    <_items dataType="Array" type="Duality.Component[]" id="3012301904" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1078718294">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3013370658</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="2720806039">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3013370658</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3696157130" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="3843670856">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2684742480</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="4044070110">
                        <item dataType="ObjectRef">1078718294</item>
                        <item dataType="ObjectRef">2720806039</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1078718294</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3930710964">0ihEndiQVUu4C74038bVpQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Eye</name>
                  <parent dataType="ObjectRef">143430740</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="512852555">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1617300583">
                    <_items dataType="Array" type="Duality.Component[]" id="255843406" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2873167487">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">512852555</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2155019123">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">512852555</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2416735872" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="258457165">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="3083112632">
                        <item dataType="ObjectRef">2873167487</item>
                        <item dataType="ObjectRef">2155019123</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2873167487</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="34961959">+M4rp7odJEySIqTUOzNblw==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopRight</name>
                  <parent dataType="ObjectRef">143430740</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="380080983">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2870676043">
                    <_items dataType="Array" type="Duality.Component[]" id="2618155766" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2740395915">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">380080983</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2022247551">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">380080983</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1864544072" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="3682623585">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="3123675168">
                        <item dataType="ObjectRef">2740395915</item>
                        <item dataType="ObjectRef">2022247551</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2740395915</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="543866099">NNdGxOF6mESiqajW5ebc+w==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomLeft</name>
                  <parent dataType="ObjectRef">143430740</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="404360881">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1950386957">
                    <_items dataType="Array" type="Duality.Component[]" id="3925077798" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2764675813">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">404360881</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2046527449">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">404360881</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="216304056" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="678515815">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="3766625920">
                        <item dataType="ObjectRef">2764675813</item>
                        <item dataType="ObjectRef">2046527449</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2764675813</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2491197733">jUD/MuUPAE6IByXv4IsYPA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopLeft</name>
                  <parent dataType="ObjectRef">143430740</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="3199545466">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="483616154">
                    <_items dataType="Array" type="Duality.Component[]" id="3525512064" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1264893102">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3199545466</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="546744738">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3199545466</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="201235258" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1030428640">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="490410894">
                        <item dataType="ObjectRef">1264893102</item>
                        <item dataType="ObjectRef">546744738</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1264893102</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3645122300">bnjVWGB6WEivgJyJkXy5TA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomRight</name>
                  <parent dataType="ObjectRef">143430740</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3332006550">
              <_items dataType="Array" type="Duality.Component[]" id="1139719062" length="8">
                <item dataType="Struct" type="Duality.Components.Transform" id="2503745672">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">143430740</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1785597308">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">143430740</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3206207264">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">143430740</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="1365942106">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">143430740</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="1219538672">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">143430740</gameobj>
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4017882728" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="2369946328">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2838274704</item>
                  <item dataType="ObjectRef">2826137326</item>
                  <item dataType="ObjectRef">1216776044</item>
                  <item dataType="ObjectRef">1852891876</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="1028219038">
                  <item dataType="ObjectRef">2503745672</item>
                  <item dataType="ObjectRef">1785597308</item>
                  <item dataType="ObjectRef">3206207264</item>
                  <item dataType="ObjectRef">1365942106</item>
                  <item dataType="ObjectRef">1219538672</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2503745672</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3190293124">E96x5z1RBkqPP76+xpnNQQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">EnemyClaymore</name>
            <parent dataType="ObjectRef">1385290432</parent>
            <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="2719872370">
              <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="985462250">
                <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="3918637344" length="4">
                  <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                    <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="2686455440">
                      <_items dataType="ObjectRef">1034047396</_items>
                      <_size dataType="Int">0</_size>
                      <_version dataType="Int">1</_version>
                    </childIndex>
                    <componentType dataType="ObjectRef">4020116262</componentType>
                    <prop dataType="ObjectRef">3402294738</prop>
                    <val dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">167.01825</X>
                      <Y dataType="Float">101.769142</Y>
                      <Z dataType="Float">0</Z>
                    </val>
                  </item>
                </_items>
                <_size dataType="Int">1</_size>
                <_version dataType="Int">152</_version>
              </changes>
              <obj dataType="ObjectRef">143430740</obj>
              <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                <contentPath dataType="String">Data\Prefabs\EnemyClaymore.Prefab.res</contentPath>
              </prefab>
            </prefabLink>
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2219384449">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3336745125">
              <_items dataType="Array" type="Duality.GameObject[]" id="3040519574" length="8">
                <item dataType="Struct" type="Duality.GameObject" id="1314617617">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2897128113">
                    <_items dataType="Array" type="Duality.Component[]" id="1342049838" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="3674932549">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1314617617</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="1022052998">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1314617617</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="321232992" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="3454699419">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2684742480</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="537414248">
                        <item dataType="ObjectRef">3674932549</item>
                        <item dataType="ObjectRef">1022052998</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">3674932549</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3633759313">UT2wt+tFtUSLdbxHAUJM9Q==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Eye</name>
                  <parent dataType="ObjectRef">2219384449</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="4268791402">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2748379038">
                    <_items dataType="Array" type="Duality.Component[]" id="784045968" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2334139038">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">4268791402</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1615990674">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">4268791402</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1277979018" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="2360799932">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="599208598">
                        <item dataType="ObjectRef">2334139038</item>
                        <item dataType="ObjectRef">1615990674</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2334139038</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2307468904">S9Ti75J7NUKsa2okDf29FA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopRight</name>
                  <parent dataType="ObjectRef">2219384449</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="421565130">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3502863678">
                    <_items dataType="Array" type="Duality.Component[]" id="3546507792" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2781880062">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">421565130</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2063731698">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">421565130</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1353062922" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="3337026332">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="163102230">
                        <item dataType="ObjectRef">2781880062</item>
                        <item dataType="ObjectRef">2063731698</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2781880062</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="113649800">5tVjRkOsF0efbPNReDE1Ow==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomLeft</name>
                  <parent dataType="ObjectRef">2219384449</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="4136792166">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2583296714">
                    <_items dataType="Array" type="Duality.Component[]" id="748573536" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2202139802">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">4136792166</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1483991438">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">4136792166</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3342505626" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="2706504624">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="190439790">
                        <item dataType="ObjectRef">2202139802</item>
                        <item dataType="ObjectRef">1483991438</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2202139802</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2682775564">0TO6iigCn0Ocbn5QPf/9KA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopLeft</name>
                  <parent dataType="ObjectRef">2219384449</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="2073150401">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3975453921">
                    <_items dataType="Array" type="Duality.Component[]" id="488662894" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="138498037">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2073150401</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3715316969">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2073150401</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="212122144" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1611385195">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="3434104520">
                        <item dataType="ObjectRef">138498037</item>
                        <item dataType="ObjectRef">3715316969</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">138498037</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3138110049">fY8SQ4N3rEyfUk/9evHeEA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomRight</name>
                  <parent dataType="ObjectRef">2219384449</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1146975336">
              <_items dataType="Array" type="Duality.Component[]" id="1954623311" length="8">
                <item dataType="Struct" type="Duality.Components.Transform" id="284732085">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2219384449</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3861551017">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2219384449</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="987193677">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2219384449</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="3441895815">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2219384449</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="3295492381">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2219384449</gameobj>
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="240635247" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="137252068">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2838274704</item>
                  <item dataType="ObjectRef">2826137326</item>
                  <item dataType="ObjectRef">1216776044</item>
                  <item dataType="ObjectRef">1852891876</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="2352197142">
                  <item dataType="ObjectRef">284732085</item>
                  <item dataType="ObjectRef">3861551017</item>
                  <item dataType="ObjectRef">987193677</item>
                  <item dataType="ObjectRef">3441895815</item>
                  <item dataType="ObjectRef">3295492381</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">284732085</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2725503712">RV63zf6bREChZNKEjrsugA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">EnemyClaymore</name>
            <parent dataType="ObjectRef">1385290432</parent>
            <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="1222538166">
              <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="2583177489">
                <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="2849971950" length="4">
                  <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                    <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3078063628">
                      <_items dataType="ObjectRef">1034047396</_items>
                      <_size dataType="Int">0</_size>
                      <_version dataType="Int">1</_version>
                    </childIndex>
                    <componentType dataType="ObjectRef">4020116262</componentType>
                    <prop dataType="ObjectRef">3402294738</prop>
                    <val dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">184.060928</X>
                      <Y dataType="Float">-9.738673</Y>
                      <Z dataType="Float">0</Z>
                    </val>
                  </item>
                </_items>
                <_size dataType="Int">1</_size>
                <_version dataType="Int">396</_version>
              </changes>
              <obj dataType="ObjectRef">2219384449</obj>
              <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                <contentPath dataType="String">Data\Prefabs\EnemyClaymore.Prefab.res</contentPath>
              </prefab>
            </prefabLink>
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2645764365">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1867126137">
              <_items dataType="Array" type="Duality.GameObject[]" id="3731237710" length="8">
                <item dataType="Struct" type="Duality.GameObject" id="38335236">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2863037520">
                    <_items dataType="Array" type="Duality.Component[]" id="2000540092" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2398650168">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">38335236</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="4040737913">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">38335236</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1594392942" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1972855074">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2684742480</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="307812106">
                        <item dataType="ObjectRef">2398650168</item>
                        <item dataType="ObjectRef">4040737913</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2398650168</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="4001944274">H7fXtJ/SSESMC2RlRLfCUw==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Eye</name>
                  <parent dataType="ObjectRef">2645764365</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="3887235839">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="28976335">
                    <_items dataType="Array" type="Duality.Component[]" id="562362414" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1952583475">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3887235839</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1234435111">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3887235839</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1489835616" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="222669029">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="1493544808">
                        <item dataType="ObjectRef">1952583475</item>
                        <item dataType="ObjectRef">1234435111</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1952583475</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="893809711">/8KToyHXY0CnXZMJbv2SYw==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopRight</name>
                  <parent dataType="ObjectRef">2645764365</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="1573782093">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2413542061">
                    <_items dataType="Array" type="Duality.Component[]" id="718432102" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="3934097025">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1573782093</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3215948661">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1573782093</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4139310968" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1482008519">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="531849472">
                        <item dataType="ObjectRef">3934097025</item>
                        <item dataType="ObjectRef">3215948661</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">3934097025</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1191247941">JU6eUFAirUyU2S90zmWTxQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomLeft</name>
                  <parent dataType="ObjectRef">2645764365</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="1724914615">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2198296135">
                    <_items dataType="Array" type="Duality.Component[]" id="1267187918" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="4085229547">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1724914615</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3367081183">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1724914615</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3930790144" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="2384658925">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="2079241464">
                        <item dataType="ObjectRef">4085229547</item>
                        <item dataType="ObjectRef">3367081183</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">4085229547</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1943381255">tVHLAxsgXkq4GkUUsmMeXA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopLeft</name>
                  <parent dataType="ObjectRef">2645764365</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="2714806266">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="92850846">
                    <_items dataType="Array" type="Duality.Component[]" id="2202141584" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="780153902">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2714806266</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="62005538">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2714806266</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4195419530" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1132452284">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="7314070">
                        <item dataType="ObjectRef">780153902</item>
                        <item dataType="ObjectRef">62005538</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">780153902</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1816121192">5u5Sf4JbdkmAEYX5rq3Zag==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomRight</name>
                  <parent dataType="ObjectRef">2645764365</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1398775168">
              <_items dataType="Array" type="Duality.Component[]" id="963994835" length="8">
                <item dataType="Struct" type="Duality.Components.Transform" id="711112001">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2645764365</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="4287930933">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2645764365</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1413573593">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2645764365</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="3868275731">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2645764365</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="3721872297">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2645764365</gameobj>
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="533428859" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="2187858964">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2838274704</item>
                  <item dataType="ObjectRef">2826137326</item>
                  <item dataType="ObjectRef">1216776044</item>
                  <item dataType="ObjectRef">1852891876</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="749775670">
                  <item dataType="ObjectRef">711112001</item>
                  <item dataType="ObjectRef">4287930933</item>
                  <item dataType="ObjectRef">1413573593</item>
                  <item dataType="ObjectRef">3868275731</item>
                  <item dataType="ObjectRef">3721872297</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">711112001</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1624374448">LWfvA1zdGk6i+0PRkFhmOg==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">EnemyClaymore</name>
            <parent dataType="ObjectRef">1385290432</parent>
            <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="4247917990">
              <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="1806344853">
                <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="2046850166" length="4">
                  <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                    <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1583019580">
                      <_items dataType="Array" type="System.Int32[]" id="591616324" />
                      <_size dataType="Int">0</_size>
                      <_version dataType="Int">1</_version>
                    </childIndex>
                    <componentType dataType="ObjectRef">4020116262</componentType>
                    <prop dataType="ObjectRef">3402294738</prop>
                    <val dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">141.519073</X>
                      <Y dataType="Float">-120.812485</Y>
                      <Z dataType="Float">0</Z>
                    </val>
                  </item>
                </_items>
                <_size dataType="Int">1</_size>
                <_version dataType="Int">125</_version>
              </changes>
              <obj dataType="ObjectRef">2645764365</obj>
              <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                <contentPath dataType="String">Data\Prefabs\EnemyClaymore.Prefab.res</contentPath>
              </prefab>
            </prefabLink>
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2269227541">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2120482465">
              <_items dataType="Array" type="Duality.GameObject[]" id="244139118" length="8">
                <item dataType="Struct" type="Duality.GameObject" id="3258964135">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="192693719">
                    <_items dataType="Array" type="Duality.Component[]" id="1092747790" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1324311771">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3258964135</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="2966399516">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3258964135</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2568068032" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="2942111837">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2684742480</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="3838065528">
                        <item dataType="ObjectRef">1324311771</item>
                        <item dataType="ObjectRef">2966399516</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1324311771</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2009390071">L5hD93yXpECkWqLQ4iYrLA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Eye</name>
                  <parent dataType="ObjectRef">2269227541</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="2391527542">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3564867018">
                    <_items dataType="Array" type="Duality.Component[]" id="1316269408" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="456875178">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2391527542</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="4033694110">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2391527542</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="42538138" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="2629814960">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="1276510062">
                        <item dataType="ObjectRef">456875178</item>
                        <item dataType="ObjectRef">4033694110</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">456875178</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="649644300">7ZT/l3Sd8ESQNoR6lIDyaw==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopRight</name>
                  <parent dataType="ObjectRef">2269227541</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="3130008096">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3592232172">
                    <_items dataType="Array" type="Duality.Component[]" id="3100086372" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1195355732">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3130008096</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="477207368">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3130008096</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2477101366" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="3972891430">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="2023498426">
                        <item dataType="ObjectRef">1195355732</item>
                        <item dataType="ObjectRef">477207368</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1195355732</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="643227686">6G6e/w4iXkyFumcCXCY5lg==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomLeft</name>
                  <parent dataType="ObjectRef">2269227541</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="3742889763">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2599029475">
                    <_items dataType="Array" type="Duality.Component[]" id="3945172198" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1808237399">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3742889763</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1090089035">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3742889763</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="274139896" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="4163133833">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="1373762880">
                        <item dataType="ObjectRef">1808237399</item>
                        <item dataType="ObjectRef">1090089035</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1808237399</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="496097579">JrR1hmJjC0OknToBBbZ7Dg==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopLeft</name>
                  <parent dataType="ObjectRef">2269227541</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="4073648570">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3614797150">
                    <_items dataType="Array" type="Duality.Component[]" id="2617929488" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2138996206">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">4073648570</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1420847842">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">4073648570</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="975835914" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="187098748">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="3457518742">
                        <item dataType="ObjectRef">2138996206</item>
                        <item dataType="ObjectRef">1420847842</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2138996206</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1935535144">ybdZ+pevDka2t6dubX6YfQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomRight</name>
                  <parent dataType="ObjectRef">2269227541</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3372665632">
              <_items dataType="Array" type="Duality.Component[]" id="2346989483" length="8">
                <item dataType="Struct" type="Duality.Components.Transform" id="334575177">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2269227541</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3911394109">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2269227541</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1037036769">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2269227541</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="3491738907">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2269227541</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="3345335473">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2269227541</gameobj>
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2727365939" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="2404668324">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2838274704</item>
                  <item dataType="ObjectRef">2826137326</item>
                  <item dataType="ObjectRef">1216776044</item>
                  <item dataType="ObjectRef">1852891876</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="2108374806">
                  <item dataType="ObjectRef">334575177</item>
                  <item dataType="ObjectRef">3911394109</item>
                  <item dataType="ObjectRef">1037036769</item>
                  <item dataType="ObjectRef">3491738907</item>
                  <item dataType="ObjectRef">3345335473</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">334575177</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1014231712">euAnA2YuAUqdy6rYvbCXbg==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">EnemyClaymore</name>
            <parent dataType="ObjectRef">1385290432</parent>
            <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="495802870">
              <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="2886162429">
                <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="3369012518" length="4">
                  <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                    <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="2004220828">
                      <_items dataType="Array" type="System.Int32[]" id="3245693380" />
                      <_size dataType="Int">0</_size>
                      <_version dataType="Int">1</_version>
                    </childIndex>
                    <componentType dataType="ObjectRef">4020116262</componentType>
                    <prop dataType="ObjectRef">3402294738</prop>
                    <val dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">-121.998993</X>
                      <Y dataType="Float">-121.5</Y>
                      <Z dataType="Float">0</Z>
                    </val>
                  </item>
                </_items>
                <_size dataType="Int">1</_size>
                <_version dataType="Int">165</_version>
              </changes>
              <obj dataType="ObjectRef">2269227541</obj>
              <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                <contentPath dataType="String">Data\Prefabs\EnemyClaymore.Prefab.res</contentPath>
              </prefab>
            </prefabLink>
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="242932114">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="815160106">
              <_items dataType="Array" type="Duality.GameObject[]" id="868446496" length="8">
                <item dataType="Struct" type="Duality.GameObject" id="2728913501">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="744782185">
                    <_items dataType="Array" type="Duality.Component[]" id="2035471630" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="794261137">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2728913501</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="2436348882">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2728913501</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="939383488" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="855164387">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2684742480</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="727751416">
                        <item dataType="ObjectRef">794261137</item>
                        <item dataType="ObjectRef">2436348882</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">794261137</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1744781641">26PNWGo7Gkm8kME1r7EPnA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Eye</name>
                  <parent dataType="ObjectRef">242932114</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="4236686595">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1351543607">
                    <_items dataType="Array" type="Duality.Component[]" id="2845147790" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2302034231">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">4236686595</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1583885867">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">4236686595</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1593880128" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="649210237">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="545146296">
                        <item dataType="ObjectRef">2302034231</item>
                        <item dataType="ObjectRef">1583885867</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2302034231</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3020955223">rv2ZZh9DUUiJl9lcJkeT/A==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopRight</name>
                  <parent dataType="ObjectRef">242932114</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="465249611">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="590708415">
                    <_items dataType="Array" type="Duality.Component[]" id="2679892398" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2825564543">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">465249611</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2107416179">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">465249611</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="190568928" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1705939317">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="1068613832">
                        <item dataType="ObjectRef">2825564543</item>
                        <item dataType="ObjectRef">2107416179</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2825564543</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1007164351">LW6dPyaWHUuP57KB/Y8Kew==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomLeft</name>
                  <parent dataType="ObjectRef">242932114</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="336767302">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="142773574">
                    <_items dataType="Array" type="Duality.Component[]" id="941314560" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2697082234">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">336767302</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1978933870">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">336767302</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="904514490" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="297206452">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="2944214006">
                        <item dataType="ObjectRef">2697082234</item>
                        <item dataType="ObjectRef">1978933870</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2697082234</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1647282448">wu4Pqh2PRkeDcDF+Amxnwg==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopLeft</name>
                  <parent dataType="ObjectRef">242932114</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="367029264">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1666987008">
                    <_items dataType="Array" type="Duality.Component[]" id="1422573724" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2727344196">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">367029264</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2009195832">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">367029264</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3678888910" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="217353938">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="2012119242">
                        <item dataType="ObjectRef">2727344196</item>
                        <item dataType="ObjectRef">2009195832</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2727344196</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2542553954">syIrJssH1E6MATT9m/5xdw==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomRight</name>
                  <parent dataType="ObjectRef">242932114</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3613037530">
              <_items dataType="Array" type="Duality.Component[]" id="3971813904" length="8">
                <item dataType="Struct" type="Duality.Components.Transform" id="2603247046">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">242932114</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1885098682">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">242932114</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3305708638">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">242932114</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="1465443480">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">242932114</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="1319040046">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">242932114</gameobj>
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3871162890" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="3313197376">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2838274704</item>
                  <item dataType="ObjectRef">2826137326</item>
                  <item dataType="ObjectRef">1216776044</item>
                  <item dataType="ObjectRef">1852891876</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="1959621198">
                  <item dataType="ObjectRef">2603247046</item>
                  <item dataType="ObjectRef">1885098682</item>
                  <item dataType="ObjectRef">3305708638</item>
                  <item dataType="ObjectRef">1465443480</item>
                  <item dataType="ObjectRef">1319040046</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2603247046</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3496618972">w5xPYjX5SkWC8JUJKTiJGw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">EnemyClaymore</name>
            <parent dataType="ObjectRef">1385290432</parent>
            <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="3711143034">
              <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="3170192112">
                <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="3391064892" length="4">
                  <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                    <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="4115980104">
                      <_items dataType="Array" type="System.Int32[]" id="3031576684" />
                      <_size dataType="Int">0</_size>
                      <_version dataType="Int">1</_version>
                    </childIndex>
                    <componentType dataType="ObjectRef">4020116262</componentType>
                    <prop dataType="ObjectRef">3402294738</prop>
                    <val dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">322.002</X>
                      <Y dataType="Float">-93.5</Y>
                      <Z dataType="Float">0</Z>
                    </val>
                  </item>
                </_items>
                <_size dataType="Int">1</_size>
                <_version dataType="Int">539</_version>
              </changes>
              <obj dataType="ObjectRef">242932114</obj>
              <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                <contentPath dataType="String">Data\Prefabs\EnemyClaymore.Prefab.res</contentPath>
              </prefab>
            </prefabLink>
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2389062831">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3998739755">
              <_items dataType="Array" type="Duality.GameObject[]" id="186619382" length="8">
                <item dataType="Struct" type="Duality.GameObject" id="3911303751">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3112804071">
                    <_items dataType="Array" type="Duality.Component[]" id="3596919118" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1976651387">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3911303751</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="3618739132">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3911303751</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4243918720" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="338365389">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2684742480</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="2233209016">
                        <item dataType="ObjectRef">1976651387</item>
                        <item dataType="ObjectRef">3618739132</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1976651387</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1621236135">1IgZojiFnkqR0c6DGVGvDQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Eye</name>
                  <parent dataType="ObjectRef">2389062831</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="3918154608">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3141054316">
                    <_items dataType="Array" type="Duality.Component[]" id="3462915940" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1983502244">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3918154608</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1265353880">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3918154608</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1089782838" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1002221990">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="3497171386">
                        <item dataType="ObjectRef">1983502244</item>
                        <item dataType="ObjectRef">1265353880</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1983502244</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2961012134">OAMwU7q6kEiN0wAL+yrs2A==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopRight</name>
                  <parent dataType="ObjectRef">2389062831</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="392384898">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="318242294">
                    <_items dataType="Array" type="Duality.Component[]" id="2306686688" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2752699830">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">392384898</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2034551466">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">392384898</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="867433498" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="3242042564">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="4180396950">
                        <item dataType="ObjectRef">2752699830</item>
                        <item dataType="ObjectRef">2034551466</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2752699830</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1650025856">tBJbljhcd0OSUN0lUleDQQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomLeft</name>
                  <parent dataType="ObjectRef">2389062831</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="1643744387">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="629235635">
                    <_items dataType="Array" type="Duality.Component[]" id="3493829158" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="4004059319">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1643744387</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3285910955">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1643744387</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3475439800" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1627647449">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="2897334784">
                        <item dataType="ObjectRef">4004059319</item>
                        <item dataType="ObjectRef">3285910955</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">4004059319</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="292073627">5M4nAt616UK/UtZlb/kkjg==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopLeft</name>
                  <parent dataType="ObjectRef">2389062831</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="2336954970">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1655787790">
                    <_items dataType="Array" type="Duality.Component[]" id="3126728144" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="402302606">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2336954970</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3979121538">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2336954970</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="674830666" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="4276863180">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="4216893174">
                        <item dataType="ObjectRef">402302606</item>
                        <item dataType="ObjectRef">3979121538</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">402302606</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1746940632">vlhS5Rmp80KscccZ5b3wFw==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomRight</name>
                  <parent dataType="ObjectRef">2389062831</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1751535176">
              <_items dataType="Array" type="Duality.Component[]" id="21595137" length="8">
                <item dataType="Struct" type="Duality.Components.Transform" id="454410467">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2389062831</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="4031229399">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2389062831</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1156872059">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2389062831</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="3611574197">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2389062831</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="3465170763">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2389062831</gameobj>
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4086586657" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="2131256580">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2838274704</item>
                  <item dataType="ObjectRef">2826137326</item>
                  <item dataType="ObjectRef">1216776044</item>
                  <item dataType="ObjectRef">1852891876</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="2920902038">
                  <item dataType="ObjectRef">454410467</item>
                  <item dataType="ObjectRef">4031229399</item>
                  <item dataType="ObjectRef">1156872059</item>
                  <item dataType="ObjectRef">3611574197</item>
                  <item dataType="ObjectRef">3465170763</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">454410467</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="818028480">5H36zJzw2km8tbIVHltWkQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">EnemyClaymore</name>
            <parent dataType="ObjectRef">1385290432</parent>
            <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="2162250966">
              <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="1836734879">
                <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="3816231278" length="4">
                  <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                    <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3333340684">
                      <_items dataType="ObjectRef">591616324</_items>
                      <_size dataType="Int">0</_size>
                      <_version dataType="Int">1</_version>
                    </childIndex>
                    <componentType dataType="ObjectRef">4020116262</componentType>
                    <prop dataType="ObjectRef">3402294738</prop>
                    <val dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">241.906387</X>
                      <Y dataType="Float">-116.698242</Y>
                      <Z dataType="Float">0</Z>
                    </val>
                  </item>
                </_items>
                <_size dataType="Int">1</_size>
                <_version dataType="Int">607</_version>
              </changes>
              <obj dataType="ObjectRef">2389062831</obj>
              <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                <contentPath dataType="String">Data\Prefabs\EnemyClaymore.Prefab.res</contentPath>
              </prefab>
            </prefabLink>
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="347102484">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2497476092">
              <_items dataType="Array" type="Duality.GameObject[]" id="1284213572" length="8">
                <item dataType="Struct" type="Duality.GameObject" id="3973150936">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="56892240">
                    <_items dataType="Array" type="Duality.Component[]" id="4109779900" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2038498572">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3973150936</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="3680586317">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3973150936</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3927154542" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="238384674">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2684742480</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="2112201994">
                        <item dataType="ObjectRef">2038498572</item>
                        <item dataType="ObjectRef">3680586317</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2038498572</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1358523858">NDpE3ZW8tEK9541qPQIeWA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Eye</name>
                  <parent dataType="ObjectRef">347102484</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="1029338680">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2669826544">
                    <_items dataType="Array" type="Duality.Component[]" id="98353980" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="3389653612">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1029338680</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2671505248">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1029338680</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2460829934" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="3417245506">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="3482388490">
                        <item dataType="ObjectRef">3389653612</item>
                        <item dataType="ObjectRef">2671505248</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">3389653612</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2511608114">4j6Pv9Wqu0i60Ch3cxFaYQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopRight</name>
                  <parent dataType="ObjectRef">347102484</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="4284896073">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1168017685">
                    <_items dataType="Array" type="Duality.Component[]" id="2050353270" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2350243709">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">4284896073</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1632095345">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">4284896073</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1643150024" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="3095707839">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="2064277984">
                        <item dataType="ObjectRef">2350243709</item>
                        <item dataType="ObjectRef">1632095345</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2350243709</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1607428717">xBWGquDkVUafvrH7Tzfz2A==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomLeft</name>
                  <parent dataType="ObjectRef">347102484</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="1885801767">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2755442011">
                    <_items dataType="Array" type="Duality.Component[]" id="3599760790" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="4246116699">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1885801767</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3527968335">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1885801767</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3125349480" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="3922074289">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="3285573728">
                        <item dataType="ObjectRef">4246116699</item>
                        <item dataType="ObjectRef">3527968335</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">4246116699</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2932759395">y0+K1O11fEK8SNHL1i4DkA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopLeft</name>
                  <parent dataType="ObjectRef">347102484</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="3987883834">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3201202778">
                    <_items dataType="Array" type="Duality.Component[]" id="3522481152" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2053231470">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3987883834</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1335083106">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3987883834</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1927080378" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="4218575264">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="1241472654">
                        <item dataType="ObjectRef">2053231470</item>
                        <item dataType="ObjectRef">1335083106</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2053231470</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="684006844">xwEt4p8/vUahZDDivO/3QA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomRight</name>
                  <parent dataType="ObjectRef">347102484</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1565527958">
              <_items dataType="Array" type="Duality.Component[]" id="1065466710" length="8">
                <item dataType="Struct" type="Duality.Components.Transform" id="2707417416">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">347102484</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1989269052">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">347102484</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3409879008">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">347102484</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="1569613850">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">347102484</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="1423210416">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">347102484</gameobj>
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3624675496" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="487180376">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2838274704</item>
                  <item dataType="ObjectRef">2826137326</item>
                  <item dataType="ObjectRef">1216776044</item>
                  <item dataType="ObjectRef">1852891876</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="250651550">
                  <item dataType="ObjectRef">2707417416</item>
                  <item dataType="ObjectRef">1989269052</item>
                  <item dataType="ObjectRef">3409879008</item>
                  <item dataType="ObjectRef">1569613850</item>
                  <item dataType="ObjectRef">1423210416</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2707417416</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3098837764">VRLIh3jX7k+T6kSVpxZwrw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">EnemyClaymore</name>
            <parent dataType="ObjectRef">1385290432</parent>
            <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="2088288498">
              <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="3101654442">
                <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="3694979104" length="4">
                  <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                    <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1088424592">
                      <_items dataType="Array" type="System.Int32[]" id="3761560892" />
                      <_size dataType="Int">0</_size>
                      <_version dataType="Int">1</_version>
                    </childIndex>
                    <componentType dataType="ObjectRef">4020116262</componentType>
                    <prop dataType="ObjectRef">3402294738</prop>
                    <val dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">170.001</X>
                      <Y dataType="Float">-21.5</Y>
                      <Z dataType="Float">0</Z>
                    </val>
                  </item>
                </_items>
                <_size dataType="Int">1</_size>
                <_version dataType="Int">85</_version>
              </changes>
              <obj dataType="ObjectRef">347102484</obj>
              <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                <contentPath dataType="String">Data\Prefabs\EnemyClaymore.Prefab.res</contentPath>
              </prefab>
            </prefabLink>
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="1076594099">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3997618887">
              <_items dataType="Array" type="Duality.GameObject[]" id="907844302" length="8">
                <item dataType="Struct" type="Duality.GameObject" id="479828669">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3422799037">
                    <_items dataType="Array" type="Duality.Component[]" id="2591297062" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2840143601">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">479828669</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="187264050">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">479828669</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3533030584" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="3904223703">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2684742480</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="122328000">
                        <item dataType="ObjectRef">2840143601</item>
                        <item dataType="ObjectRef">187264050</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2840143601</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1601246709">TNOeTAcczUm7/Ew/ESdSPA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Eye</name>
                  <parent dataType="ObjectRef">1076594099</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="3760011772">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="413698856">
                    <_items dataType="Array" type="Duality.Component[]" id="352386988" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1825359408">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3760011772</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1107211044">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3760011772</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1676611742" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="2645911786">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="468715482">
                        <item dataType="ObjectRef">1825359408</item>
                        <item dataType="ObjectRef">1107211044</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1825359408</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2638186826">H3zzHe69yEWcDkCMEzJWKw==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopRight</name>
                  <parent dataType="ObjectRef">1076594099</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="2101574325">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2899813749">
                    <_items dataType="Array" type="Duality.Component[]" id="2944023158" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="166921961">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2101574325</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3743740893">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2101574325</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3284914376" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="943273439">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="822689056">
                        <item dataType="ObjectRef">166921961</item>
                        <item dataType="ObjectRef">3743740893</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">166921961</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3024500813">wDVJSwqAKU+je7Zi7QHUXA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomLeft</name>
                  <parent dataType="ObjectRef">1076594099</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="727846504">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3810955636">
                    <_items dataType="Array" type="Duality.Component[]" id="3527819684" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="3088161436">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">727846504</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2370013072">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">727846504</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3931251702" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="2768121950">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="2187640074">
                        <item dataType="ObjectRef">3088161436</item>
                        <item dataType="ObjectRef">2370013072</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">3088161436</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3126345134">jkmdSv+BbkeD7uD74UYLiA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopLeft</name>
                  <parent dataType="ObjectRef">1076594099</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="1512653090">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2569883430">
                    <_items dataType="Array" type="Duality.Component[]" id="1611195648" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="3872968022">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1512653090</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3154819658">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1512653090</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="160287418" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="4256848020">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="1392944182">
                        <item dataType="ObjectRef">3872968022</item>
                        <item dataType="ObjectRef">3154819658</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">3872968022</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="837632304">+waTc7qxREe+CpjbYBfinQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomRight</name>
                  <parent dataType="ObjectRef">1076594099</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1346576128">
              <_items dataType="Array" type="Duality.Component[]" id="2796127597" length="8">
                <item dataType="Struct" type="Duality.Components.Transform" id="3436909031">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">1076594099</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2718760667">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">1076594099</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="4139370623">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">1076594099</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="2299105465">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">1076594099</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="2152702031">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">1076594099</gameobj>
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3359363397" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="1719473428">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2838274704</item>
                  <item dataType="ObjectRef">2826137326</item>
                  <item dataType="ObjectRef">1216776044</item>
                  <item dataType="ObjectRef">1852891876</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="2720172342">
                  <item dataType="ObjectRef">3436909031</item>
                  <item dataType="ObjectRef">2718760667</item>
                  <item dataType="ObjectRef">4139370623</item>
                  <item dataType="ObjectRef">2299105465</item>
                  <item dataType="ObjectRef">2152702031</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3436909031</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3482957744">TsVDD2gw00CDAH/C0jSfLg==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">EnemyClaymore</name>
            <parent dataType="ObjectRef">1385290432</parent>
            <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="1144946342">
              <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="3120732203">
                <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="2604844022" length="4">
                  <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                    <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1926704956">
                      <_items dataType="ObjectRef">3031576684</_items>
                      <_size dataType="Int">0</_size>
                      <_version dataType="Int">1</_version>
                    </childIndex>
                    <componentType dataType="ObjectRef">4020116262</componentType>
                    <prop dataType="ObjectRef">3402294738</prop>
                    <val dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">184.001</X>
                      <Y dataType="Float">-333.5</Y>
                      <Z dataType="Float">0</Z>
                    </val>
                  </item>
                </_items>
                <_size dataType="Int">1</_size>
                <_version dataType="Int">217</_version>
              </changes>
              <obj dataType="ObjectRef">1076594099</obj>
              <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                <contentPath dataType="String">Data\Prefabs\EnemyClaymore.Prefab.res</contentPath>
              </prefab>
            </prefabLink>
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2213742814">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3879651646">
              <_items dataType="Array" type="Duality.GameObject[]" id="3611468304" length="8">
                <item dataType="Struct" type="Duality.GameObject" id="1976635711">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1232125499">
                    <_items dataType="Array" type="Duality.Component[]" id="3851323094" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="41983347">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1976635711</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="1684071092">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1976635711</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1446975016" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="3648699729">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2684742480</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="1293319328">
                        <item dataType="ObjectRef">41983347</item>
                        <item dataType="ObjectRef">1684071092</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">41983347</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3113519555">jlNVbYRZG0K2JUGpoY2uqg==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Eye</name>
                  <parent dataType="ObjectRef">2213742814</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="2078648859">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4016803599">
                    <_items dataType="Array" type="Duality.Component[]" id="1728518062" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="143996495">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2078648859</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3720815427">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2078648859</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2955338720" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="4285659813">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="3476860008">
                        <item dataType="ObjectRef">143996495</item>
                        <item dataType="ObjectRef">3720815427</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">143996495</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1933807471">9u+s80lFxkWxZc2JywRbKw==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopRight</name>
                  <parent dataType="ObjectRef">2213742814</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="1999761815">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2324683731">
                    <_items dataType="Array" type="Duality.Component[]" id="4006869606" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="65109451">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1999761815</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3641928383">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1999761815</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2595806840" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="2929639609">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="120093440">
                        <item dataType="ObjectRef">65109451</item>
                        <item dataType="ObjectRef">3641928383</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">65109451</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1115326779">i7SldpSZyUW6hpgjRK2r1Q==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomLeft</name>
                  <parent dataType="ObjectRef">2213742814</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="1622304848">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3563871712">
                    <_items dataType="Array" type="Duality.Component[]" id="750916572" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="3982619780">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1622304848</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3264471416">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1622304848</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1264786318" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="762752306">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="2168767306">
                        <item dataType="ObjectRef">3982619780</item>
                        <item dataType="ObjectRef">3264471416</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">3982619780</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="420278146">EyPVv2cQ8EuwWs94w8FYsw==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopLeft</name>
                  <parent dataType="ObjectRef">2213742814</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="2663201263">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1496013707">
                    <_items dataType="Array" type="Duality.Component[]" id="3658731638" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="728548899">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2663201263</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="10400535">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2663201263</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3304973000" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="3788545825">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="1520779552">
                        <item dataType="ObjectRef">728548899</item>
                        <item dataType="ObjectRef">10400535</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">728548899</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3418958003">23vSqW82lEKRlAF6HCQYoA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomRight</name>
                  <parent dataType="ObjectRef">2213742814</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="496972298">
              <_items dataType="Array" type="Duality.Component[]" id="2333347612" length="8">
                <item dataType="Struct" type="Duality.Components.Transform" id="279090450">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2213742814</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3855909382">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2213742814</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="981552042">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2213742814</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="3436254180">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2213742814</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="3289850746">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2213742814</gameobj>
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2374236238" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="2207676576">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2838274704</item>
                  <item dataType="ObjectRef">2826137326</item>
                  <item dataType="ObjectRef">1216776044</item>
                  <item dataType="ObjectRef">1852891876</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="2752557198">
                  <item dataType="ObjectRef">279090450</item>
                  <item dataType="ObjectRef">3855909382</item>
                  <item dataType="ObjectRef">981552042</item>
                  <item dataType="ObjectRef">3436254180</item>
                  <item dataType="ObjectRef">3289850746</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">279090450</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="4237368508">ZULtHh76OEqYY2dMu2Utgw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">EnemyClaymore</name>
            <parent dataType="ObjectRef">1385290432</parent>
            <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="1109321050">
              <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="2683016812">
                <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="26588004" length="4">
                  <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                    <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="196025544">
                      <_items dataType="Array" type="System.Int32[]" id="4278858348" />
                      <_size dataType="Int">0</_size>
                      <_version dataType="Int">1</_version>
                    </childIndex>
                    <componentType dataType="ObjectRef">4020116262</componentType>
                    <prop dataType="ObjectRef">3402294738</prop>
                    <val dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">9.002014</X>
                      <Y dataType="Float">410.5</Y>
                      <Z dataType="Float">0</Z>
                    </val>
                  </item>
                </_items>
                <_size dataType="Int">1</_size>
                <_version dataType="Int">339</_version>
              </changes>
              <obj dataType="ObjectRef">2213742814</obj>
              <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                <contentPath dataType="String">Data\Prefabs\EnemyClaymore.Prefab.res</contentPath>
              </prefab>
            </prefabLink>
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2905865120">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1311963056">
              <_items dataType="Array" type="Duality.GameObject[]" id="330332604" length="8">
                <item dataType="Struct" type="Duality.GameObject" id="1718003899">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4050005655">
                    <_items dataType="Array" type="Duality.Component[]" id="3941152014" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="4078318831">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1718003899</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="1425439280">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1718003899</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1617541824" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1407877661">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2684742480</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="1580481272">
                        <item dataType="ObjectRef">4078318831</item>
                        <item dataType="ObjectRef">1425439280</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">4078318831</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="4099638967">IOL155O7h0qAdDpsxY7SHA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Eye</name>
                  <parent dataType="ObjectRef">2905865120</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="2625065684">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3705038548">
                    <_items dataType="Array" type="Duality.Component[]" id="1608785636" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="690413320">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2625065684</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="4267232252">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2625065684</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2851613110" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="627190398">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="4014018186">
                        <item dataType="ObjectRef">690413320</item>
                        <item dataType="ObjectRef">4267232252</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">690413320</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3290514190">zxtiRhWOO0mlX1QF83aoIA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopRight</name>
                  <parent dataType="ObjectRef">2905865120</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="1016569111">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1542181131">
                    <_items dataType="Array" type="Duality.Component[]" id="147145334" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="3376884043">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1016569111</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2658735679">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1016569111</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2772826312" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="2288650401">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="3024609056">
                        <item dataType="ObjectRef">3376884043</item>
                        <item dataType="ObjectRef">2658735679</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">3376884043</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3732717363">39oA6ZdyX0exH6fNqntRlw==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomLeft</name>
                  <parent dataType="ObjectRef">2905865120</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="4229974861">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1818592161">
                    <_items dataType="Array" type="Duality.Component[]" id="1655056494" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2295322497">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">4229974861</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1577174133">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">4229974861</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1105518368" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="127078571">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="3629959496">
                        <item dataType="ObjectRef">2295322497</item>
                        <item dataType="ObjectRef">1577174133</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2295322497</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1330587297">ZQ5KPWr8DEWjmD2rblvFJA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopLeft</name>
                  <parent dataType="ObjectRef">2905865120</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="120850460">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4083512780">
                    <_items dataType="Array" type="Duality.Component[]" id="3288267940" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2481165392">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">120850460</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1763017028">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">120850460</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1238756086" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="706164294">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="1756296634">
                        <item dataType="ObjectRef">2481165392</item>
                        <item dataType="ObjectRef">1763017028</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2481165392</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1055071814">cERZsG2lqkyK3RJisoE8xg==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomRight</name>
                  <parent dataType="ObjectRef">2905865120</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3775187310">
              <_items dataType="Array" type="Duality.Component[]" id="3103840642" length="8">
                <item dataType="Struct" type="Duality.Components.Transform" id="971212756">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2905865120</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="253064392">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2905865120</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1673674348">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2905865120</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="4128376486">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2905865120</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="3981973052">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2905865120</gameobj>
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3040433164" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="1624841144">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2838274704</item>
                  <item dataType="ObjectRef">2826137326</item>
                  <item dataType="ObjectRef">1216776044</item>
                  <item dataType="ObjectRef">1852891876</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="674504926">
                  <item dataType="ObjectRef">971212756</item>
                  <item dataType="ObjectRef">253064392</item>
                  <item dataType="ObjectRef">1673674348</item>
                  <item dataType="ObjectRef">4128376486</item>
                  <item dataType="ObjectRef">3981973052</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">971212756</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3562377700">Ab0AKTMG1UawtoJJ8zyvdA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">EnemyClaymore</name>
            <parent dataType="ObjectRef">1385290432</parent>
            <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="1296811474">
              <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="2233459862">
                <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="2892497440" length="4">
                  <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                    <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1691157136">
                      <_items dataType="ObjectRef">4278858348</_items>
                      <_size dataType="Int">0</_size>
                      <_version dataType="Int">1</_version>
                    </childIndex>
                    <componentType dataType="ObjectRef">4020116262</componentType>
                    <prop dataType="ObjectRef">3402294738</prop>
                    <val dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">-48.9969826</X>
                      <Y dataType="Float">448.5</Y>
                      <Z dataType="Float">0</Z>
                    </val>
                  </item>
                </_items>
                <_size dataType="Int">1</_size>
                <_version dataType="Int">405</_version>
              </changes>
              <obj dataType="ObjectRef">2905865120</obj>
              <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                <contentPath dataType="String">Data\Prefabs\EnemyClaymore.Prefab.res</contentPath>
              </prefab>
            </prefabLink>
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3575092865">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3731806373">
              <_items dataType="Array" type="Duality.GameObject[]" id="2566690198" length="8">
                <item dataType="Struct" type="Duality.GameObject" id="3572510950">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3490000714">
                    <_items dataType="Array" type="Duality.Component[]" id="3598729056" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1637858586">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3572510950</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="3279946331">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3572510950</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1765847706" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="674482224">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2684742480</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="860414574">
                        <item dataType="ObjectRef">1637858586</item>
                        <item dataType="ObjectRef">3279946331</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1637858586</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="993018508">1XcQ5uB82keJe056Ej6i6Q==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Eye</name>
                  <parent dataType="ObjectRef">3575092865</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="3291647820">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3640428872">
                    <_items dataType="Array" type="Duality.Component[]" id="4201568364" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1356995456">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3291647820</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="638847092">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3291647820</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1473434846" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1746094602">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="2258136090">
                        <item dataType="ObjectRef">1356995456</item>
                        <item dataType="ObjectRef">638847092</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1356995456</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3345930474">KMBSJmB6eU+eNJj9JwbEHQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopRight</name>
                  <parent dataType="ObjectRef">3575092865</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="1745617804">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2750085384">
                    <_items dataType="Array" type="Duality.Component[]" id="1932067692" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="4105932736">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1745617804</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3387784372">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1745617804</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="389459934" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="3203582922">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="2978396314">
                        <item dataType="ObjectRef">4105932736</item>
                        <item dataType="ObjectRef">3387784372</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">4105932736</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="382740266">aERhEG0j3Eeq1FsK+cIeOA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomLeft</name>
                  <parent dataType="ObjectRef">3575092865</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="3796727875">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2086269363">
                    <_items dataType="Array" type="Duality.Component[]" id="3456715302" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1862075511">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3796727875</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1143927147">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3796727875</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1136824504" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1305560025">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="825827840">
                        <item dataType="ObjectRef">1862075511</item>
                        <item dataType="ObjectRef">1143927147</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1862075511</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3755682459">RTR/1jJAXUyh13Qos7SUWw==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopLeft</name>
                  <parent dataType="ObjectRef">3575092865</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="1793517812">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="42625136">
                    <_items dataType="Array" type="Duality.Component[]" id="862798140" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="4153832744">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1793517812</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3435684380">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1793517812</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1387332334" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="3034319810">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="2984072714">
                        <item dataType="ObjectRef">4153832744</item>
                        <item dataType="ObjectRef">3435684380</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">4153832744</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="554983090">3KhJsq+fqECtUEe86NJzgA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomRight</name>
                  <parent dataType="ObjectRef">3575092865</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3920168040">
              <_items dataType="Array" type="Duality.Component[]" id="755373903" length="8">
                <item dataType="Struct" type="Duality.Components.Transform" id="1640440501">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">3575092865</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="922292137">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">3575092865</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2342902093">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">3575092865</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="502636935">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">3575092865</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="356233501">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">3575092865</gameobj>
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2008970607" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="3915893988">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2838274704</item>
                  <item dataType="ObjectRef">2826137326</item>
                  <item dataType="ObjectRef">1216776044</item>
                  <item dataType="ObjectRef">1852891876</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="1747889686">
                  <item dataType="ObjectRef">1640440501</item>
                  <item dataType="ObjectRef">922292137</item>
                  <item dataType="ObjectRef">2342902093</item>
                  <item dataType="ObjectRef">502636935</item>
                  <item dataType="ObjectRef">356233501</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1640440501</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="812868320">8NFKZF0CzEekrrOHICYevQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">EnemyClaymore</name>
            <parent dataType="ObjectRef">1385290432</parent>
            <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="4103009206">
              <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="1586848017">
                <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="3620732654" length="4">
                  <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                    <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="2324694540">
                      <_items dataType="ObjectRef">4278858348</_items>
                      <_size dataType="Int">0</_size>
                      <_version dataType="Int">1</_version>
                    </childIndex>
                    <componentType dataType="ObjectRef">4020116262</componentType>
                    <prop dataType="ObjectRef">3402294738</prop>
                    <val dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">-405.995972</X>
                      <Y dataType="Float">267.5</Y>
                      <Z dataType="Float">0</Z>
                    </val>
                  </item>
                </_items>
                <_size dataType="Int">1</_size>
                <_version dataType="Int">535</_version>
              </changes>
              <obj dataType="ObjectRef">3575092865</obj>
              <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                <contentPath dataType="String">Data\Prefabs\EnemyClaymore.Prefab.res</contentPath>
              </prefab>
            </prefabLink>
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="1593029184">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3760091024">
              <_items dataType="Array" type="Duality.GameObject[]" id="1611419964" length="8">
                <item dataType="Struct" type="Duality.GameObject" id="149740599">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1994293611">
                    <_items dataType="Array" type="Duality.Component[]" id="1112937590" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2510055531">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">149740599</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="4152143276">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">149740599</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3939423944" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1004793281">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2684742480</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="3736408288">
                        <item dataType="ObjectRef">2510055531</item>
                        <item dataType="ObjectRef">4152143276</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2510055531</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2255702291">eZCfD3NCPkyxTUXB/6o3cA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Eye</name>
                  <parent dataType="ObjectRef">1593029184</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="1114434942">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="523647654">
                    <_items dataType="Array" type="Duality.Component[]" id="2293589504" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="3474749874">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1114434942</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2756601510">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1114434942</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="458731450" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1998907156">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="1759306038">
                        <item dataType="ObjectRef">3474749874</item>
                        <item dataType="ObjectRef">2756601510</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">3474749874</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1101517232">qKodNnHFekeSTLG49xl5rA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopRight</name>
                  <parent dataType="ObjectRef">1593029184</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="2412898875">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2281723671">
                    <_items dataType="Array" type="Duality.Component[]" id="4051246862" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="478246511">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2412898875</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="4055065443">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2412898875</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="883353792" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="3490986141">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="501623032">
                        <item dataType="ObjectRef">478246511</item>
                        <item dataType="ObjectRef">4055065443</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">478246511</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3004535607">a6+mj3eqiUWUoKrsD1A22Q==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomLeft</name>
                  <parent dataType="ObjectRef">1593029184</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="3801802131">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1950128895">
                    <_items dataType="Array" type="Duality.Component[]" id="105965870" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1867149767">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3801802131</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1149001403">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3801802131</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3914206048" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="4264526133">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="330336328">
                        <item dataType="ObjectRef">1867149767</item>
                        <item dataType="ObjectRef">1149001403</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1867149767</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="4142597759">UaGrFjH8U0S2ZQinzDE5kQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopLeft</name>
                  <parent dataType="ObjectRef">1593029184</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="1615293381">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3125834473">
                    <_items dataType="Array" type="Duality.Component[]" id="1177264910" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="3975608313">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1615293381</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3257459949">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1615293381</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1375214784" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1634240867">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="490928376">
                        <item dataType="ObjectRef">3975608313</item>
                        <item dataType="ObjectRef">3257459949</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">3975608313</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2674218697">uCsXu1B4xECmBqOiZivhuQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomRight</name>
                  <parent dataType="ObjectRef">1593029184</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1579315950">
              <_items dataType="Array" type="Duality.Component[]" id="253592034" length="8">
                <item dataType="Struct" type="Duality.Components.Transform" id="3953344116">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">1593029184</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3235195752">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">1593029184</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="360838412">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">1593029184</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="2815540550">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">1593029184</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="2669137116">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">1593029184</gameobj>
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3937322604" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="4378232">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2838274704</item>
                  <item dataType="ObjectRef">2826137326</item>
                  <item dataType="ObjectRef">1216776044</item>
                  <item dataType="ObjectRef">1852891876</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="114293214">
                  <item dataType="ObjectRef">3953344116</item>
                  <item dataType="ObjectRef">3235195752</item>
                  <item dataType="ObjectRef">360838412</item>
                  <item dataType="ObjectRef">2815540550</item>
                  <item dataType="ObjectRef">2669137116</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3953344116</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1056244516">jv1oYZtC7E+Yin10rwgTIg==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">EnemyClaymore</name>
            <parent dataType="ObjectRef">1385290432</parent>
            <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="2547093266">
              <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="572283126">
                <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="1181492448" length="4">
                  <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                    <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="2498160272">
                      <_items dataType="ObjectRef">3031576684</_items>
                      <_size dataType="Int">0</_size>
                      <_version dataType="Int">1</_version>
                    </childIndex>
                    <componentType dataType="ObjectRef">4020116262</componentType>
                    <prop dataType="ObjectRef">3402294738</prop>
                    <val dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">-331.998</X>
                      <Y dataType="Float">-104.5</Y>
                      <Z dataType="Float">0</Z>
                    </val>
                  </item>
                </_items>
                <_size dataType="Int">1</_size>
                <_version dataType="Int">243</_version>
              </changes>
              <obj dataType="ObjectRef">1593029184</obj>
              <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                <contentPath dataType="String">Data\Prefabs\EnemyClaymore.Prefab.res</contentPath>
              </prefab>
            </prefabLink>
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="117827028">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1437550140">
              <_items dataType="Array" type="Duality.GameObject[]" id="74422596" length="8">
                <item dataType="Struct" type="Duality.GameObject" id="3620669008">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3002562200">
                    <_items dataType="Array" type="Duality.Component[]" id="13260332" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1686016644">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3620669008</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="3328104389">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3620669008</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3750670622" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="931467610">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2684742480</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="2080573370">
                        <item dataType="ObjectRef">1686016644</item>
                        <item dataType="ObjectRef">3328104389</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1686016644</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2542567258">SElKXIWL2E+nAwBUsI11wA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Eye</name>
                  <parent dataType="ObjectRef">117827028</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="1810151022">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2639275926">
                    <_items dataType="Array" type="Duality.Component[]" id="3292647456" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="4170465954">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1810151022</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3452317590">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1810151022</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3306876634" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1530453860">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="4238236182">
                        <item dataType="ObjectRef">4170465954</item>
                        <item dataType="ObjectRef">3452317590</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">4170465954</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1947878496">8vuhMEXTOk2i2ohd8h96pg==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopRight</name>
                  <parent dataType="ObjectRef">117827028</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="2565682116">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3544070596">
                    <_items dataType="Array" type="Duality.Component[]" id="1768069444" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="631029752">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2565682116</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="4207848684">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2565682116</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2137239958" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1137478222">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="1213733450">
                        <item dataType="ObjectRef">631029752</item>
                        <item dataType="ObjectRef">4207848684</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">631029752</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3681641470">/X5c8LG7ek2NONPVKs9BQw==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomLeft</name>
                  <parent dataType="ObjectRef">117827028</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="1663315340">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3739262012">
                    <_items dataType="Array" type="Duality.Component[]" id="4034379076" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="4023630272">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1663315340</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3305481908">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1663315340</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3616458134" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1613718550">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="379549658">
                        <item dataType="ObjectRef">4023630272</item>
                        <item dataType="ObjectRef">3305481908</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">4023630272</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1786363830">e2y0TJB6REGOYVX1A1Azsw==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeTopLeft</name>
                  <parent dataType="ObjectRef">117827028</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="2750550047">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4013202115">
                    <_items dataType="Array" type="Duality.Component[]" id="2991362598" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="815897683">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2750550047</gameobj>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="97749319">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2750550047</gameobj>
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1072560312" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="3048592809">
                        <item dataType="ObjectRef">4020116262</item>
                        <item dataType="ObjectRef">2838274704</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="3813236672">
                        <item dataType="ObjectRef">815897683</item>
                        <item dataType="ObjectRef">97749319</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">815897683</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1712998795">fluI7Ho6BUunbHUWY/zYZA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">SpikeBottomRight</name>
                  <parent dataType="ObjectRef">117827028</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3389829526">
              <_items dataType="Array" type="Duality.Component[]" id="2206995990" length="8">
                <item dataType="Struct" type="Duality.Components.Transform" id="2478141960">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">117827028</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1759993596">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">117827028</gameobj>
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3180603552">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">117827028</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="1340338394">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">117827028</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="1193934960">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">117827028</gameobj>
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3635174120" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="74800600">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="ObjectRef">2838274704</item>
                  <item dataType="ObjectRef">2826137326</item>
                  <item dataType="ObjectRef">1216776044</item>
                  <item dataType="ObjectRef">1852891876</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="3098255006">
                  <item dataType="ObjectRef">2478141960</item>
                  <item dataType="ObjectRef">1759993596</item>
                  <item dataType="ObjectRef">3180603552</item>
                  <item dataType="ObjectRef">1340338394</item>
                  <item dataType="ObjectRef">1193934960</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2478141960</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3005864324">GqUDoAaFzUCc3CKjpDqsAw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">EnemyClaymore</name>
            <parent dataType="ObjectRef">1385290432</parent>
            <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="3223739506">
              <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="782351722">
                <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="1570673184" length="4">
                  <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                    <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3887194768">
                      <_items dataType="Array" type="System.Int32[]" id="1609481532" />
                      <_size dataType="Int">0</_size>
                      <_version dataType="Int">1</_version>
                    </childIndex>
                    <componentType dataType="ObjectRef">4020116262</componentType>
                    <prop dataType="ObjectRef">3402294738</prop>
                    <val dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">-18.9370422</X>
                      <Y dataType="Float">459.261322</Y>
                      <Z dataType="Float">0</Z>
                    </val>
                  </item>
                </_items>
                <_size dataType="Int">1</_size>
                <_version dataType="Int">1262</_version>
              </changes>
              <obj dataType="ObjectRef">117827028</obj>
              <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                <contentPath dataType="String">Data\Prefabs\EnemyClaymore.Prefab.res</contentPath>
              </prefab>
            </prefabLink>
          </item>
        </_items>
        <_size dataType="Int">14</_size>
        <_version dataType="Int">24</_version>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="835895098">
        <_items dataType="Array" type="Duality.Component[]" id="2619295988" />
        <_size dataType="Int">0</_size>
        <_version dataType="Int">0</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3367671302" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="1394625280" />
          <values dataType="Array" type="Duality.Component[]" id="4131619278" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3605337500">zpPtuXyuQEm2pUW11aVhTg==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Enemies</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="1155233974">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3105378720">
        <_items dataType="Array" type="Duality.Component[]" id="80984796" length="4">
          <item dataType="Struct" type="DualStickSpaceShooter.HeadUpDisplay" id="1797132059">
            <active dataType="Bool">true</active>
            <font dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
              <contentPath dataType="String">Data\Materials\HudFont.Font.res</contentPath>
            </font>
            <gameobj dataType="ObjectRef">1155233974</gameobj>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
        <_version dataType="Int">1</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2489319054" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="1387172210">
            <item dataType="Type" id="929964240" value="DualStickSpaceShooter.HeadUpDisplay" />
          </keys>
          <values dataType="Array" type="Duality.Component[]" id="3634262090">
            <item dataType="ObjectRef">1797132059</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="658055746">sjY69eGgDk6XYRFAssb0Cg==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">HeadUpDisplay</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="1064452476">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4143745810">
        <_items dataType="Array" type="Duality.Component[]" id="2382386256" length="4">
          <item dataType="Struct" type="DualStickSpaceShooter.GameOverScreen" id="1153684968">
            <active dataType="Bool">true</active>
            <blendMaterial dataType="Struct" type="Duality.Resources.BatchInfo" id="972466776">
              <dirtyFlag dataType="Enum" type="Duality.Resources.BatchInfo+DirtyFlag" name="None" value="0" />
              <hashCode dataType="Int">343952833</hashCode>
              <mainColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                <A dataType="Byte">215</A>
                <B dataType="Byte">0</B>
                <G dataType="Byte">0</G>
                <R dataType="Byte">0</R>
              </mainColor>
              <technique dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.DrawTechnique]]">
                <contentPath dataType="String">Data\Materials\AlphaThreshold.DrawTechnique.res</contentPath>
              </technique>
              <textures dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.String],[Duality.ContentRef`1[[Duality.Resources.Texture]]]]" id="3776988844" surrogate="true">
                <header />
                <body>
                  <mainTex dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Texture]]">
                    <contentPath dataType="String">Data\Materials\Mosaic.Texture.res</contentPath>
                  </mainTex>
                </body>
              </textures>
              <uniforms dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.String],[System.Single[]]]" id="283169206" surrogate="true">
                <header />
                <body>
                  <threshold dataType="Array" type="System.Single[]" id="1154518758" length="1" />
                </body>
              </uniforms>
            </blendMaterial>
            <font dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
              <contentPath dataType="String">Data\Materials\HudFont.Font.res</contentPath>
            </font>
            <gameobj dataType="ObjectRef">1064452476</gameobj>
            <gameOver dataType="Bool">false</gameOver>
            <gameStarted dataType="Bool">false</gameStarted>
            <lastTimeAnyAlive dataType="Float">0</lastTimeAnyAlive>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
        <_version dataType="Int">1</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="115548618" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="1112601416">
            <item dataType="Type" id="92602476" value="DualStickSpaceShooter.GameOverScreen" />
          </keys>
          <values dataType="Array" type="Duality.Component[]" id="1321770206">
            <item dataType="ObjectRef">1153684968</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1784394676">FQYRJmOyT0efXKs0n5J4Jg==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">GameOverScreen</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="3900738636">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2953702338">
        <_items dataType="Array" type="Duality.GameObject[]" id="3913414160" length="4">
          <item dataType="Struct" type="Duality.GameObject" id="1916950673">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3582113525">
              <_items dataType="Array" type="Duality.Component[]" id="1382941302" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="4277265605">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">1916950673</gameobj>
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.ParticleEffect" id="2943583766">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">1916950673</gameobj>
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">4</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="143767752" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="3086393183">
                  <item dataType="ObjectRef">4020116262</item>
                  <item dataType="Type" id="2987759726" value="DualStickSpaceShooter.ParticleEffect" />
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="1059319584">
                  <item dataType="ObjectRef">4277265605</item>
                  <item dataType="ObjectRef">2943583766</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">4277265605</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3426834637">iB6fYN2Ih0aqf5q7pw+sng==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Glow</name>
            <parent dataType="ObjectRef">3900738636</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">1</_size>
        <_version dataType="Int">1</_version>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="95901194">
        <_items dataType="Array" type="Duality.Component[]" id="3208148376" length="8">
          <item dataType="Struct" type="Duality.Components.Transform" id="1966086272">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3900738636</gameobj>
          </item>
          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2668547864">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3900738636</gameobj>
          </item>
          <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="204464530">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3900738636</gameobj>
          </item>
          <item dataType="Struct" type="DualStickSpaceShooter.ParticleEffect" id="632404433">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3900738636</gameobj>
          </item>
          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="1348400162">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3900738636</gameobj>
          </item>
        </_items>
        <_size dataType="Int">5</_size>
        <_version dataType="Int">5</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3769695922" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="3710994592">
            <item dataType="ObjectRef">4020116262</item>
            <item dataType="ObjectRef">2826137326</item>
            <item dataType="ObjectRef">4016316454</item>
            <item dataType="ObjectRef">2987759726</item>
            <item dataType="Type" id="1248098524" value="Duality.Components.Renderers.TextRenderer" />
          </keys>
          <values dataType="Array" type="Duality.Component[]" id="3947163790">
            <item dataType="ObjectRef">1966086272</item>
            <item dataType="ObjectRef">2668547864</item>
            <item dataType="ObjectRef">204464530</item>
            <item dataType="ObjectRef">632404433</item>
            <item dataType="ObjectRef">1348400162</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">1966086272</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="342084796">k8HdVF3Y402hOGSBbfJFPA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">LevelGoal</name>
      <parent />
      <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="1645876570">
        <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="4280410184">
          <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="2198170732" length="4">
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3078935976">
                <_items dataType="Array" type="System.Int32[]" id="3139126956" length="4" />
                <_size dataType="Int">1</_size>
                <_version dataType="Int">2</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2987759726</componentType>
              <prop dataType="PropertyInfo" id="849483678" value="P:DualStickSpaceShooter.ParticleEffect:WorldSpace" />
              <val dataType="Bool">false</val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1422911380">
                <_items dataType="Array" type="System.Int32[]" id="3476940872" />
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">4020116262</componentType>
              <prop dataType="ObjectRef">3402294738</prop>
              <val dataType="Struct" type="OpenTK.Vector3">
                <X dataType="Float">396</X>
                <Y dataType="Float">74</Y>
                <Z dataType="Float">0</Z>
              </val>
            </item>
          </_items>
          <_size dataType="Int">2</_size>
          <_version dataType="Int">1409</_version>
        </changes>
        <obj dataType="ObjectRef">3900738636</obj>
        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
          <contentPath dataType="String">Data\Prefabs\LevelGoal.Prefab.res</contentPath>
        </prefab>
      </prefabLink>
    </item>
    <item dataType="ObjectRef">687383225</item>
    <item dataType="ObjectRef">2383024647</item>
    <item dataType="ObjectRef">3003406700</item>
    <item dataType="ObjectRef">283960702</item>
    <item dataType="ObjectRef">2198381846</item>
    <item dataType="ObjectRef">105982944</item>
    <item dataType="ObjectRef">367989753</item>
    <item dataType="ObjectRef">3837180042</item>
    <item dataType="ObjectRef">3508990515</item>
    <item dataType="ObjectRef">2373794822</item>
    <item dataType="ObjectRef">196886364</item>
    <item dataType="ObjectRef">4124357391</item>
    <item dataType="ObjectRef">3901085842</item>
    <item dataType="ObjectRef">3034479095</item>
    <item dataType="ObjectRef">143430740</item>
    <item dataType="ObjectRef">2219384449</item>
    <item dataType="ObjectRef">2645764365</item>
    <item dataType="ObjectRef">2269227541</item>
    <item dataType="ObjectRef">242932114</item>
    <item dataType="ObjectRef">2389062831</item>
    <item dataType="ObjectRef">347102484</item>
    <item dataType="ObjectRef">1076594099</item>
    <item dataType="ObjectRef">2213742814</item>
    <item dataType="ObjectRef">2905865120</item>
    <item dataType="ObjectRef">3575092865</item>
    <item dataType="ObjectRef">1593029184</item>
    <item dataType="ObjectRef">117827028</item>
    <item dataType="ObjectRef">1916950673</item>
    <item dataType="ObjectRef">1043620744</item>
    <item dataType="ObjectRef">3885667949</item>
    <item dataType="ObjectRef">840789122</item>
    <item dataType="ObjectRef">3795653702</item>
    <item dataType="ObjectRef">2546580953</item>
    <item dataType="ObjectRef">3013370658</item>
    <item dataType="ObjectRef">512852555</item>
    <item dataType="ObjectRef">380080983</item>
    <item dataType="ObjectRef">404360881</item>
    <item dataType="ObjectRef">3199545466</item>
    <item dataType="ObjectRef">1314617617</item>
    <item dataType="ObjectRef">4268791402</item>
    <item dataType="ObjectRef">421565130</item>
    <item dataType="ObjectRef">4136792166</item>
    <item dataType="ObjectRef">2073150401</item>
    <item dataType="ObjectRef">38335236</item>
    <item dataType="ObjectRef">3887235839</item>
    <item dataType="ObjectRef">1573782093</item>
    <item dataType="ObjectRef">1724914615</item>
    <item dataType="ObjectRef">2714806266</item>
    <item dataType="ObjectRef">3258964135</item>
    <item dataType="ObjectRef">2391527542</item>
    <item dataType="ObjectRef">3130008096</item>
    <item dataType="ObjectRef">3742889763</item>
    <item dataType="ObjectRef">4073648570</item>
    <item dataType="ObjectRef">2728913501</item>
    <item dataType="ObjectRef">4236686595</item>
    <item dataType="ObjectRef">465249611</item>
    <item dataType="ObjectRef">336767302</item>
    <item dataType="ObjectRef">367029264</item>
    <item dataType="ObjectRef">3911303751</item>
    <item dataType="ObjectRef">3918154608</item>
    <item dataType="ObjectRef">392384898</item>
    <item dataType="ObjectRef">1643744387</item>
    <item dataType="ObjectRef">2336954970</item>
    <item dataType="ObjectRef">3973150936</item>
    <item dataType="ObjectRef">1029338680</item>
    <item dataType="ObjectRef">4284896073</item>
    <item dataType="ObjectRef">1885801767</item>
    <item dataType="ObjectRef">3987883834</item>
    <item dataType="ObjectRef">479828669</item>
    <item dataType="ObjectRef">3760011772</item>
    <item dataType="ObjectRef">2101574325</item>
    <item dataType="ObjectRef">727846504</item>
    <item dataType="ObjectRef">1512653090</item>
    <item dataType="ObjectRef">1976635711</item>
    <item dataType="ObjectRef">2078648859</item>
    <item dataType="ObjectRef">1999761815</item>
    <item dataType="ObjectRef">1622304848</item>
    <item dataType="ObjectRef">2663201263</item>
    <item dataType="ObjectRef">1718003899</item>
    <item dataType="ObjectRef">2625065684</item>
    <item dataType="ObjectRef">1016569111</item>
    <item dataType="ObjectRef">4229974861</item>
    <item dataType="ObjectRef">120850460</item>
    <item dataType="ObjectRef">3572510950</item>
    <item dataType="ObjectRef">3291647820</item>
    <item dataType="ObjectRef">1745617804</item>
    <item dataType="ObjectRef">3796727875</item>
    <item dataType="ObjectRef">1793517812</item>
    <item dataType="ObjectRef">149740599</item>
    <item dataType="ObjectRef">1114434942</item>
    <item dataType="ObjectRef">2412898875</item>
    <item dataType="ObjectRef">3801802131</item>
    <item dataType="ObjectRef">1615293381</item>
    <item dataType="ObjectRef">3620669008</item>
    <item dataType="ObjectRef">1810151022</item>
    <item dataType="ObjectRef">2565682116</item>
    <item dataType="ObjectRef">1663315340</item>
    <item dataType="ObjectRef">2750550047</item>
  </serializeObj>
  <sourcePath />
</root>
<!-- XmlFormatterBase Document Separator -->
