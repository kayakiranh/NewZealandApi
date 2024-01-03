namespace NewZealandWalk.API.Models.DataTransferObject.LoginDtos
{
    [Serializable]
    public record LoginResponseDto
    {
        public string JwtToken { get; set; }
    }
}