public interface IMechanism_InteractCard
{
    bool InEnable(string cardGuid);
    

    void OnSelect_Any(string cardGuid);
    
    void OnSelect_Enable(string cardGuid);
}