# Modify Roll A Ballの使い方
## はじめに
Modify Roll A Ballはスクリプトをオブジェクトにアタッチするだけでとりあえず動く、GameSystemをアタッチしてセッティングすればゲームとしても動作するというスクリプトのセットです。
コードエディタを開いて中身を書き換える必要はありません。

解説動画はこちら

https://www.youtube.com/watch?v=z0wa-oJv8no

すべての手順をノーカットで収録したロング版解説動画はこちら

https://www.youtube.com/watch?v=snzUIrrAUy4

サンプルはこちらからプレイできます。

https://play.unity.com/ja/games/c75aa7ca-4c3a-4c83-af2d-bbf5811cc553/modify-roll-a-ball-with-unity-6

Unityの基本的な使い方から学びたい時は先に以下の動画を見ておきましょう。

https://www.youtube.com/watch?v=NgYG1_Si22A

## スクリプトの解説
### Playerフォルダ
#### Player.cs
Sphereを作ってアタッチするとプレイヤーが動きます。
InputSystemとRigidBodyは自動的にアタッチされます。
実行時に自動的にPlayerタグのオブジェクトとなりますが、あらかじめPlayerタグにしておくとより安定して動きます。

#### CameraFollow.cs
カメラにアタッチするとPlayerタグのオブジェクトをプレイヤーとの初期状態の距離を保って追跡します。
smoothTimeを調整することで追従速度をコントロールできます。

### Itemフォルダ
#### Item.cs
CubeオブジェクトにアタッチするとPlayerタグのオブジェクトに当たって消えるオブジェクトになります。
実行時にBoxColliderのisTriggerをオフにしています。
複数使うことになるのでPreFabにしておきましょう。

#### RotateObject.cs
アタッチすると毎秒設定された回転ベクトルに従って回ります。
アイテムに限らずHazardや床などオブジェクトを回転させたいとき全般に使います。

### GameSystem
Emptyオブジェクトなどにアタッチして使います。
実行時に自動的にGameControllerタグのオブジェクトとなりますが、予め指定しておくとより安定して動作します。
PickUpSEにアイテム取得時の音を、HitSEに障害物接触時の音をAudioClipとしてアタッチしておくと音が鳴ります。
これらはアタッチしなくても動作はします。
サンプルとしてAudioフォルダにSEのファイルを入れてあります。
なお、Enterキーでシーンをリロードしますが、初期のシーン名がSampleSceneとなっているためシーン名を変更した場合はインスペクタで変更したシーンを指定しなおしてください。


### UIフォルダ
#### TextController.cs
Canvas下のTextMeshProのオブジェクトにアタッチし、チュートリアルやクリアなどのメッセージを出すために使います。
GameSystem.csとセットで使います。

#### ScoreDisplay.cs
Canvas下のTextMeshProのオブジェクトにアタッチし、取ったオブジェクトの数とタイムを表示します。
また、ゴール時にハイスコアを記録し、スコアの記録があるときは表示します。
GameSystem.csとセットで使います。

#### RestartButton.cs
UIのButtonオブジェクトを作ってアタッチするとリスタートボタンとして機能します。
読み込み先のシーンはGameSystemのインスペクタで設定できます。

ここまでで通常のRoll A Ballを作ることができます。
地面と壁を作り、Itemを並べてアイテムをすべて集めるまでの時間を競うRoll A Ballを作ってみましょう。

ここからはRoll A Ballに障害物やItemを取ることで動くオブジェクト、ゴールなどを追加するための方法を説明します。

### Levelフォルダ
#### Hazard.cs
CubeにHazard.csをアタッチするとHazardオブジェクトとなり、Playerが接触するとRespawn（復活）ポイントまで戻されます。
Rotate.csと組み合わせて動く障害物を作ることもできます。見た目は同じマテリアルなどで統一しておきましょう。
isHazardTriggerをTrueにすると実行時にメッシュレンダラがオフになり、Hazardエリアとして機能します。こちらは落下時のRespawnなど進入禁止エリアとして使えます。
なお、Hazardタグを作ってオブジェクトに指定すれば同様に動作します。

#### Respawn.cs
Cubeオブジェクトにアタッチすると、実行時にタグがRespawn、メッシュレンダラがオフ、コライダのisTriggerがTrueとなってオブジェクトのあった範囲がRespawnエリアとなります。
PlayerがRespawnエリアに触れるとRespawnポイントが更新されてHazardに触れた際に戻る地点が変わります。
難しいポイントの前に設定しておくとリトライが容易になります。あえて設定しないことで難易度を上げることもできます。
なお、RespawnタグのついたオブジェクトにTriggerをつけても同様に動作します。

#### Gate.cs
Cubeオブジェクトなどにアタッチします。インスペクタでPlayerがアイテムを何個取ったら何秒で何メートル動くという設定ができます。
これらを使ってアイテムを一定個数取ったら開くゲートやつながる橋などを作ることができます。


#### Goal.cs
Cubeオブジェクトにアタッチすると、実行時にタグがRespawn、メッシュレンダラがオフ、コライダのisTriggerがTrueとなってオブジェクトのあった範囲がゴールエリアとなります。
初期状態ではアイテムを全部取るとゴールになりますが、IsClearOnAllItemsCollectedをFalseにするとゴールに到達するまでクリアになりません。
ハイスコアはアイテムの個数が優先され、個数が同じならタイムが早いほうが高得点となります。
なお、FinishタグのついたオブジェクトにTriggerをつけても同様に動作します。

## まとめ
スクリプトをオブジェクトにアタッチし、タグとインスペクタを設定するとレベルデザイン（ゲームを遊ぶ空間の設計）次第でRoll A Ballを迷路ゲーム、アクションゲーム、レースゲームのようなものに改造することができます。
ぜひいろんなRollABallを作ってSNSやUnity Playに投稿してみてください。ハッシュタグは#RollABallChallangeです。

