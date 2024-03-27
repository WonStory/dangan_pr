using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //인스펙터창에 띄워준다
public class Location
{
    public string name;
    public Transform tf_Spawn; //정확한 위치와 장소
}


public class TransferSpawnManager : MonoBehaviour
{
    [SerializeField] Location[] locations; //스폰장소가 여러개일 수도 있다.
    Dictionary<string, Transform> locationDic = new Dictionary<string, Transform>();

    public static bool spawnTiming = false; //항상 스폰하면 안되므로 true일 경우에만 스폰위치로 이동하도록 한다.








    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < locations.Length; i++) //시작과 동시에 찾아준다.
        {
            locationDic.Add(locations[i].name, locations[i].tf_Spawn); //네임과 위치를 딕셔너리로 추가시켜준다.
        }

        if (spawnTiming)
        {
            TransferManager theTM = FindObjectOfType<TransferManager>();
            string t_LocationName = theTM.GetLocationName();
            Transform t_Spawn = locationDic[t_LocationName];
            PlayerController.instance.transform.position = t_Spawn.position;
            PlayerController.instance.transform.rotation = t_Spawn.rotation;
            spawnTiming = false;
        }
    }


}
