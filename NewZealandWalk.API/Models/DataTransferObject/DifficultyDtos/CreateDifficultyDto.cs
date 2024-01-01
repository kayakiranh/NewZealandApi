namespace NewZealandWalk.API.Models.DataTransferObject.DifficultyDtos
{
    [Serializable]
    public record CreateDifficultyDto
    {
        public string Name { get; set; }
    }
}