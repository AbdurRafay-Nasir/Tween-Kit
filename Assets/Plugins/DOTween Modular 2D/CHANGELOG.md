## [2.1.0] - 2023-09-3
### Added
- DOAnchorPos Component
- DOSize Component
- DOShapeCircle Component
- DOLookAtBaseEditor
- ClearSavedEditorPrefs() in DOBaseEditor to clear all EditorPrefs for a specific component when it is removed or gameObject is deleted
- Stop Tween Preview if game Object was deselected/deleted or component was deleted


### Changed
- Refactored LookAt code, now to add look at functionality in DO component inherit from DOLookAt Component and call SetupLookAt after creating tween
- Refactored LookAt editor code, now to create lookAt section in DO Component Editor, inherit from DOLookAtBaseEditor

## [2.0.2] - 2023-08-30
### Added
- Undo/Redo functionality for Editor Properties
- Begin Text in scene view
- Arrow for Begin Property in scene view

### Fixed
- Postion/Rotation/Scale not resetting after Tween preview, with look at set

## [2.0.1] - 2023-08-28
### Added
- Scene view labels in DOSequence Editor

### Changed
- Minimum number of jumps in DOJump

## [2.0.0] - 2023-08-27
### Added
- DOJump component
- DOPunchPosition component
- DOPunchAnchorPos component
- DOPunchRotation component
- DOPunchScale component
- DOShakePosition component
- DOShakeAnchorPos component
- DOShakeRotation component
- DOShakeScale component 
- Background Rect for each Foldout section, for identification Purposes
- Tabs to enable/disable Foldout Sections
- Lines to display backward chain for tweenObject
- Lines to SequenceTweens in DOSequence

### Fixed
- Code Formatting for all components 

### Changed
- AddComponentMenu category:
DOTween Modular 2D -> DOSequence/Transform/UI
UI -> DOPunchAnchorPos/DOShakeAnchorPos
Transform -> Remaining components

### Removed
- DOPunch component
- DOShake component
- ApplyTo Enum


## [1.0.0] - 2023-08-21
### Added
- Released DOTween Modular 2D on Github
- Released DOTween Modular 2D Development on Github