﻿


# # Fox Audio Manager
![Fox Icon](https://i.ibb.co/br7SPBG/q3866-Ct-SP28.jpg)
______________________
Проект с Audio Manager
Используется: [Unity 2021.3.11](https://unity3d.com/unity/whats-new/2021.3.11)
За основу взят : [RFG Audio](https://github.com/retro-fall-games/rfg-audio)

Официальная документация RFG Audio : [YouTube RFG Audio](https://www.youtube.com/watch?v=Qv9bM2KaRTY&list=PLpnpTHaLzeNWMW916duN_e_Y3AWG6-tdG)

Все необходимые файлы находиться в последнем релизе репозитория.

## Как установить?

Открываем  [Releases](https://github.com/IRecs/Fox-Audio/releases)  выбираем самый новый, и качаем файл [FoxAudioManager.unitypackage](https://github.com/IRecs/Fox-Audio/releases/download/TestReleases/FoxAudioManager.unitypackage). Распаковываем в проекте.

## Как пользоваться?

Ищем  .../RFGAudio/Prefabs/FoxAudioManager.prefab
Переносим на стартовую сцену.

Компонент  ***FoxAudioManager*** основа всей логики. 
К нему мы будем обращаться что бы запустить или остановить воспроизведение любого аудио.
 
Компонент ***AudioMixerSettingsPanel*** служит исключительно для инициализации микшеров.


<a href="https://ibb.co/qRCdrFC"><img src="https://i.ibb.co/GV90539/Fox-Audio-Manager-0-3.png" alt="Fox-Audio-Manager-0-3" border="0"></a>

***FoxAudioManager*** имеет всего одно редактируемое поле ***Audio Case*** 

<a href="https://imgbb.com/"><img src="https://i.ibb.co/v1nb9xx/Fox-Audio-Manager-0-4.png" alt="Fox-Audio-Manager-0-4" border="0"></a>

Тут храниться все доступные для использования аудио файлы.
Они делается на 3 типа.

 1. **Solo Audio** =  **Audio** = Проигрывает всего одну мелодию    
 2. **Play List Audio** = **Playlist** = Хранит в себе несколько **Audio**, проигрывает их по очереди.    
 3. **Random Audio** = **RandomAudio** = Хранит в себе несколько **Audio**, проигрывает их в случайном порядке
 
 И префабы проигрывателей 
 
 1. **Audio** = Для проигрывания одиночных аудио
 2. **Playlist** = Для проигрывания плейлистов аудио
 3. **RandomAudio** = Для проигрывания случайных аудио

Имя = Ключу, по которому запускается аудио.
То есть ключ **SoundTrack** = **"SoundTrack"**

**FoxAudioManager** является **DontDestroyOnLoad** _Singleton_
Вам необходимо добавить его в свою логику в самом начале игры.
Или через DI, или иным способом.

Далее логика работы проста 

```
FoxAudioManager _manager = (...)

//Следует за целью
_manager.PlayAudioFollowingTarget("SoundTrack", Camera.main.transform, true);  
//Просто появляется в указанной позиции
_manager.PlayAudio("Warp", transform.position, true);

bool PlayAudio(string key, Vector3 spawnPosition/Transform target, bool persist = false)

string key = ключ по которому происходит поиск
Vector3 spawnPosition/Transform target = позиция или таргет 
bool persist = будет ли аудио DontDestroyOnLoad

Для остановки 
_manager.StopAudio(string key)
_manager.StopAllAudio(string key)

Все методы возвращают bool.
```
**Audio Data**
-
<a href="https://ibb.co/qB8Tmzw"><img src="https://i.ibb.co/rkhD5gJ/Fox-Audio-Manager-0-5.png" alt="Fox-Audio-Manager-0-5" border="0"></a>
______________________
**PlaylistData**
-
<a href="https://imgbb.com/"><img src="https://i.ibb.co/cQVB5fQ/Fox-Audio-Manager-0-6.png" alt="Fox-Audio-Manager-0-6" border="0"></a>
______________________
**RandomAudioData**
-
<a href="https://imgbb.com/"><img src="https://i.ibb.co/HVHQgxP/Fox-Audio-Manager-0-7.png" alt="Fox-Audio-Manager-0-7" border="0"></a>
