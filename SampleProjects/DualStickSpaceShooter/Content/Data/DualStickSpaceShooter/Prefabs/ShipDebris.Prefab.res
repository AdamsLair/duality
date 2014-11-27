<root dataType="Struct" type="Duality.Resources.Prefab" id="129723834">
  <objTree dataType="Struct" type="Duality.GameObject" id="2957948994">
    <active dataType="Bool">true</active>
    <children />
    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2595553349">
      <_items dataType="Array" type="Duality.Component[]" id="389833942" length="4">
        <item dataType="Struct" type="Duality.Components.Transform" id="1023296630">
          <active dataType="Bool">true</active>
          <angle dataType="Float">0</angle>
          <angleAbs dataType="Float">0</angleAbs>
          <angleVel dataType="Float">0</angleVel>
          <angleVelAbs dataType="Float">0</angleVelAbs>
          <deriveAngle dataType="Bool">true</deriveAngle>
          <gameobj dataType="ObjectRef">2957948994</gameobj>
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
        <item dataType="Struct" type="DualStickSpaceShooter.ParticleEffect" id="3984582087">
          <active dataType="Bool">true</active>
          <angularDrag dataType="Float">0.1</angularDrag>
          <constantForce dataType="Struct" type="OpenTK.Vector3">
            <X dataType="Float">0</X>
            <Y dataType="Float">0</Y>
            <Z dataType="Float">0</Z>
          </constantForce>
          <disposeWhenEmpty dataType="Bool">true</disposeWhenEmpty>
          <emitters dataType="Struct" type="System.Collections.Generic.List`1[[DualStickSpaceShooter.ParticleEmitter]]" id="3683865511">
            <_items dataType="Array" type="DualStickSpaceShooter.ParticleEmitter[]" id="256868302" length="4">
              <item dataType="Struct" type="DualStickSpaceShooter.ParticleEmitter" id="4120502736">
                <basePos dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </basePos>
                <baseVel dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0.5</Z>
                </baseVel>
                <burstDelay dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">2000</MaxValue>
                  <MinValue dataType="Float">2000</MinValue>
                </burstDelay>
                <burstParticleNum dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">7</MaxValue>
                  <MinValue dataType="Float">5</MinValue>
                </burstParticleNum>
                <depthMult dataType="Float">1</depthMult>
                <maxBurstCount dataType="Int">1</maxBurstCount>
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
                  <MaxValue dataType="Float">20000</MaxValue>
                  <MinValue dataType="Float">5000</MinValue>
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
                  <MaxValue dataType="Float">15</MaxValue>
                  <MinValue dataType="Float">0</MinValue>
                </randomPos>
                <randomVel dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">1.5</MaxValue>
                  <MinValue dataType="Float">0</MinValue>
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
          <gameobj dataType="ObjectRef">2957948994</gameobj>
          <linearDrag dataType="Float">0.1</linearDrag>
          <material dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
            <contentPath dataType="String">Data\DualStickSpaceShooter\Materials\AlphaShards.Material.res</contentPath>
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
      <_version dataType="Int">2</_version>
    </compList>
    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3682240552" surrogate="true">
      <header />
      <body>
        <keys dataType="Array" type="System.Type[]" id="1686935599">
          <item dataType="Type" id="1660751598" value="Duality.Components.Transform" />
          <item dataType="Type" id="1494707658" value="DualStickSpaceShooter.ParticleEffect" />
        </keys>
        <values dataType="Array" type="Duality.Component[]" id="2173448096">
          <item dataType="ObjectRef">1023296630</item>
          <item dataType="ObjectRef">3984582087</item>
        </values>
      </body>
    </compMap>
    <compTransform dataType="ObjectRef">1023296630</compTransform>
    <identifier dataType="Struct" type="System.Guid" surrogate="true">
      <header>
        <data dataType="Array" type="System.Byte[]" id="275346621">2/p6vjQSV0K2KxxDbZhYzg==</data>
      </header>
      <body />
    </identifier>
    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
    <name dataType="String">ShipDebris</name>
    <parent />
    <prefabLink />
  </objTree>
  <sourcePath dataType="String">ShipDebris</sourcePath>
</root>
<!-- XmlFormatterBase Document Separator -->
