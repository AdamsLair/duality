<root dataType="Struct" type="Duality.Resources.Prefab" id="129723834">
  <objTree dataType="Struct" type="Duality.GameObject" id="3771240307">
    <active dataType="Bool">true</active>
    <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1261459200">
      <_items dataType="Array" type="Duality.GameObject[]" id="1585975964" length="4" />
      <_size dataType="Int">0</_size>
      <_version dataType="Int">4</_version>
    </children>
    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="65001934">
      <_items dataType="Array" type="Duality.Component[]" id="3332403154" length="4">
        <item dataType="Struct" type="Duality.Components.Transform" id="1836587943">
          <active dataType="Bool">true</active>
          <angle dataType="Float">0</angle>
          <angleAbs dataType="Float">0</angleAbs>
          <angleVel dataType="Float">0</angleVel>
          <angleVelAbs dataType="Float">0</angleVelAbs>
          <deriveAngle dataType="Bool">true</deriveAngle>
          <gameobj dataType="ObjectRef">3771240307</gameobj>
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
        <item dataType="Struct" type="DualStickSpaceShooter.ParticleEffect" id="502906104">
          <active dataType="Bool">true</active>
          <angularDrag dataType="Float">0.1</angularDrag>
          <constantForce dataType="Struct" type="OpenTK.Vector3">
            <X dataType="Float">0</X>
            <Y dataType="Float">0</Y>
            <Z dataType="Float">0</Z>
          </constantForce>
          <disposeWhenEmpty dataType="Bool">true</disposeWhenEmpty>
          <emitters dataType="Struct" type="System.Collections.Generic.List`1[[DualStickSpaceShooter.ParticleEmitter]]" id="830637028">
            <_items dataType="Array" type="DualStickSpaceShooter.ParticleEmitter[]" id="2234910148" length="4">
              <item dataType="Struct" type="DualStickSpaceShooter.ParticleEmitter" id="2088268100">
                <basePos dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </basePos>
                <baseVel dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </baseVel>
                <burstDelay dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">2000</MaxValue>
                  <MinValue dataType="Float">2000</MinValue>
                </burstDelay>
                <burstParticleNum dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">50</MaxValue>
                  <MinValue dataType="Float">50</MinValue>
                </burstParticleNum>
                <depthMult dataType="Float">1</depthMult>
                <maxBurstCount dataType="Int">1</maxBurstCount>
                <maxColor dataType="Struct" type="Duality.Drawing.ColorHsva">
                  <A dataType="Float">1</A>
                  <H dataType="Float">0.0249140877</H>
                  <S dataType="Float">0.7607843</S>
                  <V dataType="Float">1</V>
                </maxColor>
                <minColor dataType="Struct" type="Duality.Drawing.ColorHsva">
                  <A dataType="Float">1</A>
                  <H dataType="Float">0.09738563</H>
                  <S dataType="Float">1</S>
                  <V dataType="Float">1</V>
                </minColor>
                <particleLifetime dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">3000</MaxValue>
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
                  <MaxValue dataType="Float">5</MaxValue>
                  <MinValue dataType="Float">2.5</MinValue>
                </randomVel>
                <spriteIndex dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">15</MaxValue>
                  <MinValue dataType="Float">0</MinValue>
                </spriteIndex>
              </item>
            </_items>
            <_size dataType="Int">1</_size>
            <_version dataType="Int">2</_version>
          </emitters>
          <fadeInAt dataType="Float">0</fadeInAt>
          <fadeOutAt dataType="Float">0.75</fadeOutAt>
          <gameobj dataType="ObjectRef">3771240307</gameobj>
          <linearDrag dataType="Float">0.3</linearDrag>
          <material dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
            <contentPath dataType="String">Data\DualStickSpaceShooter\Materials\GlowShards.Material.res</contentPath>
          </material>
          <particleSize dataType="Struct" type="OpenTK.Vector2">
            <X dataType="Float">16</X>
            <Y dataType="Float">16</Y>
          </particleSize>
          <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          <worldSpace dataType="Bool">false</worldSpace>
        </item>
      </_items>
      <_size dataType="Int">2</_size>
      <_version dataType="Int">4</_version>
    </compList>
    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2037649820" surrogate="true">
      <header />
      <body>
        <keys dataType="Array" type="System.Type[]" id="759844024">
          <item dataType="Type" id="2873852012" value="Duality.Components.Transform" />
          <item dataType="Type" id="2141170742" value="DualStickSpaceShooter.ParticleEffect" />
        </keys>
        <values dataType="Array" type="Duality.Component[]" id="1696079070">
          <item dataType="ObjectRef">1836587943</item>
          <item dataType="ObjectRef">502906104</item>
        </values>
      </body>
    </compMap>
    <compTransform dataType="ObjectRef">1836587943</compTransform>
    <identifier dataType="Struct" type="System.Guid" surrogate="true">
      <header>
        <data dataType="Array" type="System.Byte[]" id="2947520740">P8XKKwLJzUqSlwJQKd2xgQ==</data>
      </header>
      <body />
    </identifier>
    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
    <name dataType="String">ExplosionClaymoreGlow</name>
    <parent />
    <prefabLink />
  </objTree>
  <sourcePath dataType="String">ClaymoreExplosion</sourcePath>
</root>
<!-- XmlFormatterBase Document Separator -->
