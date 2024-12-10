# Unity-ADV-Player Document

## version 20241210a 対応

## Summary 

Unity-ADV-Player は、xml に記述した命令に基づいて文字と画像を表示する、いわゆるテキスト ADV を再生するアプリケーションです。

## Requirements

アプリケーションでは以下の構造のディレクトリを使用します。  
アプリのルートディレクトリに以下のような構造のフォルダを作成します。

    Application Root
    ├─commonResource
    │  ├─bgms
    │  ├─ses
    │  └─uis
    ├─scenes
    │  ├─sampleScn001
    │  │  ├─bgvs
    │  │  ├─images
    │  │  ├─masks
    │  │  ├─texts
    │  │  └─voices

`scenes` 以下のディレクトリは、 `createScenesDir.sh` で生成可能です。

- `sampleScn001` は任意の名前を設定します。
- `scenes` の直下には、同じ構造のディレクトリを任意の数だけ作成可能です。
- `scenes` の直下に作成された、正しい構造のディレクトリはアプリ起動時に自動で読み込まれます。
- `commonResource` の直下のディレクトリの音声ファイルは、アプリ起動時に、必要に応じてロードされます。

扱うサウンドファイルは全て `.ogg`、 画像ファイルは全て `.png` に統一します。

## Xml Specification

## scenario.xml

scenario.xml に関する仕様を以下に記述します。

このファイルに限らず、xml に登場するキーワードは、全て `ローワーキャメルケース` を採用します。

例 : sample, lowerCamelCase, 

## Xml タグのリスト

### scenario

シナリオタグで挟み込んだものを一つの単位として読み込み、再生します。

    <scenario>
        <text str="message">
    </scenario>

    <scn>
        <text str="message">
    </scn>

`scenario, scn` 何れも同じ意味です。 以降に記述する全てのタグは `scenario, scn` の子として記述します。

### text

- 属性
    - string string : 表示するテキストを記述します。
    - string str : 表示するテキストを記述します。


    <!-- sample --> 
    <text str="message">

### voice

ボイスを再生します。

- 属性
    - int number : `voices` フォルダ内のファイルを五十音順でソートした際の通し番号です。
    - string fileName : `voices` フォルダ内のサウンドファイル名を、拡張子を省略して指定します。
    - int channel : チャンネル番号を指定します。無指定の場合はは `0` になります。 `0-2` までをサポートします。


    <!-- sample --> 
    <voice number="1" />
    <voice fileName="sound" channel="1" />

### image

画像を表示する。このタグ指定するとデフォルトで `AlphaChanger` アニメーションが自動で追加される。

- 属性
    - string a : `images` 内の画像のファイル名を、拡張子を省略して指定します。
    - string b : `images` 内の画像のファイル名を、拡張子を省略して指定します。
    - string c : `images` 内の画像のファイル名を、拡張子を省略して指定します。
    - string d : `images` 内の画像のファイル名を、拡張子を省略して指定します。
    - int targetLayerIndex : 画像を描画するコンテナのインデックスを指定します。
    - int x : 画像の位置を指定します。
    - int y : 画像の位置を指定します。
    - double scale : 画像の拡大率を指定します。小数点以下の数値もサポートしますが、表示が荒れるため、整数倍での利用を推奨します。
    - int angle : 画像の角度を指定します。
    - string mask :   
      `masks` 内のファイル名を、拡張子を省略して指定します。マスクは、マスク画像の透明部分が不可視になります。
    - string maskFrame :  
      `masks` 内のファイル名を、拡張子を省略して指定します。マスク画像の上に表示されます。この画像はマスクの影響を受けません。
    - bool InheritStatus :  
      true に指定すると、画像の描画時に最新の画像の状態 (位置、拡大率) をコピーします。

画像を描画するコンテナは　 奥側から順に `0 -> 1 -> 2` までサポートしています。

画像は奥側のレイヤーから `a -> b -> c -> d` の順番で重なる。`a` が一番奥。`d` が一番手前のレイヤーです。

    <!-- sample --> 
    <image a=\"A02\" mask=\"testMask\" maskFrame=\"testMaskFrame\ targetLayerIndex="1" />"
    <image a="" b="" c="" d="" x="0" y="0" scale="1.0" mask="" />

### draw

既に表示されている画像に対して、上書きする形で画像を追加します。表情差分の描画等に利用します。

既存の画像の拡大率や位置を変更している場合でも、それらの指定は不要です。

