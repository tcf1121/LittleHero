using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private int _chestNum;
    // Start is called before the first frame update
    void Start()
    {
        _chestNum = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetChest(int num)
    {
        _chestNum += num;
    }

    public int GetChestNum()
    {
        return _chestNum;
    }

    public bool CheckChestNum()
    {
        return _chestNum > 0;
    }

    public void UseChestNum()
    {
        if (_chestNum > 0)
        {
            _chestNum--;
        }
    }

    public EquipmentData OpenChest()
    {
        EquipmentType type = (EquipmentType)Random.Range(0, 4);
        int index = RandomNum();
        return GameManager.Instance.Inventory.GetItem(type, index);
    }

    public int RandomNum()
    {
        int rand = Random.Range(0, 100);
        if (rand < 40) return 1;
        else if (rand < 70) return 2;
        else if (rand < 90) return 3;
        else return 4;
    }
}
