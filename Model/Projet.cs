using System.Collections.Generic;
public class Projet: BaseObject
{
    public string Name { get; set; }

    public string Description { get; set; }

    public long? ParentId { get; set; }

    public List<Version> Versions { get; set; }

    public List<ProjetEnvironment> ProjetEnvironments { get; set; }
}