- 属性
    - string a : 画像のファイル名を指定します
    - string b : 画像のファイル名を指定します
    - string c : 画像のファイル名を指定します
    - string d : 画像のファイル名を指定します
    - double depth : 一回の処理での上書きの濃さを指定します。


    <!-- sample --> 
    <draw a="" b="" c="" d="" depth="0.1" />

### dummy

何の動作もしないダミーのアニメーションです。`Duration` と `Delay` は利用可能です。

このアニメーションは `Delay` + `Duration` の回数だけ実行されると終了し、無効となります。

特殊なインターバルが必要な場合や、デバッグ作業のためのアニメーションです。

- 属性
  - int delay : 最初のアニメーション実行前の待機時間を指定します。
  - int duration : このアニメーションを実行する時間をフレームで指定します。

### se

効果音を鳴らす。

- 属性
    - int number : `commonResource/ses` フォルダ内の通し番号で指定します。
    - string fileName : `commonResource/ses` フォルダ内のファイル名で指定します。
    - int repeatCount : 繰り返し回数を指定します。デフォルトは `0` です。
    - int channel : チャンネルを指定します。
      - 現在同時に利用可能なチャンネルは `0 - 2` です。
      - デフォルトは `0` です。明示的に指定しなくても動作します。
    - double volume : サウンドのボリュームを設定します。  
      この要素の中で指定された音量は、他のサウンドの要素には影響しません。
    - float delay : SE の再生を指定時間だけ遅延させます。単位は秒です。


    <!-- sample --> 
    <se fileName="sound" repeatCount="2" channel="0" />

### backgroundVoice

bgv を鳴らす

- 属性
    - string names : `bgvs` 内のファイル名を `,` 区切りで入力します。`,`周辺に半角スペースを入れることが可能です。
    - int channel : チャンネル番号を指定します。デフォルトは `0` 。 `0-2` までをサポートします。
    - double panStereo : -1 から 1 の範囲でパンを指定します。 -1 なら完全左側。 0 なら中央。 1 なら完全右側から音声が出力されます。


    <!-- sample --> 
    <backgroundVoice names="v1, v2, v3, v4" channel="1" />

### MoveMessageWindow

メッセージウィンドウを上下に移動させます。

位置は実行ごとに　上 -> 下 -> 上 -> 下 -> ... という風にトグルします。

    <!-- sample -->
    <moveMessageWindow />

### stop

// TODO : 動作未確認

指定した対象の動作を止めます。

- 属性
    - string target
    - int layerIndex
    - int channel
    - string name

主要なアニメーションの停止機能を実装済。以下のように記述して使用する。

    <!-- sample --> 
    <stop target="anime" name="shake" />
    <stop target="anime" name="animationChain" />

### start

この要素を持つシナリオからシーンが開始します。

この要素を子に持つシナリオがある場合、そのシナリオよりも前のシナリオは読み込み時にパースされずにスキップされます。

複数の `start` が設定されている場合、先に登場した `start` が優先されます。

記述例

    <!-- sample --> 
    <scenario> <start /> <text str="test" /> </scenario>

### ignore

この要素を子に持つシナリオは読み込み時、パースされずにスキップされます。

    <!-- sample --> 
    <scenario> <ignore /> <text str="test" /> </scenario>

### anime

`name` 属性にアニメーションの名前を指定して使用する。
続いて、有効な属性を追記するという形でアニメーションを宣言する。

アニメーションによって、有効な属性が異なる。  
無効(未定義・未実装)な属性を指定しても、XML として解釈可能であれば問題なく動作する。

- 属性
    - string name : アニメーションの名前を入力します。入力可能なアニメーションを以下を参照

以下のように記述する。

    <!-- sample --> 
    <anime name="shake" duration="10" strengthX="5" delay="2" /> 

#### alphaChanger

#### shake

- 属性
    - int strengthX
    - int strengthY
    - int duration  = 60;
    - int repeatCount
    - int interval
    - int delay
    - string groupName

#### slide

画面をスライドさせます。

- 属性
    - int degree
    - int delay 
    - int distance
    - int duration
    - int repeatCount
    - int interval
    - string groupName

#### flash

画面を白発光させます。Duration に指定したフレーム数の間に１回発光します。  
複数回発光させたい場合は、repeatCount を指定します。

- 属性
    - int duration
    - int repeatCount
    - int delay
    - int interval
    - string groupName

#### bound

#### maskSlide

指定したレイヤーのマスクと、MaskLine を移動させます。  
マスクが適用されているオブジェクトは移動しません。こちらの移動には Slide を使います。

