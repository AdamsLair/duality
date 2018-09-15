<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">33</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="1099837757">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3874747407">
        <_items dataType="Array" type="Duality.Component[]" id="4254807982">
          <item dataType="Struct" type="Duality.Components.Transform" id="1157114975">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <gameobj dataType="ObjectRef">1099837757</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
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
          </item>
          <item dataType="Struct" type="Duality.Components.Camera" id="2646224234">
            <active dataType="Bool">true</active>
            <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">0</A>
              <B dataType="Byte">125</B>
              <G dataType="Byte">108</G>
              <R dataType="Byte">88</R>
            </clearColor>
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">1099837757</gameobj>
            <nearZ dataType="Float">50</nearZ>
            <priority dataType="Int">0</priority>
            <projection dataType="Enum" type="Duality.Drawing.ProjectionMode" name="Perspective" value="1" />
            <renderSetup dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderSetup]]" />
            <renderTarget dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]" />
            <shaderParameters dataType="Struct" type="Duality.Drawing.ShaderParameterCollection" id="654471342" custom="true">
              <body />
            </shaderParameters>
            <targetRect dataType="Struct" type="Duality.Rect">
              <H dataType="Float">1</H>
              <W dataType="Float">1</W>
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
            </targetRect>
            <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="All" value="4294967295" />
          </item>
          <item dataType="Struct" type="Duality.Components.VelocityTracker" id="3170972224">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1099837757</gameobj>
          </item>
          <item dataType="Struct" type="Duality.Components.SoundListener" id="3132490284">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1099837757</gameobj>
          </item>
        </_items>
        <_size dataType="Int">4</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3161949152" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2653153189">
            <item dataType="Type" id="2135040406" value="Duality.Components.Transform" />
            <item dataType="Type" id="1182745306" value="Duality.Components.Camera" />
            <item dataType="Type" id="3933355574" value="Duality.Components.SoundListener" />
            <item dataType="Type" id="2524704250" value="Duality.Components.VelocityTracker" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="619004008">
            <item dataType="ObjectRef">1157114975</item>
            <item dataType="ObjectRef">2646224234</item>
            <item dataType="ObjectRef">3132490284</item>
            <item dataType="ObjectRef">3170972224</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">1157114975</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1366756975">+73ZCQyV7EmceKGb19vXsA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Camera</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="2244529680">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2548775222">
        <_items dataType="Array" type="Duality.Component[]" id="3635739488" length="4">
          <item dataType="Struct" type="Duality.Samples.Benchmarks.SpriteRendererSetup" id="1957527084">
            <active dataType="Bool">true</active>
            <alternatingMaterials dataType="Bool">true</alternatingMaterials>
            <gameobj dataType="ObjectRef">2244529680</gameobj>
            <spriteCount dataType="Int">10000</spriteCount>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1365434010" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="1204132356">
            <item dataType="Type" id="3237358404" value="Duality.Samples.Benchmarks.SpriteRendererSetup" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="2923507606">
            <item dataType="ObjectRef">1957527084</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1294056128">pCF5zDC2j0yN0KYm1z9fMw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">SpriteRendererSetup</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="2537381417">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3707548331">
        <_items dataType="Array" type="Duality.Component[]" id="2977898742" length="4">
          <item dataType="Struct" type="Duality.Samples.Benchmarks.PerfStatsRenderer" id="1368314001">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">2537381417</gameobj>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1192829256" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2276651393">
            <item dataType="Type" id="853405486" value="Duality.Samples.Benchmarks.PerfStatsRenderer" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="38495584">
            <item dataType="ObjectRef">1368314001</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="773283539">pqUG8UJnBUKFftY7j7XtYg==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">PerfStatsRenderer</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="728581427">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="309862657">
        <_items dataType="Array" type="Duality.Component[]" id="4110661934" length="4">
          <item dataType="Struct" type="Duality.Samples.Benchmarks.BenchmarkInfo" id="3239094611">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">728581427</gameobj>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3015850848" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="1529397963">
            <item dataType="Type" id="4226943990" value="Duality.Samples.Benchmarks.BenchmarkInfo" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="152696904">
            <item dataType="ObjectRef">3239094611</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2398754177">WQoTPT4btkyqdb709SZCSw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">BenchmarkInfo</name>
      <parent />
      <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="3349459283">
        <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="2596664356">
          <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="3933067972" length="4">
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3452835656">
                <_items dataType="Array" type="System.Int32[]" id="1806815340"></_items>
                <_size dataType="Int">0</_size>
              </childIndex>
              <componentType dataType="ObjectRef">4226943990</componentType>
              <prop dataType="MemberInfo" id="3396242654" value="P:Duality.Samples.Benchmarks.BenchmarkInfo:BenchmarkName" />
              <val dataType="String">SpriteRenderers, Many Materials</val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="4101842868">
                <_items dataType="Array" type="System.Int32[]" id="3376188488"></_items>
                <_size dataType="Int">0</_size>
              </childIndex>
              <componentType dataType="ObjectRef">4226943990</componentType>
              <prop dataType="MemberInfo" id="1467441698" value="P:Duality.Samples.Benchmarks.BenchmarkInfo:BenchmarkDesc" />
              <val dataType="String">This benchmark tests 10000 SpriteRenderers (~50% culled), alternating between different materials.</val>
            </item>
          </_items>
          <_size dataType="Int">2</_size>
        </changes>
        <obj dataType="ObjectRef">728581427</obj>
        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
          <contentPath dataType="String">Data\BenchmarksSample\Content\BenchmarkInfo.Prefab.res</contentPath>
        </prefab>
      </prefabLink>
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="37883379">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3332497729">
        <_items dataType="Array" type="Duality.Component[]" id="1419323822" length="4">
          <item dataType="Struct" type="Duality.Samples.Benchmarks.RenderSetupInfo" id="3252161477">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">37883379</gameobj>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1416436192" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="393924235">
            <item dataType="Type" id="2466941558" value="Duality.Samples.Benchmarks.RenderSetupInfo" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="2779398344">
            <item dataType="ObjectRef">3252161477</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2799764545">J7mafIh9iU6aIJcgvq2o+Q==</data>
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
