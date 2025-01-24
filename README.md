The English version is below.

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

## 用語の解説
### レベルデザイン
ゲームを遊ぶ空間の設計をすること。
*難易度調整のことではないので注意してください

### リスポーン
画面から消えたプレイヤーキャラクター、敵キャラクター、アイテムなどを再生成すること。
ここではプレイヤーキャラクターが復活することを指しています。

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

# How to Use Modify Roll A Ball
## Introduction
Modify Roll A Ball is a set of scripts that allow you to create a basic functioning game just by attaching them to objects. By attaching the GameSystem and configuring the settings, it works as a complete game.  
There’s no need to open a code editor or rewrite the contents.

Instructional video(Japanese audio with English subtitles): 

https://www.youtube.com/watch?v=z0wa-oJv8no  

Long version with all steps included: 

https://www.youtube.com/watch?v=snzUIrrAUy4  

You can try the sample here(Japanese audio with auto-generated English subtitles):  

https://play.unity.com/ja/games/c75aa7ca-4c3a-4c83-af2d-bbf5811cc553/modify-roll-a-ball-with-unity-6  

If you want to learn the basics of Unity first, check out the following video:  

https://www.youtube.com/watch?v=NgYG1_Si22A  

## Script Explanation
### Player Folder
#### Player.cs
Create a Sphere and attach the script to make it the player.  
InputSystem and RigidBody will automatically be attached.  
During runtime, the object will automatically become a Player-tagged object, but preassigning the Player tag will ensure more stable behavior.

#### CameraFollow.cs
Attach this to the camera to have it follow Player-tagged objects while maintaining the initial distance.  
You can adjust the follow speed by modifying the `smoothTime` parameter.

### Item Folder
#### Item.cs
Attach this to a Cube object to make it disappear upon collision with a Player-tagged object.  
The `isTrigger` property of the BoxCollider will automatically be turned off during runtime.  
Since you’ll use it frequently, consider turning it into a Prefab.

#### RotateObject.cs
Attach this script to rotate the object at a specified angular velocity per second.  
You can use it for items, hazards, floors, or any other object you wish to rotate.

### GameSystem
Attach this to an Empty GameObject or a similar object.  
During runtime, it will automatically become a GameController-tagged object, but assigning the tag in advance will ensure more stable operation.  

- Attach an AudioClip for `PickUpSE` to play a sound when picking up an item.  
- Attach an AudioClip for `HitSE` for a sound when hitting a hazard.  
These are optional; the system will still work without them.  
Sample sound effect files are provided in the Audio folder.  

Pressing the Enter key will reload the scene. If you’ve renamed the initial scene (default is `SampleScene`), update the scene reference in the Inspector accordingly.

### UI Folder
#### TextController.cs
Attach this to a TextMeshPro object under a Canvas to display tutorial or clear messages.  
Use this together with `GameSystem.cs`.

#### ScoreDisplay.cs
Attach this to a TextMeshPro object under a Canvas to display the number of collected items and elapsed time.  
It also records high scores and displays them if a score exists.  
Use this together with `GameSystem.cs`.

#### RestartButton.cs
Create a Button UI object and attach this script to make it function as a restart button.  
You can configure the destination scene in the Inspector of the GameSystem object.

With the steps above, you can create a basic Roll A Ball game.  
Design the ground and walls, place items, and create a Roll A Ball game where the goal is to collect all the items as fast as possible.

From here onwards, we’ll explain how to add obstacles, objects triggered by collecting items, and goal areas to enhance your Roll A Ball game.

### Level Folder
#### Hazard.cs
Attach this script to a Cube to make it a Hazard object. If the Player collides with it, they will be sent back to the Respawn point.  
Combine it with `RotateObject.cs` to create moving hazards. Keep the appearance consistent by using the same materials.  

If you set `isHazardTrigger` to `True`, the MeshRenderer will be turned off during runtime, and it will function as a Hazard area. You can use this for no-entry zones like fall-detection areas.  
Hazard-tagged objects with a Trigger set will behave similarly.

#### Respawn.cs
Attach this to a Cube object to make it a Respawn point. During runtime, the object will automatically:  
- Be assigned the Respawn tag.  
- Have its MeshRenderer turned off.  
- Have its Collider's `isTrigger` property set to `True`.  

If a Player touches the Respawn area, the Respawn point is updated. Changing the Respawn point makes it easier to retry challenging sections. Alternatively, omitting Respawn points can increase the difficulty.  
Objects with a Respawn tag and a Trigger behave the same way.

#### Gate.cs
Attach this to a Cube or similar object. In the Inspector, you can configure how many items the Player needs to collect, how many seconds the process takes, and how far the object moves.  
Using this, you can create gates that open or bridges that connect when a certain number of items are collected.

#### Goal.cs
Attach this to a Cube object to make it a goal area. During runtime, the object will automatically:  
- Be assigned the Respawn tag.  
- Have its MeshRenderer turned off.  
- Have its Collider’s `isTrigger` property set to `True`.  

By default, clearing the game requires collecting all items. Setting `IsClearOnAllItemsCollected` to `False` makes reaching the goal necessary to clear the game.  
High scores prioritize the number of items collected, and if tied, faster times result in higher scores.  
Objects with a Finish tag and a Trigger behave the same way.

## Summary
By attaching scripts to objects and configuring tags and Inspector settings, you can modify Roll A Ball into different types of games, such as maze games, action games, or racing games, depending on your level design.  

Feel free to create various Roll A Ball games and share them on social media or Unity Play. The hashtag is #RollABallChallenge.  

