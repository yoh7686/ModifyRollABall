# Modify Roll A Ballの使い方
## はじめに
Modify Roll A Ballはスクリプトをオブジェクトにアタッチするだけでとりあえず動く、GameSystemをアタッチしてセッティングすればゲームとしても動作するというスクリプトのセットです。
基本的にはコードエディタを開いて中身を書き換える必要はありません。

サンプルはこちらからプレイできます。
https://play.unity.com/ja/games/c75aa7ca-4c3a-4c83-af2d-bbf5811cc553/modify-roll-a-ball-with-unity-6

Unityの基本的な使い方から学びたい時は先に以下の動画を見ておきましょう。
https://www.youtube.com/watch?v=NgYG1_Si22A

## スクリプトの解説
### PlayerMoce.cs
Sphereを作ってアタッチするとプレイヤーが動きます。
InputSystemとRigidBodyは自動的にアタッチされます。
タグはPlayerにしてください。
実行する前にPlayerが動き回れる地面を作っておきましょう。

### Item.cs
オブジェクトにアタッチするとPlayerタグのオブジェクトに当たって消えるオブジェクトになります。
タグはItemにしてください。PreFabにしておくと便利です。

### Rotate.cs
アタッチすると毎秒設定された回転ベクトルに従って回ります。
アイテムの回転その他に使います。固定画面の場合は使わなくても問題ありません。

### CameraFollow.cs
カメラにアタッチするとPlayerタグのオブジェクトを設定されたOffsetの距離で追いかけます。
smoothTimeを調整することで追従速度をコントロールできます。

### UI関連

#### MessageController.cs
Canvas下のTextMeshProのオブジェクトにアタッチし、チュートリアルやクリアなどのメッセージを出すために使います。

#### ScoreDisplay.cs
Canvas下のTextMeshProのオブジェクトにアタッチし、取ったオブジェクトの数を表示するのに使います。

#### TimerDisplay.cs
Canvas下のTextMeshProのオブジェクトにアタッチし、タイマーを表示するのに使います。

#### HighScoreDisplay
スクリプトはありませんがハイスコア表示用のTextMeshProをCanvas下に作っておいてください。

### GameSystem
Emptyオブジェクトなどにアタッチして使います。
インスペクタで上記4つのUIを指定してください。
PlayerやItem、MainCameraなどはタグから自動的に設定されます。

AudioSourceを2つアタッチし、アイテム取得時、障害物ヒット時の音を指定してPickUpSEとHitSEに入れておくと音を鳴らすこともできます。
サンプルとしてAudioフォルダにサウンドファイルを入れてあります。
※なくても動作はします

ここまでで通常のRoll A Ballを作ることができます。
地面と壁を作り、Itemを並べてアイテムをすべて集めるまでの時間を競うRoll A Ballを作ってみましょう。

ここからはRoll A Ballに障害物やItemを取ることで動くオブジェクト、ゴールなどを追加するための方法を説明します。

### Hazardオブジェクト
タグをHazardに設定したオブジェクトに触れるとPlayerはplayerRespawnAtで設定された座標からやりなおしとなります。初期値はインスペクタで変更できます。
スクリプトは必要ありません。Rotate.csと組み合わせて動く障害物を作ることができます。見た目は同じマテリアルなどで統一しておきましょう。

### Respawnエリア
タグをRespawnに設定したEmptyオブジェクトなどにBoxColliderなどをつけ、isTriggerをチェックするとそのエリアにPlayerが入ったときにオブジェクトの原点座標がplayerRespawnAtに設定されます。
スクリプトは必要ありません。
難しいポイントの前に設定しておくとリトライが容易になります。あえて設定しないことで難易度を上げることもできます。

### Gate.cs
Cubeオブジェクトなどにアタッチします。インスペクタでPlayerがアイテムを何個取ったら何秒で何メートル動くという設定ができます。
これらを使ってアイテムを一定個数取ったら開くゲートやつながる橋などを作ることができます。オブジェクトのタグはGateとしてください。

### Goalエリア
タグをGoalに設定したEmptyオブジェクトなどにBoxColliderなどをつけ、isTriggerをチェックするとそのエリアにPlayerが入ったときにゴールとなります。
初期状態ではアイテムを全部取るとゴールになりますが、IsClearOnAllItemsCollectedをFalseにするとゴールに到達するまでクリアできなくなります。
なお、ハイスコアはアイテムの個数が優先され、個数が同じならタイムが早いほうが高得点となります。

## まとめ
スクリプトをオブジェクトにアタッチし、タグとインスペクタを設定するとレベルデザイン（ゲームを遊ぶ空間の設計）次第でRoll A Ballを迷路ゲーム、アクションゲーム、レースゲームのようなものに改造することができます。
ぜひいろんなRollABallを作ってSNSやUnity Playに投稿してみてください。ハッシュタグは#RollABallChallangeです。

