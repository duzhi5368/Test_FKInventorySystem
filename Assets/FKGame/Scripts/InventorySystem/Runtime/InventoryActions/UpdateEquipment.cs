//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [Icon("Item")]
    [ComponentMenu("Inventory System/Update Equipment")]
    public class UpdateEquipment : Action
    {
        public override ActionStatus OnUpdate()
        {
            EquipmentHandler handler = playerInfo.gameObject.GetComponent<EquipmentHandler>();
            handler.UpdateEquipment();
            return ActionStatus.Success;
        }
    }
}