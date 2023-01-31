namespace VistasCamunda.Models
{
    public class Variable
    {
        public Dictionary<string, Atribute> modifications { get; set; }
        public void Add(string name, object value, string type)
        {
            modifications.Add(name, new Atribute { value = value, type = type });
        }
    }
    public class Atribute
    {
        public object value { get; set; }
        public string type { get; set; }
    }
}
