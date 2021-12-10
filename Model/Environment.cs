using System.Collections.Generic;
public class Environment: BaseObject
{
    public string Name { get; set; }

    public int? Order { get; set; }
    public List<ProjetEnvironment> ProjetEnvironments { get; set; }
    
}