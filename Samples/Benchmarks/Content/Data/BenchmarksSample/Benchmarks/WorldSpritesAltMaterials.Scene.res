<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">33</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="1037431975">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1862252549">
        <_items dataType="Array" type="Duality.Component[]" id="1600975446" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="3397746907">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">1037431975</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <parentTransform />
            <pos dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">-500</Z>
            </pos>
            <posAbs dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">-500</Z>
            </posAbs>
            <scale dataType="Float">1</scale>
            <scaleAbs dataType="Float">1</scaleAbs>
            <vel dataType="Struct" type="Duality.Vector3" />
            <velAbs dataType="Struct" type="Duality.Vector3" />
          </item>
          <item dataType="Struct" type="Duality.Components.Camera" id="1574707782">
            <active dataType="Bool">true</active>
            <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">0</A>
              <B dataType="Byte">125</B>
              <G dataType="Byte">108</G>
              <R dataType="Byte">88</R>
            </clearColor>
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">1037431975</gameobj>
            <nearZ dataType="Float">0</nearZ>
            <perspective dataType="Enum" type="Duality.Drawing.PerspectiveMode" name="Parallax" value="1" />
            <renderSetup dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderSetup]]" />
            <renderTarget dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]" />
            <targetRect dataType="Struct" type="Duality.Rect">
              <H dataType="Float">1</H>
              <W dataType="Float">1</W>
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
            </targetRect>
            <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="All" value="4294967295" />
          </item>
          <item dataType="Struct" type="Duality.Components.SoundListener" id="1690913346">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1037431975</gameobj>
          </item>
        </_items>
        <_size dataType="Int">3</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="906428328" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="918465519">
            <item dataType="Type" id="2479789294" value="Duality.Components.Transform" />
            <item dataType="Type" id="2087821770" value="Duality.Components.Camera" />
            <item dataType="Type" id="3501989598" value="Duality.Components.SoundListener" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="1661995424">
            <item dataType="ObjectRef">3397746907</item>
            <item dataType="ObjectRef">1574707782</item>
            <item dataType="ObjectRef">1690913346</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">3397746907</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="341472893">+73ZCQyV7EmceKGb19vXsA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Camera</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="3695799576">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2281692926">
        <_items dataType="Array" type="Duality.Component[]" id="365722000" length="4">
          <item dataType="Struct" type="Duality.Samples.Benchmarks.SpriteRendererSetup" id="4169242324">
            <active dataType="Bool">true</active>
            <alternatingMaterials dataType="Bool">true</alternatingMaterials>
            <gameobj dataType="ObjectRef">3695799576</gameobj>
            <spriteCount dataType="Int">10000</spriteCount>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1634514826" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="4278949084">
            <item dataType="Type" id="2210745028" value="Duality.Samples.Benchmarks.SpriteRendererSetup" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="1417361686">
            <item dataType="ObjectRef">4169242324</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="27135560">pCF5zDC2j0yN0KYm1z9fMw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">SpriteRendererSetup</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="1078257601">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3116672579">
        <_items dataType="Array" type="Duality.Component[]" id="1290913830" length="4">
          <item dataType="Struct" type="Duality.Samples.Benchmarks.PerfStatsRenderer" id="3401382239">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1078257601</gameobj>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2735549112" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="1943695657">
            <item dataType="Type" id="103512078" value="Duality.Samples.Benchmarks.PerfStatsRenderer" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="1359349184">
            <item dataType="ObjectRef">3401382239</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3018368779">pqUG8UJnBUKFftY7j7XtYg==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">PerfStatsRenderer</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="2553104036">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2630841882">
        <_items dataType="Array" type="Duality.Component[]" id="693412224" length="4">
          <item dataType="Struct" type="Duality.Samples.Benchmarks.BenchmarkInfo" id="1073141920">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">2553104036</gameobj>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1821943098" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3538251104">
            <item dataType="Type" id="770661596" value="Duality.Samples.Benchmarks.BenchmarkInfo" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="2166146190">
            <item dataType="ObjectRef">1073141920</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1193037692">WQoTPT4btkyqdb709SZCSw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">BenchmarkInfo</name>
      <parent />
      <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="802018458">
        <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="100392704">
          <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="2309943964" length="4">
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="4282201288">
                <_items dataType="Array" type="System.Int32[]" id="2869756524"></_items>
                <_size dataType="Int">0</_size>
              </childIndex>
              <componentType dataType="ObjectRef">770661596</componentType>
              <prop dataType="MemberInfo" id="3706604254" value="P:Duality.Samples.Benchmarks.BenchmarkInfo:BenchmarkName" />
              <val dataType="String">SpriteRenderers, Many Materials</val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="1807907124">
                <_items dataType="Array" type="System.Int32[]" id="1124287816"></_items>
                <_size dataType="Int">0</_size>
              </childIndex>
              <componentType dataType="ObjectRef">770661596</componentType>
              <prop dataType="MemberInfo" id="2532933410" value="P:Duality.Samples.Benchmarks.BenchmarkInfo:BenchmarkDesc" />
              <val dataType="String">This benchmark tests 10000 SpriteRenderers (~50% culled), alternating between different materials.</val>
            </item>
          </_items>
          <_size dataType="Int">2</_size>
        </changes>
        <obj dataType="ObjectRef">2553104036</obj>
        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
          <contentPath dataType="String">Data\BenchmarksSample\Content\BenchmarkInfo.Prefab.res</contentPath>
        </prefab>
      </prefabLink>
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="2231991091">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2115976961">
        <_items dataType="Array" type="Duality.Component[]" id="3842158894" length="4">
          <item dataType="Struct" type="Duality.Samples.Benchmarks.BenchmarkController" id="1355404571">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">2231991091</gameobj>
            <renderAAQualityLevels dataType="Struct" type="System.Collections.Generic.List`1[[Duality.AAQuality]]" id="2020561467">
              <_items dataType="Array" type="Duality.AAQuality[]" id="3140372182" length="4">
                <item dataType="Enum" type="Duality.AAQuality" name="Off" value="3" />
                <item dataType="Enum" type="Duality.AAQuality" name="Low" value="2" />
                <item dataType="Enum" type="Duality.AAQuality" name="Medium" value="1" />
              </_items>
              <_size dataType="Int">4</_size>
            </renderAAQualityLevels>
            <renderResolutionScales dataType="Struct" type="System.Collections.Generic.List`1[[System.Single]]" id="528930344">
              <_items dataType="Array" type="System.Single[]" id="2920198481">0, 0.5, 1, 2, 4, 0, 0, 0</_items>
              <_size dataType="Int">5</_size>
            </renderResolutionScales>
            <renderSizes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Point2]]" id="467766705">
              <_items dataType="Array" type="Duality.Point2[]" id="2047269476">
                <item dataType="Struct" type="Duality.Point2">
                  <X dataType="Int">320</X>
                  <Y dataType="Int">300</Y>
                </item>
                <item dataType="Struct" type="Duality.Point2">
                  <X dataType="Int">800</X>
                  <Y dataType="Int">600</Y>
                </item>
                <item dataType="Struct" type="Duality.Point2">
                  <X dataType="Int">1024</X>
                  <Y dataType="Int">768</Y>
                </item>
                <item dataType="Struct" type="Duality.Point2">
                  <X dataType="Int">1920</X>
                  <Y dataType="Int">1080</Y>
                </item>
              </_items>
              <_size dataType="Int">4</_size>
            </renderSizes>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3657569120" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="833597643">
            <item dataType="Type" id="976499702" value="Duality.Samples.Benchmarks.BenchmarkController" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="3035340872">
            <item dataType="ObjectRef">1355404571</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="930614145">VzMz9zp4TUi0lwrjon6gKQ==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">BenchmarkController</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="2517825367">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3287372501">
        <_items dataType="Array" type="Duality.Component[]" id="3500709366" length="4">
          <item dataType="Struct" type="Duality.Samples.Benchmarks.RenderSetupInfo" id="592990701">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">2517825367</gameobj>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1785417288" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="1158790143">
            <item dataType="Type" id="3261511470" value="Duality.Samples.Benchmarks.RenderSetupInfo" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="626675040">
            <item dataType="ObjectRef">592990701</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2071439789">J7mafIh9iU6aIJcgvq2o+Q==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">RenderSetupInfo</name>
      <parent />
      <prefabLink />
    </item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
