using UnityEngine;

// 脚本说明：
// TerrainRaycastHeightSync 脚本会遍历地形高度图的每个点，
// 在每个点的正下方发射一条向上的射线，检测场景中的碰撞体，
// 并用碰撞点的高度来动态修改地形的高度，实现地形与场景物体高度的同步或适配。
// 主要流程：获取地形参数 -> 获取高度图 -> 射线检测 -> 更新高度图 -> 应用到地形

public class TerrainRaycastHeightSync : MonoBehaviour
{
    Terrain terrain; // 地形组件引用
    // Start is called before the first frame update
    void Start()
    {
        //【Terrain Data】获取地形组件和相关参数
        terrain = GetComponent<Terrain>(); // 获取当前物体上的Terrain组件
        int resolution = terrain.terrainData.heightmapResolution; // 获取高度图分辨率
        Debug.Log("resolution:" + resolution);
        float sizeX = terrain.terrainData.size.x; // 地形宽度
        float sizeY = terrain.terrainData.size.y; // 地形高度（最大高度）
        float sizeZ = terrain.terrainData.size.z; // 地形深度
        Debug.Log("size x:" + sizeX + " size y:" + sizeY + " size z:" + sizeZ);
        float posX = terrain.transform.position.x; // 地形世界坐标x
        float posY = terrain.transform.position.y; // 地形世界坐标y
        float posZ = terrain.transform.position.z; // 地形世界坐标z

        //【Process】
        // 获取地形所有点的高度值，hts为二维数组，存储高度图上所有点的高度（归一化0~1）
        float[,] hts = terrain.terrainData.GetHeights(0, 0, resolution, resolution);
        RaycastHit hit; // 用于存储射线检测结果
        // 遍历高度图的每个点
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                // 计算当前点在世界空间中的位置origin
                Vector3 origin = new Vector3(
                    ((float)x / (float)(resolution - 1) * sizeX) + posX, // x坐标
                    posY + terrain.terrainData.GetHeight(x, y),           // y坐标（地形表面高度）
                    (float)y / (float)(resolution - 1) * sizeZ + posZ     // z坐标
                );
                // 在origin点正下方发射一条向上的射线，检测是否有碰撞体
                if (Physics.Raycast(new Vector3(origin.x, origin.y - sizeY, origin.z), Vector3.up, out hit, sizeY * 2))
                {
                    // 如果检测到碰撞，则用碰撞点的y坐标更新该点的高度（归一化到0~1）
                    hts[y, x] = (hit.point.y - posY) / sizeY;
                }
            }
        }
        // 将修改后的高度图应用到地形上
        terrain.terrainData.SetHeights(0, 0, hts);
    }
}