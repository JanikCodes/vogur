<div align="center">
<a href="https://assetstore.unity.com/publishers/26774"><img src="/Core/Editor/EditorResources/Images/Logotype/ExLib_FullHD.png" alt="ExLib Logotype" width="700" align="center"></a>
</div>


## ExLib

ExLib is a external library for Unity engine from Renowned Games, which contains solutions for editor and runtime assemblies.
<p align="center">
<a href="http://www.apache.org/licenses/LICENSE-2.0"><img src="https://img.shields.io/badge/license-Apache%20License%202.0-blue.svg?style=flat" alt="license" title=""></a>
</p>



## Features

- **CoroutineObject** - representation of the coroutine in the form of an object, the possibility of detailed start/stop control.
- **EditorResources** - Unity _Resources_ soulution for editor. Easy loading assets from any `EditorResources` directories.
- **ProjectDatabase** - Allow to load and iterate through all assets in the project.
- **Patterns** - Ready made `singleton` pattern for mono behaviour classes.
- **Extensions** - Extensions methods for `System.Type`, `SerializedObject`, `SerializedProperty`, `EditorWindow`.
- **ScreenUtility** - Utilities of current Unity editor screen.
- **ExSearchWindow** - Unity search window implementation for easy handling by analogy with `GenericMenu`.



## Requirements

ExLib recommended to use on Unity **2021.3 LTS** or higher.

> We aim to support all upcoming Unity versions, but keep in mind that Alpha and Beta versions of Unity are not officially supported.

<br>

**Verified on Unity versions**
* Unity 2022.1
* Unity 2021.3 (LTS)
* Unity 2021.2
* Unity 2021.1
* Unity 2020.3 (LTS)
* Unity 2020.2
* Unity 2020.1
* Unity 2019.4 (LTS)
* Unity 2019.3
* Unity 2019.2
* Unity 2019.1



## Quick Start

Download and install Unity **2021.3 LTS** or higher.

Clone, add as submodule or download this repository and open the project in Unity.

### Clone repository

`git clone https://gitlab.com/TamerlanShakirov/ExLib Assets/RenownedGames/ExLib`

### Add repository as submudule

> The `.git` folder should be located in the root folder of the project.

`git submodule add https://gitlab.com/TamerlanShakirov/ExLib Assets/RenownedGames/ExLib`

### Assemblies and Namespaces

The library is divided into two main assemblies and namespaces, respectively: **ExLib** and **ExLibEditor**.

- **ExLib** - contains all class which will be available in build.
- **ExLibEditor** - contains all class which will be available only in editor.

 > _If you create your own custom assemblies don't forget to add ExLib assemblies as Assembly Definition Reference. Otherwise, your assemblies will not be able to find the namespace of ExLib library._



## Licence

Full license terms are in [LICENSE](LICENSE)
```
Copyright 2022 Tamerlan Shakirov

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
```