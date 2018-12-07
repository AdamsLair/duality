<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">33</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="3959643605">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3686105255">
        <_items dataType="Array" type="Duality.Component[]" id="2326212558" length="8">
          <item dataType="Struct" type="Duality.Components.Transform" id="2024991241">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3959643605</gameobj>
          </item>
          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2727452833">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3959643605</gameobj>
          </item>
          <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="263369499">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3959643605</gameobj>
          </item>
          <item dataType="Struct" type="Steering.Agent" id="328169131">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3959643605</gameobj>
          </item>
          <item dataType="Struct" type="Steering.AgentAttributeTranslator" id="1986470889">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3959643605</gameobj>
          </item>
        </_items>
        <_size dataType="Int">5</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="617595904" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2905685645">
            <item dataType="Type" id="13978918" value="Duality.Components.Transform" />
            <item dataType="Type" id="439788218" value="Duality.Components.Physics.RigidBody" />
            <item dataType="Type" id="2771621414" value="Duality.Components.Renderers.RigidBodyRenderer" />
            <item dataType="Type" id="2144167866" value="Steering.Agent" />
            <item dataType="Type" id="2730200870" value="Steering.AgentAttributeTranslator" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="225485752">
            <item dataType="ObjectRef">2024991241</item>
            <item dataType="ObjectRef">2727452833</item>
            <item dataType="ObjectRef">263369499</item>
            <item dataType="ObjectRef">328169131</item>
            <item dataType="ObjectRef">1986470889</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">2024991241</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3225074023">pRU1ZTaZSEimHRA/5v51OA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Agent</name>
      <parent />
      <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="1205944037">
        <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="4176347348">
          <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="4083202788" length="8">
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="4066257096">
                <_items dataType="Array" type="System.Int32[]" id="3998382700"></_items>
                <_size dataType="Int">0</_size>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop dataType="MemberInfo" id="771728094" value="P:Steering.Agent:Target" />
              <val dataType="Struct" type="Steering.PointTarget" id="2243493172">
                <location dataType="Struct" type="Duality.Vector2">
                  <X dataType="Float">200</X>
                  <Y dataType="Float">0</Y>
                </location>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3301130018">
                <_items dataType="ObjectRef">3998382700</_items>
                <_size dataType="Int">0</_size>
              </childIndex>
              <componentType dataType="ObjectRef">13978918</componentType>
              <prop dataType="MemberInfo" id="111936128" value="P:Duality.Components.Transform:LocalPos" />
              <val dataType="Struct" type="Duality.Vector3">
                <X dataType="Float">-200</X>
                <Y dataType="Float">0</Y>
                <Z dataType="Float">0</Z>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1158369158">
                <_items dataType="Array" type="System.Int32[]" id="993453730"></_items>
                <_size dataType="Int">0</_size>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop dataType="MemberInfo" id="2206450092" value="P:Steering.Agent:Characteristics" />
              <val dataType="Struct" type="Steering.DefaultAgentCharacteristics" id="1960959242">
                <aggressiveness dataType="Float">0.5</aggressiveness>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1840298872">
                <_items dataType="Array" type="System.Int32[]" id="950995964"></_items>
                <_size dataType="Int">0</_size>
              </childIndex>
              <componentType dataType="ObjectRef">2771621414</componentType>
              <prop dataType="MemberInfo" id="1291869230" value="P:Duality.Components.Renderers.RigidBodyRenderer:ColorTint" />
              <val dataType="Struct" type="Duality.Drawing.ColorRgba">
                <A dataType="Byte">255</A>
                <B dataType="Byte">224</B>
                <G dataType="Byte">128</G>
                <R dataType="Byte">255</R>
              </val>
            </item>
          </_items>
          <_size dataType="Int">4</_size>
        </changes>
        <obj dataType="ObjectRef">3959643605</obj>
        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
          <contentPath dataType="String">Data\SteeringSample\Data\DummyAgent.Prefab.res</contentPath>
        </prefab>
      </prefabLink>
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="1396203244">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="15337378">
        <_items dataType="Array" type="Duality.Component[]" id="1398273296" length="8">
          <item dataType="Struct" type="Duality.Components.Transform" id="3756518176">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1396203244</gameobj>
          </item>
          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="164012472">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1396203244</gameobj>
          </item>
          <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="1994896434">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1396203244</gameobj>
          </item>
          <item dataType="Struct" type="Steering.Agent" id="2059696066">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1396203244</gameobj>
          </item>
          <item dataType="Struct" type="Steering.AgentAttributeTranslator" id="3717997824">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1396203244</gameobj>
          </item>
        </_items>
        <_size dataType="Int">5</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2124598538" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2173735480">
            <item dataType="ObjectRef">13978918</item>
            <item dataType="ObjectRef">439788218</item>
            <item dataType="ObjectRef">2771621414</item>
            <item dataType="ObjectRef">2144167866</item>
            <item dataType="ObjectRef">2730200870</item>
          </keys>
          <values dataType="Array" type="System.Object[]" id="1714155230">
            <item dataType="ObjectRef">3756518176</item>
            <item dataType="ObjectRef">164012472</item>
            <item dataType="ObjectRef">1994896434</item>
            <item dataType="ObjectRef">2059696066</item>
            <item dataType="ObjectRef">3717997824</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">3756518176</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1671743588">2xsrKxOv1kWYfNciDMZp4Q==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Agent</name>
      <parent />
      <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="2361657938">
        <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="4171603232">
          <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="3570681820" length="8">
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="372639176">
                <_items dataType="Array" type="System.Int32[]" id="1626048108"></_items>
                <_size dataType="Int">0</_size>
              </childIndex>
              <componentType dataType="ObjectRef">13978918</componentType>
              <prop dataType="ObjectRef">111936128</prop>
              <val dataType="Struct" type="Duality.Vector3">
                <X dataType="Float">200</X>
                <Y dataType="Float">0</Y>
                <Z dataType="Float">0</Z>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="2208595678">
                <_items dataType="Array" type="System.Int32[]" id="2131038858"></_items>
                <_size dataType="Int">0</_size>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop dataType="ObjectRef">771728094</prop>
              <val dataType="Struct" type="Steering.PointTarget" id="414026804">
                <location dataType="Struct" type="Duality.Vector2">
                  <X dataType="Float">-200</X>
                  <Y dataType="Float">0</Y>
                </location>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1644117794">
                <_items dataType="ObjectRef">993453730</_items>
                <_size dataType="Int">0</_size>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop dataType="ObjectRef">2206450092</prop>
              <val dataType="Struct" type="Steering.DefaultAgentCharacteristics" id="1663811456">
                <aggressiveness dataType="Float">0.5</aggressiveness>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="2084416390">
                <_items dataType="ObjectRef">950995964</_items>
                <_size dataType="Int">0</_size>
              </childIndex>
              <componentType dataType="ObjectRef">2771621414</componentType>
              <prop dataType="ObjectRef">1291869230</prop>
              <val dataType="Struct" type="Duality.Drawing.ColorRgba">
                <A dataType="Byte">255</A>
                <B dataType="Byte">159</B>
                <G dataType="Byte">255</G>
                <R dataType="Byte">128</R>
              </val>
            </item>
          </_items>
          <_size dataType="Int">4</_size>
        </changes>
        <obj dataType="ObjectRef">1396203244</obj>
        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
          <contentPath dataType="String">Data\SteeringSample\Data\DummyAgent.Prefab.res</contentPath>
        </prefab>
      </prefabLink>
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="4183369267">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3838536705">
        <_items dataType="Array" type="Duality.Component[]" id="79972142" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="2248716903">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">4183369267</gameobj>
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
          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="1631030793">
            <active dataType="Bool">true</active>
            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">64</A>
              <B dataType="Byte">255</B>
              <G dataType="Byte">255</G>
              <R dataType="Byte">255</R>
            </colorTint>
            <customMat />
            <gameobj dataType="ObjectRef">4183369267</gameobj>
            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
            <offset dataType="Float">0</offset>
            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="360764537">
              <flowAreas />
              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="3760060238">
                <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                  <contentPath dataType="String">Default:Font:GenericMonospace10</contentPath>
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
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3967565152" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="735578059">
            <item dataType="ObjectRef">13978918</item>
            <item dataType="Type" id="1574506998" value="Duality.Components.Renderers.TextRenderer" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="1554012744">
            <item dataType="ObjectRef">2248716903</item>
            <item dataType="ObjectRef">1631030793</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">2248716903</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="815015553">I2y59OBrqkq7mmZM38aTug==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">CollisionInfo</name>
      <parent />
      <prefabLink />
    </item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
