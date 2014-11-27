<root dataType="Struct" type="Duality.Resources.Prefab" id="129723834">
  <objTree dataType="Struct" type="Duality.GameObject" id="3692936090">
    <active dataType="Bool">true</active>
    <children />
    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2912374333">
      <_items dataType="Array" type="Duality.Component[]" id="4266898470" length="4">
        <item dataType="Struct" type="Duality.Components.Transform" id="1758283726">
          <active dataType="Bool">true</active>
          <angle dataType="Float">0</angle>
          <angleAbs dataType="Float">0</angleAbs>
          <angleVel dataType="Float">0</angleVel>
          <angleVelAbs dataType="Float">0</angleVelAbs>
          <deriveAngle dataType="Bool">true</deriveAngle>
          <gameobj dataType="ObjectRef">3692936090</gameobj>
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
        <item dataType="Struct" type="DualStickSpaceShooter.ParticleEffect" id="424601887">
          <active dataType="Bool">true</active>
          <angularDrag dataType="Float">0.3</angularDrag>
          <constantForce dataType="Struct" type="OpenTK.Vector3">
            <X dataType="Float">0</X>
            <Y dataType="Float">0</Y>
            <Z dataType="Float">0</Z>
          </constantForce>
          <disposeWhenEmpty dataType="Bool">true</disposeWhenEmpty>
          <emitters dataType="Struct" type="System.Collections.Generic.List`1[[DualStickSpaceShooter.ParticleEmitter]]" id="2012284639">
            <_items dataType="Array" type="DualStickSpaceShooter.ParticleEmitter[]" id="2754942574" length="4">
              <item dataType="Struct" type="DualStickSpaceShooter.ParticleEmitter" id="2285270608">
                <basePos dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </basePos>
                <baseVel dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">1</Y>
                  <Z dataType="Float">0</Z>
                </baseVel>
                <burstDelay dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">1000</MaxValue>
                  <MinValue dataType="Float">1000</MinValue>
                </burstDelay>
                <burstParticleNum dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">5</MaxValue>
                  <MinValue dataType="Float">3</MinValue>
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
                  <MaxValue dataType="Float">1500</MaxValue>
                  <MinValue dataType="Float">1000</MinValue>
                </particleLifetime>
                <randomAngle dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">6.28318548</MaxValue>
                  <MinValue dataType="Float">0</MinValue>
                </randomAngle>
                <randomAngleVel dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">0.4</MaxValue>
                  <MinValue dataType="Float">-0.4</MinValue>
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
          <gameobj dataType="ObjectRef">3692936090</gameobj>
          <linearDrag dataType="Float">0.4</linearDrag>
          <material dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
            <contentPath dataType="String">Data\DualStickSpaceShooter\Materials\AlphaShards.Material.res</contentPath>
          </material>
          <particleSize dataType="Struct" type="OpenTK.Vector2">
            <X dataType="Float">4</X>
            <Y dataType="Float">4</Y>
          </particleSize>
          <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          <worldSpace dataType="Bool">false</worldSpace>
        </item>
      </_items>
      <_size dataType="Int">2</_size>
      <_version dataType="Int">2</_version>
    </compList>
    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3658172088" surrogate="true">
      <header />
      <body>
        <keys dataType="Array" type="System.Type[]" id="3263821655">
          <item dataType="Type" id="104844302" value="Duality.Components.Transform" />
          <item dataType="Type" id="2339488586" value="DualStickSpaceShooter.ParticleEffect" />
        </keys>
        <values dataType="Array" type="Duality.Component[]" id="4276562368">
          <item dataType="ObjectRef">1758283726</item>
          <item dataType="ObjectRef">424601887</item>
        </values>
      </body>
    </compMap>
    <compTransform dataType="ObjectRef">1758283726</compTransform>
    <identifier dataType="Struct" type="System.Guid" surrogate="true">
      <header>
        <data dataType="Array" type="System.Byte[]" id="164310389">kvGH1JoDuUqfpAuhmC0bsQ==</data>
      </header>
      <body />
    </identifier>
    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
    <name dataType="String">BulletHitSparks</name>
    <parent />
    <prefabLink />
  </objTree>
  <sourcePath dataType="String">BulletHitSparks</sourcePath>
</root>
<!-- XmlFormatterBase Document Separator -->
