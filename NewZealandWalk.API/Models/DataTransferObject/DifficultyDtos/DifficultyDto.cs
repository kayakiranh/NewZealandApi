namespace NewZealandWalk.API.Models.DataTransferObject.DifficultyDtos
{
    [Serializable]
    public record DifficultyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}