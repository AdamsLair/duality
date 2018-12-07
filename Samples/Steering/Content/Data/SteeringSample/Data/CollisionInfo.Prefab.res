<root dataType="Struct" type="Duality.Resources.Prefab" id="129723834">
  <assetInfo />
  <objTree dataType="Struct" type="Duality.GameObject" id="422295141">
    <active dataType="Bool">true</active>
    <children />
    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1718889222">
      <_items dataType="Array" type="Duality.Component[]" id="2630619520" length="4">
        <item dataType="Struct" type="Duality.Components.Transform" id="2782610073">
          <active dataType="Bool">true</active>
          <angle dataType="Float">0</angle>
          <angleAbs dataType="Float">0</angleAbs>
          <angleVel dataType="Float">0</angleVel>
          <angleVelAbs dataType="Float">0</angleVelAbs>
          <deriveAngle dataType="Bool">true</deriveAngle>
          <gameobj dataType="ObjectRef">422295141</gameobj>
          <ignoreParent dataType="Bool">false</ignoreParent>
          <parentTransform />
          <pos dataType="Struct" type="Duality.Vector3">
            <X dataType="Float">0</X>
            <Y dataType="Float">0</Y>
            <Z dataType="Float">0.01</Z>
          </pos>
          <posAbs dataType="Struct" type="Duality.Vector3">
            <X dataType="Float">0</X>
            <Y dataType="Float">0</Y>
            <Z dataType="Float">0.01</Z>
          </posAbs>
          <scale dataType="Float">1</scale>
          <scaleAbs dataType="Float">1</scaleAbs>
          <vel dataType="Struct" type="Duality.Vector3" />
          <velAbs dataType="Struct" type="Duality.Vector3" />
        </item>
        <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="2164923963">
          <active dataType="Bool">true</active>
          <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
          <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
            <A dataType="Byte">64</A>
            <B dataType="Byte">255</B>
            <G dataType="Byte">255</G>
            <R dataType="Byte">255</R>
          </colorTint>
          <customMat />
          <gameobj dataType="ObjectRef">422295141</gameobj>
          <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
          <offset dataType="Float">0</offset>
          <text dataType="Struct" type="Duality.Drawing.FormattedText" id="3727897359">
            <flowAreas />
            <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="216848302">
              <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                <contentPath dataType="String">Data\SteeringSample\Data\InfoTextFont.Font.res</contentPath>
              </item>
            </fonts>
            <icons />
            <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
            <maxHeight dataType="Int">0</maxHeight>
            <maxWidth dataType="Int">300</maxWidth>
            <sourceText dataType="String">The RigidBodies used in this sample do not collide. All interaction is based solely on Agent decisions.</sourceText>
            <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
          </text>
          <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
        </item>
      </_items>
      <_size dataType="Int">2</_size>
    </compList>
    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1234171194" surrogate="true">
      <header />
      <body>
        <keys dataType="Array" type="System.Object[]" id="2888100468">
          <item dataType="Type" id="1185136548" value="Duality.Components.Transform" />
          <item dataType="Type" id="3546349334" value="Duality.Components.Renderers.TextRenderer" />
        </keys>
        <values dataType="Array" type="System.Object[]" id="4165465590">
          <item dataType="ObjectRef">2782610073</item>
          <item dataType="ObjectRef">2164923963</item>
        </values>
      </body>
    </compMap>
    <compTransform dataType="ObjectRef">2782610073</compTransform>
    <identifier dataType="Struct" type="System.Guid" surrogate="true">
      <header>
        <data dataType="Array" type="System.Byte[]" id="4216685648">T/WGba0m2kO8AlIjlh6z8A==</data>
      </header>
      <body />
    </identifier>
    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
    <name dataType="String">CollisionInfo</name>
    <parent />
    <prefabLink />
  </objTree>
</root>
<!-- XmlFormatterBase Document Separator -->
