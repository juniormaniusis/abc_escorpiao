namespace Assets.Scripts.Player
{
    public class PersonagemConfig
    {
        public string Actor { get; }
        public string TagGrafico { get; }

        public PersonagemConfig(string actor, string tagGrafico)
        {
            Actor = actor;
            TagGrafico = tagGrafico;
        }
    }
}