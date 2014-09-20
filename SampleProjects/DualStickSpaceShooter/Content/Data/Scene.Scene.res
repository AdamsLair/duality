<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <globalGravity dataType="Struct" type="OpenTK.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">0</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="1163277037">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1531404383">
        <_items dataType="Array" type="Duality.Component[]" id="1422731374">
          <item dataType="Struct" type="Duality.Components.Transform" id="3523591969">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">1163277037</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
          </item>
          <item dataType="Struct" type="Duality.Components.Camera" id="1700552844">
            <active dataType="Bool">true</active>
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">1163277037</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <nearZ dataType="Float">0</nearZ>
            <passes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Camera+Pass]]" id="290342936">
              <_items dataType="Array" type="Duality.Components.Camera+Pass[]" id="1201734700" length="4">
                <item dataType="Struct" type="Duality.Components.Camera+Pass" id="3445462244">
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
                <item dataType="Struct" type="Duality.Components.Camera+Pass" id="3178618390">
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
          <item dataType="Struct" type="Duality.Components.SoundListener" id="1816758408">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1163277037</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
          <item dataType="Struct" type="DualStickSpaceShooter.CameraController" id="1908534968">
            <active dataType="Bool">true</active>
            <followObjects dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Transform]]" id="4106506788">
              <_items dataType="Array" type="Duality.Components.Transform[]" id="3677707972" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2466297876">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="Struct" type="Duality.GameObject" id="105982944">
                    <active dataType="Bool">true</active>
                    <children />
                    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3445067032">
                      <_items dataType="Array" type="Duality.Component[]" id="2860228140">
                        <item dataType="ObjectRef">2466297876</item>
                        <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1748149512">
                          <active dataType="Bool">true</active>
                          <gameobj dataType="ObjectRef">105982944</gameobj>
                          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        </item>
                        <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3168759468">
                          <active dataType="Bool">true</active>
                          <gameobj dataType="ObjectRef">105982944</gameobj>
                          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        </item>
                        <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="1328494310">
                          <active dataType="Bool">true</active>
                          <gameobj dataType="ObjectRef">105982944</gameobj>
                          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        </item>
                      </_items>
                      <_size dataType="Int">4</_size>
                      <_version dataType="Int">4</_version>
                    </compList>
                    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2316769566" surrogate="true">
                      <header />
                      <body>
                        <keys dataType="Array" type="System.Type[]" id="2811143898">
                          <item dataType="Type" id="3622757632" value="Duality.Components.Transform" />
                          <item dataType="Type" id="723657166" value="Duality.Components.Renderers.SpriteRenderer" />
                          <item dataType="Type" id="2913314716" value="Duality.Components.Physics.RigidBody" />
                          <item dataType="Type" id="1128125138" value="DualStickSpaceShooter.Ship" />
                        </keys>
                        <values dataType="Array" type="Duality.Component[]" id="3116616378">
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
                        <data dataType="Array" type="System.Byte[]" id="3225816538">kmxmnTfjNEev6HiiGEn5Rg==</data>
                      </header>
                      <body />
                    </identifier>
                    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                    <name dataType="String">PlayerShip</name>
                    <parent dataType="Struct" type="Duality.GameObject" id="1924155130">
                      <active dataType="Bool">true</active>
                      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3585445190">
                        <_items dataType="Array" type="Duality.GameObject[]" id="3973140992" length="4">
                          <item dataType="ObjectRef">105982944</item>
                        </_items>
                        <_size dataType="Int">1</_size>
                        <_version dataType="Int">1</_version>
                      </children>
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4081044410">
                        <_items dataType="Array" type="Duality.Component[]" id="2895119028" length="4">
                          <item dataType="Struct" type="DualStickSpaceShooter.Player" id="2837740131">
                            <active dataType="Bool">true</active>
                            <gameobj dataType="ObjectRef">1924155130</gameobj>
                            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                          </item>
                        </_items>
                        <_size dataType="Int">1</_size>
                        <_version dataType="Int">1</_version>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2720145222" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Type[]" id="622273536">
                            <item dataType="Type" id="820797596" value="DualStickSpaceShooter.Player" />
                          </keys>
                          <values dataType="Array" type="Duality.Component[]" id="2490139598">
                            <item dataType="ObjectRef">2837740131</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform />
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="1003830428">bQCmfxY3+kaGNzoAOVkdbQ==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Player</name>
                      <parent />
                      <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="2368315834">
                        <changes />
                        <obj dataType="ObjectRef">1924155130</obj>
                        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                          <contentPath dataType="String">Data\Player.Prefab.res</contentPath>
                        </prefab>
                      </prefabLink>
                    </parent>
                    <prefabLink />
                  </gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Transform" id="1966433478">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="Struct" type="Duality.GameObject" id="3901085842">
                    <active dataType="Bool">true</active>
                    <children />
                    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3408423860">
                      <_items dataType="Array" type="Duality.Component[]" id="3149135780">
                        <item dataType="ObjectRef">1966433478</item>
                        <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1248285114">
                          <active dataType="Bool">true</active>
                          <gameobj dataType="ObjectRef">3901085842</gameobj>
                          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        </item>
                        <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2668895070">
                          <active dataType="Bool">true</active>
                          <gameobj dataType="ObjectRef">3901085842</gameobj>
                          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        </item>
                        <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="828629912">
                          <active dataType="Bool">true</active>
                          <gameobj dataType="ObjectRef">3901085842</gameobj>
                          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        </item>
                      </_items>
                      <_size dataType="Int">4</_size>
                      <_version dataType="Int">4</_version>
                    </compList>
                    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="912799222" surrogate="true">
                      <header />
                      <body>
                        <keys dataType="Array" type="System.Type[]" id="1940246174">
                          <item dataType="ObjectRef">3622757632</item>
                          <item dataType="ObjectRef">723657166</item>
                          <item dataType="ObjectRef">2913314716</item>
                          <item dataType="ObjectRef">1128125138</item>
                        </keys>
                        <values dataType="Array" type="Duality.Component[]" id="2680755594">
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
                        <data dataType="Array" type="System.Byte[]" id="693779822">MJJheeRXBkq4J8aJ26nrpA==</data>
                      </header>
                      <body />
                    </identifier>
                    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                    <name dataType="String">PlayerShip</name>
                    <parent dataType="Struct" type="Duality.GameObject" id="3691642060">
                      <active dataType="Bool">true</active>
                      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3783358412">
                        <_items dataType="Array" type="Duality.GameObject[]" id="2434446500" length="4">
                          <item dataType="ObjectRef">3901085842</item>
                        </_items>
                        <_size dataType="Int">1</_size>
                        <_version dataType="Int">1</_version>
                      </children>
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4239229686">
                        <_items dataType="Array" type="Duality.Component[]" id="3116400710" length="4">
                          <item dataType="Struct" type="DualStickSpaceShooter.Player" id="310259765">
                            <active dataType="Bool">true</active>
                            <gameobj dataType="ObjectRef">3691642060</gameobj>
                            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                          </item>
                        </_items>
                        <_size dataType="Int">1</_size>
                        <_version dataType="Int">1</_version>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1173565400" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Type[]" id="3329857400">
                            <item dataType="ObjectRef">820797596</item>
                          </keys>
                          <values dataType="Array" type="Duality.Component[]" id="701981662">
                            <item dataType="ObjectRef">310259765</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform />
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="4011278884">f7P3Gf6IbE+ZoU0W66W73w==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Player2</name>
                      <parent />
                      <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="2604791826">
                        <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="3419435738">
                          <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="3262078208" length="4">
                            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="542463952">
                                <_items dataType="Array" type="System.Int32[]" id="984640188" />
                                <_size dataType="Int">0</_size>
                                <_version dataType="Int">1</_version>
                              </childIndex>
                              <componentType />
                              <prop dataType="PropertyInfo" id="1543627374" value="P:Duality.GameObject:Name" />
                              <val dataType="String">Player2</val>
                            </item>
                            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1799862188">
                                <_items dataType="Array" type="System.Int32[]" id="1778466168" length="4" />
                                <_size dataType="Int">1</_size>
                                <_version dataType="Int">2</_version>
                              </childIndex>
                              <componentType dataType="ObjectRef">3622757632</componentType>
                              <prop dataType="PropertyInfo" id="3616504338" value="P:Duality.Components.Transform:RelativePos" />
                              <val dataType="Struct" type="OpenTK.Vector3">
                                <X dataType="Float">-97</X>
                                <Y dataType="Float">66</Y>
                                <Z dataType="Float">0</Z>
                              </val>
                            </item>
                            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3331740040">
                                <_items dataType="ObjectRef">984640188</_items>
                                <_size dataType="Int">0</_size>
                                <_version dataType="Int">1</_version>
                              </childIndex>
                              <componentType dataType="ObjectRef">820797596</componentType>
                              <prop dataType="PropertyInfo" id="3630076774" value="P:DualStickSpaceShooter.Player:Color" />
                              <val dataType="Struct" type="Duality.Drawing.ColorRgba">
                                <A dataType="Byte">255</A>
                                <B dataType="Byte">89</B>
                                <G dataType="Byte">176</G>
                                <R dataType="Byte">255</R>
                              </val>
                            </item>
                          </_items>
                          <_size dataType="Int">3</_size>
                          <_version dataType="Int">115</_version>
                        </changes>
                        <obj dataType="ObjectRef">3691642060</obj>
                        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                          <contentPath dataType="String">Data\Player.Prefab.res</contentPath>
                        </prefab>
                      </prefabLink>
                    </parent>
                    <prefabLink />
                  </gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">5</_version>
            </followObjects>
            <gameobj dataType="ObjectRef">1163277037</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <maxZoomOutDist dataType="Float">350</maxZoomOutDist>
            <softness dataType="Float">1</softness>
            <zoomOutScale dataType="Float">1</zoomOutScale>
          </item>
        </_items>
        <_size dataType="Int">4</_size>
        <_version dataType="Int">4</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="944234272" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="217129813">
            <item dataType="ObjectRef">3622757632</item>
            <item dataType="Type" id="4189950198" value="Duality.Components.Camera" />
            <item dataType="Type" id="845504026" value="Duality.Components.SoundListener" />
            <item dataType="Type" id="4227104278" value="DualStickSpaceShooter.CameraController" />
          </keys>
          <values dataType="Array" type="Duality.Component[]" id="3549131080">
            <item dataType="ObjectRef">3523591969</item>
            <item dataType="ObjectRef">1700552844</item>
            <item dataType="ObjectRef">1816758408</item>
            <item dataType="ObjectRef">1908534968</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">3523591969</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="631356767">+now62bvREivU0EqlajnUw==</data>
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
          <item dataType="Struct" type="Duality.GameObject" id="4184273295">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4121544115">
              <_items dataType="Array" type="Duality.Component[]" id="28471846" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2249620931">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">4184273295</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <parentTransform />
                  <pos dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">135.3767</X>
                    <Y dataType="Float">23.1199951</Y>
                    <Z dataType="Float">93.17166</Z>
                  </pos>
                  <posAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">135.3767</X>
                    <Y dataType="Float">23.1199951</Y>
                    <Z dataType="Float">93.17166</Z>
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
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1531472567">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">92</B>
                    <G dataType="Byte">92</G>
                    <R dataType="Byte">92</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">4184273295</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <offset dataType="Int">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">256</H>
                    <W dataType="Float">256</W>
                    <X dataType="Float">-128</X>
                    <Y dataType="Float">-128</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:DualityIcon</contentPath>
                  </sharedMat>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3711850680" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="2690414553">
                  <item dataType="ObjectRef">3622757632</item>
                  <item dataType="ObjectRef">723657166</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="3172166144">
                  <item dataType="ObjectRef">2249620931</item>
                  <item dataType="ObjectRef">1531472567</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2249620931</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1134796443">m7ZO4ShtTEe8HXU/mT+oWA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">DualityIcon</name>
            <parent dataType="ObjectRef">1042201066</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="1479744303">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1762484627">
              <_items dataType="Array" type="Duality.Component[]" id="1869849830" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="3840059235">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">1479744303</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <parentTransform />
                  <pos dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">-306.217865</X>
                    <Y dataType="Float">98.37827</Y>
                    <Z dataType="Float">372.979248</Z>
                  </pos>
                  <posAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">-306.217865</X>
                    <Y dataType="Float">98.37827</Y>
                    <Z dataType="Float">372.979248</Z>
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
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3121910871">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">62</B>
                    <G dataType="Byte">62</G>
                    <R dataType="Byte">62</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">1479744303</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <offset dataType="Int">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">256</H>
                    <W dataType="Float">256</W>
                    <X dataType="Float">-128</X>
                    <Y dataType="Float">-128</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:DualityIcon</contentPath>
                  </sharedMat>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1262650104" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="2855003129">
                  <item dataType="ObjectRef">3622757632</item>
                  <item dataType="ObjectRef">723657166</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="3325790336">
                  <item dataType="ObjectRef">3840059235</item>
                  <item dataType="ObjectRef">3121910871</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3840059235</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3299660283">d1s4wgu2wkCmbyyfG8Jzmg==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">DualityIcon</name>
            <parent dataType="ObjectRef">1042201066</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="151522024">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2081187872">
              <_items dataType="Array" type="Duality.Component[]" id="3381701596" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2511836956">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">151522024</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <parentTransform />
                  <pos dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">45.5323677</X>
                    <Y dataType="Float">-123.841858</Y>
                    <Z dataType="Float">1060.76587</Z>
                  </pos>
                  <posAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">45.5323677</X>
                    <Y dataType="Float">-123.841858</Y>
                    <Z dataType="Float">1060.76587</Z>
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
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1793688592">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">31</B>
                    <G dataType="Byte">31</G>
                    <R dataType="Byte">31</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">151522024</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <offset dataType="Int">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">256</H>
                    <W dataType="Float">256</W>
                    <X dataType="Float">-128</X>
                    <Y dataType="Float">-128</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:DualityIcon</contentPath>
                  </sharedMat>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3119846286" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="3798125810">
                  <item dataType="ObjectRef">3622757632</item>
                  <item dataType="ObjectRef">723657166</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="3815485770">
                  <item dataType="ObjectRef">2511836956</item>
                  <item dataType="ObjectRef">1793688592</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2511836956</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="682319810">C0z2J/6XFUaPx7kfmfjm9g==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">DualityIcon</name>
            <parent dataType="ObjectRef">1042201066</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="4216766194">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="843402082">
              <_items dataType="Array" type="Duality.Component[]" id="1439669136" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2282113830">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">4216766194</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <parentTransform />
                  <pos dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">-35.0974922</X>
                    <Y dataType="Float">755.7848</Y>
                    <Z dataType="Float">2345.04419</Z>
                  </pos>
                  <posAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">-35.0974922</X>
                    <Y dataType="Float">755.7848</Y>
                    <Z dataType="Float">2345.04419</Z>
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
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1563965466">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">14</B>
                    <G dataType="Byte">14</G>
                    <R dataType="Byte">14</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">4216766194</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <offset dataType="Int">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">256</H>
                    <W dataType="Float">256</W>
                    <X dataType="Float">-128</X>
                    <Y dataType="Float">-128</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:DualityIcon</contentPath>
                  </sharedMat>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2994609546" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="862788088">
                  <item dataType="ObjectRef">3622757632</item>
                  <item dataType="ObjectRef">723657166</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="1675345374">
                  <item dataType="ObjectRef">2282113830</item>
                  <item dataType="ObjectRef">1563965466</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2282113830</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3658441380">CfPUY4LAhU2hoVhqKRpMfg==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">DualityIcon</name>
            <parent dataType="ObjectRef">1042201066</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">4</_size>
        <_version dataType="Int">4</_version>
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                <item dataType="Struct" type="Duality.Components.Diagnostics.RigidBodyRenderer" id="525214455">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:Checkerboard</contentPath>
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                  <item dataType="ObjectRef">3622757632</item>
                  <item dataType="ObjectRef">2913314716</item>
                  <item dataType="Type" id="4016316454" value="Duality.Components.Diagnostics.RigidBodyRenderer" />
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="4176143032">
                  <item dataType="ObjectRef">2728304685</item>
                  <item dataType="ObjectRef">3430766277</item>
                  <item dataType="ObjectRef">525214455</item>
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                <item dataType="Struct" type="Duality.Components.Diagnostics.RigidBodyRenderer" id="3994404744">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:Checkerboard</contentPath>
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                  <item dataType="ObjectRef">3622757632</item>
                  <item dataType="ObjectRef">2913314716</item>
                  <item dataType="ObjectRef">4016316454</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="4214801142">
                  <item dataType="ObjectRef">1902527678</item>
                  <item dataType="ObjectRef">2604989270</item>
                  <item dataType="ObjectRef">3994404744</item>
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                <item dataType="Struct" type="Duality.Components.Diagnostics.RigidBodyRenderer" id="3666215217">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:Checkerboard</contentPath>
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                  <item dataType="ObjectRef">3622757632</item>
                  <item dataType="ObjectRef">2913314716</item>
                  <item dataType="ObjectRef">4016316454</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="2111389952">
                  <item dataType="ObjectRef">1574338151</item>
                  <item dataType="ObjectRef">2276799743</item>
                  <item dataType="ObjectRef">3666215217</item>
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <parentTransform />
                  <pos dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">267.002</X>
                    <Y dataType="Float">-87</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">267.002</X>
                    <Y dataType="Float">-87</Y>
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                <item dataType="Struct" type="Duality.Components.Diagnostics.RigidBodyRenderer" id="2531019524">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:Checkerboard</contentPath>
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                  <item dataType="ObjectRef">3622757632</item>
                  <item dataType="ObjectRef">2913314716</item>
                  <item dataType="ObjectRef">4016316454</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="416939982">
                  <item dataType="ObjectRef">439142458</item>
                  <item dataType="ObjectRef">1141604050</item>
                  <item dataType="ObjectRef">2531019524</item>
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                <item dataType="Struct" type="Duality.Components.Diagnostics.RigidBodyRenderer" id="354111066">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:Checkerboard</contentPath>
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                  <item dataType="ObjectRef">3622757632</item>
                  <item dataType="ObjectRef">2913314716</item>
                  <item dataType="ObjectRef">4016316454</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="1322274330">
                  <item dataType="ObjectRef">2557201296</item>
                  <item dataType="ObjectRef">3259662888</item>
                  <item dataType="ObjectRef">354111066</item>
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                <item dataType="Struct" type="Duality.Components.Diagnostics.RigidBodyRenderer" id="4281582093">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:Checkerboard</contentPath>
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                  <item dataType="ObjectRef">3622757632</item>
                  <item dataType="ObjectRef">2913314716</item>
                  <item dataType="ObjectRef">4016316454</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="2282684616">
                  <item dataType="ObjectRef">2189705027</item>
                  <item dataType="ObjectRef">2892166619</item>
                  <item dataType="ObjectRef">4281582093</item>
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
    <item dataType="ObjectRef">4184273295</item>
    <item dataType="ObjectRef">1479744303</item>
    <item dataType="ObjectRef">151522024</item>
    <item dataType="ObjectRef">4216766194</item>
    <item dataType="ObjectRef">105982944</item>
    <item dataType="ObjectRef">367989753</item>
    <item dataType="ObjectRef">3837180042</item>
    <item dataType="ObjectRef">3508990515</item>
    <item dataType="ObjectRef">2373794822</item>
    <item dataType="ObjectRef">196886364</item>
    <item dataType="ObjectRef">4124357391</item>
    <item dataType="ObjectRef">3901085842</item>
  </serializeObj>
  <sourcePath />
</root>
<!-- XmlFormatterBase Document Separator -->
