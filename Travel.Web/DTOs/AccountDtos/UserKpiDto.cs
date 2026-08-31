namespace Travel.Web.DTOs.AccountDtos
{
    public class UserKpiDto
    {
        public int TotalUserCount { get; set; }
        public int ActiveUserCount { get; set; }
        public int PassiveUserCount { get; set; }
        public int TodayUserCount { get; set; }
    }
}
