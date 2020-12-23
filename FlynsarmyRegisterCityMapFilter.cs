using UnityEngine;
using System.Collections;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Game.UserInterfaceWindows;

namespace FlynsarmyRegisterCityMapFilterMod
{
    public class FlynsarmyRegisterCityMapFilter : MonoBehaviour
    {
        //this method will be called automatically by the modmanager after the main game scene is loaded.
        //The following requirements must be met to be invoked automatically by the ModManager during setup for this to happen:
        //1. Marked with the [Invoke] custom attribute
        //2. Be public & static class method
        //3. Take in an InitParams struct as the only parameter
        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams)
        {
            GameObject go = new GameObject(initParams.ModTitle);
            go.AddComponent<FlynsarmyRegisterCityMapFilter>();

            //after finishing, set the mod's IsReady flag to true.
            ModManager.Instance.GetMod(initParams.ModTitle).IsReady = true;
        }

        void Awake()
        {
            UIWindowFactory.RegisterCustomUIWindow(UIWindowType.ExteriorAutomap, typeof(FlynsarmyExteriorAutomapWindow));
            Debug.Log("CityMapFilter: Inventory registered windows");
        }
    }
}