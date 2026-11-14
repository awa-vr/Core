# Core

Core for AwA's tools

## UI

Core has helpers for both IMGUI and UI Toolkit editors:

| IMGUI | UI Toolkit |
| --- | --- |
| `Core.Title(name)` | `Core.UI.CreateTitle(name)` |
| `Core.GetAvatar(ref avatar, ref avatars)` | `new Core.AvatarSelector()` (listen with `RegisterValueChangedCallback`) |
| `GUILayout.VerticalScope("helpbox")` | `Core.UI.CreateBox(title)` |

The IMGUI helpers are kept so tools that have not migrated yet keep working.
