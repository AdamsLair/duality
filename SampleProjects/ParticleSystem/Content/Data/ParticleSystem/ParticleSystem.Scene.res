<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">33</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="2261523538">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3683899068">
        <_items dataType="Array" type="Duality.GameObject[]" id="99951172" length="4" />
        <_size dataType="Int">0</_size>
        <_version dataType="Int">2</_version>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2857280150">
        <_items dataType="Array" type="Duality.Component[]" id="3962699670" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="326871174">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">2261523538</gameobj>
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
          <item dataType="Struct" type="Duality.Components.Camera" id="2798799345">
            <active dataType="Bool">true</active>
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">2261523538</gameobj>
            <nearZ dataType="Float">0</nearZ>
            <passes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Camera+Pass]]" id="1586507857">
              <_items dataType="Array" type="Duality.Components.Camera+Pass[]" id="1602373614" length="4">
                <item dataType="Struct" type="Duality.Components.Camera+Pass" id="2346966608">
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
                <item dataType="Struct" type="Duality.Components.Camera+Pass" id="3661249902">
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
        </_items>
        <_size dataType="Int">2</_size>
        <_version dataType="Int">2</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="271252072" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2317615832">
            <item dataType="Type" id="3664359340" value="Duality.Components.Transform" />
            <item dataType="Type" id="3808766902" value="Duality.Components.Camera" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="4274806942">
            <item dataType="ObjectRef">326871174</item>
            <item dataType="ObjectRef">2798799345</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">326871174</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="4188701316">GVpfGpMvH0a0lrNNTy8FVA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Camera</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="3486933666">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="608786540">
        <_items dataType="Array" type="Duality.Component[]" id="4173209444" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="1552281302">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">3486933666</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <parentTransform />
            <pos dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">500</Z>
            </pos>
            <posAbs dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">500</Z>
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
          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="834132938">
            <active dataType="Bool">true</active>
            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">128</B>
              <G dataType="Byte">128</G>
              <R dataType="Byte">128</R>
            </colorTint>
            <customMat />
            <gameobj dataType="ObjectRef">3486933666</gameobj>
            <offset dataType="Int">100</offset>
            <pixelGrid dataType="Bool">false</pixelGrid>
            <rect dataType="Struct" type="Duality.Rect">
              <H dataType="Float">4096</H>
              <W dataType="Float">4096</W>
              <X dataType="Float">-2048</X>
              <Y dataType="Float">-2048</Y>
            </rect>
            <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="WrapBoth" value="3" />
            <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
              <contentPath dataType="String">Data\ParticleSystem\Visuals\BackgroundTile.Material.res</contentPath>
            </sharedMat>
            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          </item>
        </_items>
        <_size dataType="Int">2</_size>
        <_version dataType="Int">2</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4158192694" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3519472806">
            <item dataType="ObjectRef">3664359340</item>
            <item dataType="Type" id="1068503552" value="Duality.Components.Renderers.SpriteRenderer" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="1272548282">
            <item dataType="ObjectRef">1552281302</item>
            <item dataType="ObjectRef">834132938</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">1552281302</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="375649958">sS2xMmd5sU+NpIT6E00Qog==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Background</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="2883986298">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="83949668">
        <_items dataType="Array" type="Duality.GameObject[]" id="1818241476" length="4">
          <item dataType="Struct" type="Duality.GameObject" id="4113979440">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2006725688">
              <_items dataType="Array" type="Duality.Component[]" id="818727532" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2179327076">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">4113979440</gameobj>
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
                <item dataType="Struct" type="ParticleSystem.ParticleEffect" id="4083609470">
                  <active dataType="Bool">true</active>
                  <angularDrag dataType="Float">0.3</angularDrag>
                  <constantForce dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </constantForce>
                  <emitters dataType="Struct" type="System.Collections.Generic.List`1[[ParticleSystem.ParticleEmitter]]" id="4142054598">
                    <_items dataType="Array" type="ParticleSystem.ParticleEmitter[]" id="1278867712" length="4">
                      <item dataType="Struct" type="ParticleSystem.ParticleEmitter" id="253448860">
                        <basePos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">0</Y>
                          <Z dataType="Float">0</Z>
                        </basePos>
                        <baseVel dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">0</Y>
                          <Z dataType="Float">0</Z>
                        </baseVel>
                        <burstDelay dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">100</MaxValue>
                          <MinValue dataType="Float">100</MinValue>
                        </burstDelay>
                        <burstParticleNum dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">1</MaxValue>
                          <MinValue dataType="Float">1</MinValue>
                        </burstParticleNum>
                        <depthMult dataType="Float">1</depthMult>
                        <maxBurstCount dataType="Int">-1</maxBurstCount>
                        <maxColor dataType="Struct" type="Duality.Drawing.ColorHsva">
                          <A dataType="Float">1</A>
                          <H dataType="Float">0</H>
                          <S dataType="Float">0</S>
                          <V dataType="Float">1</V>
                        </maxColor>
                        <minColor dataType="Struct" type="Duality.Drawing.ColorHsva">
                          <A dataType="Float">1</A>
                          <H dataType="Float">0</H>
                          <S dataType="Float">0</S>
                          <V dataType="Float">1</V>
                        </minColor>
                        <particleLifetime dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">1000</MaxValue>
                          <MinValue dataType="Float">1000</MinValue>
                        </particleLifetime>
                        <randomAngle dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">6.28318548</MaxValue>
                          <MinValue dataType="Float">0</MinValue>
                        </randomAngle>
                        <randomAngleVel dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">0.05</MaxValue>
                          <MinValue dataType="Float">-0.05</MinValue>
                        </randomAngleVel>
                        <randomPos dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">0</MaxValue>
                          <MinValue dataType="Float">0</MinValue>
                        </randomPos>
                        <randomVel dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">3</MaxValue>
                          <MinValue dataType="Float">0</MinValue>
                        </randomVel>
                        <spriteIndex dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">0</MaxValue>
                          <MinValue dataType="Float">0</MinValue>
                        </spriteIndex>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                    <_version dataType="Int">2</_version>
                  </emitters>
                  <fadeInAt dataType="Float">0</fadeInAt>
                  <fadeOutAt dataType="Float">0.75</fadeOutAt>
                  <gameobj dataType="ObjectRef">4113979440</gameobj>
                  <linearDrag dataType="Float">0.3</linearDrag>
                  <material dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\ParticleSystem\Visuals\Shards.Material.res</contentPath>
                  </material>
                  <particleSize dataType="Struct" type="Duality.Vector2">
                    <X dataType="Float">16</X>
                    <Y dataType="Float">16</Y>
                  </particleSize>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2686997214" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="768924282">
                  <item dataType="ObjectRef">3664359340</item>
                  <item dataType="Type" id="3134076800" value="ParticleSystem.ParticleEffect" />
                </keys>
                <values dataType="Array" type="System.Object[]" id="874706746">
                  <item dataType="ObjectRef">2179327076</item>
                  <item dataType="ObjectRef">4083609470</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2179327076</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3673096954">qBftO9uHtE+tSkmeaxogLA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">DefaultEffect</name>
            <parent dataType="ObjectRef">2883986298</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="829250152">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="898559520">
              <_items dataType="Array" type="Duality.GameObject[]" id="3757050844" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="4042308553">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3763988501">
                    <_items dataType="Array" type="Duality.Component[]" id="3884295798" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2107656189">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">4042308553</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="3189565084">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">829250152</gameobj>
                          <ignoreParent dataType="Bool">false</ignoreParent>
                          <parentTransform />
                          <pos dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">256</X>
                            <Y dataType="Float">0</Y>
                            <Z dataType="Float">0</Z>
                          </pos>
                          <posAbs dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">256</X>
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
                          <X dataType="Float">0</X>
                          <Y dataType="Float">0</Y>
                          <Z dataType="Float">0</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">256</X>
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
                      <item dataType="Struct" type="ParticleSystem.ParticleEffect" id="4011938583">
                        <active dataType="Bool">true</active>
                        <angularDrag dataType="Float">0.15</angularDrag>
                        <constantForce dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">1</X>
                          <Y dataType="Float">8</Y>
                          <Z dataType="Float">0</Z>
                        </constantForce>
                        <emitters dataType="Struct" type="System.Collections.Generic.List`1[[ParticleSystem.ParticleEmitter]]" id="1267613623">
                          <_items dataType="Array" type="ParticleSystem.ParticleEmitter[]" id="2624508302" length="4">
                            <item dataType="Struct" type="ParticleSystem.ParticleEmitter" id="3657408720">
                              <basePos dataType="Struct" type="Duality.Vector3">
                                <X dataType="Float">0</X>
                                <Y dataType="Float">0</Y>
                                <Z dataType="Float">0</Z>
                              </basePos>
                              <baseVel dataType="Struct" type="Duality.Vector3">
                                <X dataType="Float">0</X>
                                <Y dataType="Float">-3</Y>
                                <Z dataType="Float">0</Z>
                              </baseVel>
                              <burstDelay dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">2500</MaxValue>
                                <MinValue dataType="Float">250</MinValue>
                              </burstDelay>
                              <burstParticleNum dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">3</MaxValue>
                                <MinValue dataType="Float">1</MinValue>
                              </burstParticleNum>
                              <depthMult dataType="Float">1</depthMult>
                              <maxBurstCount dataType="Int">-1</maxBurstCount>
                              <maxColor dataType="Struct" type="Duality.Drawing.ColorHsva">
                                <A dataType="Float">1</A>
                                <H dataType="Float">0.118279561</H>
                                <S dataType="Float">0.243137255</S>
                                <V dataType="Float">1</V>
                              </maxColor>
                              <minColor dataType="Struct" type="Duality.Drawing.ColorHsva">
                                <A dataType="Float">1</A>
                                <H dataType="Float">0.04379085</H>
                                <S dataType="Float">1</S>
                                <V dataType="Float">1</V>
                              </minColor>
                              <particleLifetime dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">1000</MaxValue>
                                <MinValue dataType="Float">500</MinValue>
                              </particleLifetime>
                              <randomAngle dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">6.28318548</MaxValue>
                                <MinValue dataType="Float">0</MinValue>
                              </randomAngle>
                              <randomAngleVel dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">0.05</MaxValue>
                                <MinValue dataType="Float">-0.05</MinValue>
                              </randomAngleVel>
                              <randomPos dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">35</MaxValue>
                                <MinValue dataType="Float">0</MinValue>
                              </randomPos>
                              <randomVel dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">8</MaxValue>
                                <MinValue dataType="Float">4</MinValue>
                              </randomVel>
                              <spriteIndex dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">16</MaxValue>
                                <MinValue dataType="Float">4</MinValue>
                              </spriteIndex>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                          <_version dataType="Int">2</_version>
                        </emitters>
                        <fadeInAt dataType="Float">0.15</fadeInAt>
                        <fadeOutAt dataType="Float">0.9</fadeOutAt>
                        <gameobj dataType="ObjectRef">4042308553</gameobj>
                        <linearDrag dataType="Float">0.3</linearDrag>
                        <material dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                          <contentPath dataType="String">Data\ParticleSystem\Visuals\GlowShards.Material.res</contentPath>
                        </material>
                        <particleSize dataType="Struct" type="Duality.Vector2">
                          <X dataType="Float">12</X>
                          <Y dataType="Float">12</Y>
                        </particleSize>
                        <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2343587016" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="4155479999">
                        <item dataType="ObjectRef">3664359340</item>
                        <item dataType="ObjectRef">3134076800</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="2178321888">
                        <item dataType="ObjectRef">2107656189</item>
                        <item dataType="ObjectRef">4011938583</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2107656189</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="133088621">/PcAQPDTKEygk2PjljdCHQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">BurstSparks</name>
                  <parent dataType="ObjectRef">829250152</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="16560041">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3893390645">
                    <_items dataType="Array" type="Duality.Component[]" id="3464645622" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2376874973">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">16560041</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="ObjectRef">3189565084</parentTransform>
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">0</Y>
                          <Z dataType="Float">0</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">256</X>
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
                      <item dataType="Struct" type="ParticleSystem.ParticleEffect" id="4281157367">
                        <active dataType="Bool">true</active>
                        <angularDrag dataType="Float">0.15</angularDrag>
                        <constantForce dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">1</X>
                          <Y dataType="Float">-8</Y>
                          <Z dataType="Float">0</Z>
                        </constantForce>
                        <emitters dataType="Struct" type="System.Collections.Generic.List`1[[ParticleSystem.ParticleEmitter]]" id="2166227543">
                          <_items dataType="Array" type="ParticleSystem.ParticleEmitter[]" id="1487513102" length="4">
                            <item dataType="Struct" type="ParticleSystem.ParticleEmitter" id="2511740880">
                              <basePos dataType="Struct" type="Duality.Vector3">
                                <X dataType="Float">0</X>
                                <Y dataType="Float">0</Y>
                                <Z dataType="Float">0</Z>
                              </basePos>
                              <baseVel dataType="Struct" type="Duality.Vector3">
                                <X dataType="Float">0</X>
                                <Y dataType="Float">0</Y>
                                <Z dataType="Float">0</Z>
                              </baseVel>
                              <burstDelay dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">100</MaxValue>
                                <MinValue dataType="Float">100</MinValue>
                              </burstDelay>
                              <burstParticleNum dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">1</MaxValue>
                                <MinValue dataType="Float">1</MinValue>
                              </burstParticleNum>
                              <depthMult dataType="Float">1</depthMult>
                              <maxBurstCount dataType="Int">-1</maxBurstCount>
                              <maxColor dataType="Struct" type="Duality.Drawing.ColorHsva">
                                <A dataType="Float">1</A>
                                <H dataType="Float">0.118279561</H>
                                <S dataType="Float">0.243137255</S>
                                <V dataType="Float">1</V>
                              </maxColor>
                              <minColor dataType="Struct" type="Duality.Drawing.ColorHsva">
                                <A dataType="Float">1</A>
                                <H dataType="Float">0.04379085</H>
                                <S dataType="Float">1</S>
                                <V dataType="Float">1</V>
                              </minColor>
                              <particleLifetime dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">2500</MaxValue>
                                <MinValue dataType="Float">1000</MinValue>
                              </particleLifetime>
                              <randomAngle dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">6.28318548</MaxValue>
                                <MinValue dataType="Float">0</MinValue>
                              </randomAngle>
                              <randomAngleVel dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">0.05</MaxValue>
                                <MinValue dataType="Float">-0.05</MinValue>
                              </randomAngleVel>
                              <randomPos dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">35</MaxValue>
                                <MinValue dataType="Float">0</MinValue>
                              </randomPos>
                              <randomVel dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">2</MaxValue>
                                <MinValue dataType="Float">0</MinValue>
                              </randomVel>
                              <spriteIndex dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">16</MaxValue>
                                <MinValue dataType="Float">4</MinValue>
                              </spriteIndex>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                          <_version dataType="Int">2</_version>
                        </emitters>
                        <fadeInAt dataType="Float">0.15</fadeInAt>
                        <fadeOutAt dataType="Float">0.75</fadeOutAt>
                        <gameobj dataType="ObjectRef">16560041</gameobj>
                        <linearDrag dataType="Float">0.3</linearDrag>
                        <material dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                          <contentPath dataType="String">Data\ParticleSystem\Visuals\GlowShards.Material.res</contentPath>
                        </material>
                        <particleSize dataType="Struct" type="Duality.Vector2">
                          <X dataType="Float">12</X>
                          <Y dataType="Float">12</Y>
                        </particleSize>
                        <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3678550088" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="2098457375">
                        <item dataType="ObjectRef">3664359340</item>
                        <item dataType="ObjectRef">3134076800</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="1559779872">
                        <item dataType="ObjectRef">2376874973</item>
                        <item dataType="ObjectRef">4281157367</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2376874973</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="480802189">lOEGaPro5U6FUYAr34dv2A==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">FlyingSparks</name>
                  <parent dataType="ObjectRef">829250152</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="3985077999">
                  <active dataType="Bool">true</active>
                  <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2114756947">
                    <_items dataType="Array" type="Duality.GameObject[]" id="2458443622" length="4" />
                    <_size dataType="Int">0</_size>
                    <_version dataType="Int">4</_version>
                  </children>
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1407530872">
                    <_items dataType="Array" type="Duality.Component[]" id="2940685369" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2050425635">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">3985077999</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="ObjectRef">3189565084</parentTransform>
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">-32</Y>
                          <Z dataType="Float">16</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">256</X>
                          <Y dataType="Float">-32</Y>
                          <Z dataType="Float">16</Z>
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
                      <item dataType="Struct" type="ParticleSystem.ParticleEffect" id="3954708029">
                        <active dataType="Bool">true</active>
                        <angularDrag dataType="Float">0.15</angularDrag>
                        <constantForce dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">1</X>
                          <Y dataType="Float">-5</Y>
                          <Z dataType="Float">0</Z>
                        </constantForce>
                        <emitters dataType="Struct" type="System.Collections.Generic.List`1[[ParticleSystem.ParticleEmitter]]" id="589203847">
                          <_items dataType="Array" type="ParticleSystem.ParticleEmitter[]" id="4185817934" length="4">
                            <item dataType="Struct" type="ParticleSystem.ParticleEmitter" id="683379408">
                              <basePos dataType="Struct" type="Duality.Vector3">
                                <X dataType="Float">0</X>
                                <Y dataType="Float">0</Y>
                                <Z dataType="Float">0</Z>
                              </basePos>
                              <baseVel dataType="Struct" type="Duality.Vector3">
                                <X dataType="Float">0.25</X>
                                <Y dataType="Float">0</Y>
                                <Z dataType="Float">0</Z>
                              </baseVel>
                              <burstDelay dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">50</MaxValue>
                                <MinValue dataType="Float">50</MinValue>
                              </burstDelay>
                              <burstParticleNum dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">1</MaxValue>
                                <MinValue dataType="Float">1</MinValue>
                              </burstParticleNum>
                              <depthMult dataType="Float">1</depthMult>
                              <maxBurstCount dataType="Int">-1</maxBurstCount>
                              <maxColor dataType="Struct" type="Duality.Drawing.ColorHsva">
                                <A dataType="Float">0.5019608</A>
                                <H dataType="Float">0.027777778</H>
                                <S dataType="Float">0.181818187</S>
                                <V dataType="Float">0.258823544</V>
                              </maxColor>
                              <minColor dataType="Struct" type="Duality.Drawing.ColorHsva">
                                <A dataType="Float">0.5019608</A>
                                <H dataType="Float">0</H>
                                <S dataType="Float">0</S>
                                <V dataType="Float">0</V>
                              </minColor>
                              <particleLifetime dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">5000</MaxValue>
                                <MinValue dataType="Float">2000</MinValue>
                              </particleLifetime>
                              <randomAngle dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">6.28318548</MaxValue>
                                <MinValue dataType="Float">0</MinValue>
                              </randomAngle>
                              <randomAngleVel dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">0.05</MaxValue>
                                <MinValue dataType="Float">-0.05</MinValue>
                              </randomAngleVel>
                              <randomPos dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">35</MaxValue>
                                <MinValue dataType="Float">0</MinValue>
                              </randomPos>
                              <randomVel dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">1</MaxValue>
                                <MinValue dataType="Float">0</MinValue>
                              </randomVel>
                              <spriteIndex dataType="Struct" type="Duality.Range">
                                <MaxValue dataType="Float">4</MaxValue>
                                <MinValue dataType="Float">0</MinValue>
                              </spriteIndex>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                          <_version dataType="Int">2</_version>
                        </emitters>
                        <fadeInAt dataType="Float">0.25</fadeInAt>
                        <fadeOutAt dataType="Float">0.5</fadeOutAt>
                        <gameobj dataType="ObjectRef">3985077999</gameobj>
                        <linearDrag dataType="Float">0.3</linearDrag>
                        <material dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                          <contentPath dataType="String">Data\ParticleSystem\Visuals\AlphaShards.Material.res</contentPath>
                        </material>
                        <particleSize dataType="Struct" type="Duality.Vector2">
                          <X dataType="Float">64</X>
                          <Y dataType="Float">64</Y>
                        </particleSize>
                        <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1469916985" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="2082656084">
                        <item dataType="ObjectRef">3664359340</item>
                        <item dataType="ObjectRef">3134076800</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="4225531318">
                        <item dataType="ObjectRef">2050425635</item>
                        <item dataType="ObjectRef">3954708029</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2050425635</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="753962096">ZjxjuuRDN0qg9hTc7mSyvg==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Smoke</name>
                  <parent dataType="ObjectRef">829250152</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">3</_size>
              <_version dataType="Int">3</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="406254478">
              <_items dataType="Array" type="Duality.Component[]" id="1449571058" length="4">
                <item dataType="ObjectRef">3189565084</item>
                <item dataType="Struct" type="ParticleSystem.ParticleEffect" id="798880182">
                  <active dataType="Bool">true</active>
                  <angularDrag dataType="Float">0.15</angularDrag>
                  <constantForce dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">1</X>
                    <Y dataType="Float">-5</Y>
                    <Z dataType="Float">0</Z>
                  </constantForce>
                  <emitters dataType="Struct" type="System.Collections.Generic.List`1[[ParticleSystem.ParticleEmitter]]" id="487753994">
                    <_items dataType="Array" type="ParticleSystem.ParticleEmitter[]" id="723247328" length="4">
                      <item dataType="Struct" type="ParticleSystem.ParticleEmitter" id="983715804">
                        <basePos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">0</Y>
                          <Z dataType="Float">0</Z>
                        </basePos>
                        <baseVel dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">0</Y>
                          <Z dataType="Float">0</Z>
                        </baseVel>
                        <burstDelay dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">50</MaxValue>
                          <MinValue dataType="Float">50</MinValue>
                        </burstDelay>
                        <burstParticleNum dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">1</MaxValue>
                          <MinValue dataType="Float">1</MinValue>
                        </burstParticleNum>
                        <depthMult dataType="Float">1</depthMult>
                        <maxBurstCount dataType="Int">-1</maxBurstCount>
                        <maxColor dataType="Struct" type="Duality.Drawing.ColorHsva">
                          <A dataType="Float">1</A>
                          <H dataType="Float">0.0952381045</H>
                          <S dataType="Float">0.549019635</S>
                          <V dataType="Float">1</V>
                        </maxColor>
                        <minColor dataType="Struct" type="Duality.Drawing.ColorHsva">
                          <A dataType="Float">0.5019608</A>
                          <H dataType="Float">0.03935185</H>
                          <S dataType="Float">1</S>
                          <V dataType="Float">0.847058833</V>
                        </minColor>
                        <particleLifetime dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">3000</MaxValue>
                          <MinValue dataType="Float">1500</MinValue>
                        </particleLifetime>
                        <randomAngle dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">6.28318548</MaxValue>
                          <MinValue dataType="Float">0</MinValue>
                        </randomAngle>
                        <randomAngleVel dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">0.05</MaxValue>
                          <MinValue dataType="Float">-0.05</MinValue>
                        </randomAngleVel>
                        <randomPos dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">35</MaxValue>
                          <MinValue dataType="Float">0</MinValue>
                        </randomPos>
                        <randomVel dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">1</MaxValue>
                          <MinValue dataType="Float">0</MinValue>
                        </randomVel>
                        <spriteIndex dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">4</MaxValue>
                          <MinValue dataType="Float">0</MinValue>
                        </spriteIndex>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                    <_version dataType="Int">2</_version>
                  </emitters>
                  <fadeInAt dataType="Float">0.25</fadeInAt>
                  <fadeOutAt dataType="Float">0.4</fadeOutAt>
                  <gameobj dataType="ObjectRef">829250152</gameobj>
                  <linearDrag dataType="Float">0.3</linearDrag>
                  <material dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\ParticleSystem\Visuals\GlowShards.Material.res</contentPath>
                  </material>
                  <particleSize dataType="Struct" type="Duality.Vector2">
                    <X dataType="Float">40</X>
                    <Y dataType="Float">40</Y>
                  </particleSize>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3130379580" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1529572472">
                  <item dataType="ObjectRef">3664359340</item>
                  <item dataType="ObjectRef">3134076800</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3648822750">
                  <item dataType="ObjectRef">3189565084</item>
                  <item dataType="ObjectRef">798880182</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3189565084</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1678472484">LB7b0rZBd0S0SaLueD1WMw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">FireEffect</name>
            <parent dataType="ObjectRef">2883986298</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2707195565">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3399148353">
              <_items dataType="Array" type="Duality.Component[]" id="3402482094" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="772543201">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">2707195565</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-256</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-256</X>
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
                <item dataType="Struct" type="ParticleSystem.ParticleEffect" id="2676825595">
                  <active dataType="Bool">true</active>
                  <angularDrag dataType="Float">0.3</angularDrag>
                  <constantForce dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </constantForce>
                  <emitters dataType="Struct" type="System.Collections.Generic.List`1[[ParticleSystem.ParticleEmitter]]" id="1561152987">
                    <_items dataType="Array" type="ParticleSystem.ParticleEmitter[]" id="2879698070" length="4">
                      <item dataType="Struct" type="ParticleSystem.ParticleEmitter" id="4236755488">
                        <basePos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">0</Y>
                          <Z dataType="Float">0</Z>
                        </basePos>
                        <baseVel dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">0</Y>
                          <Z dataType="Float">0</Z>
                        </baseVel>
                        <burstDelay dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">3000</MaxValue>
                          <MinValue dataType="Float">3000</MinValue>
                        </burstDelay>
                        <burstParticleNum dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">100</MaxValue>
                          <MinValue dataType="Float">100</MinValue>
                        </burstParticleNum>
                        <depthMult dataType="Float">1</depthMult>
                        <maxBurstCount dataType="Int">-1</maxBurstCount>
                        <maxColor dataType="Struct" type="Duality.Drawing.ColorHsva">
                          <A dataType="Float">1</A>
                          <H dataType="Float">0.571729958</H>
                          <S dataType="Float">0.309803933</S>
                          <V dataType="Float">1</V>
                        </maxColor>
                        <minColor dataType="Struct" type="Duality.Drawing.ColorHsva">
                          <A dataType="Float">1</A>
                          <H dataType="Float">0.793464</H>
                          <S dataType="Float">1</S>
                          <V dataType="Float">1</V>
                        </minColor>
                        <particleLifetime dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">2500</MaxValue>
                          <MinValue dataType="Float">2000</MinValue>
                        </particleLifetime>
                        <randomAngle dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">6.28318548</MaxValue>
                          <MinValue dataType="Float">0</MinValue>
                        </randomAngle>
                        <randomAngleVel dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">0.05</MaxValue>
                          <MinValue dataType="Float">-0.05</MinValue>
                        </randomAngleVel>
                        <randomPos dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">0</MaxValue>
                          <MinValue dataType="Float">0</MinValue>
                        </randomPos>
                        <randomVel dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">4</MaxValue>
                          <MinValue dataType="Float">4</MinValue>
                        </randomVel>
                        <spriteIndex dataType="Struct" type="Duality.Range">
                          <MaxValue dataType="Float">0</MaxValue>
                          <MinValue dataType="Float">0</MinValue>
                        </spriteIndex>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                    <_version dataType="Int">2</_version>
                  </emitters>
                  <fadeInAt dataType="Float">0</fadeInAt>
                  <fadeOutAt dataType="Float">0.8</fadeOutAt>
                  <gameobj dataType="ObjectRef">2707195565</gameobj>
                  <linearDrag dataType="Float">0.3</linearDrag>
                  <material dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\ParticleSystem\Visuals\GlowShards.Material.res</contentPath>
                  </material>
                  <particleSize dataType="Struct" type="Duality.Vector2">
                    <X dataType="Float">16</X>
                    <Y dataType="Float">16</Y>
                  </particleSize>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2041909728" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3736511627">
                  <item dataType="ObjectRef">3664359340</item>
                  <item dataType="ObjectRef">3134076800</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2968896712">
                  <item dataType="ObjectRef">772543201</item>
                  <item dataType="ObjectRef">2676825595</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">772543201</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3762998849">Ks29TuE7BkuBQ9SwWv44gQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">FireworksEffect</name>
            <parent dataType="ObjectRef">2883986298</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">3</_size>
        <_version dataType="Int">7</_version>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="930755606">
        <_items dataType="Array" type="Duality.Component[]" id="1970291502" length="0" />
        <_size dataType="Int">0</_size>
        <_version dataType="Int">0</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1701022048" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2672198152" length="0" />
          <values dataType="Array" type="System.Object[]" id="2331579870" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3985124852">s2efQXlPJ0S78aUhFTEp7g==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Effects</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="708372333">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1944571615">
        <_items dataType="Array" type="Duality.Component[]" id="2140022382" length="4">
          <item dataType="Struct" type="Duality.Components.Diagnostics.ProfileRenderer" id="975207225">
            <active dataType="Bool">true</active>
            <counterGraphs dataType="Struct" type="System.Collections.Generic.List`1[[System.String]]" id="2187109705">
              <_items dataType="Array" type="System.String[]" id="338561934">
                <item dataType="String">Duality\Frame</item>
                <item dataType="String">Duality\Frame\Render</item>
                <item dataType="String">Duality\Frame\Update</item>
                <item dataType="String">Duality\Stats\Memory\TotalUsage</item>
              </_items>
              <_size dataType="Int">4</_size>
              <_version dataType="Int">4</_version>
            </counterGraphs>
            <drawGraphs dataType="Bool">false</drawGraphs>
            <gameobj dataType="ObjectRef">708372333</gameobj>
            <keyToggleGraph dataType="Enum" type="Duality.Input.Key" name="F4" value="13" />
            <keyToggleTextPerf dataType="Enum" type="Duality.Input.Key" name="F2" value="11" />
            <keyToggleTextStat dataType="Enum" type="Duality.Input.Key" name="F3" value="12" />
            <textReportOptions dataType="Enum" type="Duality.ProfileReportOptions" name="LastValue, OmitMinorValues" value="32769" />
            <textReportPerf dataType="Bool">false</textReportPerf>
            <textReportStat dataType="Bool">false</textReportStat>
            <updateInterval dataType="Int">250</updateInterval>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
        <_version dataType="Int">1</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="703926560" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2996708565">
            <item dataType="Type" id="3528536566" value="Duality.Components.Diagnostics.ProfileRenderer" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="1340996168">
            <item dataType="ObjectRef">975207225</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2531351775">SKMoUxCQbkSzUOlnREtPSw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">ProfileRenderer</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">4113979440</item>
    <item dataType="ObjectRef">829250152</item>
    <item dataType="ObjectRef">2707195565</item>
    <item dataType="ObjectRef">4042308553</item>
    <item dataType="ObjectRef">16560041</item>
    <item dataType="ObjectRef">3985077999</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