- 属性
    - int degree
    - int distance 
    - int duration
    - int repeatCount
    - int delay
    - int interval
    - int targetLayerIndex
    - string groupName

#### animationChain

- 属性
    - int repeatCount = 0;

特殊なアニメーションタグです。内部に anime 要素を入力して使用します。

```
  <anime name="animationChain">
    <!-- shake, slide の順でアニメーションが実行される -->
    <anime name="shake" />
    <anime name="slide" />
  </anime>
```

この要素の子のアニメーションは、記述された順番で順次実行され、この要素自体が単体のアニメーションとして扱われます。

ただし、同じ `groupName` 属性のアニメーションが複数ある場合は、属性付きのアニメーションは同時実行されます。

```
  <anime name="animationChain">
    <!-- shake, slide に同じグループ名を指定すると同時に動作する -->
    <anime name="shake" groupName="sampleGroup" />
    <anime name="slide" groupName="sampleGroup" />
  </anime>
```

`groupName` 属性で同時にアニメーションを動かした場合、両方のアニメーションが停止するまで次のアニメーションは再生されません。

Image により、画像の描画命令があった場合、他のアニメーション同様にストップします。

#### chain

`animationChain` の別名です。nameにこれを指定した場合は、`animationChain` が生成されます。

#### image

新規レイヤーを追加しつつ画像を描画します。

- 属性
  - string a;
  - string b;
  - string c;
  - string d;
  - int x = 0;
  - int y = 0;
  - double scale = 1.0;
  - int wait = 0;

#### draw

現在のレイヤーに画像を描画します。

- 属性
    - string a;
    - string b;
    - string c;
    - string d;
    - double depth : １度に描画する画像の透明度です。例えば `0.1` を指定した場合、画像を完全に表示するまでに 10 回の描画を要します。無指定の場合は `0.1` が設定されます。

#### scaleChange

画像を拡大、縮小します。現在の拡大率から `to` で指定した倍率まで、`Duration` に指定した時間をかけて変化します。

- double to
- int duration
- int repeatCount // 未実装
- int delay
- string groupName

アニメーション開始時点での拡大率が 1.0 であった場合、
100フレームをかけて 1.0 -> 1.5 まで拡大率が変化します。

    <!-- sample --> 
    <anime name="scaleChange" to="1.5" duration="100" />

# setting.xml の仕様

各シーンの `texts` ディレクトリ以下に `setting.xml` と命名して配置する。  
以下のようにルートを `<setting>`  として使用する。

    <setting>
      <bgm number="1" />
    </setting>

実装済みのタグを以下に示す。

## bgm

シーンで流れる BGM を番号、ファイル名で指定する。  
BGM は `commonResource/bgms` の `.ogg` ファイルのみがカウントの対象となる。  
デフォルトは `0` となっている。

- 属性
    - int number : BGM の番号を指定します。インデックスは 0 始まり。デフォルトは 0
    - string fileName : 再生する BGM をファイル名で指定します。 `number` と一緒に指定した場合は、`fileName` が優先されます。
    - float volume : BGM の音量を指定します。`0 - 1.0` の範囲で設定します。  
      デフォルトは 1.0 です。
  
## bgv

シーンで流れる BGV の音量を設定します。

- 属性
    - float volume : 音量を設定します。 `0 - 2.0` の間で入力します。
      - デフォルト値は `1.0` です。
      - 最大値の `2.0` は本来の音量の二倍の音量です。
  
## backgroundVoice

bgv のエイリアスです。 両方について記述した場合の動作は未検証です。どちらか片方のみを記述してください。

## defaultSize

ウィンドウのサイズを設定します。有効な設定項目は横幅のみです。縦幅の指定は意味がありません。

- 属性
  - int width : ウィンドウの横幅を指定します。

実際のウィンドウのサイズは、モニターの仕様によって変化します。
本アプリは FullHD (1920x1080) 以上のモニターでの使用を想定して開発されています。

1920x1080 のモニターで利用する場合は、`width` を `1280` に設定します。
これにより、アプリがモニター全体に拡大して表示されます。

2560x1080 など更に横に広いモニターで上記のように指定した場合、画面の左右に余白ができます。

`width` を `1600` 等に指定すると、 ウィンドウの範囲を FullHD 以上にすることができます。

## messageWindow

メッセージウィンドウに関する設定をします。

- 属性
  - float alpha : メッセージウィンドウの透明度を `0 - 1.0` の間で入力します。