namespace Base.Application.CQRS.Commands
{
    public class ChangeActivatorsResponse
    {
        public int ResponseChange { get; set; } = 0;
        public bool? NewValue { get; set; }
        public List<string>? ResponseMessage { get; set; }
    }
}
