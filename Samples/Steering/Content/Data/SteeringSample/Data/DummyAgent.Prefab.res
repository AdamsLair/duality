<root dataType="Struct" type="Duality.Resources.Prefab" id="129723834">
  <assetInfo />
  <objTree dataType="Struct" type="Duality.GameObject" id="1557773463">
    <active dataType="Bool">true</active>
    <children />
    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1663315804">
      <_items dataType="Array" type="Duality.Component[]" id="4020678852" length="8">
        <item dataType="Struct" type="Duality.Components.Transform" id="3918088395">
          <active dataType="Bool">true</active>
          <angle dataType="Float">0</angle>
          <angleAbs dataType="Float">0</angleAbs>
          <angleVel dataType="Float">0</angleVel>
          <angleVelAbs dataType="Float">0</angleVelAbs>
          <deriveAngle dataType="Bool">true</deriveAngle>
          <gameobj dataType="ObjectRef">1557773463</gameobj>
          <ignoreParent dataType="Bool">false</ignoreParent>
          <parentTransform />
          <pos dataType="Struct" type="Duality.Vector3">
            <X dataType="Float">-377</X>
            <Y dataType="Float">-224</Y>
            <Z dataType="Float">0</Z>
          </pos>
          <posAbs dataType="Struct" type="Duality.Vector3">
            <X dataType="Float">-377</X>
            <Y dataType="Float">-224</Y>
            <Z dataType="Float">0</Z>
          </posAbs>
          <scale dataType="Float">1</scale>
          <scaleAbs dataType="Float">1</scaleAbs>
          <vel dataType="Struct" type="Duality.Vector3" />
          <velAbs dataType="Struct" type="Duality.Vector3" />
        </item>
        <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="325582691">
          <active dataType="Bool">true</active>
          <angularDamp dataType="Float">0.3</angularDamp>
          <angularVel dataType="Float">0</angularVel>
          <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
          <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
          <colFilter />
          <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="None" value="0" />
          <continous dataType="Bool">false</continous>
          <explicitInertia dataType="Float">0</explicitInertia>
          <explicitMass dataType="Float">0</explicitMass>
          <fixedAngle dataType="Bool">false</fixedAngle>
          <gameobj dataType="ObjectRef">1557773463</gameobj>
          <ignoreGravity dataType="Bool">true</ignoreGravity>
          <joints />
          <linearDamp dataType="Float">0.3</linearDamp>
          <linearVel dataType="Struct" type="Duality.Vector2" />
          <revolutions dataType="Float">0</revolutions>
          <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1329389839">
            <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="476054446" length="4">
              <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="3605793104">
                <density dataType="Float">1</density>
                <friction dataType="Float">0.3</friction>
                <parent dataType="ObjectRef">325582691</parent>
                <position dataType="Struct" type="Duality.Vector2" />
                <radius dataType="Float">40</radius>
                <restitution dataType="Float">0.3</restitution>
                <sensor dataType="Bool">false</sensor>
              </item>
            </_items>
            <_size dataType="Int">1</_size>
            <_version dataType="Int">1</_version>
          </shapes>
        </item>
        <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="2156466653">
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
          <gameobj dataType="ObjectRef">1557773463</gameobj>
          <offset dataType="Int">0</offset>
          <outlineMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
            <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
          </outlineMaterial>
          <outlineWidth dataType="Float">3</outlineWidth>
          <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          <wrapTexture dataType="Bool">false</wrapTexture>
        </item>
        <item dataType="Struct" type="Steering.Agent" id="2221266285">
          <active dataType="Bool">true</active>
          <characteristics dataType="Struct" type="Steering.DefaultAgentCharacteristics" id="3251264513">
            <aggressiveness dataType="Float">0.5</aggressiveness>
          </characteristics>
          <debugVisualizationMode dataType="Enum" type="Steering.Agent+VisualLoggingMode" name="None" value="0" />
          <gameobj dataType="ObjectRef">1557773463</gameobj>
          <radius dataType="Float">0</radius>
          <sampler dataType="Struct" type="Steering.AdaptiveVelocitySampler" id="2833030496">
            <layerCount dataType="Int">3</layerCount>
            <outerLayerSampleCount dataType="Int">11</outerLayerSampleCount>
          </sampler>
          <target dataType="Struct" type="Steering.PointTarget" id="3763273299">
            <location dataType="Struct" type="Duality.Vector2" />
          </target>
          <toiHorizon dataType="Float">240</toiHorizon>
        </item>
        <item dataType="Struct" type="Steering.AgentAttributeTranslator" id="3879568043">
          <active dataType="Bool">true</active>
          <gameobj dataType="ObjectRef">1557773463</gameobj>
        </item>
      </_items>
      <_size dataType="Int">5</_size>
      <_version dataType="Int">5</_version>
    </compList>
    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2884270870" surrogate="true">
      <header />
      <body>
        <keys dataType="Array" type="System.Object[]" id="921189750">
          <item dataType="Type" id="2285360096" value="Duality.Components.Transform" />
          <item dataType="Type" id="3213052814" value="Duality.Components.Physics.RigidBody" />
          <item dataType="Type" id="440111868" value="Duality.Components.Renderers.RigidBodyRenderer" />
          <item dataType="Type" id="2060713746" value="Steering.Agent" />
          <item dataType="Type" id="2856434328" value="Steering.AgentAttributeTranslator" />
        </keys>
        <values dataType="Array" type="System.Object[]" id="3442451738">
          <item dataType="ObjectRef">3918088395</item>
          <item dataType="ObjectRef">325582691</item>
          <item dataType="ObjectRef">2156466653</item>
          <item dataType="ObjectRef">2221266285</item>
          <item dataType="ObjectRef">3879568043</item>
        </values>
      </body>
    </compMap>
    <compTransform dataType="ObjectRef">3918088395</compTransform>
    <identifier dataType="Struct" type="System.Guid" surrogate="true">
      <header>
        <data dataType="Array" type="System.Byte[]" id="940801174">Dfn6yiM0EEW69aLSUglDig==</data>
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
