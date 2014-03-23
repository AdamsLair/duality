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
          <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="4083202788" length="4">
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="4066257096">
                <_items dataType="Array" type="System.Int32[]" id="3998382700" />
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">13978918</componentType>
              <prop dataType="PropertyInfo" id="771728094" value="P:Duality.Components.Transform:RelativePos" />
              <val dataType="Struct" type="OpenTK.Vector3">
                <X dataType="Float">200</X>
                <Y dataType="Float">180</Y>
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
                  <X dataType="Float">-400</X>
                  <Y dataType="Float">180</Y>
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
              <prop dataType="PropertyInfo" id="2206450092" value="P:Duality.Plugins.Steering.Agent:Characteristics" />
              <val dataType="Struct" type="Duality.Plugins.Steering.DefaultAgentCharacteristics" id="1960959242">
                <aggressiveness dataType="Float">0.5</aggressiveness>
              </val>
            </item>
          </_items>
          <_size dataType="Int">3</_size>
          <_version dataType="Int">108</_version>
        </changes>
        <obj dataType="ObjectRef">3959643605</obj>
        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
          <contentPath dataType="String">Data\DummyAgent.Prefab.res</contentPath>
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
          <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="3570681820" length="4">
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="372639176">
                <_items dataType="Array" type="System.Int32[]" id="1626048108" />
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
                  <X dataType="Float">-400</X>
                  <Y dataType="Float">0</Y>
                </location>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1644117794">
                <_items dataType="ObjectRef">993453730</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop dataType="ObjectRef">2206450092</prop>
              <val dataType="Struct" type="Duality.Plugins.Steering.DefaultAgentCharacteristics" id="1663811456">
                <aggressiveness dataType="Float">0.5</aggressiveness>
              </val>
            </item>
          </_items>
          <_size dataType="Int">3</_size>
          <_version dataType="Int">425</_version>
        </changes>
        <obj dataType="ObjectRef">1396203244</obj>
        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
          <contentPath dataType="String">Data\DummyAgent.Prefab.res</contentPath>
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
          <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="611986244" length="4">
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="2820327496">
                <_items dataType="ObjectRef">3998382700</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">13978918</componentType>
              <prop dataType="ObjectRef">771728094</prop>
              <val dataType="Struct" type="OpenTK.Vector3">
                <X dataType="Float">200</X>
                <Y dataType="Float">90</Y>
                <Z dataType="Float">0</Z>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="2670287070">
                <_items dataType="ObjectRef">1315571016</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop dataType="ObjectRef">3301130018</prop>
              <val dataType="Struct" type="Duality.Plugins.Steering.PointTarget" id="2744184500">
                <location dataType="Struct" type="OpenTK.Vector2">
                  <X dataType="Float">-400</X>
                  <Y dataType="Float">90</Y>
                </location>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="4145318946">
                <_items dataType="ObjectRef">993453730</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop dataType="ObjectRef">2206450092</prop>
              <val dataType="Struct" type="Duality.Plugins.Steering.DefaultAgentCharacteristics" id="4098598400">
                <aggressiveness dataType="Float">0.5</aggressiveness>
              </val>
            </item>
          </_items>
          <_size dataType="Int">3</_size>
          <_version dataType="Int">463</_version>
        </changes>
        <obj dataType="ObjectRef">2587165737</obj>
        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
          <contentPath dataType="String">Data\DummyAgent.Prefab.res</contentPath>
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
          <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="3242176108" length="4">
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3220388264">
                <_items dataType="ObjectRef">3998382700</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">13978918</componentType>
              <prop dataType="ObjectRef">771728094</prop>
              <val dataType="Struct" type="OpenTK.Vector3">
                <X dataType="Float">200</X>
                <Y dataType="Float">-90</Y>
                <Z dataType="Float">0</Z>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="2991546270">
                <_items dataType="ObjectRef">1315571016</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop dataType="ObjectRef">3301130018</prop>
              <val dataType="Struct" type="Duality.Plugins.Steering.PointTarget" id="891667348">
                <location dataType="Struct" type="OpenTK.Vector2">
                  <X dataType="Float">-400</X>
                  <Y dataType="Float">-90</Y>
                </location>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1511702050">
                <_items dataType="ObjectRef">993453730</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop dataType="ObjectRef">2206450092</prop>
              <val dataType="Struct" type="Duality.Plugins.Steering.DefaultAgentCharacteristics" id="1180595040">
                <aggressiveness dataType="Float">0.5</aggressiveness>
              </val>
            </item>
          </_items>
          <_size dataType="Int">3</_size>
          <_version dataType="Int">85</_version>
        </changes>
        <obj dataType="ObjectRef">354782310</obj>
        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
          <contentPath dataType="String">Data\DummyAgent.Prefab.res</contentPath>
        </prefab>
      </prefabLink>
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="1918743486">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3777061352">
        <_items dataType="Array" type="Duality.Component[]" id="1168833580" length="8">
          <item dataType="Struct" type="Duality.Components.Transform" id="4279058418">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1918743486</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="686552714">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1918743486</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Components.Diagnostics.RigidBodyRenderer" id="2075968188">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1918743486</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Plugins.Steering.Agent" id="3000774098">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1918743486</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Plugins.Steering.Testbed.AgentAttributeTranslator" id="3979679693">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1918743486</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
        </_items>
        <_size dataType="Int">5</_size>
        <_version dataType="Int">5</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1743336222" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="2481970858">
            <item dataType="ObjectRef">13978918</item>
            <item dataType="ObjectRef">439788218</item>
            <item dataType="ObjectRef">2771621414</item>
            <item dataType="ObjectRef">2144167866</item>
            <item dataType="ObjectRef">2730200870</item>
          </keys>
          <values dataType="Array" type="Duality.Component[]" id="658219226">
            <item dataType="ObjectRef">4279058418</item>
            <item dataType="ObjectRef">686552714</item>
            <item dataType="ObjectRef">2075968188</item>
            <item dataType="ObjectRef">3000774098</item>
            <item dataType="ObjectRef">3979679693</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">4279058418</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="154947210">VPA3Imp33UO9ByfwjWvDNw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Agent</name>
      <parent />
      <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="759403860">
        <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="1142795080">
          <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="4246659180" length="8">
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="4212997544">
                <_items dataType="ObjectRef">3998382700</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop dataType="ObjectRef">3301130018</prop>
              <val dataType="Struct" type="Duality.Plugins.Steering.PointTarget" id="1150891934">
                <location dataType="Struct" type="OpenTK.Vector2">
                  <X dataType="Float">200</X>
                  <Y dataType="Float">45</Y>
                </location>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="41213844">
                <_items dataType="Array" type="System.Int32[]" id="1176959048" />
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop dataType="PropertyInfo" id="13639202" value="P:Duality.Plugins.Steering.Agent:DebugVisualizationMode" />
              <val dataType="Enum" type="Duality.Plugins.Steering.Agent+VisualLoggingMode" name="None" value="0" />
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3147937632">
                <_items dataType="ObjectRef">993453730</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">2144167866</componentType>
              <prop dataType="ObjectRef">2206450092</prop>
              <val dataType="Struct" type="Duality.Plugins.Steering.DefaultAgentCharacteristics" id="2658337862">
                <aggressiveness dataType="Float">0.5</aggressiveness>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="2095813132">
                <_items dataType="Array" type="System.Int32[]" id="4202877712" />
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">13978918</componentType>
              <prop dataType="ObjectRef">771728094</prop>
              <val dataType="Struct" type="OpenTK.Vector3">
                <X dataType="Float">-400</X>
                <Y dataType="Float">45</Y>
                <Z dataType="Float">0</Z>
              </val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1555916554">
                <_items dataType="Array" type="System.Int32[]" id="703923846" />
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">13978918</componentType>
              <prop dataType="PropertyInfo" id="856024728" value="P:Duality.Components.Transform:RelativeScale" />
              <val dataType="Float">1</val>
            </item>
          </_items>
          <_size dataType="Int">5</_size>
          <_version dataType="Int">193</_version>
        </changes>
        <obj dataType="ObjectRef">1918743486</obj>
        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
          <contentPath dataType="String">Data\DummyAgent.Prefab.res</contentPath>
        </prefab>
      </prefabLink>
    </item>
  </serializeObj>
  <sourcePath />
</root>
<!-- XmlFormatterBase Document Separator -->
