public class Version: BaseObject
{
    public string VersionNumber { get; set; }
    public long ProjetId { get; set; }
    
    public Projet Projet { get; set; }
}