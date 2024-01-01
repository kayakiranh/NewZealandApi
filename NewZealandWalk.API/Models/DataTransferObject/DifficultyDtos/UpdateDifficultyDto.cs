namespace NewZealandWalk.API.Models.DataTransferObject.DifficultyDtos
{
    [Serializable]
    public record UpdateDifficultyDto
    {
        public string Name { get; set; }
    }
}