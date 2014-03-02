<?xml version="1.0" encoding="utf-8"?>
<root>
  <object dataType="Class" type="Duality.Resources.Prefab">
    <objTree dataType="Class" type="Duality.GameObject" id="1557773463">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Class" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3108262088">
        <_items dataType="Array" type="Duality.Component[]" id="3111646800" length="8">
          <object dataType="Class" type="Duality.Components.Transform" id="3918088395">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">1557773463</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <parentTransform />
            <pos dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">-377</X>
              <Y dataType="Float">-224</Y>
              <Z dataType="Float">0</Z>
            </pos>
            <posAbs dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">-377</X>
              <Y dataType="Float">-224</Y>
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
          </object>
          <object dataType="Class" type="Duality.Components.Physics.RigidBody" id="325582691">
            <active dataType="Bool">true</active>
            <angularDamp dataType="Float">0.3</angularDamp>
            <angularVel dataType="Float">0</angularVel>
            <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
            <colCat dataType="Enum" type="FarseerPhysics.Dynamics.Category" name="Cat1" value="1" />
            <colWith dataType="Enum" type="FarseerPhysics.Dynamics.Category" name="None" value="0" />
            <continous dataType="Bool">false</continous>
            <explicitMass dataType="Float">0</explicitMass>
            <fixedAngle dataType="Bool">false</fixedAngle>
            <gameobj dataType="ObjectRef">1557773463</gameobj>
            <ignoreGravity dataType="Bool">true</ignoreGravity>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <joints />
            <linearDamp dataType="Float">0.3</linearDamp>
            <linearVel dataType="Struct" type="OpenTK.Vector2">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
            </linearVel>
            <revolutions dataType="Float">0</revolutions>
            <shapes dataType="Class" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2849843259">
              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3556864082" length="4">
                <object dataType="Class" type="Duality.Components.Physics.CircleShapeInfo" id="3955723540">
                  <density dataType="Float">1</density>
                  <friction dataType="Float">0.3</friction>
                  <parent dataType="ObjectRef">325582691</parent>
                  <position dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                  </position>
                  <radius dataType="Float">40</radius>
                  <restitution dataType="Float">0.3</restitution>
                  <sensor dataType="Bool">false</sensor>
                </object>
                <object />
                <object />
                <object />
              </_items>
              <_size dataType="Int">1</_size>
              <_version dataType="Int">1</_version>
            </shapes>
          </object>
          <object dataType="Class" type="Duality.Components.Diagnostics.RigidBodyRenderer" id="1714998165">
            <active dataType="Bool">true</active>
            <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
              <contentPath dataType="String">Data\Textures\Agent.Material.res</contentPath>
            </areaMaterial>
            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">255</B>
              <G dataType="Byte">255</G>
              <R dataType="Byte">255</R>
            </colorTint>
            <customAreaMaterial />
            <customOutlineMaterial />
            <gameobj dataType="ObjectRef">1557773463</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <offset dataType="Int">0</offset>
            <outlineMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
              <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
            </outlineMaterial>
            <outlineWidth dataType="Float">3</outlineWidth>
            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
            <wrapTexture dataType="Bool">false</wrapTexture>
          </object>
          <object dataType="Class" type="Duality.Plugins.Steering.Agent" id="2639804075">
            <active dataType="Bool">true</active>
            <characteristics dataType="Class" type="Duality.Plugins.Steering.DefaultAgentCharacteristics" id="3135367491">
              <aggressiveness dataType="Float">0.5</aggressiveness>
            </characteristics>
            <debugVisualizationMode dataType="Enum" type="Duality.Plugins.Steering.Agent+VisualLoggingMode" name="None" value="0" />
            <gameobj dataType="ObjectRef">1557773463</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <radius dataType="Float">0</radius>
            <sampler dataType="Class" type="Duality.Plugins.Steering.AdaptiveVelocitySampler" id="2860577942">
              <layerCount dataType="Int">3</layerCount>
              <outerLayerSampleCount dataType="Int">11</outerLayerSampleCount>
            </sampler>
            <target dataType="Class" type="Duality.Plugins.Steering.PointTarget" id="383030277">
              <location dataType="Struct" type="OpenTK.Vector2">
                <X dataType="Float">0</X>
                <Y dataType="Float">0</Y>
              </location>
            </target>
            <toiHorizon dataType="Float">240</toiHorizon>
          </object>
          <object dataType="Class" type="Duality.Plugins.Steering.Testbed.AgentAttributeTranslator" id="3618709670">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1557773463</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </object>
          <object />
          <object />
          <object />
        </_items>
        <_size dataType="Int">5</_size>
        <_version dataType="Int">5</_version>
      </compList>
      <compMap dataType="Class" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3520029296" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="829463976" length="5">
            <object dataType="Type" id="95016848" value="Duality.Components.Transform" />
            <object dataType="Type" id="796452064" value="Duality.Components.Physics.RigidBody" />
            <object dataType="Type" id="3199248240" value="Duality.Components.Diagnostics.RigidBodyRenderer" />
            <object dataType="Type" id="2059484608" value="Duality.Plugins.Steering.Agent" />
            <object dataType="Type" id="942372048" value="Duality.Plugins.Steering.Testbed.AgentAttributeTranslator" />
          </keys>
          <values dataType="Array" type="Duality.Component[]" id="3067593776" length="5">
            <object dataType="ObjectRef">3918088395</object>
            <object dataType="ObjectRef">325582691</object>
            <object dataType="ObjectRef">1714998165</object>
            <object dataType="ObjectRef">2639804075</object>
            <object dataType="ObjectRef">3618709670</object>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">3918088395</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1865238040" length="16">Dfn6yiM0EEW69aLSUglDig==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Agent</name>
      <parent />
      <prefabLink />
    </objTree>
    <sourcePath dataType="String">Agent</sourcePath>
  </object>
</root>