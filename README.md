# Unity 2022 game jam template

This is a template project which contains tools I frequently use when starting a new unity project. 
Including but not limited to:

- Tweening [DOTween](https://dotween.demigiant.com)
- Reactivity (Observables, reactive properties) [R3](https://github.com/Cysharp/R3)
- Lightweight and web-compatible async/await [UniTask](https://github.com/Cysharp/UniTask)
- Custom utilities [com.dman.utilities](https://github.com/dsmiller95/DmanUtilities/tree/master/Assets/UtilityScripts/com.dman.utilities)
- Text Mesh Pro
- Leaderboard and analytics [Playfab](https://learn.microsoft.com/en-us/gaming/playfab/sdks/unity3d/quickstart)
- Newtonsoft json serialization [Newtonsoft.Json](https://www.newtonsoft.com/json)
- Player-prefs style save system [SaveSystem](Assets/PluginsDev/SaveSystem/README.md)
- Volume slider script in Scripts/Audio, with a preconfigured audio mixer
- Asset postprocessing utility in Scripts/Editor/CustomPostprocessor



## Setup

- Use the template to create a new repository
- Clone the repository
- Set up git lfs
  - `git lfs install`
- Open the project in Unity
  - It will warn you about compilation errors.
  - DO NOT enter safe mode. instead, select "ignore", and let the project load.
  - After loading, it should no longer have compiler errors.
  - This is because nugetforunity must load and then download the packages which the rest of the project compiles against.
- Get to work!