<root dataType="Struct" type="Duality.Resources.Prefab" id="129723834">
  <assetInfo />
  <objTree dataType="Struct" type="Duality.GameObject" id="3398526949">
    <active dataType="Bool">true</active>
    <children />
    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2734610566">
      <_items dataType="Array" type="Duality.Component[]" id="2242210688" length="8">
        <item dataType="Struct" type="Duality.Components.Transform" id="3455804167">
          <active dataType="Bool">true</active>
          <angle dataType="Float">0</angle>
          <angleAbs dataType="Float">0</angleAbs>
          <gameobj dataType="ObjectRef">3398526949</gameobj>
          <ignoreParent dataType="Bool">false</ignoreParent>
          <pos dataType="Struct" type="Duality.Vector3" />
          <posAbs dataType="Struct" type="Duality.Vector3" />
          <scale dataType="Float">1</scale>
          <scaleAbs dataType="Float">1</scaleAbs>
        </item>
        <item dataType="Struct" type="Duality.Components.VelocityTracker" id="1174694120">
          <active dataType="Bool">true</active>
          <gameobj dataType="ObjectRef">3398526949</gameobj>
        </item>
        <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2933456437">
          <active dataType="Bool">true</active>
          <allowParent dataType="Bool">false</allowParent>
          <angularDamp dataType="Float">0.3</angularDamp>
          <angularVel dataType="Float">0</angularVel>
          <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
          <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
          <colFilter />
          <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="None" value="0" />
          <explicitInertia dataType="Float">0</explicitInertia>
          <explicitMass dataType="Float">0</explicitMass>
          <fixedAngle dataType="Bool">false</fixedAngle>
          <gameobj dataType="ObjectRef">3398526949</gameobj>
          <ignoreGravity dataType="Bool">true</ignoreGravity>
          <joints />
          <linearDamp dataType="Float">0.3</linearDamp>
          <linearVel dataType="Struct" type="Duality.Vector2" />
          <revolutions dataType="Float">0</revolutions>
          <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="318504961">
            <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2662516526" length="4">
              <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="455290704">
                <density dataType="Float">1</density>
                <friction dataType="Float">0.3</friction>
                <parent dataType="ObjectRef">2933456437</parent>
                <position dataType="Struct" type="Duality.Vector2" />
                <radius dataType="Float">40</radius>
                <restitution dataType="Float">0.3</restitution>
                <sensor dataType="Bool">false</sensor>
                <userTag dataType="Int">0</userTag>
              </item>
            </_items>
            <_size dataType="Int">1</_size>
          </shapes>
          <useCCD dataType="Bool">false</useCCD>
        </item>
        <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="2157533611">
          <active dataType="Bool">true</active>
          <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
            <contentPath dataType="String">Data\SteeringSample\Textures\Agent.Material.res</contentPath>
          </areaMaterial>
          <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
            <A dataType="Byte">255</A>
            <B dataType="Byte">255</B>
            <G dataType="Byte">255</G>
            <R dataType="Byte">255</R>
          </colorTint>
          <customAreaMaterial />
          <customOutlineMaterial />
          <fillHollowShapes dataType="Bool">false</fillHollowShapes>
          <gameobj dataType="ObjectRef">3398526949</gameobj>
          <offset dataType="Float">0</offset>
          <outlineMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
            <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
          </outlineMaterial>
          <outlineWidth dataType="Float">3</outlineWidth>
          <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          <wrapTexture dataType="Bool">false</wrapTexture>
        </item>
        <item dataType="Struct" type="Steering.Agent" id="2443005651">
          <active dataType="Bool">true</active>
          <characteristics dataType="Struct" type="Steering.DefaultAgentCharacteristics" id="3984650151">
            <aggressiveness dataType="Float">0.5</aggressiveness>
          </characteristics>
          <debugVisualizationMode dataType="Enum" type="Steering.Agent+VisualLoggingMode" name="None" value="0" />
          <gameobj dataType="ObjectRef">3398526949</gameobj>
          <radius dataType="Float">0</radius>
          <sampler dataType="Struct" type="Steering.AdaptiveVelocitySampler" id="3903986688">
            <layerCount dataType="Int">3</layerCount>
            <outerLayerSampleCount dataType="Int">11</outerLayerSampleCount>
          </sampler>
          <target dataType="Struct" type="Steering.PointTarget" id="2410154981">
            <location dataType="Struct" type="Duality.Vector2" />
          </target>
          <toiHorizon dataType="Float">240</toiHorizon>
        </item>
        <item dataType="Struct" type="Steering.AgentAttributeTranslator" id="3940420613">
          <active dataType="Bool">true</active>
          <gameobj dataType="ObjectRef">3398526949</gameobj>
        </item>
      </_items>
      <_size dataType="Int">6</_size>
    </compList>
    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3658316602" surrogate="true">
      <header />
      <body>
        <keys dataType="Array" type="System.Object[]" id="288805620">
          <item dataType="Type" id="3646840996" value="Duality.Components.Transform" />
          <item dataType="Type" id="3446586134" value="Duality.Components.Physics.RigidBody" />
          <item dataType="Type" id="1598455712" value="Duality.Components.Renderers.RigidBodyRenderer" />
          <item dataType="Type" id="2634619490" value="Steering.Agent" />
          <item dataType="Type" id="353654012" value="Steering.AgentAttributeTranslator" />
          <item dataType="Type" id="1845439934" value="Duality.Components.VelocityTracker" />
        </keys>
        <values dataType="Array" type="System.Object[]" id="2189131510">
          <item dataType="ObjectRef">3455804167</item>
          <item dataType="ObjectRef">2933456437</item>
          <item dataType="ObjectRef">2157533611</item>
          <item dataType="ObjectRef">2443005651</item>
          <item dataType="ObjectRef">3940420613</item>
          <item dataType="ObjectRef">1174694120</item>
        </values>
      </body>
    </compMap>
    <compTransform dataType="ObjectRef">3455804167</compTransform>
    <identifier dataType="Struct" type="System.Guid" surrogate="true">
      <header>
        <data dataType="Array" type="System.Byte[]" id="3725715408">Dfn6yiM0EEW69aLSUglDig==</data>
      </header>
      <body />
    </identifier>
    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
    <name dataType="String">Agent</name>
    <parent />
    <prefabLink />
  </objTree>
</root>
<!-- XmlFormatterBase Document Separator -->
