<root dataType="Struct" type="Duality.Resources.Prefab" id="129723834">
  <objTree dataType="Struct" type="Duality.GameObject" id="2747808710">
    <active dataType="Bool">true</active>
    <children />
    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2475945809">
      <_items dataType="Array" type="Duality.Component[]" id="1653200878" length="4">
        <item dataType="Struct" type="Duality.Components.Transform" id="813156346">
          <active dataType="Bool">true</active>
          <angle dataType="Float">0</angle>
          <angleAbs dataType="Float">0</angleAbs>
          <angleVel dataType="Float">0</angleVel>
          <angleVelAbs dataType="Float">0</angleVelAbs>
          <deriveAngle dataType="Bool">true</deriveAngle>
          <gameobj dataType="ObjectRef">2747808710</gameobj>
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
        <item dataType="Struct" type="DualStickSpaceShooter.ParticleEffect" id="3774441803">
          <active dataType="Bool">true</active>
          <angularDrag dataType="Float">0.1</angularDrag>
          <burstCount dataType="Int">0</burstCount>
          <burstTimer dataType="Float">0</burstTimer>
          <disposeWhenEmpty dataType="Bool">true</disposeWhenEmpty>
          <emitData dataType="Struct" type="DualStickSpaceShooter.ParticleEffect+EmissionData">
            <BasePos dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">0</Z>
            </BasePos>
            <BaseVel dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">0</Z>
            </BaseVel>
            <Lifetime dataType="Struct" type="Duality.Range">
              <MaxValue dataType="Float">3000</MaxValue>
              <MinValue dataType="Float">1000</MinValue>
            </Lifetime>
            <MaxColor dataType="Struct" type="Duality.Drawing.ColorHsva">
              <A dataType="Float">1</A>
              <H dataType="Float">0</H>
              <S dataType="Float">0</S>
              <V dataType="Float">0.419607848</V>
            </MaxColor>
            <MinColor dataType="Struct" type="Duality.Drawing.ColorHsva">
              <A dataType="Float">1</A>
              <H dataType="Float">0</H>
              <S dataType="Float">0</S>
              <V dataType="Float">0</V>
            </MinColor>
            <RandomAngle dataType="Struct" type="Duality.Range">
              <MaxValue dataType="Float">6.28318548</MaxValue>
              <MinValue dataType="Float">0</MinValue>
            </RandomAngle>
            <RandomAngleVel dataType="Struct" type="Duality.Range">
              <MaxValue dataType="Float">0.05</MaxValue>
              <MinValue dataType="Float">-0.05</MinValue>
            </RandomAngleVel>
            <RandomPos dataType="Struct" type="Duality.Range">
              <MaxValue dataType="Float">0</MaxValue>
              <MinValue dataType="Float">0</MinValue>
            </RandomPos>
            <RandomVel dataType="Struct" type="Duality.Range">
              <MaxValue dataType="Float">5</MaxValue>
              <MinValue dataType="Float">2</MinValue>
            </RandomVel>
            <SpriteIndex dataType="Struct" type="Duality.Range">
              <MaxValue dataType="Float">15</MaxValue>
              <MinValue dataType="Float">0</MinValue>
            </SpriteIndex>
          </emitData>
          <emitPattern dataType="Struct" type="DualStickSpaceShooter.ParticleEffect+EmissionPattern">
            <Count dataType="Struct" type="Duality.Range">
              <MaxValue dataType="Float">25</MaxValue>
              <MinValue dataType="Float">25</MinValue>
            </Count>
            <Delay dataType="Struct" type="Duality.Range">
              <MaxValue dataType="Float">2000</MaxValue>
              <MinValue dataType="Float">2000</MinValue>
            </Delay>
            <MaxBurstCount dataType="Int">1</MaxBurstCount>
          </emitPattern>
          <fadeOutAt dataType="Float">0.75</fadeOutAt>
          <gameobj dataType="ObjectRef">2747808710</gameobj>
          <linearDrag dataType="Float">0.3</linearDrag>
          <material dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
            <contentPath dataType="String">Data\Materials\SolidShards.Material.res</contentPath>
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
    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="644061344" surrogate="true">
      <header />
      <body>
        <keys dataType="Array" type="System.Type[]" id="981236859">
          <item dataType="Type" id="4013095254" value="Duality.Components.Transform" />
          <item dataType="Type" id="2647838938" value="DualStickSpaceShooter.ParticleEffect" />
        </keys>
        <values dataType="Array" type="Duality.Component[]" id="465871528">
          <item dataType="ObjectRef">813156346</item>
          <item dataType="ObjectRef">3774441803</item>
        </values>
      </body>
    </compMap>
    <compTransform dataType="ObjectRef">813156346</compTransform>
    <identifier dataType="Struct" type="System.Guid" surrogate="true">
      <header>
        <data dataType="Array" type="System.Byte[]" id="112707441">xHZemJbBxk2p4fp/kUIIxA==</data>
      </header>
      <body />
    </identifier>
    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
    <name dataType="String">ExplosionClaymoreSmoke</name>
    <parent />
    <prefabLink />
  </objTree>
  <sourcePath dataType="String">ExplosionClaymoreSmoke</sourcePath>
</root>
<!-- XmlFormatterBase Document Separator -->
