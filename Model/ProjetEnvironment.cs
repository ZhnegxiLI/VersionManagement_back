public class ProjetEnvironment: BaseObject
{
  public long ProjetId { get; set; }
  public long EnvironmentId { get; set; }
  public string Description { get; set; }

  public Projet Projet { get; set; }

  public Environment Environment { get; set; }
}