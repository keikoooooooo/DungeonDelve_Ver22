public interface ICalculateDMG
{
    /// <summary>
    /// Tính sát thương của Normal Attack
    /// </summary>  
    public void CalculateDMG_NA();
    
    /// <summary>
    /// Tính sát thương của Charged Attack
    /// </summary>  
    public void CalculateDMG_CA();
    
    /// <summary>
    /// Tính sát thương của Elemental Skill
    /// </summary>  
    public void CalculateDMG_EK();
    
    /// <summary>
    /// Tính sát thương của Elemental Burst
    /// </summary>  
    public void CalculateDMG_EB();
   
}
