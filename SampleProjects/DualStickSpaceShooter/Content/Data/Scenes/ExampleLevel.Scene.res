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
            <focusDist dataType="Float">470.941376</focusDist>
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
                        <_version dataType="Int">3</_version>
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
                          <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="2165640420" length="4">
                            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="771872968">
                                <_items dataType="Array" type="System.Int32[]" id="1197357676" length="4" />
                                <_size dataType="Int">1</_size>
                                <_version dataType="Int">2</_version>
                              </childIndex>
                              <componentType />
                              <prop dataType="PropertyInfo" id="886661854" value="P:Duality.GameObject:ActiveSingle" />
                              <val dataType="Bool">false</val>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                          <_version dataType="Int">139</_version>
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
                                <_items dataType="ObjectRef">2697357756</_items>
                                <_size dataType="Int">0</_size>
                                <_version dataType="Int">1</_version>
                              </childIndex>
                              <componentType dataType="ObjectRef">2425790428</componentType>
                              <prop dataType="PropertyInfo" id="3402294738" value="P:DualStickSpaceShooter.Player:Color" />
                              <val dataType="Struct" type="Duality.Drawing.ColorRgba">
                                <A dataType="Byte">255</A>
                                <B dataType="Byte">89</B>
                                <G dataType="Byte">176</G>
                                <R dataType="Byte">255</R>
                              </val>
                            </item>
                            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="619957096">
                                <_items dataType="Array" type="System.Int32[]" id="1174937044" />
                                <_size dataType="Int">0</_size>
                                <_version dataType="Int">1</_version>
                              </childIndex>
                              <componentType />
                              <prop dataType="ObjectRef">886661854</prop>
                              <val dataType="Bool">true</val>
                            </item>
                            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="2814194406">
                                <_items dataType="Array" type="System.Int32[]" id="2175012026" />
                                <_size dataType="Int">0</_size>
                                <_version dataType="Int">1</_version>
                              </childIndex>
                              <componentType dataType="ObjectRef">2425790428</componentType>
                              <prop dataType="PropertyInfo" id="1387688228" value="P:DualStickSpaceShooter.Player:Id" />
                              <val dataType="Enum" type="DualStickSpaceShooter.PlayerId" name="PlayerTwo" value="2" />
                            </item>
                            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3043999498">
                                <_items dataType="Array" type="System.Int32[]" id="1212756782" length="4" />
                                <_size dataType="Int">1</_size>
                                <_version dataType="Int">2</_version>
                              </childIndex>
                              <componentType />
                              <prop dataType="ObjectRef">886661854</prop>
                              <val dataType="Bool">false</val>
                            </item>
                            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3964229984">
                                <_items dataType="Array" type="System.Int32[]" id="3266911372" length="4" />
                                <_size dataType="Int">1</_size>
                                <_version dataType="Int">2</_version>
                              </childIndex>
                              <componentType dataType="ObjectRef">4020116262</componentType>
                              <prop dataType="PropertyInfo" id="1967265150" value="P:Duality.Components.Transform:RelativePos" />
                              <val dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">0</X>
                                <Y dataType="Float">0</Y>
                                <Z dataType="Float">0</Z>
                              </val>
                            </item>
                          </_items>
                          <_size dataType="Int">6</_size>
                          <_version dataType="Int">124</_version>
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
    <item dataType="ObjectRef">1924155130</item>
    <item dataType="ObjectRef">3691642060</item>
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
              <hashCode dataType="Int">340838081</hashCode>
              <mainColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                <A dataType="Byte">215</A>
                <B dataType="Byte">255</B>
                <G dataType="Byte">255</G>
                <R dataType="Byte">255</R>
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
    <item dataType="Struct" type="Duality.GameObject" id="4117782497">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3236554019">
        <_items dataType="Array" type="Duality.GameObject[]" id="4008161382" length="4">
          <item dataType="Struct" type="Duality.GameObject" id="4637401">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2573090905">
              <_items dataType="Array" type="Duality.GameObject[]" id="4024256462" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="2424808843">
                  <active dataType="Bool">true</active>
                  <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3252955979">
                    <_items dataType="Array" type="Duality.GameObject[]" id="2550693622" length="16">
                      <item dataType="Struct" type="Duality.GameObject" id="1696349013">
                        <active dataType="Bool">true</active>
                        <children />
                        <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="729104805">
                          <_items dataType="Array" type="Duality.Component[]" id="3742171542" length="4">
                            <item dataType="Struct" type="Duality.Components.Transform" id="4056663945">
                              <active dataType="Bool">true</active>
                              <angle dataType="Float">3.69937277</angle>
                              <angleAbs dataType="Float">3.69937277</angleAbs>
                              <angleVel dataType="Float">0</angleVel>
                              <angleVelAbs dataType="Float">0</angleVelAbs>
                              <deriveAngle dataType="Bool">true</deriveAngle>
                              <gameobj dataType="ObjectRef">1696349013</gameobj>
                              <ignoreParent dataType="Bool">false</ignoreParent>
                              <parentTransform />
                              <pos dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">812.228455</X>
                                <Y dataType="Float">-1245.74927</Y>
                                <Z dataType="Float">0</Z>
                              </pos>
                              <posAbs dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">812.228455</X>
                                <Y dataType="Float">-1245.74927</Y>
                                <Z dataType="Float">0</Z>
                              </posAbs>
                              <scale dataType="Float">5.89991665</scale>
                              <scaleAbs dataType="Float">5.89991665</scaleAbs>
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
                            <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3338515581">
                              <active dataType="Bool">true</active>
                              <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                <A dataType="Byte">255</A>
                                <B dataType="Byte">255</B>
                                <G dataType="Byte">255</G>
                                <R dataType="Byte">255</R>
                              </colorTint>
                              <customMat />
                              <gameobj dataType="ObjectRef">1696349013</gameobj>
                              <offset dataType="Int">0</offset>
                              <pixelGrid dataType="Bool">false</pixelGrid>
                              <rect dataType="Struct" type="Duality.Rect">
                                <H dataType="Float">172</H>
                                <W dataType="Float">179</W>
                                <X dataType="Float">-89.5</X>
                                <Y dataType="Float">-86</Y>
                              </rect>
                              <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                              <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                                <contentPath dataType="String">Data\Materials\Background\Backdrop1.Material.res</contentPath>
                              </sharedMat>
                              <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                            </item>
                          </_items>
                          <_size dataType="Int">2</_size>
                          <_version dataType="Int">2</_version>
                        </compList>
                        <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2164826216" surrogate="true">
                          <header />
                          <body>
                            <keys dataType="Array" type="System.Type[]" id="2277525071">
                              <item dataType="ObjectRef">4020116262</item>
                              <item dataType="ObjectRef">2838274704</item>
                            </keys>
                            <values dataType="Array" type="Duality.Component[]" id="1522893920">
                              <item dataType="ObjectRef">4056663945</item>
                              <item dataType="ObjectRef">3338515581</item>
                            </values>
                          </body>
                        </compMap>
                        <compTransform dataType="ObjectRef">4056663945</compTransform>
                        <identifier dataType="Struct" type="System.Guid" surrogate="true">
                          <header>
                            <data dataType="Array" type="System.Byte[]" id="2587657117">yHiwjKw/XkOxctYVjBYeww==</data>
                          </header>
                          <body />
                        </identifier>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        <name dataType="String">Backdrop1</name>
                        <parent dataType="ObjectRef">2424808843</parent>
                        <prefabLink />
                      </item>
                      <item dataType="Struct" type="Duality.GameObject" id="2491661213">
                        <active dataType="Bool">true</active>
                        <children />
                        <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="178401581">
                          <_items dataType="Array" type="Duality.Component[]" id="1243514470" length="4">
                            <item dataType="Struct" type="Duality.Components.Transform" id="557008849">
                              <active dataType="Bool">true</active>
                              <angle dataType="Float">2.20910144</angle>
                              <angleAbs dataType="Float">2.20910144</angleAbs>
                              <angleVel dataType="Float">0</angleVel>
                              <angleVelAbs dataType="Float">0</angleVelAbs>
                              <deriveAngle dataType="Bool">true</deriveAngle>
                              <gameobj dataType="ObjectRef">2491661213</gameobj>
                              <ignoreParent dataType="Bool">false</ignoreParent>
                              <parentTransform />
                              <pos dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">-568.061646</X>
                                <Y dataType="Float">-1664.3208</Y>
                                <Z dataType="Float">0</Z>
                              </pos>
                              <posAbs dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">-568.061646</X>
                                <Y dataType="Float">-1664.3208</Y>
                                <Z dataType="Float">0</Z>
                              </posAbs>
                              <scale dataType="Float">10.1820793</scale>
                              <scaleAbs dataType="Float">10.1820793</scaleAbs>
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
                            <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="4133827781">
                              <active dataType="Bool">true</active>
                              <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                <A dataType="Byte">255</A>
                                <B dataType="Byte">255</B>
                                <G dataType="Byte">255</G>
                                <R dataType="Byte">255</R>
                              </colorTint>
                              <customMat />
                              <gameobj dataType="ObjectRef">2491661213</gameobj>
                              <offset dataType="Int">0</offset>
                              <pixelGrid dataType="Bool">false</pixelGrid>
                              <rect dataType="Struct" type="Duality.Rect">
                                <H dataType="Float">203</H>
                                <W dataType="Float">160</W>
                                <X dataType="Float">-80</X>
                                <Y dataType="Float">-101.5</Y>
                              </rect>
                              <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                              <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                                <contentPath dataType="String">Data\Materials\Background\Backdrop2.Material.res</contentPath>
                              </sharedMat>
                              <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                            </item>
                          </_items>
                          <_size dataType="Int">2</_size>
                          <_version dataType="Int">2</_version>
                        </compList>
                        <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="132979320" surrogate="true">
                          <header />
                          <body>
                            <keys dataType="Array" type="System.Type[]" id="453805639">
                              <item dataType="ObjectRef">4020116262</item>
                              <item dataType="ObjectRef">2838274704</item>
                            </keys>
                            <values dataType="Array" type="Duality.Component[]" id="595327232">
                              <item dataType="ObjectRef">557008849</item>
                              <item dataType="ObjectRef">4133827781</item>
                            </values>
                          </body>
                        </compMap>
                        <compTransform dataType="ObjectRef">557008849</compTransform>
                        <identifier dataType="Struct" type="System.Guid" surrogate="true">
                          <header>
                            <data dataType="Array" type="System.Byte[]" id="3979633605">WvwxcbL16EenqhA03sECLw==</data>
                          </header>
                          <body />
                        </identifier>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        <name dataType="String">Backdrop2</name>
                        <parent dataType="ObjectRef">2424808843</parent>
                        <prefabLink />
                      </item>
                      <item dataType="Struct" type="Duality.GameObject" id="787323885">
                        <active dataType="Bool">true</active>
                        <children />
                        <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3752324285">
                          <_items dataType="Array" type="Duality.Component[]" id="2348342822" length="4">
                            <item dataType="Struct" type="Duality.Components.Transform" id="3147638817">
                              <active dataType="Bool">true</active>
                              <angle dataType="Float">5.77391434</angle>
                              <angleAbs dataType="Float">5.77391434</angleAbs>
                              <angleVel dataType="Float">0</angleVel>
                              <angleVelAbs dataType="Float">0</angleVelAbs>
                              <deriveAngle dataType="Bool">true</deriveAngle>
                              <gameobj dataType="ObjectRef">787323885</gameobj>
                              <ignoreParent dataType="Bool">false</ignoreParent>
                              <parentTransform />
                              <pos dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">436.587524</X>
                                <Y dataType="Float">628.677</Y>
                                <Z dataType="Float">0</Z>
                              </pos>
                              <posAbs dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">436.587524</X>
                                <Y dataType="Float">628.677</Y>
                                <Z dataType="Float">0</Z>
                              </posAbs>
                              <scale dataType="Float">6.178851</scale>
                              <scaleAbs dataType="Float">6.178851</scaleAbs>
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
                            <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2429490453">
                              <active dataType="Bool">true</active>
                              <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                <A dataType="Byte">255</A>
                                <B dataType="Byte">255</B>
                                <G dataType="Byte">255</G>
                                <R dataType="Byte">255</R>
                              </colorTint>
                              <customMat />
                              <gameobj dataType="ObjectRef">787323885</gameobj>
                              <offset dataType="Int">0</offset>
                              <pixelGrid dataType="Bool">false</pixelGrid>
                              <rect dataType="Struct" type="Duality.Rect">
                                <H dataType="Float">174</H>
                                <W dataType="Float">334</W>
                                <X dataType="Float">-167</X>
                                <Y dataType="Float">-87</Y>
                              </rect>
                              <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                              <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                                <contentPath dataType="String">Data\Materials\Background\Backdrop3.Material.res</contentPath>
                              </sharedMat>
                              <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                            </item>
                          </_items>
                          <_size dataType="Int">2</_size>
                          <_version dataType="Int">2</_version>
                        </compList>
                        <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="274986168" surrogate="true">
                          <header />
                          <body>
                            <keys dataType="Array" type="System.Type[]" id="529875415">
                              <item dataType="ObjectRef">4020116262</item>
                              <item dataType="ObjectRef">2838274704</item>
                            </keys>
                            <values dataType="Array" type="Duality.Component[]" id="380810176">
                              <item dataType="ObjectRef">3147638817</item>
                              <item dataType="ObjectRef">2429490453</item>
                            </values>
                          </body>
                        </compMap>
                        <compTransform dataType="ObjectRef">3147638817</compTransform>
                        <identifier dataType="Struct" type="System.Guid" surrogate="true">
                          <header>
                            <data dataType="Array" type="System.Byte[]" id="4246728181">dtHPRCLCNEqWA6jReyFRrw==</data>
                          </header>
                          <body />
                        </identifier>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        <name dataType="String">Backdrop3</name>
                        <parent dataType="ObjectRef">2424808843</parent>
                        <prefabLink />
                      </item>
                      <item dataType="Struct" type="Duality.GameObject" id="2880709120">
                        <active dataType="Bool">true</active>
                        <children />
                        <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="895851324">
                          <_items dataType="Array" type="Duality.Component[]" id="823105348" length="4">
                            <item dataType="Struct" type="Duality.Components.Transform" id="946056756">
                              <active dataType="Bool">true</active>
                              <angle dataType="Float">0</angle>
                              <angleAbs dataType="Float">0</angleAbs>
                              <angleVel dataType="Float">0</angleVel>
                              <angleVelAbs dataType="Float">0</angleVelAbs>
                              <deriveAngle dataType="Bool">true</deriveAngle>
                              <gameobj dataType="ObjectRef">2880709120</gameobj>
                              <ignoreParent dataType="Bool">false</ignoreParent>
                              <parentTransform />
                              <pos dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">-1081.3103</X>
                                <Y dataType="Float">1205.88513</Y>
                                <Z dataType="Float">0</Z>
                              </pos>
                              <posAbs dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">-1081.3103</X>
                                <Y dataType="Float">1205.88513</Y>
                                <Z dataType="Float">0</Z>
                              </posAbs>
                              <scale dataType="Float">7.02857</scale>
                              <scaleAbs dataType="Float">7.02857</scaleAbs>
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
                            <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="227908392">
                              <active dataType="Bool">true</active>
                              <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                <A dataType="Byte">255</A>
                                <B dataType="Byte">255</B>
                                <G dataType="Byte">255</G>
                                <R dataType="Byte">255</R>
                              </colorTint>
                              <customMat />
                              <gameobj dataType="ObjectRef">2880709120</gameobj>
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
                          </_items>
                          <_size dataType="Int">2</_size>
                          <_version dataType="Int">2</_version>
                        </compList>
                        <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="498453398" surrogate="true">
                          <header />
                          <body>
                            <keys dataType="Array" type="System.Type[]" id="2811060502">
                              <item dataType="ObjectRef">4020116262</item>
                              <item dataType="ObjectRef">2838274704</item>
                            </keys>
                            <values dataType="Array" type="Duality.Component[]" id="3260503002">
                              <item dataType="ObjectRef">946056756</item>
                              <item dataType="ObjectRef">227908392</item>
                            </values>
                          </body>
                        </compMap>
                        <compTransform dataType="ObjectRef">946056756</compTransform>
                        <identifier dataType="Struct" type="System.Guid" surrogate="true">
                          <header>
                            <data dataType="Array" type="System.Byte[]" id="3864922294">T2HW42KZx0O33NHYJSnQiA==</data>
                          </header>
                          <body />
                        </identifier>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        <name dataType="String">Backdrop4</name>
                        <parent dataType="ObjectRef">2424808843</parent>
                        <prefabLink />
                      </item>
                      <item dataType="Struct" type="Duality.GameObject" id="1391548966">
                        <active dataType="Bool">true</active>
                        <children />
                        <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="161774922">
                          <_items dataType="Array" type="Duality.Component[]" id="2285049696" length="4">
                            <item dataType="Struct" type="Duality.Components.Transform" id="3751863898">
                              <active dataType="Bool">true</active>
                              <angle dataType="Float">4.426233</angle>
                              <angleAbs dataType="Float">4.426233</angleAbs>
                              <angleVel dataType="Float">0</angleVel>
                              <angleVelAbs dataType="Float">0</angleVelAbs>
                              <deriveAngle dataType="Bool">true</deriveAngle>
                              <gameobj dataType="ObjectRef">1391548966</gameobj>
                              <ignoreParent dataType="Bool">false</ignoreParent>
                              <parentTransform />
                              <pos dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">-2112.79053</X>
                                <Y dataType="Float">-792.296448</Y>
                                <Z dataType="Float">0</Z>
                              </pos>
                              <posAbs dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">-2112.79053</X>
                                <Y dataType="Float">-792.296448</Y>
                                <Z dataType="Float">0</Z>
                              </posAbs>
                              <scale dataType="Float">9.377846</scale>
                              <scaleAbs dataType="Float">9.377846</scaleAbs>
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
                            <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3033715534">
                              <active dataType="Bool">true</active>
                              <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                <A dataType="Byte">255</A>
                                <B dataType="Byte">255</B>
                                <G dataType="Byte">255</G>
                                <R dataType="Byte">255</R>
                              </colorTint>
                              <customMat />
                              <gameobj dataType="ObjectRef">1391548966</gameobj>
                              <offset dataType="Int">0</offset>
                              <pixelGrid dataType="Bool">false</pixelGrid>
                              <rect dataType="Struct" type="Duality.Rect">
                                <H dataType="Float">229</H>
                                <W dataType="Float">243</W>
                                <X dataType="Float">-121.5</X>
                                <Y dataType="Float">-114.5</Y>
                              </rect>
                              <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                              <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                                <contentPath dataType="String">Data\Materials\Background\Backdrop5.Material.res</contentPath>
                              </sharedMat>
                              <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                            </item>
                          </_items>
                          <_size dataType="Int">2</_size>
                          <_version dataType="Int">2</_version>
                        </compList>
                        <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="538954394" surrogate="true">
                          <header />
                          <body>
                            <keys dataType="Array" type="System.Type[]" id="2198107696">
                              <item dataType="ObjectRef">4020116262</item>
                              <item dataType="ObjectRef">2838274704</item>
                            </keys>
                            <values dataType="Array" type="Duality.Component[]" id="3123326574">
                              <item dataType="ObjectRef">3751863898</item>
                              <item dataType="ObjectRef">3033715534</item>
                            </values>
                          </body>
                        </compMap>
                        <compTransform dataType="ObjectRef">3751863898</compTransform>
                        <identifier dataType="Struct" type="System.Guid" surrogate="true">
                          <header>
                            <data dataType="Array" type="System.Byte[]" id="528423052">gRWhAVACr0+aslGOsqSwCA==</data>
                          </header>
                          <body />
                        </identifier>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        <name dataType="String">Backdrop5</name>
                        <parent dataType="ObjectRef">2424808843</parent>
                        <prefabLink />
                      </item>
                      <item dataType="Struct" type="Duality.GameObject" id="1465644832">
                        <active dataType="Bool">true</active>
                        <children />
                        <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4260150556">
                          <_items dataType="Array" type="Duality.Component[]" id="1466828740" length="4">
                            <item dataType="Struct" type="Duality.Components.Transform" id="3825959764">
                              <active dataType="Bool">true</active>
                              <angle dataType="Float">0</angle>
                              <angleAbs dataType="Float">0</angleAbs>
                              <angleVel dataType="Float">0</angleVel>
                              <angleVelAbs dataType="Float">0</angleVelAbs>
                              <deriveAngle dataType="Bool">true</deriveAngle>
                              <gameobj dataType="ObjectRef">1465644832</gameobj>
                              <ignoreParent dataType="Bool">false</ignoreParent>
                              <parentTransform />
                              <pos dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">0</X>
                                <Y dataType="Float">0</Y>
                                <Z dataType="Float">0</Z>
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
                            <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="233454060">
                              <active dataType="Bool">true</active>
                              <angularDamp dataType="Float">0.3</angularDamp>
                              <angularVel dataType="Float">0</angularVel>
                              <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Static" value="0" />
                              <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat5" value="16" />
                              <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1, Cat2, Cat3, Cat4, Cat6, Cat7, Cat8, Cat9, Cat10, Cat11, Cat12, Cat13, Cat14, Cat15, Cat16, Cat17, Cat18, Cat19, Cat20, Cat21, Cat22, Cat23, Cat24, Cat25, Cat26, Cat27, Cat28, Cat29, Cat30, Cat31" value="2147483631" />
                              <continous dataType="Bool">false</continous>
                              <explicitMass dataType="Float">0</explicitMass>
                              <fixedAngle dataType="Bool">false</fixedAngle>
                              <gameobj dataType="ObjectRef">1465644832</gameobj>
                              <ignoreGravity dataType="Bool">false</ignoreGravity>
                              <joints />
                              <linearDamp dataType="Float">0.3</linearDamp>
                              <linearVel dataType="Struct" type="OpenTK.Vector2">
                                <X dataType="Float">0</X>
                                <Y dataType="Float">0</Y>
                              </linearVel>
                              <revolutions dataType="Float">0</revolutions>
                              <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1619011164">
                                <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3186371780" length="4" />
                                <_size dataType="Int">0</_size>
                                <_version dataType="Int">2</_version>
                              </shapes>
                            </item>
                          </_items>
                          <_size dataType="Int">2</_size>
                          <_version dataType="Int">2</_version>
                        </compList>
                        <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2124787222" surrogate="true">
                          <header />
                          <body>
                            <keys dataType="Array" type="System.Type[]" id="787212214">
                              <item dataType="ObjectRef">4020116262</item>
                              <item dataType="ObjectRef">2826137326</item>
                            </keys>
                            <values dataType="Array" type="Duality.Component[]" id="2476469402">
                              <item dataType="ObjectRef">3825959764</item>
                              <item dataType="ObjectRef">233454060</item>
                            </values>
                          </body>
                        </compMap>
                        <compTransform dataType="ObjectRef">3825959764</compTransform>
                        <identifier dataType="Struct" type="System.Guid" surrogate="true">
                          <header>
                            <data dataType="Array" type="System.Byte[]" id="3699417686">TLe4VeOdpkC1U5+20O+ryg==</data>
                          </header>
                          <body />
                        </identifier>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        <name dataType="String">PhysicalWall</name>
                        <parent dataType="ObjectRef">2424808843</parent>
                        <prefabLink />
                      </item>
                    </_items>
                    <_size dataType="Int">6</_size>
                    <_version dataType="Int">14</_version>
                  </children>
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2642238280">
                    <_items dataType="Array" type="Duality.Component[]" id="2362166625" />
                    <_size dataType="Int">0</_size>
                    <_version dataType="Int">0</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2578739201" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="2688627524" />
                      <values dataType="Array" type="Duality.Component[]" id="4242691734" />
                    </body>
                  </compMap>
                  <compTransform />
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3426153216">V+BOtMlYoUWUFnwbffiGfQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Geometry</name>
                  <parent dataType="ObjectRef">4637401</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="3069815738">
                  <active dataType="Bool">true</active>
                  <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1212084446">
                    <_items dataType="Array" type="Duality.GameObject[]" id="3417549584" length="4">
                      <item dataType="Struct" type="Duality.GameObject" id="2363636901">
                        <active dataType="Bool">true</active>
                        <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2520004145">
                          <_items dataType="Array" type="Duality.GameObject[]" id="1915993134" length="4">
                            <item dataType="Struct" type="Duality.GameObject" id="911839951">
                              <active dataType="Bool">true</active>
                              <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1539617535">
                                <_items dataType="Array" type="Duality.GameObject[]" id="2042136878" length="4">
                                  <item dataType="Struct" type="Duality.GameObject" id="2553046142">
                                    <active dataType="Bool">true</active>
                                    <children />
                                    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2474002450">
                                      <_items dataType="Array" type="Duality.Component[]" id="906198608" length="4">
                                        <item dataType="Struct" type="Duality.Components.Transform" id="618393778">
                                          <active dataType="Bool">true</active>
                                          <gameobj dataType="ObjectRef">2553046142</gameobj>
                                        </item>
                                        <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1320855370">
                                          <active dataType="Bool">true</active>
                                          <gameobj dataType="ObjectRef">2553046142</gameobj>
                                        </item>
                                        <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="3151739332">
                                          <active dataType="Bool">true</active>
                                          <gameobj dataType="ObjectRef">2553046142</gameobj>
                                        </item>
                                      </_items>
                                      <_size dataType="Int">3</_size>
                                      <_version dataType="Int">3</_version>
                                    </compList>
                                    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="768726986" surrogate="true">
                                      <header />
                                      <body>
                                        <keys dataType="Array" type="System.Type[]" id="1174868552">
                                          <item dataType="ObjectRef">4020116262</item>
                                          <item dataType="ObjectRef">2826137326</item>
                                          <item dataType="Type" id="2845262956" value="Duality.Components.Renderers.RigidBodyRenderer" />
                                        </keys>
                                        <values dataType="Array" type="Duality.Component[]" id="3978850526">
                                          <item dataType="ObjectRef">618393778</item>
                                          <item dataType="ObjectRef">1320855370</item>
                                          <item dataType="ObjectRef">3151739332</item>
                                        </values>
                                      </body>
                                    </compMap>
                                    <compTransform dataType="ObjectRef">618393778</compTransform>
                                    <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                      <header>
                                        <data dataType="Array" type="System.Byte[]" id="435605684">uC4dpzGS60qNf60kQfRrLQ==</data>
                                      </header>
                                      <body />
                                    </identifier>
                                    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                    <name dataType="String">DoorPanel</name>
                                    <parent dataType="ObjectRef">911839951</parent>
                                    <prefabLink />
                                  </item>
                                </_items>
                                <_size dataType="Int">1</_size>
                                <_version dataType="Int">1</_version>
                              </children>
                              <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3532139360">
                                <_items dataType="Array" type="Duality.Component[]" id="870070581" length="4">
                                  <item dataType="Struct" type="Duality.Components.Transform" id="3272154883">
                                    <active dataType="Bool">true</active>
                                    <gameobj dataType="ObjectRef">911839951</gameobj>
                                  </item>
                                  <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3974616475">
                                    <active dataType="Bool">true</active>
                                    <gameobj dataType="ObjectRef">911839951</gameobj>
                                  </item>
                                  <item dataType="Struct" type="DualStickSpaceShooter.DoorControl" id="2501450154">
                                    <active dataType="Bool">true</active>
                                    <gameobj dataType="ObjectRef">911839951</gameobj>
                                  </item>
                                </_items>
                                <_size dataType="Int">3</_size>
                                <_version dataType="Int">3</_version>
                              </compList>
                              <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3178224301" surrogate="true">
                                <header />
                                <body>
                                  <keys dataType="Array" type="System.Type[]" id="3993431076">
                                    <item dataType="ObjectRef">4020116262</item>
                                    <item dataType="ObjectRef">2826137326</item>
                                    <item dataType="Type" id="4267879108" value="DualStickSpaceShooter.DoorControl" />
                                  </keys>
                                  <values dataType="Array" type="Duality.Component[]" id="3643939094">
                                    <item dataType="ObjectRef">3272154883</item>
                                    <item dataType="ObjectRef">3974616475</item>
                                    <item dataType="ObjectRef">2501450154</item>
                                  </values>
                                </body>
                              </compMap>
                              <compTransform dataType="ObjectRef">3272154883</compTransform>
                              <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                <header>
                                  <data dataType="Array" type="System.Byte[]" id="2486689312">r9iaFsM+wUqMPR71sSxBoQ==</data>
                                </header>
                                <body />
                              </identifier>
                              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                              <name dataType="String">DoorLower</name>
                              <parent dataType="ObjectRef">2363636901</parent>
                              <prefabLink />
                            </item>
                            <item dataType="Struct" type="Duality.GameObject" id="3484063288">
                              <active dataType="Bool">true</active>
                              <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="883117220">
                                <_items dataType="Array" type="Duality.GameObject[]" id="4010481860" length="4">
                                  <item dataType="Struct" type="Duality.GameObject" id="1429076166">
                                    <active dataType="Bool">true</active>
                                    <children />
                                    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1884145022">
                                      <_items dataType="Array" type="Duality.Component[]" id="2316655248" length="4">
                                        <item dataType="Struct" type="Duality.Components.Transform" id="3789391098">
                                          <active dataType="Bool">true</active>
                                          <gameobj dataType="ObjectRef">1429076166</gameobj>
                                        </item>
                                        <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="196885394">
                                          <active dataType="Bool">true</active>
                                          <gameobj dataType="ObjectRef">1429076166</gameobj>
                                        </item>
                                        <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="2027769356">
                                          <active dataType="Bool">true</active>
                                          <gameobj dataType="ObjectRef">1429076166</gameobj>
                                        </item>
                                      </_items>
                                      <_size dataType="Int">3</_size>
                                      <_version dataType="Int">3</_version>
                                    </compList>
                                    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1091008650" surrogate="true">
                                      <header />
                                      <body>
                                        <keys dataType="Array" type="System.Type[]" id="2135085148">
                                          <item dataType="ObjectRef">4020116262</item>
                                          <item dataType="ObjectRef">2826137326</item>
                                          <item dataType="ObjectRef">2845262956</item>
                                        </keys>
                                        <values dataType="Array" type="Duality.Component[]" id="75603734">
                                          <item dataType="ObjectRef">3789391098</item>
                                          <item dataType="ObjectRef">196885394</item>
                                          <item dataType="ObjectRef">2027769356</item>
                                        </values>
                                      </body>
                                    </compMap>
                                    <compTransform dataType="ObjectRef">3789391098</compTransform>
                                    <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                      <header>
                                        <data dataType="Array" type="System.Byte[]" id="2880513224">wrRPiGLgB0yF8D02FbEfAw==</data>
                                      </header>
                                      <body />
                                    </identifier>
                                    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                    <name dataType="String">DoorPanel</name>
                                    <parent dataType="ObjectRef">3484063288</parent>
                                    <prefabLink />
                                  </item>
                                </_items>
                                <_size dataType="Int">1</_size>
                                <_version dataType="Int">1</_version>
                              </children>
                              <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1577542422">
                                <_items dataType="Array" type="Duality.Component[]" id="3548719854" length="4">
                                  <item dataType="Struct" type="Duality.Components.Transform" id="1549410924">
                                    <active dataType="Bool">true</active>
                                    <gameobj dataType="ObjectRef">3484063288</gameobj>
                                  </item>
                                  <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2251872516">
                                    <active dataType="Bool">true</active>
                                    <gameobj dataType="ObjectRef">3484063288</gameobj>
                                  </item>
                                  <item dataType="Struct" type="DualStickSpaceShooter.DoorControl" id="778706195">
                                    <active dataType="Bool">true</active>
                                    <gameobj dataType="ObjectRef">3484063288</gameobj>
                                  </item>
                                </_items>
                                <_size dataType="Int">3</_size>
                                <_version dataType="Int">3</_version>
                              </compList>
                              <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3886767008" surrogate="true">
                                <header />
                                <body>
                                  <keys dataType="Array" type="System.Type[]" id="511109384">
                                    <item dataType="ObjectRef">4020116262</item>
                                    <item dataType="ObjectRef">2826137326</item>
                                    <item dataType="ObjectRef">4267879108</item>
                                  </keys>
                                  <values dataType="Array" type="Duality.Component[]" id="2192732126">
                                    <item dataType="ObjectRef">1549410924</item>
                                    <item dataType="ObjectRef">2251872516</item>
                                    <item dataType="ObjectRef">778706195</item>
                                  </values>
                                </body>
                              </compMap>
                              <compTransform dataType="ObjectRef">1549410924</compTransform>
                              <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                <header>
                                  <data dataType="Array" type="System.Byte[]" id="1819842292">a9/tUUfPaEm0clHk1OL/Ow==</data>
                                </header>
                                <body />
                              </identifier>
                              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                              <name dataType="String">DoorUpper</name>
                              <parent dataType="ObjectRef">2363636901</parent>
                              <prefabLink />
                            </item>
                          </_items>
                          <_size dataType="Int">2</_size>
                          <_version dataType="Int">2</_version>
                        </children>
                        <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="317475424">
                          <_items dataType="Array" type="Duality.Component[]" id="2128309275" length="4">
                            <item dataType="Struct" type="Duality.Components.Transform" id="428984537">
                              <active dataType="Bool">true</active>
                              <gameobj dataType="ObjectRef">2363636901</gameobj>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                          <_version dataType="Int">1</_version>
                        </compList>
                        <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2294205923" surrogate="true">
                          <header />
                          <body>
                            <keys dataType="Array" type="System.Type[]" id="3410800548">
                              <item dataType="ObjectRef">4020116262</item>
                            </keys>
                            <values dataType="Array" type="Duality.Component[]" id="3912802070">
                              <item dataType="ObjectRef">428984537</item>
                            </values>
                          </body>
                        </compMap>
                        <compTransform dataType="ObjectRef">428984537</compTransform>
                        <identifier dataType="Struct" type="System.Guid" surrogate="true">
                          <header>
                            <data dataType="Array" type="System.Byte[]" id="1193037472">YXNOXqwSb0+CZcAjYCyMqw==</data>
                          </header>
                          <body />
                        </identifier>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        <name dataType="String">Door</name>
                        <parent dataType="ObjectRef">3069815738</parent>
                        <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="1975420406">
                          <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="3072009165">
                            <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="2127264294" length="8">
                              <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                                <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1271920540">
                                  <_items dataType="Array" type="System.Int32[]" id="1995426244" />
                                  <_size dataType="Int">0</_size>
                                  <_version dataType="Int">1</_version>
                                </childIndex>
                                <componentType dataType="ObjectRef">4020116262</componentType>
                                <prop dataType="ObjectRef">1967265150</prop>
                                <val dataType="Struct" type="OpenTK.Vector3">
                                  <X dataType="Float">993.5928</X>
                                  <Y dataType="Float">-359.476868</Y>
                                  <Z dataType="Float">0</Z>
                                </val>
                              </item>
                              <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                                <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="999052310">
                                  <_items dataType="Array" type="System.Int32[]" id="2585193782" length="4">
                                    <item dataType="Int">1</item>
                                  </_items>
                                  <_size dataType="Int">1</_size>
                                  <_version dataType="Int">2</_version>
                                </childIndex>
                                <componentType dataType="ObjectRef">2826137326</componentType>
                                <prop dataType="PropertyInfo" id="443587080" value="P:Duality.Components.Physics.RigidBody:Shapes" />
                                <val dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2724264882">
                                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1184906" length="4" />
                                  <_size dataType="Int">0</_size>
                                  <_version dataType="Int">12</_version>
                                </val>
                              </item>
                              <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                                <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3921967668">
                                  <_items dataType="Array" type="System.Int32[]" id="915149276" length="4" />
                                  <_size dataType="Int">1</_size>
                                  <_version dataType="Int">2</_version>
                                </childIndex>
                                <componentType dataType="ObjectRef">2826137326</componentType>
                                <prop dataType="ObjectRef">443587080</prop>
                                <val dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2921737614">
                                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3790919694" length="4" />
                                  <_size dataType="Int">0</_size>
                                  <_version dataType="Int">12</_version>
                                </val>
                              </item>
                            </_items>
                            <_size dataType="Int">3</_size>
                            <_version dataType="Int">3654</_version>
                          </changes>
                          <obj dataType="ObjectRef">2363636901</obj>
                          <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                            <contentPath dataType="String">Data\Prefabs\Door.Prefab.res</contentPath>
                          </prefab>
                        </prefabLink>
                      </item>
                      <item dataType="Struct" type="Duality.GameObject" id="1432135143">
                        <active dataType="Bool">true</active>
                        <children />
                        <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4247062595">
                          <_items dataType="Array" type="Duality.Component[]" id="1357315110" length="4">
                            <item dataType="Struct" type="Duality.Components.Transform" id="3792450075">
                              <active dataType="Bool">true</active>
                              <angle dataType="Float">0</angle>
                              <angleAbs dataType="Float">0</angleAbs>
                              <angleVel dataType="Float">0</angleVel>
                              <angleVelAbs dataType="Float">0</angleVelAbs>
                              <deriveAngle dataType="Bool">true</deriveAngle>
                              <gameobj dataType="ObjectRef">1432135143</gameobj>
                              <ignoreParent dataType="Bool">false</ignoreParent>
                              <parentTransform />
                              <pos dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">877.2349</X>
                                <Y dataType="Float">-356.560944</Y>
                                <Z dataType="Float">0</Z>
                              </pos>
                              <posAbs dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">877.2349</X>
                                <Y dataType="Float">-356.560944</Y>
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
                            <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="199944371">
                              <active dataType="Bool">true</active>
                              <angularDamp dataType="Float">0.3</angularDamp>
                              <angularVel dataType="Float">0</angularVel>
                              <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Static" value="0" />
                              <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                              <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1, Cat2, Cat4, Cat6, Cat7, Cat8, Cat9, Cat10, Cat11, Cat12, Cat13, Cat14, Cat15, Cat16, Cat17, Cat18, Cat19, Cat20, Cat21, Cat22, Cat23, Cat24, Cat25, Cat26, Cat27, Cat28, Cat29, Cat30, Cat31" value="2147483627" />
                              <continous dataType="Bool">false</continous>
                              <explicitMass dataType="Float">0</explicitMass>
                              <fixedAngle dataType="Bool">false</fixedAngle>
                              <gameobj dataType="ObjectRef">1432135143</gameobj>
                              <ignoreGravity dataType="Bool">false</ignoreGravity>
                              <joints />
                              <linearDamp dataType="Float">0.3</linearDamp>
                              <linearVel dataType="Struct" type="OpenTK.Vector2">
                                <X dataType="Float">0</X>
                                <Y dataType="Float">0</Y>
                              </linearVel>
                              <revolutions dataType="Float">0</revolutions>
                              <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1574837059">
                                <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3235368486" length="4">
                                  <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="2269479168">
                                    <density dataType="Float">1</density>
                                    <friction dataType="Float">0.3</friction>
                                    <parent dataType="ObjectRef">199944371</parent>
                                    <restitution dataType="Float">0.3</restitution>
                                    <sensor dataType="Bool">true</sensor>
                                    <vertices dataType="Array" type="OpenTK.Vector2[]" id="200061596">
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-17.9537354</X>
                                        <Y dataType="Float">-133.656158</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">112.612915</X>
                                        <Y dataType="Float">-93.20889</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">116.870483</X>
                                        <Y dataType="Float">96.96423</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-15.0975952</X>
                                        <Y dataType="Float">154.631927</Y>
                                      </item>
                                    </vertices>
                                  </item>
                                </_items>
                                <_size dataType="Int">1</_size>
                                <_version dataType="Int">3</_version>
                              </shapes>
                            </item>
                            <item dataType="Struct" type="DualStickSpaceShooter.Trigger" id="4232909143">
                              <active dataType="Bool">true</active>
                              <gameobj dataType="ObjectRef">1432135143</gameobj>
                              <targets dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2248019287">
                                <_items dataType="Array" type="Duality.GameObject[]" id="3933766670" length="4">
                                  <item dataType="ObjectRef">911839951</item>
                                  <item dataType="ObjectRef">3484063288</item>
                                </_items>
                                <_size dataType="Int">2</_size>
                                <_version dataType="Int">4</_version>
                              </targets>
                            </item>
                          </_items>
                          <_size dataType="Int">3</_size>
                          <_version dataType="Int">3</_version>
                        </compList>
                        <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3695685304" surrogate="true">
                          <header />
                          <body>
                            <keys dataType="Array" type="System.Type[]" id="1238112041">
                              <item dataType="ObjectRef">4020116262</item>
                              <item dataType="ObjectRef">2826137326</item>
                              <item dataType="Type" id="1446879246" value="DualStickSpaceShooter.Trigger" />
                            </keys>
                            <values dataType="Array" type="Duality.Component[]" id="65064384">
                              <item dataType="ObjectRef">3792450075</item>
                              <item dataType="ObjectRef">199944371</item>
                              <item dataType="ObjectRef">4232909143</item>
                            </values>
                          </body>
                        </compMap>
                        <compTransform dataType="ObjectRef">3792450075</compTransform>
                        <identifier dataType="Struct" type="System.Guid" surrogate="true">
                          <header>
                            <data dataType="Array" type="System.Byte[]" id="800126219">H+eCJd4PIEKv3H//Ldze2A==</data>
                          </header>
                          <body />
                        </identifier>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        <name dataType="String">DoorTrigger</name>
                        <parent dataType="ObjectRef">3069815738</parent>
                        <prefabLink />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">6</_version>
                  </children>
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2604908298">
                    <_items dataType="Array" type="Duality.Component[]" id="160848636" />
                    <_size dataType="Int">0</_size>
                    <_version dataType="Int">0</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1551854382" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1691571488" />
                      <values dataType="Array" type="Duality.Component[]" id="1875591054" />
                    </body>
                  </compMap>
                  <compTransform />
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2671643708">1+OlG1w8GEieCJGDwG9SdQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Objects</name>
                  <parent dataType="ObjectRef">4637401</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">6</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1848531968">
              <_items dataType="ObjectRef">2362166625</_items>
              <_size dataType="Int">0</_size>
              <_version dataType="Int">0</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="141296667" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="1643527892" />
                <values dataType="Array" type="Duality.Component[]" id="2281535926" />
              </body>
            </compMap>
            <compTransform />
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2056603632">3oYhamckyESPe06f2CzNzw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">StartRoom</name>
            <parent dataType="ObjectRef">4117782497</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2980356935">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="129088455">
              <_items dataType="Array" type="Duality.GameObject[]" id="3578226894" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="904119494">
                  <active dataType="Bool">true</active>
                  <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2187599226">
                    <_items dataType="Array" type="Duality.GameObject[]" id="2625527680" length="4">
                      <item dataType="Struct" type="Duality.GameObject" id="1705981593">
                        <active dataType="Bool">true</active>
                        <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3594937693">
                          <_items dataType="Array" type="Duality.GameObject[]" id="3049987430" length="4">
                            <item dataType="Struct" type="Duality.GameObject" id="1527978758">
                              <active dataType="Bool">true</active>
                              <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="588807498">
                                <_items dataType="Array" type="Duality.GameObject[]" id="3226146656" length="4">
                                  <item dataType="Struct" type="Duality.GameObject" id="3283637799">
                                    <active dataType="Bool">true</active>
                                    <children />
                                    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="829143459">
                                      <_items dataType="Array" type="Duality.Component[]" id="3772059494" length="4">
                                        <item dataType="Struct" type="Duality.Components.Transform" id="1348985435">
                                          <active dataType="Bool">true</active>
                                          <gameobj dataType="ObjectRef">3283637799</gameobj>
                                        </item>
                                        <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2051447027">
                                          <active dataType="Bool">true</active>
                                          <gameobj dataType="ObjectRef">3283637799</gameobj>
                                        </item>
                                        <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="3882330989">
                                          <active dataType="Bool">true</active>
                                          <gameobj dataType="ObjectRef">3283637799</gameobj>
                                        </item>
                                      </_items>
                                      <_size dataType="Int">3</_size>
                                      <_version dataType="Int">3</_version>
                                    </compList>
                                    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1048840056" surrogate="true">
                                      <header />
                                      <body>
                                        <keys dataType="Array" type="System.Type[]" id="2102827721">
                                          <item dataType="ObjectRef">4020116262</item>
                                          <item dataType="ObjectRef">2826137326</item>
                                          <item dataType="ObjectRef">2845262956</item>
                                        </keys>
                                        <values dataType="Array" type="Duality.Component[]" id="308326976">
                                          <item dataType="ObjectRef">1348985435</item>
                                          <item dataType="ObjectRef">2051447027</item>
                                          <item dataType="ObjectRef">3882330989</item>
                                        </values>
                                      </body>
                                    </compMap>
                                    <compTransform dataType="ObjectRef">1348985435</compTransform>
                                    <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                      <header>
                                        <data dataType="Array" type="System.Byte[]" id="1741356779">tRg9S4WSmE6JrRqdpT/gCg==</data>
                                      </header>
                                      <body />
                                    </identifier>
                                    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                    <name dataType="String">DoorPanel</name>
                                    <parent dataType="ObjectRef">1527978758</parent>
                                    <prefabLink />
                                  </item>
                                </_items>
                                <_size dataType="Int">1</_size>
                                <_version dataType="Int">1</_version>
                              </children>
                              <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2039990938">
                                <_items dataType="Array" type="Duality.Component[]" id="2926605872" length="4">
                                  <item dataType="Struct" type="Duality.Components.Transform" id="3888293690">
                                    <active dataType="Bool">true</active>
                                    <gameobj dataType="ObjectRef">1527978758</gameobj>
                                  </item>
                                  <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="295787986">
                                    <active dataType="Bool">true</active>
                                    <gameobj dataType="ObjectRef">1527978758</gameobj>
                                  </item>
                                  <item dataType="Struct" type="DualStickSpaceShooter.DoorControl" id="3117588961">
                                    <active dataType="Bool">true</active>
                                    <gameobj dataType="ObjectRef">1527978758</gameobj>
                                  </item>
                                </_items>
                                <_size dataType="Int">3</_size>
                                <_version dataType="Int">3</_version>
                              </compList>
                              <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1963401898" surrogate="true">
                                <header />
                                <body>
                                  <keys dataType="Array" type="System.Type[]" id="1868608064">
                                    <item dataType="ObjectRef">4020116262</item>
                                    <item dataType="ObjectRef">2826137326</item>
                                    <item dataType="ObjectRef">4267879108</item>
                                  </keys>
                                  <values dataType="Array" type="Duality.Component[]" id="746431566">
                                    <item dataType="ObjectRef">3888293690</item>
                                    <item dataType="ObjectRef">295787986</item>
                                    <item dataType="ObjectRef">3117588961</item>
                                  </values>
                                </body>
                              </compMap>
                              <compTransform dataType="ObjectRef">3888293690</compTransform>
                              <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                <header>
                                  <data dataType="Array" type="System.Byte[]" id="1270911196">eli6KqmPnkO99f6T97Gz4g==</data>
                                </header>
                                <body />
                              </identifier>
                              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                              <name dataType="String">DoorLower</name>
                              <parent dataType="ObjectRef">1705981593</parent>
                              <prefabLink />
                            </item>
                            <item dataType="Struct" type="Duality.GameObject" id="3182992989">
                              <active dataType="Bool">true</active>
                              <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3210980941">
                                <_items dataType="Array" type="Duality.GameObject[]" id="1591183910" length="4">
                                  <item dataType="Struct" type="Duality.GameObject" id="1740149320">
                                    <active dataType="Bool">true</active>
                                    <children />
                                    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1159980708">
                                      <_items dataType="Array" type="Duality.Component[]" id="2349617348" length="4">
                                        <item dataType="Struct" type="Duality.Components.Transform" id="4100464252">
                                          <active dataType="Bool">true</active>
                                          <gameobj dataType="ObjectRef">1740149320</gameobj>
                                        </item>
                                        <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="507958548">
                                          <active dataType="Bool">true</active>
                                          <gameobj dataType="ObjectRef">1740149320</gameobj>
                                        </item>
                                        <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="2338842510">
                                          <active dataType="Bool">true</active>
                                          <gameobj dataType="ObjectRef">1740149320</gameobj>
                                        </item>
                                      </_items>
                                      <_size dataType="Int">3</_size>
                                      <_version dataType="Int">3</_version>
                                    </compList>
                                    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3689126678" surrogate="true">
                                      <header />
                                      <body>
                                        <keys dataType="Array" type="System.Type[]" id="3615950062">
                                          <item dataType="ObjectRef">4020116262</item>
                                          <item dataType="ObjectRef">2826137326</item>
                                          <item dataType="ObjectRef">2845262956</item>
                                        </keys>
                                        <values dataType="Array" type="Duality.Component[]" id="2604917194">
                                          <item dataType="ObjectRef">4100464252</item>
                                          <item dataType="ObjectRef">507958548</item>
                                          <item dataType="ObjectRef">2338842510</item>
                                        </values>
                                      </body>
                                    </compMap>
                                    <compTransform dataType="ObjectRef">4100464252</compTransform>
                                    <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                      <header>
                                        <data dataType="Array" type="System.Byte[]" id="274681566">ONNc2OS1HkGJmwrY4b5jbg==</data>
                                      </header>
                                      <body />
                                    </identifier>
                                    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                    <name dataType="String">DoorPanel</name>
                                    <parent dataType="ObjectRef">3182992989</parent>
                                    <prefabLink />
                                  </item>
                                </_items>
                                <_size dataType="Int">1</_size>
                                <_version dataType="Int">1</_version>
                              </children>
                              <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3185692856">
                                <_items dataType="Array" type="Duality.Component[]" id="1762286631" length="4">
                                  <item dataType="Struct" type="Duality.Components.Transform" id="1248340625">
                                    <active dataType="Bool">true</active>
                                    <gameobj dataType="ObjectRef">3182992989</gameobj>
                                  </item>
                                  <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1950802217">
                                    <active dataType="Bool">true</active>
                                    <gameobj dataType="ObjectRef">3182992989</gameobj>
                                  </item>
                                  <item dataType="Struct" type="DualStickSpaceShooter.DoorControl" id="477635896">
                                    <active dataType="Bool">true</active>
                                    <gameobj dataType="ObjectRef">3182992989</gameobj>
                                  </item>
                                </_items>
                                <_size dataType="Int">3</_size>
                                <_version dataType="Int">3</_version>
                              </compList>
                              <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2470398503" surrogate="true">
                                <header />
                                <body>
                                  <keys dataType="Array" type="System.Type[]" id="1784545428">
                                    <item dataType="ObjectRef">4020116262</item>
                                    <item dataType="ObjectRef">2826137326</item>
                                    <item dataType="ObjectRef">4267879108</item>
                                  </keys>
                                  <values dataType="Array" type="Duality.Component[]" id="1164026934">
                                    <item dataType="ObjectRef">1248340625</item>
                                    <item dataType="ObjectRef">1950802217</item>
                                    <item dataType="ObjectRef">477635896</item>
                                  </values>
                                </body>
                              </compMap>
                              <compTransform dataType="ObjectRef">1248340625</compTransform>
                              <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                <header>
                                  <data dataType="Array" type="System.Byte[]" id="3516414256">NtsTOqtIyEWj2s+BJ1Daow==</data>
                                </header>
                                <body />
                              </identifier>
                              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                              <name dataType="String">DoorUpper</name>
                              <parent dataType="ObjectRef">1705981593</parent>
                              <prefabLink />
                            </item>
                          </_items>
                          <_size dataType="Int">2</_size>
                          <_version dataType="Int">2</_version>
                        </children>
                        <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="172657016">
                          <_items dataType="Array" type="Duality.Component[]" id="3010496055" length="4">
                            <item dataType="Struct" type="Duality.Components.Transform" id="4066296525">
                              <active dataType="Bool">true</active>
                              <gameobj dataType="ObjectRef">1705981593</gameobj>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                          <_version dataType="Int">1</_version>
                        </compList>
                        <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3002118903" surrogate="true">
                          <header />
                          <body>
                            <keys dataType="Array" type="System.Type[]" id="2228777876">
                              <item dataType="ObjectRef">4020116262</item>
                            </keys>
                            <values dataType="Array" type="Duality.Component[]" id="3428537398">
                              <item dataType="ObjectRef">4066296525</item>
                            </values>
                          </body>
                        </compMap>
                        <compTransform dataType="ObjectRef">4066296525</compTransform>
                        <identifier dataType="Struct" type="System.Guid" surrogate="true">
                          <header>
                            <data dataType="Array" type="System.Byte[]" id="125313584">IXTgj0+8/UKQDVbBQS+PRQ==</data>
                          </header>
                          <body />
                        </identifier>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        <name dataType="String">Door</name>
                        <parent dataType="ObjectRef">904119494</parent>
                        <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="4165609254">
                          <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="2055841129">
                            <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="2877596942">
                              <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                                <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3179483788">
                                  <_items dataType="Array" type="System.Int32[]" id="1272220068" />
                                  <_size dataType="Int">0</_size>
                                  <_version dataType="Int">1</_version>
                                </childIndex>
                                <componentType dataType="ObjectRef">4020116262</componentType>
                                <prop dataType="PropertyInfo" id="898993142" value="P:Duality.Components.Transform:RelativeAngle" />
                                <val dataType="Float">0.495453477</val>
                              </item>
                              <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                                <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="116537880">
                                  <_items dataType="ObjectRef">1272220068</_items>
                                  <_size dataType="Int">0</_size>
                                  <_version dataType="Int">1</_version>
                                </childIndex>
                                <componentType dataType="ObjectRef">4020116262</componentType>
                                <prop dataType="ObjectRef">1967265150</prop>
                                <val dataType="Struct" type="OpenTK.Vector3">
                                  <X dataType="Float">2855.55273</X>
                                  <Y dataType="Float">-351.9199</Y>
                                  <Z dataType="Float">0</Z>
                                </val>
                              </item>
                              <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                                <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1221633938">
                                  <_items dataType="Array" type="System.Int32[]" id="179960730" length="4">
                                    <item dataType="Int">1</item>
                                  </_items>
                                  <_size dataType="Int">1</_size>
                                  <_version dataType="Int">2</_version>
                                </childIndex>
                                <componentType dataType="ObjectRef">2826137326</componentType>
                                <prop dataType="ObjectRef">443587080</prop>
                                <val dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="519263108">
                                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1213319644" length="4" />
                                  <_size dataType="Int">0</_size>
                                  <_version dataType="Int">12</_version>
                                </val>
                              </item>
                              <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                                <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="378062222">
                                  <_items dataType="Array" type="System.Int32[]" id="864672990" length="4" />
                                  <_size dataType="Int">1</_size>
                                  <_version dataType="Int">2</_version>
                                </childIndex>
                                <componentType dataType="ObjectRef">2826137326</componentType>
                                <prop dataType="ObjectRef">443587080</prop>
                                <val dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1741742032">
                                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="4085717488" length="4" />
                                  <_size dataType="Int">0</_size>
                                  <_version dataType="Int">12</_version>
                                </val>
                              </item>
                            </_items>
                            <_size dataType="Int">4</_size>
                            <_version dataType="Int">4057</_version>
                          </changes>
                          <obj dataType="ObjectRef">1705981593</obj>
                          <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                            <contentPath dataType="String">Data\Prefabs\Door.Prefab.res</contentPath>
                          </prefab>
                        </prefabLink>
                      </item>
                      <item dataType="Struct" type="Duality.GameObject" id="2309548617">
                        <active dataType="Bool">true</active>
                        <children />
                        <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="541854765">
                          <_items dataType="Array" type="Duality.Component[]" id="3589102182" length="4">
                            <item dataType="Struct" type="Duality.Components.Transform" id="374896253">
                              <active dataType="Bool">true</active>
                              <angle dataType="Float">0.5474664</angle>
                              <angleAbs dataType="Float">0.5474664</angleAbs>
                              <angleVel dataType="Float">0</angleVel>
                              <angleVelAbs dataType="Float">0</angleVelAbs>
                              <deriveAngle dataType="Bool">true</deriveAngle>
                              <gameobj dataType="ObjectRef">2309548617</gameobj>
                              <ignoreParent dataType="Bool">false</ignoreParent>
                              <parentTransform />
                              <pos dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">2773.19141</X>
                                <Y dataType="Float">-381.261963</Y>
                                <Z dataType="Float">0</Z>
                              </pos>
                              <posAbs dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">2773.19141</X>
                                <Y dataType="Float">-381.261963</Y>
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
                            <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1077357845">
                              <active dataType="Bool">true</active>
                              <angularDamp dataType="Float">0.3</angularDamp>
                              <angularVel dataType="Float">0</angularVel>
                              <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Static" value="0" />
                              <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                              <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1, Cat2, Cat4, Cat6, Cat7, Cat8, Cat9, Cat10, Cat11, Cat12, Cat13, Cat14, Cat15, Cat16, Cat17, Cat18, Cat19, Cat20, Cat21, Cat22, Cat23, Cat24, Cat25, Cat26, Cat27, Cat28, Cat29, Cat30, Cat31" value="2147483627" />
                              <continous dataType="Bool">false</continous>
                              <explicitMass dataType="Float">0</explicitMass>
                              <fixedAngle dataType="Bool">false</fixedAngle>
                              <gameobj dataType="ObjectRef">2309548617</gameobj>
                              <ignoreGravity dataType="Bool">false</ignoreGravity>
                              <joints />
                              <linearDamp dataType="Float">0.3</linearDamp>
                              <linearVel dataType="Struct" type="OpenTK.Vector2">
                                <X dataType="Float">0</X>
                                <Y dataType="Float">0</Y>
                              </linearVel>
                              <revolutions dataType="Float">0</revolutions>
                              <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2111324613">
                                <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="966559446" length="4">
                                  <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="2735582496">
                                    <density dataType="Float">1</density>
                                    <friction dataType="Float">0.3</friction>
                                    <parent dataType="ObjectRef">1077357845</parent>
                                    <restitution dataType="Float">0.3</restitution>
                                    <sensor dataType="Bool">true</sensor>
                                    <vertices dataType="Array" type="OpenTK.Vector2[]" id="2606827484">
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">77.84215</X>
                                        <Y dataType="Float">-111.54895</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">87.215004</X>
                                        <Y dataType="Float">84.83937</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">8.528244</X>
                                        <Y dataType="Float">126.609314</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-26.3484974</X>
                                        <Y dataType="Float">-107.546524</Y>
                                      </item>
                                    </vertices>
                                  </item>
                                </_items>
                                <_size dataType="Int">1</_size>
                                <_version dataType="Int">4</_version>
                              </shapes>
                            </item>
                            <item dataType="Struct" type="DualStickSpaceShooter.Trigger" id="815355321">
                              <active dataType="Bool">true</active>
                              <gameobj dataType="ObjectRef">2309548617</gameobj>
                              <targets dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3486678329">
                                <_items dataType="Array" type="Duality.GameObject[]" id="4211440334" length="4">
                                  <item dataType="ObjectRef">1527978758</item>
                                  <item dataType="ObjectRef">3182992989</item>
                                </_items>
                                <_size dataType="Int">2</_size>
                                <_version dataType="Int">6</_version>
                              </targets>
                            </item>
                          </_items>
                          <_size dataType="Int">3</_size>
                          <_version dataType="Int">3</_version>
                        </compList>
                        <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3244015224" surrogate="true">
                          <header />
                          <body>
                            <keys dataType="Array" type="System.Type[]" id="1434085191">
                              <item dataType="ObjectRef">4020116262</item>
                              <item dataType="ObjectRef">2826137326</item>
                              <item dataType="ObjectRef">1446879246</item>
                            </keys>
                            <values dataType="Array" type="Duality.Component[]" id="1924090624">
                              <item dataType="ObjectRef">374896253</item>
                              <item dataType="ObjectRef">1077357845</item>
                              <item dataType="ObjectRef">815355321</item>
                            </values>
                          </body>
                        </compMap>
                        <compTransform dataType="ObjectRef">374896253</compTransform>
                        <identifier dataType="Struct" type="System.Guid" surrogate="true">
                          <header>
                            <data dataType="Array" type="System.Byte[]" id="154013381">Y6CcpzTz+0qU0d1Ekqkq0Q==</data>
                          </header>
                          <body />
                        </identifier>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        <name dataType="String">DoorTrigger</name>
                        <parent dataType="ObjectRef">904119494</parent>
                        <prefabLink />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">4</_version>
                  </children>
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="73056058">
                    <_items dataType="ObjectRef">160848636</_items>
                    <_size dataType="Int">0</_size>
                    <_version dataType="Int">0</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1972857338" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="63195904" />
                      <values dataType="Array" type="Duality.Component[]" id="1615561166" />
                    </body>
                  </compMap>
                  <compTransform />
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2957055388">vIDmOgUQ8ES8Eh83ppuNig==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Objects</name>
                  <parent dataType="ObjectRef">2980356935</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="326400557">
                  <active dataType="Bool">true</active>
                  <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3596987981">
                    <_items dataType="Array" type="Duality.GameObject[]" id="4198730278">
                      <item dataType="Struct" type="Duality.GameObject" id="1080969295">
                        <active dataType="Bool">true</active>
                        <children />
                        <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3909702415">
                          <_items dataType="Array" type="Duality.Component[]" id="3144619950" length="4">
                            <item dataType="Struct" type="Duality.Components.Transform" id="3441284227">
                              <active dataType="Bool">true</active>
                              <angle dataType="Float">5.134634</angle>
                              <angleAbs dataType="Float">5.134634</angleAbs>
                              <angleVel dataType="Float">0</angleVel>
                              <angleVelAbs dataType="Float">0</angleVelAbs>
                              <deriveAngle dataType="Bool">true</deriveAngle>
                              <gameobj dataType="ObjectRef">1080969295</gameobj>
                              <ignoreParent dataType="Bool">false</ignoreParent>
                              <parentTransform />
                              <pos dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">2806.50586</X>
                                <Y dataType="Float">-1404.89636</Y>
                                <Z dataType="Float">0</Z>
                              </pos>
                              <posAbs dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">2806.50586</X>
                                <Y dataType="Float">-1404.89636</Y>
                                <Z dataType="Float">0</Z>
                              </posAbs>
                              <scale dataType="Float">7.46937275</scale>
                              <scaleAbs dataType="Float">7.46937275</scaleAbs>
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
                            <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2723135863">
                              <active dataType="Bool">true</active>
                              <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                <A dataType="Byte">255</A>
                                <B dataType="Byte">255</B>
                                <G dataType="Byte">255</G>
                                <R dataType="Byte">255</R>
                              </colorTint>
                              <customMat />
                              <gameobj dataType="ObjectRef">1080969295</gameobj>
                              <offset dataType="Int">0</offset>
                              <pixelGrid dataType="Bool">false</pixelGrid>
                              <rect dataType="Struct" type="Duality.Rect">
                                <H dataType="Float">229</H>
                                <W dataType="Float">243</W>
                                <X dataType="Float">-121.5</X>
                                <Y dataType="Float">-114.5</Y>
                              </rect>
                              <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                              <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                                <contentPath dataType="String">Data\Materials\Background\Backdrop5.Material.res</contentPath>
                              </sharedMat>
                              <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                            </item>
                          </_items>
                          <_size dataType="Int">2</_size>
                          <_version dataType="Int">2</_version>
                        </compList>
                        <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1966007264" surrogate="true">
                          <header />
                          <body>
                            <keys dataType="Array" type="System.Type[]" id="3167168165">
                              <item dataType="ObjectRef">4020116262</item>
                              <item dataType="ObjectRef">2838274704</item>
                            </keys>
                            <values dataType="Array" type="Duality.Component[]" id="3346857064">
                              <item dataType="ObjectRef">3441284227</item>
                              <item dataType="ObjectRef">2723135863</item>
                            </values>
                          </body>
                        </compMap>
                        <compTransform dataType="ObjectRef">3441284227</compTransform>
                        <identifier dataType="Struct" type="System.Guid" surrogate="true">
                          <header>
                            <data dataType="Array" type="System.Byte[]" id="782355311">WwrUR7pedkKNSFlwBCLrYg==</data>
                          </header>
                          <body />
                        </identifier>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        <name dataType="String">Backdrop5</name>
                        <parent dataType="ObjectRef">326400557</parent>
                        <prefabLink />
                      </item>
                      <item dataType="Struct" type="Duality.GameObject" id="1057680313">
                        <active dataType="Bool">true</active>
                        <children />
                        <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4004210361">
                          <_items dataType="Array" type="Duality.Component[]" id="3564626638" length="4">
                            <item dataType="Struct" type="Duality.Components.Transform" id="3417995245">
                              <active dataType="Bool">true</active>
                              <angle dataType="Float">5.996236</angle>
                              <angleAbs dataType="Float">5.996236</angleAbs>
                              <angleVel dataType="Float">0</angleVel>
                              <angleVelAbs dataType="Float">0</angleVelAbs>
                              <deriveAngle dataType="Bool">true</deriveAngle>
                              <gameobj dataType="ObjectRef">1057680313</gameobj>
                              <ignoreParent dataType="Bool">false</ignoreParent>
                              <parentTransform />
                              <pos dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">1723.75366</X>
                                <Y dataType="Float">-1689.47546</Y>
                                <Z dataType="Float">0</Z>
                              </pos>
                              <posAbs dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">1723.75366</X>
                                <Y dataType="Float">-1689.47546</Y>
                                <Z dataType="Float">0</Z>
                              </posAbs>
                              <scale dataType="Float">10.15502</scale>
                              <scaleAbs dataType="Float">10.15502</scaleAbs>
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
                            <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2699846881">
                              <active dataType="Bool">true</active>
                              <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                <A dataType="Byte">255</A>
                                <B dataType="Byte">255</B>
                                <G dataType="Byte">255</G>
                                <R dataType="Byte">255</R>
                              </colorTint>
                              <customMat />
                              <gameobj dataType="ObjectRef">1057680313</gameobj>
                              <offset dataType="Int">0</offset>
                              <pixelGrid dataType="Bool">false</pixelGrid>
                              <rect dataType="Struct" type="Duality.Rect">
                                <H dataType="Float">229</H>
                                <W dataType="Float">243</W>
                                <X dataType="Float">-121.5</X>
                                <Y dataType="Float">-114.5</Y>
                              </rect>
                              <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                              <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                                <contentPath dataType="String">Data\Materials\Background\Backdrop5.Material.res</contentPath>
                              </sharedMat>
                              <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                            </item>
                          </_items>
                          <_size dataType="Int">2</_size>
                          <_version dataType="Int">2</_version>
                        </compList>
                        <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1608935168" surrogate="true">
                          <header />
                          <body>
                            <keys dataType="Array" type="System.Type[]" id="4172298515">
                              <item dataType="ObjectRef">4020116262</item>
                              <item dataType="ObjectRef">2838274704</item>
                            </keys>
                            <values dataType="Array" type="Duality.Component[]" id="1806731512">
                              <item dataType="ObjectRef">3417995245</item>
                              <item dataType="ObjectRef">2699846881</item>
                            </values>
                          </body>
                        </compMap>
                        <compTransform dataType="ObjectRef">3417995245</compTransform>
                        <identifier dataType="Struct" type="System.Guid" surrogate="true">
                          <header>
                            <data dataType="Array" type="System.Byte[]" id="4248117241">3KN0WX4EakiMKWLjWtUSDw==</data>
                          </header>
                          <body />
                        </identifier>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        <name dataType="String">Backdrop5</name>
                        <parent dataType="ObjectRef">326400557</parent>
                        <prefabLink />
                      </item>
                      <item dataType="Struct" type="Duality.GameObject" id="336249212">
                        <active dataType="Bool">true</active>
                        <children />
                        <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="255537048">
                          <_items dataType="Array" type="Duality.Component[]" id="4286895148" length="4">
                            <item dataType="Struct" type="Duality.Components.Transform" id="2696564144">
                              <active dataType="Bool">true</active>
                              <angle dataType="Float">3.40393353</angle>
                              <angleAbs dataType="Float">3.40393353</angleAbs>
                              <angleVel dataType="Float">0</angleVel>
                              <angleVelAbs dataType="Float">0</angleVelAbs>
                              <deriveAngle dataType="Bool">true</deriveAngle>
                              <gameobj dataType="ObjectRef">336249212</gameobj>
                              <ignoreParent dataType="Bool">false</ignoreParent>
                              <parentTransform />
                              <pos dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">1729.98</X>
                                <Y dataType="Float">466.721222</Y>
                                <Z dataType="Float">0</Z>
                              </pos>
                              <posAbs dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">1729.98</X>
                                <Y dataType="Float">466.721222</Y>
                                <Z dataType="Float">0</Z>
                              </posAbs>
                              <scale dataType="Float">5.21719551</scale>
                              <scaleAbs dataType="Float">5.21719551</scaleAbs>
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
                            <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1978415780">
                              <active dataType="Bool">true</active>
                              <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                <A dataType="Byte">255</A>
                                <B dataType="Byte">255</B>
                                <G dataType="Byte">255</G>
                                <R dataType="Byte">255</R>
                              </colorTint>
                              <customMat />
                              <gameobj dataType="ObjectRef">336249212</gameobj>
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
                          </_items>
                          <_size dataType="Int">2</_size>
                          <_version dataType="Int">2</_version>
                        </compList>
                        <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1663257374" surrogate="true">
                          <header />
                          <body>
                            <keys dataType="Array" type="System.Type[]" id="2375626842">
                              <item dataType="ObjectRef">4020116262</item>
                              <item dataType="ObjectRef">2838274704</item>
                            </keys>
                            <values dataType="Array" type="Duality.Component[]" id="3167486394">
                              <item dataType="ObjectRef">2696564144</item>
                              <item dataType="ObjectRef">1978415780</item>
                            </values>
                          </body>
                        </compMap>
                        <compTransform dataType="ObjectRef">2696564144</compTransform>
                        <identifier dataType="Struct" type="System.Guid" surrogate="true">
                          <header>
                            <data dataType="Array" type="System.Byte[]" id="594390106">II68VrbnHU+A+aX8PnjbQQ==</data>
                          </header>
                          <body />
                        </identifier>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        <name dataType="String">Backdrop4</name>
                        <parent dataType="ObjectRef">326400557</parent>
                        <prefabLink />
                      </item>
                      <item dataType="Struct" type="Duality.GameObject" id="2098191875">
                        <active dataType="Bool">true</active>
                        <children />
                        <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4060175635">
                          <_items dataType="Array" type="Duality.Component[]" id="3188530918" length="4">
                            <item dataType="Struct" type="Duality.Components.Transform" id="163539511">
                              <active dataType="Bool">true</active>
                              <angle dataType="Float">0.0216563344</angle>
                              <angleAbs dataType="Float">0.0216563344</angleAbs>
                              <angleVel dataType="Float">0</angleVel>
                              <angleVelAbs dataType="Float">0</angleVelAbs>
                              <deriveAngle dataType="Bool">true</deriveAngle>
                              <gameobj dataType="ObjectRef">2098191875</gameobj>
                              <ignoreParent dataType="Bool">false</ignoreParent>
                              <parentTransform />
                              <pos dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">1870.54163</X>
                                <Y dataType="Float">-271.3443</Y>
                                <Z dataType="Float">0</Z>
                              </pos>
                              <posAbs dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">1870.54163</X>
                                <Y dataType="Float">-271.3443</Y>
                                <Z dataType="Float">0</Z>
                              </posAbs>
                              <scale dataType="Float">4.20805</scale>
                              <scaleAbs dataType="Float">4.20805</scaleAbs>
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
                            <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3740358443">
                              <active dataType="Bool">true</active>
                              <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                <A dataType="Byte">255</A>
                                <B dataType="Byte">255</B>
                                <G dataType="Byte">255</G>
                                <R dataType="Byte">255</R>
                              </colorTint>
                              <customMat />
                              <gameobj dataType="ObjectRef">2098191875</gameobj>
                              <offset dataType="Int">0</offset>
                              <pixelGrid dataType="Bool">false</pixelGrid>
                              <rect dataType="Struct" type="Duality.Rect">
                                <H dataType="Float">172</H>
                                <W dataType="Float">179</W>
                                <X dataType="Float">-89.5</X>
                                <Y dataType="Float">-86</Y>
                              </rect>
                              <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                              <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                                <contentPath dataType="String">Data\Materials\Background\Backdrop1.Material.res</contentPath>
                              </sharedMat>
                              <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                            </item>
                          </_items>
                          <_size dataType="Int">2</_size>
                          <_version dataType="Int">2</_version>
                        </compList>
                        <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="133538040" surrogate="true">
                          <header />
                          <body>
                            <keys dataType="Array" type="System.Type[]" id="2303685497">
                              <item dataType="ObjectRef">4020116262</item>
                              <item dataType="ObjectRef">2838274704</item>
                            </keys>
                            <values dataType="Array" type="Duality.Component[]" id="225376640">
                              <item dataType="ObjectRef">163539511</item>
                              <item dataType="ObjectRef">3740358443</item>
                            </values>
                          </body>
                        </compMap>
                        <compTransform dataType="ObjectRef">163539511</compTransform>
                        <identifier dataType="Struct" type="System.Guid" surrogate="true">
                          <header>
                            <data dataType="Array" type="System.Byte[]" id="123022459">LpXjquyom0u2QX/JZOtNtg==</data>
                          </header>
                          <body />
                        </identifier>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        <name dataType="String">Backdrop1</name>
                        <parent dataType="ObjectRef">326400557</parent>
                        <prefabLink />
                      </item>
                    </_items>
                    <_size dataType="Int">4</_size>
                    <_version dataType="Int">4</_version>
                  </children>
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2198196408">
                    <_items dataType="ObjectRef">160848636</_items>
                    <_size dataType="Int">0</_size>
                    <_version dataType="Int">0</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="506022439" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="3365011604" />
                      <values dataType="Array" type="Duality.Component[]" id="2536612918" />
                    </body>
                  </compMap>
                  <compTransform />
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3106683184">+zwc0GhZ0kaRCZpgMyY3BA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Geometry</name>
                  <parent dataType="ObjectRef">2980356935</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">4</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2585805056">
              <_items dataType="ObjectRef">160848636</_items>
              <_size dataType="Int">0</_size>
              <_version dataType="Int">0</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2930551877" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="3680555796" />
                <values dataType="Array" type="Duality.Component[]" id="2001711414" />
              </body>
            </compMap>
            <compTransform />
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3745494448">qOzeLXr1YE+JKmk4kDVAjw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">DoorTutorial</name>
            <parent dataType="ObjectRef">4117782497</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">2</_size>
        <_version dataType="Int">4</_version>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3404826744">
        <_items dataType="ObjectRef">2362166625</_items>
        <_size dataType="Int">0</_size>
        <_version dataType="Int">0</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2127926665" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="2082541460" />
          <values dataType="Array" type="Duality.Component[]" id="1091726390" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3708208688">kBlTPCPU60a+yx0Kw9i2mg==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Rooms</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="3709783311">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3733319789">
        <_items dataType="Array" type="Duality.Component[]" id="2856376550" length="4">
          <item dataType="Struct" type="Duality.Components.Diagnostics.ProfileRenderer" id="3976618203">
            <active dataType="Bool">true</active>
            <counterGraphs dataType="Struct" type="System.Collections.Generic.List`1[[System.String]]" id="1703851947">
              <_items dataType="Array" type="System.String[]" id="4269419766">
                <item dataType="String">Duality\Frame</item>
                <item dataType="String">Duality\Frame\Render</item>
                <item dataType="String">Duality\Frame\Update</item>
                <item dataType="String">Duality\Stats\Memory\TotalUsage</item>
              </_items>
              <_size dataType="Int">4</_size>
              <_version dataType="Int">4</_version>
            </counterGraphs>
            <drawGraphs dataType="Bool">false</drawGraphs>
            <gameobj dataType="ObjectRef">3709783311</gameobj>
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
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="515049208" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="3047586311">
            <item dataType="Type" id="802854478" value="Duality.Components.Diagnostics.ProfileRenderer" />
          </keys>
          <values dataType="Array" type="Duality.Component[]" id="3245790336">
            <item dataType="ObjectRef">3976618203</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1097273349">FICQso7hx026l44yVEMD3A==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">ProfileRenderer</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="2246488040">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4077688110">
        <_items dataType="Array" type="Duality.Component[]" id="776918864" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="311835676">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">2246488040</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <parentTransform />
            <pos dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">1550.92627</X>
              <Y dataType="Float">-327.3073</Y>
              <Z dataType="Float">0</Z>
            </pos>
            <posAbs dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">1550.92627</X>
              <Y dataType="Float">-327.3073</Y>
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
          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3888654608">
            <active dataType="Bool">true</active>
            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">0</B>
              <G dataType="Byte">0</G>
              <R dataType="Byte">255</R>
            </colorTint>
            <customMat />
            <gameobj dataType="ObjectRef">2246488040</gameobj>
            <offset dataType="Int">0</offset>
            <pixelGrid dataType="Bool">false</pixelGrid>
            <rect dataType="Struct" type="Duality.Rect">
              <H dataType="Float">30</H>
              <W dataType="Float">24</W>
              <X dataType="Float">-12</X>
              <Y dataType="Float">-19</Y>
            </rect>
            <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
            <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
              <contentPath dataType="String">Data\Materials\Player.Material.res</contentPath>
            </sharedMat>
            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          </item>
        </_items>
        <_size dataType="Int">2</_size>
        <_version dataType="Int">6</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3088511178" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="2090733484">
            <item dataType="ObjectRef">4020116262</item>
            <item dataType="ObjectRef">2838274704</item>
          </keys>
          <values dataType="Array" type="Duality.Component[]" id="1637133238">
            <item dataType="ObjectRef">311835676</item>
            <item dataType="ObjectRef">3888654608</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">311835676</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3491246072">ZeGr2k1HKE6JthgbkqbGpQ==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">ReferenceObject</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">687383225</item>
    <item dataType="ObjectRef">105982944</item>
    <item dataType="ObjectRef">3901085842</item>
    <item dataType="ObjectRef">4637401</item>
    <item dataType="ObjectRef">2980356935</item>
    <item dataType="ObjectRef">2424808843</item>
    <item dataType="ObjectRef">3069815738</item>
    <item dataType="ObjectRef">904119494</item>
    <item dataType="ObjectRef">326400557</item>
    <item dataType="ObjectRef">1696349013</item>
    <item dataType="ObjectRef">2491661213</item>
    <item dataType="ObjectRef">787323885</item>
    <item dataType="ObjectRef">2880709120</item>
    <item dataType="ObjectRef">1391548966</item>
    <item dataType="ObjectRef">1465644832</item>
    <item dataType="ObjectRef">2363636901</item>
    <item dataType="ObjectRef">1432135143</item>
    <item dataType="ObjectRef">1705981593</item>
    <item dataType="ObjectRef">2309548617</item>
    <item dataType="ObjectRef">1080969295</item>
    <item dataType="ObjectRef">1057680313</item>
    <item dataType="ObjectRef">336249212</item>
    <item dataType="ObjectRef">2098191875</item>
    <item dataType="ObjectRef">911839951</item>
    <item dataType="ObjectRef">3484063288</item>
    <item dataType="ObjectRef">1527978758</item>
    <item dataType="ObjectRef">3182992989</item>
    <item dataType="ObjectRef">2553046142</item>
    <item dataType="ObjectRef">1429076166</item>
    <item dataType="ObjectRef">3283637799</item>
    <item dataType="ObjectRef">1740149320</item>
  </serializeObj>
  <sourcePath />
</root>
<!-- XmlFormatterBase Document Separator -->
