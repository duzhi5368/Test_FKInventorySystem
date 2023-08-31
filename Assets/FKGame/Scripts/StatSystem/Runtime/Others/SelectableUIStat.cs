//------------------------------------------------------------------------
namespace FKGame.StatSystem
{
    public class SelectableUIStat : UIStat
    {
        protected override StatsHandler GetStatsHandler()
        {
            if (ComponentSelectableObject.current != null)
            {
                return ComponentSelectableObject.current.GetComponent<StatsHandler>();

            }
            return null;
        }
    }
}