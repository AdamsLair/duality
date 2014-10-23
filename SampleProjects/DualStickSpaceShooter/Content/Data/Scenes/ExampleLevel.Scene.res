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
            <focusDist dataType="Float">492.094055</focusDist>
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
                    <active dataType="Bool">true</active>
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
                          <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="2165640420" length="4">
                            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="771872968">
                                <_items dataType="Array" type="System.Int32[]" id="1197357676" length="4" />
                                <_size dataType="Int">1</_size>
                                <_version dataType="Int">2</_version>
                              </childIndex>
                              <componentType />
                              <prop dataType="PropertyInfo" id="886661854" value="P:Duality.GameObject:ActiveSingle" />
                              <val dataType="Bool">true</val>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                          <_version dataType="Int">137</_version>
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
                    <_items dataType="Array" type="Duality.GameObject[]" id="2550693622" length="4">
                      <item dataType="Struct" type="Duality.GameObject" id="2503631456">
                        <active dataType="Bool">true</active>
                        <children />
                        <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2995161692">
                          <_items dataType="Array" type="Duality.Component[]" id="3518373060" length="4">
                            <item dataType="Struct" type="Duality.Components.Transform" id="568979092">
                              <active dataType="Bool">true</active>
                              <angle dataType="Float">0</angle>
                              <angleAbs dataType="Float">0</angleAbs>
                              <angleVel dataType="Float">0</angleVel>
                              <angleVelAbs dataType="Float">0</angleVelAbs>
                              <deriveAngle dataType="Bool">true</deriveAngle>
                              <gameobj dataType="ObjectRef">2503631456</gameobj>
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
                            <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1271440684">
                              <active dataType="Bool">true</active>
                              <angularDamp dataType="Float">0.3</angularDamp>
                              <angularVel dataType="Float">0</angularVel>
                              <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Static" value="0" />
                              <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat4" value="8" />
                              <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1, Cat2, Cat3, Cat5, Cat6, Cat7, Cat8, Cat9, Cat10, Cat11, Cat12, Cat13, Cat14, Cat15, Cat16, Cat17, Cat18, Cat19, Cat20, Cat21, Cat22, Cat23, Cat24, Cat25, Cat26, Cat27, Cat28, Cat29, Cat30, Cat31" value="2147483639" />
                              <continous dataType="Bool">false</continous>
                              <explicitMass dataType="Float">0</explicitMass>
                              <fixedAngle dataType="Bool">false</fixedAngle>
                              <gameobj dataType="ObjectRef">2503631456</gameobj>
                              <ignoreGravity dataType="Bool">false</ignoreGravity>
                              <joints />
                              <linearDamp dataType="Float">0.3</linearDamp>
                              <linearVel dataType="Struct" type="OpenTK.Vector2">
                                <X dataType="Float">0</X>
                                <Y dataType="Float">0</Y>
                              </linearVel>
                              <revolutions dataType="Float">0</revolutions>
                              <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1889525660">
                                <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1705388484" length="4">
                                  <item dataType="Struct" type="Duality.Components.Physics.LoopShapeInfo" id="436132164">
                                    <density dataType="Float">1</density>
                                    <friction dataType="Float">0.3</friction>
                                    <parent dataType="ObjectRef">1271440684</parent>
                                    <restitution dataType="Float">0.3</restitution>
                                    <sensor dataType="Bool">false</sensor>
                                    <vertices dataType="Array" type="OpenTK.Vector2[]" id="3729080900">
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-33.4153175</X>
                                        <Y dataType="Float">213.962692</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">2.58468246</X>
                                        <Y dataType="Float">159.962692</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-35.4153175</X>
                                        <Y dataType="Float">126.962685</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-122.415314</X>
                                        <Y dataType="Float">125.962685</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-166.415314</X>
                                        <Y dataType="Float">181.962692</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-210.415314</X>
                                        <Y dataType="Float">180.962692</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-230.415314</X>
                                        <Y dataType="Float">142.962692</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-223.415314</X>
                                        <Y dataType="Float">93.9626846</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-158.415314</X>
                                        <Y dataType="Float">57.96269</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-95.41531</X>
                                        <Y dataType="Float">59.96269</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-67.41531</X>
                                        <Y dataType="Float">19.9626884</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-95.41531</X>
                                        <Y dataType="Float">-40.03731</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-176.415314</X>
                                        <Y dataType="Float">-57.03731</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-275.4153</X>
                                        <Y dataType="Float">-8.03731251</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-361.4153</X>
                                        <Y dataType="Float">-40.03731</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-352.4153</X>
                                        <Y dataType="Float">-107.037315</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-279.4153</X>
                                        <Y dataType="Float">-176.037308</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-179.415314</X>
                                        <Y dataType="Float">-202.037308</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-75.41531</X>
                                        <Y dataType="Float">-188.037308</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-32.4153175</X>
                                        <Y dataType="Float">-122.037315</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-36.4153175</X>
                                        <Y dataType="Float">-40.03731</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-24.4153175</X>
                                        <Y dataType="Float">11.9626875</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">24.5846825</X>
                                        <Y dataType="Float">59.96269</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">101.584686</X>
                                        <Y dataType="Float">72.9626846</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">141.584686</X>
                                        <Y dataType="Float">46.96269</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">123.584686</X>
                                        <Y dataType="Float">3.96268749</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">59.5846825</X>
                                        <Y dataType="Float">-16.0373116</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">50.5846825</X>
                                        <Y dataType="Float">-52.03731</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">85.58469</X>
                                        <Y dataType="Float">-103.037315</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">177.584686</X>
                                        <Y dataType="Float">-116.037315</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">302.5847</X>
                                        <Y dataType="Float">-71.0373154</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">350.5847</X>
                                        <Y dataType="Float">27.9626884</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">320.5847</X>
                                        <Y dataType="Float">91.9626846</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">268.5847</X>
                                        <Y dataType="Float">117.962685</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">194.584686</X>
                                        <Y dataType="Float">124.962685</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">154.584686</X>
                                        <Y dataType="Float">203.962692</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">178.584686</X>
                                        <Y dataType="Float">241.962692</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">189.584686</X>
                                        <Y dataType="Float">305.962677</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">144.584686</X>
                                        <Y dataType="Float">345.962677</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">61.5846825</X>
                                        <Y dataType="Float">373.962677</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-15.4153175</X>
                                        <Y dataType="Float">348.962677</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-15.4153175</X>
                                        <Y dataType="Float">314.962677</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-4.41531754</X>
                                        <Y dataType="Float">291.962677</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-56.4153175</X>
                                        <Y dataType="Float">263.962677</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-114.415314</X>
                                        <Y dataType="Float">257.962677</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-129.415314</X>
                                        <Y dataType="Float">209.962692</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-79.41531</X>
                                        <Y dataType="Float">191.962692</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-75.41531</X>
                                        <Y dataType="Float">221.962692</Y>
                                      </item>
                                      <item dataType="Struct" type="OpenTK.Vector2">
                                        <X dataType="Float">-33.4153175</X>
                                        <Y dataType="Float">240.962692</Y>
                                      </item>
                                    </vertices>
                                  </item>
                                </_items>
                                <_size dataType="Int">1</_size>
                                <_version dataType="Int">11</_version>
                              </shapes>
                            </item>
                            <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="3102324646">
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
                              <customAreaMaterial dataType="Struct" type="Duality.Resources.BatchInfo" id="3285518366">
                                <dirtyFlag dataType="Enum" type="Duality.Resources.BatchInfo+DirtyFlag" name="All" value="3" />
                                <hashCode dataType="Int">-645760906</hashCode>
                                <mainColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                                  <A dataType="Byte">255</A>
                                  <B dataType="Byte">107</B>
                                  <G dataType="Byte">33</G>
                                  <R dataType="Byte">0</R>
                                </mainColor>
                                <technique dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.DrawTechnique]]">
                                  <contentPath dataType="String">Default:DrawTechnique:Solid</contentPath>
                                </technique>
                                <textures dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.String],[Duality.ContentRef`1[[Duality.Resources.Texture]]]]" id="3601561232" surrogate="true">
                                  <header />
                                  <body>
                                    <mainTex dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Texture]]">
                                      <contentPath dataType="String">Default:Texture:White</contentPath>
                                    </mainTex>
                                  </body>
                                </textures>
                                <uniforms />
                              </customAreaMaterial>
                              <customOutlineMaterial dataType="Struct" type="Duality.Resources.BatchInfo" id="2461295754">
                                <dirtyFlag dataType="Enum" type="Duality.Resources.BatchInfo+DirtyFlag" name="All" value="3" />
                                <hashCode dataType="Int">507382646</hashCode>
                                <mainColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                                  <A dataType="Byte">255</A>
                                  <B dataType="Byte">0</B>
                                  <G dataType="Byte">0</G>
                                  <R dataType="Byte">255</R>
                                </mainColor>
                                <technique dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.DrawTechnique]]">
                                  <contentPath dataType="String">Default:DrawTechnique:Solid</contentPath>
                                </technique>
                                <textures dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.String],[Duality.ContentRef`1[[Duality.Resources.Texture]]]]" id="1672880188" surrogate="true">
                                  <header />
                                  <body>
                                    <mainTex dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Texture]]">
                                      <contentPath dataType="String">Default:Texture:White</contentPath>
                                    </mainTex>
                                  </body>
                                </textures>
                                <uniforms />
                              </customOutlineMaterial>
                              <fillHollowShapes dataType="Bool">true</fillHollowShapes>
                              <gameobj dataType="ObjectRef">2503631456</gameobj>
                              <offset dataType="Int">0</offset>
                              <outlineMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                                <contentPath />
                              </outlineMaterial>
                              <outlineWidth dataType="Float">1</outlineWidth>
                              <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                              <wrapTexture dataType="Bool">true</wrapTexture>
                            </item>
                          </_items>
                          <_size dataType="Int">3</_size>
                          <_version dataType="Int">3</_version>
                        </compList>
                        <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1944852246" surrogate="true">
                          <header />
                          <body>
                            <keys dataType="Array" type="System.Type[]" id="1799680118">
                              <item dataType="ObjectRef">4020116262</item>
                              <item dataType="ObjectRef">2826137326</item>
                              <item dataType="Type" id="2028740576" value="Duality.Components.Renderers.RigidBodyRenderer" />
                            </keys>
                            <values dataType="Array" type="Duality.Component[]" id="3159728410">
                              <item dataType="ObjectRef">568979092</item>
                              <item dataType="ObjectRef">1271440684</item>
                              <item dataType="ObjectRef">3102324646</item>
                            </values>
                          </body>
                        </compMap>
                        <compTransform dataType="ObjectRef">568979092</compTransform>
                        <identifier dataType="Struct" type="System.Guid" surrogate="true">
                          <header>
                            <data dataType="Array" type="System.Byte[]" id="2987811734">UyOCxLjuK0G/4r8JheV6GA==</data>
                          </header>
                          <body />
                        </identifier>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        <name dataType="String">LevelPart</name>
                        <parent dataType="ObjectRef">2424808843</parent>
                        <prefabLink />
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                    <_version dataType="Int">1</_version>
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
              </_items>
              <_size dataType="Int">1</_size>
              <_version dataType="Int">3</_version>
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
        </_items>
        <_size dataType="Int">1</_size>
        <_version dataType="Int">1</_version>
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
    <item dataType="ObjectRef">687383225</item>
    <item dataType="ObjectRef">105982944</item>
    <item dataType="ObjectRef">3901085842</item>
    <item dataType="ObjectRef">4637401</item>
    <item dataType="ObjectRef">2424808843</item>
    <item dataType="ObjectRef">2503631456</item>
  </serializeObj>
  <sourcePath />
</root>
<!-- XmlFormatterBase Document Separator -->
