namespace NewZealandWalk.API.Models.DataTransferObjects.DifficultyDtos
{
    [Serializable]
    public record DifficultyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}