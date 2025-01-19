# Modify Roll A Ballの使い方
## はじめに
Modify Roll A Ballはスクリプトをオブジェクトにアタッチするだけでとりあえず動く、GameSystemをアタッチしてセッティングすればゲームとしても動作するというスクリプトのセットです。
コードエディタを開いて中身を書き換える必要はありません。

サンプルはこちらからプレイできます。
https://play.unity.com/ja/games/c75aa7ca-4c3a-4c83-af2d-bbf5811cc553/modify-roll-a-ball-with-unity-6

Unityの基本的な使い方から学びたい時は先に以下の動画を見ておきましょう。
https://www.youtube.com/watch?v=NgYG1_Si22A

## スクリプトの解説
### PlayerMoce.cs
Sphereを作ってアタッチするとプレイヤーが動きます。
InputSystemとRigidBodyは自動的にアタッチされます。
実行時に自動的にPlayerタグのオブジェクトとなりますが、あらかじめPlayerタグにしておくとより安定して動きます。

### Item.cs
CubeオブジェクトにアタッチするとPlayerタグのオブジェクトに当たって消えるオブジェクトになります。
実行時にBoxColliderのisTriggerをTrueにしているのでCube以外にアタッチするときは手動でisTriggerをオンにしてください。
複数使うことになるのでPreFabにしておきましょう。

### Rotate.cs
アタッチすると毎秒設定された回転ベクトルに従って回ります。
アイテムの回転その他に使います。固定画面の場合は使わなくても問題ありません。

### CameraFollow.cs
カメラにアタッチするとPlayerタグのオブジェクトを設定されたOffsetの距離で追いかけます。
smoothTimeを調整することで追従速度をコントロールできます。

### UI関連

#### MessageController.cs
Canvas下のTextMeshProのオブジェクトにアタッチし、チュートリアルやクリアなどのメッセージを出すために使います。
GameSystem.csとセットで使います。

#### ScoreDisplay.cs
Canvas下のTextMeshProのオブジェクトにアタッチし、取ったオブジェクトの数とタイムを表示します。
また、ゴール時にハイスコアを記録し、スコアの記録があるときは表示します。
GameSystem.csとセットで使います。

### GameSystem
Emptyオブジェクトなどにアタッチして使います。
実行時に自動的にGameControllerタグのオブジェクトとなりますが、予め指定しておくとより安定して動作します。
PickUpSEにアイテム取得時の音を、HitSEに障害物接触時の音をAudioClipとしてアタッチしておくと音が鳴ります。
これらはアタッチしなくても動作はします。
サンプルとしてAudioフォルダにSEのファイルを入れてあります。

ここまでで通常のRoll A Ballを作ることができます。
地面と壁を作り、Itemを並べてアイテムをすべて集めるまでの時間を競うRoll A Ballを作ってみましょう。

ここからはRoll A Ballに障害物やItemを取ることで動くオブジェクト、ゴールなどを追加するための方法を説明します。

### Hazardオブジェクト
タグをHazardに設定したオブジェクトに触れるとPlayerはplayerRespawnAtで設定された座標からやりなおしとなります。初期値はインスペクタで変更できます。
スクリプトは必要ありません。Rotate.csと組み合わせて動く障害物を作ることができます。見た目は同じマテリアルなどで統一しておきましょう。
Hazardタグは最初から用意されていないためインスペクタのAdd Tagsから追加して設定してください。
コリジョン、トリガーどちらでも動作しますので落下判定にはTrigger、障害物としてはCollisionを使うと良いでしょう。

### Respawnエリア
タグをRespawnに設定したEmptyオブジェクトなどにBoxColliderなどをつけ、isTriggerをチェックするとそのエリアにPlayerが入ったときにオブジェクトの原点座標がGameSystemのplayerRespawnAtに設定されます。
スクリプトは必要ありません。
難しいポイントの前に設定しておくとリトライが容易になります。あえて設定しないことで難易度を上げることもできます。

### Gate.cs
Cubeオブジェクトなどにアタッチします。インスペクタでPlayerがアイテムを何個取ったら何秒で何メートル動くという設定ができます。
これらを使ってアイテムを一定個数取ったら開くゲートやつながる橋などを作ることができます。

### Goalエリア
タグをGoalに設定したEmptyオブジェクトなどにBoxColliderなどをつけ、isTriggerをチェックするとそのエリアにPlayerが入ったときにゴールとなります。
初期状態ではアイテムを全部取るとゴールになりますが、IsClearOnAllItemsCollectedをFalseにするとゴールに到達するまでクリアできなくなります。
なお、ハイスコアはアイテムの個数が優先され、個数が同じならタイムが早いほうが高得点となります。

## まとめ
スクリプトをオブジェクトにアタッチし、タグとインスペクタを設定するとレベルデザイン（ゲームを遊ぶ空間の設計）次第でRoll A Ballを迷路ゲーム、アクションゲーム、レースゲームのようなものに改造することができます。
ぜひいろんなRollABallを作ってSNSやUnity Playに投稿してみてください。ハッシュタグは#RollABallChallangeです。

