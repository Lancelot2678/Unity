# Unity
一些地编用到的脚本

TerrainRaycastHeightSync          from-SuperWiwi

TerrainRaycastHeightSync 脚本会遍历地形高度图的每个点，
在每个点的正下方发射一条向上的射线，检测场景中的碰撞体，
并用碰撞点的高度来动态修改地形的高度，实现地形与场景物体高度的同步或适配。

主要流程：获取地形参数 -> 获取高度图 -> 射线检测 -> 更新高度图 -> 应用到地形
