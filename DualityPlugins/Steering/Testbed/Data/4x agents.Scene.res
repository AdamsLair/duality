<?xml version="1.0" encoding="utf-8"?>
<root>
  <object dataType="Class" type="Duality.Resources.Scene">
    <globalGravity dataType="Struct" type="OpenTK.Vector2">
      <X dataType="Float">0</X>
      <Y dataType="Float">33</Y>
    </globalGravity>
    <serializeObj dataType="Array" type="Duality.GameObject[]" id="292984781" length="4">
      <object dataType="Class" type="Duality.GameObject" id="3959643605">
        <active dataType="Bool">true</active>
        <children />
        <compList dataType="Class" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2377033339">
          <_items dataType="Array" type="Duality.Component[]" id="58262226" length="8">
            <object dataType="Class" type="Duality.Components.Transform" id="2024991241">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">3959643605</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object dataType="Class" type="Duality.Components.Physics.RigidBody" id="2727452833">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">3959643605</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object dataType="Class" type="Duality.Components.Diagnostics.RigidBodyRenderer" id="4116868307">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">3959643605</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object dataType="Class" type="Duality.Plugins.Steering.Agent" id="746706921">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">3959643605</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object dataType="Class" type="Duality.Plugins.Steering.Testbed.AgentAttributeTranslator" id="1725612516">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">3959643605</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object />
            <object />
            <object />
          </_items>
          <_size dataType="Int">5</_size>
          <_version dataType="Int">9</_version>
        </compList>
        <compMap dataType="Class" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1895595302" surrogate="true">
          <header />
          <body>
            <keys dataType="Array" type="System.Type[]" id="4240341495" length="5">
              <object dataType="Type" id="1914743562" value="Duality.Components.Transform" />
              <object dataType="Type" id="1696569932" value="Duality.Components.Physics.RigidBody" />
              <object dataType="Type" id="2224275174" value="Duality.Components.Diagnostics.RigidBodyRenderer" />
              <object dataType="Type" id="4190115896" value="Duality.Plugins.Steering.Agent" />
              <object dataType="Type" id="1943294466" value="Duality.Plugins.Steering.Testbed.AgentAttributeTranslator" />
            </keys>
            <values dataType="Array" type="Duality.Component[]" id="323893006" length="5">
              <object dataType="ObjectRef">2024991241</object>
              <object dataType="ObjectRef">2727452833</object>
              <object dataType="ObjectRef">4116868307</object>
              <object dataType="ObjectRef">746706921</object>
              <object dataType="ObjectRef">1725612516</object>
            </values>
          </body>
        </compMap>
        <compTransform dataType="ObjectRef">2024991241</compTransform>
        <identifier dataType="Struct" type="System.Guid" surrogate="true">
          <header>
            <data dataType="Array" type="System.Byte[]" id="2859681593" length="16">pRU1ZTaZSEimHRA/5v51OA==</data>
          </header>
          <body />
        </identifier>
        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
        <name dataType="String">Agent</name>
        <parent />
        <prefabLink dataType="Class" type="Duality.Resources.PrefabLink" id="356349069">
          <changes dataType="Class" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="3873413140">
            <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="1315188552" length="4">
              <object dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                <childIndex dataType="Class" type="System.Collections.Generic.List`1[[System.Int32]]" id="4270174704">
                  <_items dataType="Array" type="System.Int32[]" id="20813472" length="0" />
                  <_size dataType="Int">0</_size>
                  <_version dataType="Int">1</_version>
                </childIndex>
                <componentType dataType="ObjectRef">1914743562</componentType>
                <prop dataType="PropertyInfo" id="97301216" value="P:Duality.Components.Transform:RelativePos" />
                <val dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">-200</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </val>
              </object>
              <object dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                <childIndex dataType="Class" type="System.Collections.Generic.List`1[[System.Int32]]" id="286482192">
                  <_items dataType="Array" type="System.Int32[]" id="398980416" length="0" />
                  <_size dataType="Int">0</_size>
                  <_version dataType="Int">1</_version>
                </childIndex>
                <componentType dataType="ObjectRef">4190115896</componentType>
                <prop dataType="PropertyInfo" id="909540800" value="P:Duality.Plugins.Steering.Agent:Target" />
                <val dataType="Class" type="Duality.Plugins.Steering.PointTarget" id="851604912">
                  <location dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">200</X>
                    <Y dataType="Float">0</Y>
                  </location>
                </val>
              </object>
              <object dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                <childIndex dataType="Class" type="System.Collections.Generic.List`1[[System.Int32]]" id="2238417824">
                  <_items dataType="Array" type="System.Int32[]" id="1163071856" length="0" />
                  <_size dataType="Int">0</_size>
                  <_version dataType="Int">1</_version>
                </childIndex>
                <componentType dataType="ObjectRef">4190115896</componentType>
                <prop />
                <val />
              </object>
              <object dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                <childIndex dataType="Class" type="System.Collections.Generic.List`1[[System.Int32]]" id="3660402384">
                  <_items dataType="Array" type="System.Int32[]" id="3598235264" length="0" />
                  <_size dataType="Int">0</_size>
                  <_version dataType="Int">1</_version>
                </childIndex>
                <componentType dataType="ObjectRef">4190115896</componentType>
                <prop dataType="PropertyInfo" id="2710601600" value="P:Duality.Plugins.Steering.Agent:Characteristics" />
                <val dataType="Class" type="Duality.Plugins.Steering.DefaultAgentCharacteristics" id="3776446576">
                  <aggressiveness dataType="Float">0.5</aggressiveness>
                </val>
              </object>
            </_items>
            <_size dataType="Int">4</_size>
            <_version dataType="Int">73</_version>
          </changes>
          <obj dataType="ObjectRef">3959643605</obj>
          <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
            <contentPath dataType="String">Data\DummyAgent.Prefab.res</contentPath>
          </prefab>
        </prefabLink>
      </object>
      <object dataType="Class" type="Duality.GameObject" id="1396203244">
        <active dataType="Bool">true</active>
        <children />
        <compList dataType="Class" type="System.Collections.Generic.List`1[[Duality.Component]]" id="28335114">
          <_items dataType="Array" type="Duality.Component[]" id="560027556" length="8">
            <object dataType="Class" type="Duality.Components.Transform" id="3756518176">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">1396203244</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object dataType="Class" type="Duality.Components.Physics.RigidBody" id="164012472">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">1396203244</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object dataType="Class" type="Duality.Components.Diagnostics.RigidBodyRenderer" id="1553427946">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">1396203244</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object dataType="Class" type="Duality.Plugins.Steering.Agent" id="2478233856">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">1396203244</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object dataType="Class" type="Duality.Plugins.Steering.Testbed.AgentAttributeTranslator" id="3457139451">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">1396203244</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object />
            <object />
            <object />
          </_items>
          <_size dataType="Int">5</_size>
          <_version dataType="Int">5</_version>
        </compList>
        <compMap dataType="Class" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1831313484" surrogate="true">
          <header />
          <body>
            <keys dataType="Array" type="System.Type[]" id="2187600146" length="5">
              <object dataType="ObjectRef">1914743562</object>
              <object dataType="ObjectRef">1696569932</object>
              <object dataType="ObjectRef">2224275174</object>
              <object dataType="ObjectRef">4190115896</object>
              <object dataType="ObjectRef">1943294466</object>
            </keys>
            <values dataType="Array" type="Duality.Component[]" id="2780890140" length="5">
              <object dataType="ObjectRef">3756518176</object>
              <object dataType="ObjectRef">164012472</object>
              <object dataType="ObjectRef">1553427946</object>
              <object dataType="ObjectRef">2478233856</object>
              <object dataType="ObjectRef">3457139451</object>
            </values>
          </body>
        </compMap>
        <compTransform dataType="ObjectRef">3756518176</compTransform>
        <identifier dataType="Struct" type="System.Guid" surrogate="true">
          <header>
            <data dataType="Array" type="System.Byte[]" id="1672541326" length="16">2xsrKxOv1kWYfNciDMZp4Q==</data>
          </header>
          <body />
        </identifier>
        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
        <name dataType="String">Agent</name>
        <parent />
        <prefabLink dataType="Class" type="Duality.Resources.PrefabLink" id="1818283494">
          <changes dataType="Class" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="2255402536">
            <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="4201210512" length="4">
              <object dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                <childIndex dataType="Class" type="System.Collections.Generic.List`1[[System.Int32]]" id="649982944">
                  <_items dataType="ObjectRef">20813472</_items>
                  <_size dataType="Int">0</_size>
                  <_version dataType="Int">1</_version>
                </childIndex>
                <componentType dataType="ObjectRef">1914743562</componentType>
                <prop dataType="ObjectRef">97301216</prop>
                <val dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">200</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </val>
              </object>
              <object dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                <childIndex dataType="Class" type="System.Collections.Generic.List`1[[System.Int32]]" id="103687616">
                  <_items dataType="ObjectRef">398980416</_items>
                  <_size dataType="Int">0</_size>
                  <_version dataType="Int">1</_version>
                </childIndex>
                <componentType dataType="ObjectRef">4190115896</componentType>
                <prop dataType="ObjectRef">909540800</prop>
                <val dataType="Class" type="Duality.Plugins.Steering.PointTarget" id="2642480672">
                  <location dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">-200</X>
                    <Y dataType="Float">0</Y>
                  </location>
                </val>
              </object>
              <object dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                <childIndex dataType="Class" type="System.Collections.Generic.List`1[[System.Int32]]" id="1624087424">
                  <_items dataType="Array" type="System.Int32[]" id="4282759200" length="0" />
                  <_size dataType="Int">0</_size>
                  <_version dataType="Int">1</_version>
                </childIndex>
                <componentType dataType="ObjectRef">4190115896</componentType>
                <prop />
                <val />
              </object>
              <object dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                <childIndex dataType="Class" type="System.Collections.Generic.List`1[[System.Int32]]" id="3885955936">
                  <_items dataType="ObjectRef">3598235264</_items>
                  <_size dataType="Int">0</_size>
                  <_version dataType="Int">1</_version>
                </childIndex>
                <componentType dataType="ObjectRef">4190115896</componentType>
                <prop dataType="ObjectRef">2710601600</prop>
                <val dataType="Class" type="Duality.Plugins.Steering.DefaultAgentCharacteristics" id="2718521152">
                  <aggressiveness dataType="Float">0.5</aggressiveness>
                </val>
              </object>
            </_items>
            <_size dataType="Int">4</_size>
            <_version dataType="Int">414</_version>
          </changes>
          <obj dataType="ObjectRef">1396203244</obj>
          <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
            <contentPath dataType="String">Data\DummyAgent.Prefab.res</contentPath>
          </prefab>
        </prefabLink>
      </object>
      <object dataType="Class" type="Duality.GameObject" id="2587165737">
        <active dataType="Bool">true</active>
        <children />
        <compList dataType="Class" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2523430999">
          <_items dataType="Array" type="Duality.Component[]" id="485776010" length="8">
            <object dataType="Class" type="Duality.Components.Transform" id="652513373">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">2587165737</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object dataType="Class" type="Duality.Components.Physics.RigidBody" id="1354974965">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">2587165737</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object dataType="Class" type="Duality.Components.Diagnostics.RigidBodyRenderer" id="2744390439">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">2587165737</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object dataType="Class" type="Duality.Plugins.Steering.Agent" id="3669196349">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">2587165737</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object dataType="Class" type="Duality.Plugins.Steering.Testbed.AgentAttributeTranslator" id="353134648">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">2587165737</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object />
            <object />
            <object />
          </_items>
          <_size dataType="Int">5</_size>
          <_version dataType="Int">5</_version>
        </compList>
        <compMap dataType="Class" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="773290382" surrogate="true">
          <header />
          <body>
            <keys dataType="Array" type="System.Type[]" id="3908373851" length="5">
              <object dataType="ObjectRef">1914743562</object>
              <object dataType="ObjectRef">1696569932</object>
              <object dataType="ObjectRef">2224275174</object>
              <object dataType="ObjectRef">4190115896</object>
              <object dataType="ObjectRef">1943294466</object>
            </keys>
            <values dataType="Array" type="Duality.Component[]" id="2417673446" length="5">
              <object dataType="ObjectRef">652513373</object>
              <object dataType="ObjectRef">1354974965</object>
              <object dataType="ObjectRef">2744390439</object>
              <object dataType="ObjectRef">3669196349</object>
              <object dataType="ObjectRef">353134648</object>
            </values>
          </body>
        </compMap>
        <compTransform dataType="ObjectRef">652513373</compTransform>
        <identifier dataType="Struct" type="System.Guid" surrogate="true">
          <header>
            <data dataType="Array" type="System.Byte[]" id="4134765421" length="16">TUZU+WEBbUKUtmKSehY1CQ==</data>
          </header>
          <body />
        </identifier>
        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
        <name dataType="String">Agent</name>
        <parent />
        <prefabLink dataType="Class" type="Duality.Resources.PrefabLink" id="509559321">
          <changes dataType="Class" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="3764712404">
            <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="3155483080" length="4">
              <object dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                <childIndex dataType="Class" type="System.Collections.Generic.List`1[[System.Int32]]" id="3919545328">
                  <_items dataType="Array" type="System.Int32[]" id="912984736" length="0" />
                  <_size dataType="Int">0</_size>
                  <_version dataType="Int">1</_version>
                </childIndex>
                <componentType dataType="ObjectRef">1914743562</componentType>
                <prop dataType="ObjectRef">97301216</prop>
                <val dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">200</Y>
                  <Z dataType="Float">0</Z>
                </val>
              </object>
              <object dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                <childIndex dataType="Class" type="System.Collections.Generic.List`1[[System.Int32]]" id="4213305056">
                  <_items dataType="ObjectRef">912984736</_items>
                  <_size dataType="Int">0</_size>
                  <_version dataType="Int">1</_version>
                </childIndex>
                <componentType dataType="ObjectRef">4190115896</componentType>
                <prop dataType="ObjectRef">909540800</prop>
                <val dataType="Class" type="Duality.Plugins.Steering.PointTarget" id="3377182992">
                  <location dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">-200</Y>
                  </location>
                </val>
              </object>
              <object dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                <childIndex dataType="Class" type="System.Collections.Generic.List`1[[System.Int32]]" id="1079764416">
                  <_items dataType="ObjectRef">4282759200</_items>
                  <_size dataType="Int">0</_size>
                  <_version dataType="Int">1</_version>
                </childIndex>
                <componentType dataType="ObjectRef">4190115896</componentType>
                <prop />
                <val />
              </object>
              <object dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                <childIndex dataType="Class" type="System.Collections.Generic.List`1[[System.Int32]]" id="785491888">
                  <_items dataType="ObjectRef">3598235264</_items>
                  <_size dataType="Int">0</_size>
                  <_version dataType="Int">1</_version>
                </childIndex>
                <componentType dataType="ObjectRef">4190115896</componentType>
                <prop dataType="ObjectRef">2710601600</prop>
                <val dataType="Class" type="Duality.Plugins.Steering.DefaultAgentCharacteristics" id="3744219040">
                  <aggressiveness dataType="Float">0.5</aggressiveness>
                </val>
              </object>
            </_items>
            <_size dataType="Int">4</_size>
            <_version dataType="Int">420</_version>
          </changes>
          <obj dataType="ObjectRef">2587165737</obj>
          <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
            <contentPath dataType="String">Data\DummyAgent.Prefab.res</contentPath>
          </prefab>
        </prefabLink>
      </object>
      <object dataType="Class" type="Duality.GameObject" id="354782310">
        <active dataType="Bool">true</active>
        <children />
        <compList dataType="Class" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3971163760">
          <_items dataType="Array" type="Duality.Component[]" id="2926435488" length="8">
            <object dataType="Class" type="Duality.Components.Transform" id="2715097242">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">354782310</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object dataType="Class" type="Duality.Components.Physics.RigidBody" id="3417558834">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">354782310</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object dataType="Class" type="Duality.Components.Diagnostics.RigidBodyRenderer" id="512007012">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">354782310</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object dataType="Class" type="Duality.Plugins.Steering.Agent" id="1436812922">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">354782310</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object dataType="Class" type="Duality.Plugins.Steering.Testbed.AgentAttributeTranslator" id="2415718517">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">354782310</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object />
            <object />
            <object />
          </_items>
          <_size dataType="Int">5</_size>
          <_version dataType="Int">5</_version>
        </compList>
        <compMap dataType="Class" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1553929440" surrogate="true">
          <header />
          <body>
            <keys dataType="Array" type="System.Type[]" id="1626401968" length="5">
              <object dataType="ObjectRef">1914743562</object>
              <object dataType="ObjectRef">1696569932</object>
              <object dataType="ObjectRef">2224275174</object>
              <object dataType="ObjectRef">4190115896</object>
              <object dataType="ObjectRef">1943294466</object>
            </keys>
            <values dataType="Array" type="Duality.Component[]" id="409860192" length="5">
              <object dataType="ObjectRef">2715097242</object>
              <object dataType="ObjectRef">3417558834</object>
              <object dataType="ObjectRef">512007012</object>
              <object dataType="ObjectRef">1436812922</object>
              <object dataType="ObjectRef">2415718517</object>
            </values>
          </body>
        </compMap>
        <compTransform dataType="ObjectRef">2715097242</compTransform>
        <identifier dataType="Struct" type="System.Guid" surrogate="true">
          <header>
            <data dataType="Array" type="System.Byte[]" id="1374534608" length="16">Tsk1UxBOs0WmXAg8eW12mw==</data>
          </header>
          <body />
        </identifier>
        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
        <name dataType="String">Agent</name>
        <parent />
        <prefabLink dataType="Class" type="Duality.Resources.PrefabLink" id="1543376528">
          <changes dataType="Class" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="1131486528">
            <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="2755796096" length="4">
              <object dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                <childIndex dataType="Class" type="System.Collections.Generic.List`1[[System.Int32]]" id="1732984576">
                  <_items dataType="ObjectRef">912984736</_items>
                  <_size dataType="Int">0</_size>
                  <_version dataType="Int">1</_version>
                </childIndex>
                <componentType dataType="ObjectRef">1914743562</componentType>
                <prop dataType="ObjectRef">97301216</prop>
                <val dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">-200</Y>
                  <Z dataType="Float">0</Z>
                </val>
              </object>
              <object dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                <childIndex dataType="Class" type="System.Collections.Generic.List`1[[System.Int32]]" id="1680403968">
                  <_items dataType="ObjectRef">912984736</_items>
                  <_size dataType="Int">0</_size>
                  <_version dataType="Int">1</_version>
                </childIndex>
                <componentType dataType="ObjectRef">4190115896</componentType>
                <prop dataType="ObjectRef">909540800</prop>
                <val dataType="Class" type="Duality.Plugins.Steering.PointTarget" id="3122172160">
                  <location dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">200</Y>
                  </location>
                </val>
              </object>
              <object dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                <childIndex dataType="Class" type="System.Collections.Generic.List`1[[System.Int32]]" id="113531904">
                  <_items dataType="ObjectRef">4282759200</_items>
                  <_size dataType="Int">0</_size>
                  <_version dataType="Int">1</_version>
                </childIndex>
                <componentType dataType="ObjectRef">4190115896</componentType>
                <prop />
                <val />
              </object>
              <object dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                <childIndex dataType="Class" type="System.Collections.Generic.List`1[[System.Int32]]" id="4175133440">
                  <_items dataType="ObjectRef">3598235264</_items>
                  <_size dataType="Int">0</_size>
                  <_version dataType="Int">1</_version>
                </childIndex>
                <componentType dataType="ObjectRef">4190115896</componentType>
                <prop dataType="ObjectRef">2710601600</prop>
                <val dataType="Class" type="Duality.Plugins.Steering.DefaultAgentCharacteristics" id="3427006976">
                  <aggressiveness dataType="Float">0.5</aggressiveness>
                </val>
              </object>
            </_items>
            <_size dataType="Int">4</_size>
            <_version dataType="Int">54</_version>
          </changes>
          <obj dataType="ObjectRef">354782310</obj>
          <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
            <contentPath dataType="String">Data\DummyAgent.Prefab.res</contentPath>
          </prefab>
        </prefabLink>
      </object>
    </serializeObj>
    <sourcePath />
  </object>
</root>