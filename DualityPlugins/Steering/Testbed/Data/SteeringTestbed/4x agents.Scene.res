<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <globalGravity dataType="Struct" type="OpenTK.Vector2">
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
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2727452833">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3959643605</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Components.Diagnostics.RigidBodyRenderer" id="4116868307">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3959643605</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Plugins.Steering.Agent" id="746706921">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3959643605</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Plugins.Steering.Testbed.AgentAttributeTranslator" id="1725612516">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3959643605</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
        </_items>
        <_size dataType="Int">5</_size>
        <_version dataType="Int">9</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="617595904" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="2905685645">
            <item dataType="Type" id="13978918" value="Duality.Components.Transform" />
            <item dataType="Type" id="439788218" value="Duality.Components.Physics.RigidBody" />
            <item dataType="Type" id="2771621414" value="Duality.Components.Diagnostics.RigidBodyRenderer" />
            <item dataType="Type" id="2144167866" value="Duality.Plugins.Steering.Agent" />
            <item dataType="Type" id="2730200870" value="Duality.Plugins.Steering.Testbed.AgentAttributeTranslator" />
          </keys>
          <values dataType="Array" type="Duality.Component[]" id="225485752">
            <item dataType="ObjectRef">2024991241</item>
            <item dataType="ObjectRef">2727452833</item>
            <item dataType="ObjectRef">4116868307</item>
            <item dataType="ObjectRef">746706921</item>
            <item dataType="ObjectRef">1725612516</item>
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
                <_items dataType="Array" type="System.Int32[]" id="3998382700" />
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">13978918</componentType>
              <prop dataType="PropertyInfo" id="771728094" value="P:Duality.Components.Transform:RelativePos" />
              <val dataType="Struct" type="OpenTK.Vector3">
                <X dataType="Float">-200</X>
                <Y dataType="Float">0</Y>
                <Z dataType="Float">0</Z>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="2243493172">
                <_items dataType="Array" type="System.Int32[]" id="1315571016" />
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop dataType="PropertyInfo" id="3301130018" value="P:Duality.Plugins.Steering.Agent:Target" />
              <val dataType="Struct" type="Duality.Plugins.Steering.PointTarget" id="111936128">
                <location dataType="Struct" type="OpenTK.Vector2">
                  <X dataType="Float">200</X>
                  <Y dataType="Float">0</Y>
                </location>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1158369158">
                <_items dataType="Array" type="System.Int32[]" id="993453730" />
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop />
              <val />
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="2206450092">
                <_items dataType="Array" type="System.Int32[]" id="1204036880" />
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop dataType="PropertyInfo" id="1960959242" value="P:Duality.Plugins.Steering.Agent:Characteristics" />
              <val dataType="Struct" type="Duality.Plugins.Steering.DefaultAgentCharacteristics" id="1840298872">
                <aggressiveness dataType="Float">0.5</aggressiveness>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1291869230">
                <_items dataType="Array" type="System.Int32[]" id="3522568314" />
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2771621414</componentType>
              <prop dataType="PropertyInfo" id="2336586852" value="P:Duality.Components.Diagnostics.RigidBodyRenderer:ColorTint" />
              <val dataType="Struct" type="Duality.Drawing.ColorRgba">
                <A dataType="Byte">255</A>
                <B dataType="Byte">224</B>
                <G dataType="Byte">128</G>
                <R dataType="Byte">255</R>
              </val>
            </item>
          </_items>
          <_size dataType="Int">5</_size>
          <_version dataType="Int">74</_version>
        </changes>
        <obj dataType="ObjectRef">3959643605</obj>
        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
          <contentPath dataType="String">Data\SteeringTestbed\Data\DummyAgent.Prefab.res</contentPath>
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
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="164012472">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1396203244</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Components.Diagnostics.RigidBodyRenderer" id="1553427946">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1396203244</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Plugins.Steering.Agent" id="2478233856">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1396203244</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Plugins.Steering.Testbed.AgentAttributeTranslator" id="3457139451">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1396203244</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
        </_items>
        <_size dataType="Int">5</_size>
        <_version dataType="Int">5</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2124598538" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="2173735480">
            <item dataType="ObjectRef">13978918</item>
            <item dataType="ObjectRef">439788218</item>
            <item dataType="ObjectRef">2771621414</item>
            <item dataType="ObjectRef">2144167866</item>
            <item dataType="ObjectRef">2730200870</item>
          </keys>
          <values dataType="Array" type="Duality.Component[]" id="1714155230">
            <item dataType="ObjectRef">3756518176</item>
            <item dataType="ObjectRef">164012472</item>
            <item dataType="ObjectRef">1553427946</item>
            <item dataType="ObjectRef">2478233856</item>
            <item dataType="ObjectRef">3457139451</item>
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
                <_items dataType="ObjectRef">3998382700</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">13978918</componentType>
              <prop dataType="ObjectRef">771728094</prop>
              <val dataType="Struct" type="OpenTK.Vector3">
                <X dataType="Float">200</X>
                <Y dataType="Float">0</Y>
                <Z dataType="Float">0</Z>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="2208595678">
                <_items dataType="ObjectRef">1315571016</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop dataType="ObjectRef">3301130018</prop>
              <val dataType="Struct" type="Duality.Plugins.Steering.PointTarget" id="414026804">
                <location dataType="Struct" type="OpenTK.Vector2">
                  <X dataType="Float">-200</X>
                  <Y dataType="Float">0</Y>
                </location>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1644117794">
                <_items dataType="Array" type="System.Int32[]" id="2314924366" />
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop />
              <val />
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1663811456">
                <_items dataType="ObjectRef">1204036880</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop dataType="ObjectRef">1960959242</prop>
              <val dataType="Struct" type="Duality.Plugins.Steering.DefaultAgentCharacteristics" id="2084416390">
                <aggressiveness dataType="Float">0.5</aggressiveness>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3874015916">
                <_items dataType="ObjectRef">3522568314</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2771621414</componentType>
              <prop dataType="ObjectRef">2336586852</prop>
              <val dataType="Struct" type="Duality.Drawing.ColorRgba">
                <A dataType="Byte">255</A>
                <B dataType="Byte">159</B>
                <G dataType="Byte">255</G>
                <R dataType="Byte">128</R>
              </val>
            </item>
          </_items>
          <_size dataType="Int">5</_size>
          <_version dataType="Int">415</_version>
        </changes>
        <obj dataType="ObjectRef">1396203244</obj>
        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
          <contentPath dataType="String">Data\SteeringTestbed\Data\DummyAgent.Prefab.res</contentPath>
        </prefab>
      </prefabLink>
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="2587165737">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1533032619">
        <_items dataType="Array" type="Duality.Component[]" id="93165814" length="8">
          <item dataType="Struct" type="Duality.Components.Transform" id="652513373">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">2587165737</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1354974965">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">2587165737</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Components.Diagnostics.RigidBodyRenderer" id="2744390439">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">2587165737</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Plugins.Steering.Agent" id="3669196349">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">2587165737</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Plugins.Steering.Testbed.AgentAttributeTranslator" id="353134648">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">2587165737</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
        </_items>
        <_size dataType="Int">5</_size>
        <_version dataType="Int">5</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="452958536" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="3878695809">
            <item dataType="ObjectRef">13978918</item>
            <item dataType="ObjectRef">439788218</item>
            <item dataType="ObjectRef">2771621414</item>
            <item dataType="ObjectRef">2144167866</item>
            <item dataType="ObjectRef">2730200870</item>
          </keys>
          <values dataType="Array" type="Duality.Component[]" id="3392997728">
            <item dataType="ObjectRef">652513373</item>
            <item dataType="ObjectRef">1354974965</item>
            <item dataType="ObjectRef">2744390439</item>
            <item dataType="ObjectRef">3669196349</item>
            <item dataType="ObjectRef">353134648</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">652513373</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2828166355">TUZU+WEBbUKUtmKSehY1CQ==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Agent</name>
      <parent />
      <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="3636600481">
        <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="331225604">
          <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="611986244" length="8">
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="2820327496">
                <_items dataType="Array" type="System.Int32[]" id="2452229228" />
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">13978918</componentType>
              <prop dataType="ObjectRef">771728094</prop>
              <val dataType="Struct" type="OpenTK.Vector3">
                <X dataType="Float">0</X>
                <Y dataType="Float">200</Y>
                <Z dataType="Float">0</Z>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="2670287070">
                <_items dataType="ObjectRef">2452229228</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop dataType="ObjectRef">3301130018</prop>
              <val dataType="Struct" type="Duality.Plugins.Steering.PointTarget" id="2744184500">
                <location dataType="Struct" type="OpenTK.Vector2">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">-200</Y>
                </location>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="4145318946">
                <_items dataType="ObjectRef">2314924366</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop />
              <val />
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="4098598400">
                <_items dataType="ObjectRef">1204036880</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop dataType="ObjectRef">1960959242</prop>
              <val dataType="Struct" type="Duality.Plugins.Steering.DefaultAgentCharacteristics" id="1708567942">
                <aggressiveness dataType="Float">0.5</aggressiveness>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1130797868">
                <_items dataType="ObjectRef">3522568314</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2771621414</componentType>
              <prop dataType="ObjectRef">2336586852</prop>
              <val dataType="Struct" type="Duality.Drawing.ColorRgba">
                <A dataType="Byte">255</A>
                <B dataType="Byte">255</B>
                <G dataType="Byte">159</G>
                <R dataType="Byte">128</R>
              </val>
            </item>
          </_items>
          <_size dataType="Int">5</_size>
          <_version dataType="Int">421</_version>
        </changes>
        <obj dataType="ObjectRef">2587165737</obj>
        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
          <contentPath dataType="String">Data\SteeringTestbed\Data\DummyAgent.Prefab.res</contentPath>
        </prefab>
      </prefabLink>
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="354782310">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="96108016">
        <_items dataType="Array" type="Duality.Component[]" id="1950020412" length="8">
          <item dataType="Struct" type="Duality.Components.Transform" id="2715097242">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">354782310</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3417558834">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">354782310</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Components.Diagnostics.RigidBodyRenderer" id="512007012">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">354782310</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Plugins.Steering.Agent" id="1436812922">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">354782310</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Plugins.Steering.Testbed.AgentAttributeTranslator" id="2415718517">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">354782310</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
        </_items>
        <_size dataType="Int">5</_size>
        <_version dataType="Int">5</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2875611374" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="1685867330">
            <item dataType="ObjectRef">13978918</item>
            <item dataType="ObjectRef">439788218</item>
            <item dataType="ObjectRef">2771621414</item>
            <item dataType="ObjectRef">2144167866</item>
            <item dataType="ObjectRef">2730200870</item>
          </keys>
          <values dataType="Array" type="Duality.Component[]" id="1232187402">
            <item dataType="ObjectRef">2715097242</item>
            <item dataType="ObjectRef">3417558834</item>
            <item dataType="ObjectRef">512007012</item>
            <item dataType="ObjectRef">1436812922</item>
            <item dataType="ObjectRef">2415718517</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">2715097242</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1841138994">Tsk1UxBOs0WmXAg8eW12mw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Agent</name>
      <parent />
      <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="1724675404">
        <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="3095088440">
          <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="3242176108" length="8">
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3220388264">
                <_items dataType="ObjectRef">2452229228</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">13978918</componentType>
              <prop dataType="ObjectRef">771728094</prop>
              <val dataType="Struct" type="OpenTK.Vector3">
                <X dataType="Float">0</X>
                <Y dataType="Float">-200</Y>
                <Z dataType="Float">0</Z>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="2991546270">
                <_items dataType="ObjectRef">2452229228</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop dataType="ObjectRef">3301130018</prop>
              <val dataType="Struct" type="Duality.Plugins.Steering.PointTarget" id="891667348">
                <location dataType="Struct" type="OpenTK.Vector2">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">200</Y>
                </location>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1511702050">
                <_items dataType="ObjectRef">2314924366</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop />
              <val />
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1180595040">
                <_items dataType="ObjectRef">1204036880</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop dataType="ObjectRef">1960959242</prop>
              <val dataType="Struct" type="Duality.Plugins.Steering.DefaultAgentCharacteristics" id="833600582">
                <aggressiveness dataType="Float">0.5</aggressiveness>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1982629388">
                <_items dataType="ObjectRef">3522568314</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2771621414</componentType>
              <prop dataType="ObjectRef">2336586852</prop>
              <val dataType="Struct" type="Duality.Drawing.ColorRgba">
                <A dataType="Byte">255</A>
                <B dataType="Byte">128</B>
                <G dataType="Byte">224</G>
                <R dataType="Byte">255</R>
              </val>
            </item>
          </_items>
          <_size dataType="Int">5</_size>
          <_version dataType="Int">55</_version>
        </changes>
        <obj dataType="ObjectRef">354782310</obj>
        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
          <contentPath dataType="String">Data\SteeringTestbed\Data\DummyAgent.Prefab.res</contentPath>
        </prefab>
      </prefabLink>
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="2299002789">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="117206487">
        <_items dataType="Array" type="Duality.Component[]" id="558813710" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="364350425">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">2299002789</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <parentTransform />
            <pos dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">0.01</Z>
            </pos>
            <posAbs dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">0.01</Z>
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
          <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="4041631611">
            <active dataType="Bool">true</active>
            <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">64</A>
              <B dataType="Byte">255</B>
              <G dataType="Byte">255</G>
              <R dataType="Byte">255</R>
            </colorTint>
            <customMat />
            <gameobj dataType="ObjectRef">2299002789</gameobj>
            <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
              <contentPath />
            </iconMat>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <text dataType="Struct" type="Duality.Drawing.FormattedText" id="719435995">
              <displayedText dataType="String">The RigidBodies used in this sample do not collide. All interaction is based solely on Agent decisions.</displayedText>
              <elements dataType="Array" type="Duality.Drawing.FormattedText+Element[]" id="2698066582">
                <item dataType="Struct" type="Duality.Drawing.FormattedText+TextElement" id="811176480">
                  <text dataType="String">The RigidBodies used in this sample do not collide. All interaction is based solely on Agent decisions.</text>
                </item>
              </elements>
              <flowAreas />
              <fontGlyphCount dataType="Array" type="System.Int32[]" id="3140523226">
                <item dataType="Int">103</item>
              </fontGlyphCount>
              <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="2531402550">
                <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                  <contentPath dataType="String">Data\SteeringTestbed\Data\InfoTextFont.Font.res</contentPath>
                </item>
              </fonts>
              <iconCount dataType="Int">0</iconCount>
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
        <_version dataType="Int">2</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="112692160" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="249792605">
            <item dataType="ObjectRef">13978918</item>
            <item dataType="Type" id="2236797798" value="Duality.Components.Renderers.TextRenderer" />
          </keys>
          <values dataType="Array" type="Duality.Component[]" id="2932767608">
            <item dataType="ObjectRef">364350425</item>
            <item dataType="ObjectRef">4041631611</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">364350425</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1432941559">uLXyzPWOJUquQctF4KyDOA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">CollisionInfo</name>
      <parent />
      <prefabLink />
    </item>
  </serializeObj>
  <sourcePath />
</root>
<!-- XmlFormatterBase Document Separator -->
