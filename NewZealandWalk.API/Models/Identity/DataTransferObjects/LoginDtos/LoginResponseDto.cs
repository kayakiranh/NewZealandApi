﻿namespace NewZealandWalk.API.Models.Identity.DataTransferObjects.LoginDtos
{
    [Serializable]
    public record LoginResponseDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}