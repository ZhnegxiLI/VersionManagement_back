public class DeploimentHistory: BaseObject
{
    public long VersionId { get; set; }
   
    public long ProjetEnvironmentId { get; set; }
    public Version Version { get; set; }
 
    public ProjetEnvironment ProjetEnvironment { get; set; }
}