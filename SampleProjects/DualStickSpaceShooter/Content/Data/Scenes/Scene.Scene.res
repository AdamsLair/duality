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
                        <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="3562856628">
                          <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="1485000100" length="4" />
                          <_size dataType="Int">0</_size>
                          <_version dataType="Int">2</_version>
                        </changes>
                        <obj dataType="ObjectRef">1924155130</obj>
                        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                          <contentPath dataType="String">Data\Prefabs\Player.Prefab.res</contentPath>
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
                          <contentPath dataType="String">Data\Prefabs\Player.Prefab.res</contentPath>
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
            <zoomFactor dataType="Float">1</zoomFactor>
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
              </_items>
              <_size dataType="Int">3</_size>
              <_version dataType="Int">3</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3338222248" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="1639240593">
                  <item dataType="ObjectRef">3622757632</item>
                  <item dataType="ObjectRef">723657166</item>
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
              </_items>
              <_size dataType="Int">3</_size>
              <_version dataType="Int">3</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2650890006" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="189677174">
                  <item dataType="ObjectRef">3622757632</item>
                  <item dataType="ObjectRef">723657166</item>
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
              </_items>
              <_size dataType="Int">3</_size>
              <_version dataType="Int">3</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2089323194" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="2219571348">
                  <item dataType="ObjectRef">3622757632</item>
                  <item dataType="ObjectRef">723657166</item>
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
              </_items>
              <_size dataType="Int">3</_size>
              <_version dataType="Int">3</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2301570378" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="2947722380">
                  <item dataType="ObjectRef">3622757632</item>
                  <item dataType="ObjectRef">723657166</item>
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
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
        <_items dataType="Array" type="Duality.GameObject[]" id="680943488" length="8">
          <item dataType="Struct" type="Duality.GameObject" id="3034479095">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2479421587">
              <_items dataType="Array" type="Duality.GameObject[]" id="461756646" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="1043620744">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1382186340">
                    <_items dataType="Array" type="Duality.Component[]" id="1073961924" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="3403935676">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1043620744</gameobj>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="751056125">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1043620744</gameobj>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="576975382" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1955749934">
                        <item dataType="ObjectRef">3622757632</item>
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
              </_items>
              <_size dataType="Int">1</_size>
              <_version dataType="Int">1</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="108267256">
              <_items dataType="Array" type="Duality.Component[]" id="1523111161" length="8">
                <item dataType="Struct" type="Duality.Components.Transform" id="1099826731">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">3034479095</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="381678367">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">3034479095</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1802288323">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">3034479095</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="4256990461">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">3034479095</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="4110587027">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">3034479095</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2740747129" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="4194494292">
                  <item dataType="ObjectRef">3622757632</item>
                  <item dataType="ObjectRef">723657166</item>
                  <item dataType="ObjectRef">2913314716</item>
                  <item dataType="ObjectRef">1128125138</item>
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
                    <componentType dataType="ObjectRef">3622757632</componentType>
                    <prop dataType="ObjectRef">3616504338</prop>
                    <val dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">158</X>
                      <Y dataType="Float">-37.5</Y>
                      <Z dataType="Float">0</Z>
                    </val>
                  </item>
                </_items>
                <_size dataType="Int">1</_size>
                <_version dataType="Int">1</_version>
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
              <_items dataType="Array" type="Duality.GameObject[]" id="856351300" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="3013370658">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1422490386">
                    <_items dataType="Array" type="Duality.Component[]" id="3012301904" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1078718294">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3013370658</gameobj>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="2720806039">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3013370658</gameobj>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3696157130" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="3843670856">
                        <item dataType="ObjectRef">3622757632</item>
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
              </_items>
              <_size dataType="Int">1</_size>
              <_version dataType="Int">1</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3332006550">
              <_items dataType="Array" type="Duality.Component[]" id="1139719062" length="8">
                <item dataType="Struct" type="Duality.Components.Transform" id="2503745672">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">143430740</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1785597308">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">143430740</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3206207264">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">143430740</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="1365942106">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">143430740</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="1219538672">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">143430740</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4017882728" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="2369946328">
                  <item dataType="ObjectRef">3622757632</item>
                  <item dataType="ObjectRef">723657166</item>
                  <item dataType="ObjectRef">2913314716</item>
                  <item dataType="ObjectRef">1128125138</item>
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
                    <componentType dataType="ObjectRef">3622757632</componentType>
                    <prop dataType="ObjectRef">3616504338</prop>
                    <val dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">129</X>
                      <Y dataType="Float">19.5</Y>
                      <Z dataType="Float">0</Z>
                    </val>
                  </item>
                </_items>
                <_size dataType="Int">1</_size>
                <_version dataType="Int">1</_version>
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
              <_items dataType="Array" type="Duality.GameObject[]" id="3040519574" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="1314617617">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2897128113">
                    <_items dataType="Array" type="Duality.Component[]" id="1342049838" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="3674932549">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1314617617</gameobj>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="1022052998">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1314617617</gameobj>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="321232992" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="3454699419">
                        <item dataType="ObjectRef">3622757632</item>
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
              </_items>
              <_size dataType="Int">1</_size>
              <_version dataType="Int">1</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1146975336">
              <_items dataType="Array" type="Duality.Component[]" id="1954623311" length="8">
                <item dataType="Struct" type="Duality.Components.Transform" id="284732085">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2219384449</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3861551017">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2219384449</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="987193677">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2219384449</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="3441895815">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2219384449</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="3295492381">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2219384449</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="240635247" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="137252068">
                  <item dataType="ObjectRef">3622757632</item>
                  <item dataType="ObjectRef">723657166</item>
                  <item dataType="ObjectRef">2913314716</item>
                  <item dataType="ObjectRef">1128125138</item>
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
                    <componentType dataType="ObjectRef">3622757632</componentType>
                    <prop dataType="ObjectRef">3616504338</prop>
                    <val dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">200</X>
                      <Y dataType="Float">-1.5</Y>
                      <Z dataType="Float">0</Z>
                    </val>
                  </item>
                </_items>
                <_size dataType="Int">1</_size>
                <_version dataType="Int">1</_version>
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
              <_items dataType="Array" type="Duality.GameObject[]" id="3731237710" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="38335236">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2863037520">
                    <_items dataType="Array" type="Duality.Component[]" id="2000540092" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2398650168">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">38335236</gameobj>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="4040737913">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">38335236</gameobj>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1594392942" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1972855074">
                        <item dataType="ObjectRef">3622757632</item>
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
              </_items>
              <_size dataType="Int">1</_size>
              <_version dataType="Int">1</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1398775168">
              <_items dataType="Array" type="Duality.Component[]" id="963994835" length="8">
                <item dataType="Struct" type="Duality.Components.Transform" id="711112001">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2645764365</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="4287930933">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2645764365</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1413573593">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2645764365</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="3868275731">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2645764365</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="3721872297">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2645764365</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="533428859" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="2187858964">
                  <item dataType="ObjectRef">3622757632</item>
                  <item dataType="ObjectRef">723657166</item>
                  <item dataType="ObjectRef">2913314716</item>
                  <item dataType="ObjectRef">1128125138</item>
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
                      <_items dataType="ObjectRef">1034047396</_items>
                      <_size dataType="Int">0</_size>
                      <_version dataType="Int">1</_version>
                    </childIndex>
                    <componentType dataType="ObjectRef">3622757632</componentType>
                    <prop dataType="ObjectRef">3616504338</prop>
                    <val dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">238</X>
                      <Y dataType="Float">43.5</Y>
                      <Z dataType="Float">0</Z>
                    </val>
                  </item>
                </_items>
                <_size dataType="Int">1</_size>
                <_version dataType="Int">1</_version>
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
              <_items dataType="Array" type="Duality.GameObject[]" id="244139118" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="3258964135">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="192693719">
                    <_items dataType="Array" type="Duality.Component[]" id="1092747790" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1324311771">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3258964135</gameobj>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="2966399516">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3258964135</gameobj>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2568068032" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="2942111837">
                        <item dataType="ObjectRef">3622757632</item>
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
              </_items>
              <_size dataType="Int">1</_size>
              <_version dataType="Int">1</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3372665632">
              <_items dataType="Array" type="Duality.Component[]" id="2346989483" length="8">
                <item dataType="Struct" type="Duality.Components.Transform" id="334575177">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2269227541</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3911394109">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2269227541</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1037036769">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2269227541</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="3491738907">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2269227541</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="3345335473">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2269227541</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2727365939" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="2404668324">
                  <item dataType="ObjectRef">3622757632</item>
                  <item dataType="ObjectRef">723657166</item>
                  <item dataType="ObjectRef">2913314716</item>
                  <item dataType="ObjectRef">1128125138</item>
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
                    <componentType dataType="ObjectRef">3622757632</componentType>
                    <prop dataType="ObjectRef">3616504338</prop>
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
              <_items dataType="Array" type="Duality.GameObject[]" id="868446496" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="2728913501">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="744782185">
                    <_items dataType="Array" type="Duality.Component[]" id="2035471630" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="794261137">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2728913501</gameobj>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="2436348882">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">2728913501</gameobj>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="939383488" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="855164387">
                        <item dataType="ObjectRef">3622757632</item>
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
              </_items>
              <_size dataType="Int">1</_size>
              <_version dataType="Int">1</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3613037530">
              <_items dataType="Array" type="Duality.Component[]" id="3971813904" length="8">
                <item dataType="Struct" type="Duality.Components.Transform" id="2603247046">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">242932114</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1885098682">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">242932114</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3305708638">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">242932114</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="1465443480">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">242932114</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="1319040046">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">242932114</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3871162890" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="3313197376">
                  <item dataType="ObjectRef">3622757632</item>
                  <item dataType="ObjectRef">723657166</item>
                  <item dataType="ObjectRef">2913314716</item>
                  <item dataType="ObjectRef">1128125138</item>
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
                      <_items dataType="ObjectRef">3245693380</_items>
                      <_size dataType="Int">0</_size>
                      <_version dataType="Int">1</_version>
                    </childIndex>
                    <componentType dataType="ObjectRef">3622757632</componentType>
                    <prop dataType="ObjectRef">3616504338</prop>
                    <val dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">192.002014</X>
                      <Y dataType="Float">-89.5</Y>
                      <Z dataType="Float">0</Z>
                    </val>
                  </item>
                </_items>
                <_size dataType="Int">1</_size>
                <_version dataType="Int">475</_version>
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
              <_items dataType="Array" type="Duality.GameObject[]" id="186619382" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="3911303751">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3112804071">
                    <_items dataType="Array" type="Duality.Component[]" id="3596919118" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1976651387">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3911303751</gameobj>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="3618739132">
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">3911303751</gameobj>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4243918720" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="338365389">
                        <item dataType="ObjectRef">3622757632</item>
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
              </_items>
              <_size dataType="Int">1</_size>
              <_version dataType="Int">1</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1751535176">
              <_items dataType="Array" type="Duality.Component[]" id="21595137" length="8">
                <item dataType="Struct" type="Duality.Components.Transform" id="454410467">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2389062831</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="4031229399">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2389062831</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1156872059">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2389062831</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="3611574197">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2389062831</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
                <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="3465170763">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">2389062831</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">5</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4086586657" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="2131256580">
                  <item dataType="ObjectRef">3622757632</item>
                  <item dataType="ObjectRef">723657166</item>
                  <item dataType="ObjectRef">2913314716</item>
                  <item dataType="ObjectRef">1128125138</item>
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
                      <_items dataType="ObjectRef">3245693380</_items>
                      <_size dataType="Int">0</_size>
                      <_version dataType="Int">1</_version>
                    </childIndex>
                    <componentType dataType="ObjectRef">3622757632</componentType>
                    <prop dataType="ObjectRef">3616504338</prop>
                    <val dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">228.003021</X>
                      <Y dataType="Float">-136.5</Y>
                      <Z dataType="Float">0</Z>
                    </val>
                  </item>
                </_items>
                <_size dataType="Int">1</_size>
                <_version dataType="Int">531</_version>
              </changes>
              <obj dataType="ObjectRef">2389062831</obj>
              <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                <contentPath dataType="String">Data\Prefabs\EnemyClaymore.Prefab.res</contentPath>
              </prefab>
            </prefabLink>
          </item>
        </_items>
        <_size dataType="Int">7</_size>
        <_version dataType="Int">15</_version>
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
    <item dataType="ObjectRef">1043620744</item>
    <item dataType="ObjectRef">3013370658</item>
    <item dataType="ObjectRef">1314617617</item>
    <item dataType="ObjectRef">38335236</item>
    <item dataType="ObjectRef">3258964135</item>
    <item dataType="ObjectRef">2728913501</item>
    <item dataType="ObjectRef">3911303751</item>
  </serializeObj>
  <sourcePath />
</root>
<!-- XmlFormatterBase Document Separator -->
