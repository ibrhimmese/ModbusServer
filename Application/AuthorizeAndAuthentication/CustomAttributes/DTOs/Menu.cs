namespace Application.CustomAttributes.DTOs;

public class Menu
{
    public string Name { get; set; }
    public List<Action> Actions { get; set; } = new List<Action>();
}
