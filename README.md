# Land Value Heatmap
Makes Land Value heatmap more informative.
 - Land and Buildings use the same color scale so it is easy to spot differences that would influence LV i.e. buildings darker than land will drive your LV up.
 - Increased range of values so the heatmap does not saturate easy in bigger cities, where you usually end up with green buildings and blue land everywhere.

![Heatmap](https://raw.githubusercontent.com/infixo/cs2-landvalueheatmap/main/docs/heatmap.png)

## Scaling
- Vanilla version is scaled for LV up to 500 and it includes 10 gradients.
- Modded version is scaled up to 2500 and it includes 25 gradients, so 1 gradient is approx. 100 LV.

![Scaling](https://raw.githubusercontent.com/infixo/cs2-landvalueheatmap/main/docs/scaling.png)

## Technical

### Requirements and Compatibility
- Cities Skylines II v1.0.19f1 or later; check GitHub or Discord if the mod is compatible with the latest game version.
- BepInEx 5.
- The mod does NOT modify savefiles.
- Modified systems: OverlayInfomodeSystem.

### Installation
1. Place the `LandValueHeatmap.dll` file in your BepInEx `Plugins` folder.

### Known Issues
- Nothing atm.

### Changelog
- v1.0.0 (2024-03-07)
  - Initial build.

### Support
- Please report bugs and issues on [GitHub](https://github.com/Infixo/CS2-LandValueHeatmap).
- You may also leave comments on [Discord1](https://discord.com/channels/1169011184557637825/1198627819475976263) or [Discord2](https://discord.com/channels/1024242828114673724/1185672922212347944).
