using UnityEngine;

// �ű�˵����
// TerrainRaycastHeightSync �ű���������θ߶�ͼ��ÿ���㣬
// ��ÿ��������·�����һ�����ϵ����ߣ���ⳡ���е���ײ�壬
// ������ײ��ĸ߶�����̬�޸ĵ��εĸ߶ȣ�ʵ�ֵ����볡������߶ȵ�ͬ�������䡣
// ��Ҫ���̣���ȡ���β��� -> ��ȡ�߶�ͼ -> ���߼�� -> ���¸߶�ͼ -> Ӧ�õ�����

public class TerrainRaycastHeightSync : MonoBehaviour
{
    Terrain terrain; // �����������
    // Start is called before the first frame update
    void Start()
    {
        //��Terrain Data����ȡ�����������ز���
        terrain = GetComponent<Terrain>(); // ��ȡ��ǰ�����ϵ�Terrain���
        int resolution = terrain.terrainData.heightmapResolution; // ��ȡ�߶�ͼ�ֱ���
        Debug.Log("resolution:" + resolution);
        float sizeX = terrain.terrainData.size.x; // ���ο��
        float sizeY = terrain.terrainData.size.y; // ���θ߶ȣ����߶ȣ�
        float sizeZ = terrain.terrainData.size.z; // �������
        Debug.Log("size x:" + sizeX + " size y:" + sizeY + " size z:" + sizeZ);
        float posX = terrain.transform.position.x; // ������������x
        float posY = terrain.transform.position.y; // ������������y
        float posZ = terrain.transform.position.z; // ������������z

        //��Process��
        // ��ȡ�������е�ĸ߶�ֵ��htsΪ��ά���飬�洢�߶�ͼ�����е�ĸ߶ȣ���һ��0~1��
        float[,] hts = terrain.terrainData.GetHeights(0, 0, resolution, resolution);
        RaycastHit hit; // ���ڴ洢���߼����
        // �����߶�ͼ��ÿ����
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                // ���㵱ǰ��������ռ��е�λ��origin
                Vector3 origin = new Vector3(
                    ((float)x / (float)(resolution - 1) * sizeX) + posX, // x����
                    posY + terrain.terrainData.GetHeight(x, y),           // y���꣨���α���߶ȣ�
                    (float)y / (float)(resolution - 1) * sizeZ + posZ     // z����
                );
                // ��origin�����·�����һ�����ϵ����ߣ�����Ƿ�����ײ��
                if (Physics.Raycast(new Vector3(origin.x, origin.y - sizeY, origin.z), Vector3.up, out hit, sizeY * 2))
                {
                    // �����⵽��ײ��������ײ���y������¸õ�ĸ߶ȣ���һ����0~1��
                    hts[y, x] = (hit.point.y - posY) / sizeY;
                }
            }
        }
        // ���޸ĺ�ĸ߶�ͼӦ�õ�������
        terrain.terrainData.SetHeights(0, 0, hts);
    }
}