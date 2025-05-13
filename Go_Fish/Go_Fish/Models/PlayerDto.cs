namespace GoFish.Models
{
    public class PlayerDto
    {
        public Guid? UserId { get; set; } // null if computer player
        public string Name { get; set; } = default!;
        public List<CardDto> Cards { get; set; }
    }
}
