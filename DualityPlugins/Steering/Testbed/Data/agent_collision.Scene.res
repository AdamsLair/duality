<?xml version="1.0" encoding="utf-8"?>
<root>
  <object dataType="Class" type="Duality.Resources.Scene">
    <globalGravity dataType="Struct" type="OpenTK.Vector2">
      <X dataType="Float">0</X>
      <Y dataType="Float">33</Y>
    </globalGravity>
    <serializeObj dataType="Array" type="Duality.GameObject[]" id="292984781" length="3">
      <object dataType="Class" type="Duality.GameObject" id="1015542799">
        <active dataType="Bool">true</active>
        <children />
        <compList dataType="Class" type="System.Collections.Generic.List`1[[Duality.Component]]" id="188368065">
          <_items dataType="Array" type="Duality.Component[]" id="104730282" length="4">
            <object dataType="Class" type="Duality.Plugins.Navigation.AgentManager" id="2091897961">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">1015542799</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object />
            <object />
            <object />
          </_items>
          <_size dataType="Int">1</_size>
          <_version dataType="Int">1</_version>
        </compList>
        <compMap dataType="Class" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1695549678" surrogate="true">
          <header />
          <body>
            <keys dataType="Array" type="System.Type[]" id="3716293565" length="1">
              <object dataType="Type" id="228898" value="Duality.Plugins.Navigation.AgentManager" />
            </keys>
            <values dataType="Array" type="Duality.Component[]" id="3595986582" length="1">
              <object dataType="ObjectRef">2091897961</object>
            </values>
          </body>
        </compMap>
        <compTransform />
        <identifier dataType="Struct" type="System.Guid" surrogate="true">
          <header>
            <data dataType="Array" type="System.Byte[]" id="2217814779" length="16">srdwjfcvHEOMvJ76ZPs3oQ==</data>
          </header>
          <body />
        </identifier>
        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
        <name dataType="String">AgentManager</name>
        <parent />
        <prefabLink />
      </object>
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
            <object dataType="Class" type="Duality.Plugins.Navigation.Agent" id="2431818930">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">3959643605</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object dataType="Class" type="NavigationTestbed.AgentAttributeTranslator" id="352408392">
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
              <object dataType="Type" id="4190115896" value="Duality.Plugins.Navigation.Agent" />
              <object dataType="Type" id="1943294466" value="NavigationTestbed.AgentAttributeTranslator" />
            </keys>
            <values dataType="Array" type="Duality.Component[]" id="323893006" length="5">
              <object dataType="ObjectRef">2024991241</object>
              <object dataType="ObjectRef">2727452833</object>
              <object dataType="ObjectRef">4116868307</object>
              <object dataType="ObjectRef">2431818930</object>
              <object dataType="ObjectRef">352408392</object>
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
                <componentType dataType="ObjectRef">4190115896</componentType>
                <prop dataType="PropertyInfo" id="97301216" value="P:Duality.Plugins.Navigation.Agent:Target" />
                <val dataType="Class" type="Duality.Plugins.Navigation.PointTarget" id="286482192">
                  <point dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                  </point>
                </val>
              </object>
              <object dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                <childIndex dataType="Class" type="System.Collections.Generic.List`1[[System.Int32]]" id="909540800">
                  <_items dataType="ObjectRef">20813472</_items>
                  <_size dataType="Int">0</_size>
                  <_version dataType="Int">1</_version>
                </childIndex>
                <componentType dataType="ObjectRef">1914743562</componentType>
                <prop dataType="PropertyInfo" id="851604912" value="P:Duality.Components.Transform:RelativePos" />
                <val dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">-10</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </val>
              </object>
              <object dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                <childIndex dataType="Class" type="System.Collections.Generic.List`1[[System.Int32]]" id="2238417824">
                  <_items dataType="Array" type="System.Int32[]" id="1163071856" length="0" />
                  <_size dataType="Int">0</_size>
                  <_version dataType="Int">1</_version>
                </childIndex>
                <componentType dataType="ObjectRef">4190115896</componentType>
                <prop dataType="PropertyInfo" id="3660402384" value="P:Duality.Plugins.Navigation.Agent:Characteristics" />
                <val dataType="Class" type="Duality.Plugins.Navigation.DefaultCharacteristics" id="2710601600">
                  <aggresivity dataType="Float">0</aggresivity>
                </val>
              </object>
              <object dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                <childIndex />
                <componentType />
                <prop />
                <val />
              </object>
            </_items>
            <_size dataType="Int">3</_size>
            <_version dataType="Int">60</_version>
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
            <object dataType="Class" type="Duality.Plugins.Navigation.Agent" id="4163345865">
              <active dataType="Bool">true</active>
              <gameobj dataType="ObjectRef">1396203244</gameobj>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            </object>
            <object dataType="Class" type="NavigationTestbed.AgentAttributeTranslator" id="2083935327">
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
              <object dataType="ObjectRef">4163345865</object>
              <object dataType="ObjectRef">2083935327</object>
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
                <componentType dataType="ObjectRef">4190115896</componentType>
                <prop dataType="ObjectRef">97301216</prop>
                <val dataType="Class" type="Duality.Plugins.Navigation.PointTarget" id="103687616">
                  <point dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                  </point>
                </val>
              </object>
              <object dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                <childIndex dataType="Class" type="System.Collections.Generic.List`1[[System.Int32]]" id="2642480672">
                  <_items dataType="ObjectRef">20813472</_items>
                  <_size dataType="Int">0</_size>
                  <_version dataType="Int">1</_version>
                </childIndex>
                <componentType dataType="ObjectRef">1914743562</componentType>
                <prop dataType="ObjectRef">851604912</prop>
                <val dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">10</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </val>
              </object>
              <object dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                <childIndex dataType="Class" type="System.Collections.Generic.List`1[[System.Int32]]" id="1624087424">
                  <_items dataType="ObjectRef">1163071856</_items>
                  <_size dataType="Int">0</_size>
                  <_version dataType="Int">1</_version>
                </childIndex>
                <componentType dataType="ObjectRef">4190115896</componentType>
                <prop dataType="ObjectRef">3660402384</prop>
                <val dataType="Class" type="Duality.Plugins.Navigation.DefaultCharacteristics" id="3885955936">
                  <aggresivity dataType="Float">0</aggresivity>
                </val>
              </object>
              <object dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
                <childIndex />
                <componentType />
                <prop />
                <val />
              </object>
            </_items>
            <_size dataType="Int">3</_size>
            <_version dataType="Int">389</_version>
          </changes>
          <obj dataType="ObjectRef">1396203244</obj>
          <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
            <contentPath dataType="String">Data\DummyAgent.Prefab.res</contentPath>
          </prefab>
        </prefabLink>
      </object>
    </serializeObj>
    <sourcePath />
  </object>
</root>