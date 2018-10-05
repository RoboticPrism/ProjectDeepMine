using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryActionBar : MonoBehaviour {

    public List<ActionBarButton> buttonList = new List<ActionBarButton>();
    FactoryBase factory;

    public void Setup(FactoryBase factory)
    {
        this.factory = factory;
        int i = 0;
        foreach(FactoryTask factoryTask in factory.factoryTaskPrefabs)
        {
            buttonList[i].Setup(factoryTask, () => factory.CreateTask(factoryTask));
            i++;
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        int i = 0;
        foreach(ActionBarButton button in buttonList)
        {
            button.SetInteractible(false);
            if (!factory.currentTask && factory.factoryTaskPrefabs.Count - 1 >= i && factory.factoryTaskPrefabs[i])
            {
                button.SetInteractible(true);
            }
            i++;
        }
        
    }
}
