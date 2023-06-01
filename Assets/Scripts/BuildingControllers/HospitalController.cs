using System.Collections.Generic;
using UnityEngine;

public class HospitalController : BuildingController {
    //variable with mapping multiplier to level {level: 1, multiplier: 1.1}, {level: 2, multiplier: 1.2}
    private Dictionary<int, float> levelMultiplier = new Dictionary<int, float> { { 1, 1f }, { 2, 2f } };

    //precisa de mapping de nivel para custo de upgrade
    //private Dictionary<int, int> levelCost = new Dictionary<int, int> { { 1, 100 }, { 2, 200 } };

    // Start is called before the first frame update
    void Start()
    {
        buildingState.buildingType = BuildingType.Hospital;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void UpgradeBuilding() //button ui
    {
        buildingState.level++;
        buildingState.multiplier = levelMultiplier[buildingState.level];
    }

    private void OnTriggerStay(Collider other)
    {
        HealPlayer(other.gameObject);
    }

    private void HealPlayer(GameObject player)
    {
        var healtGained = buildingState.multiplier * Time.deltaTime;
        //var playerController = player.GetComponent<PlayerController>();
        //playerController.Heal(healthGained);
    }
}