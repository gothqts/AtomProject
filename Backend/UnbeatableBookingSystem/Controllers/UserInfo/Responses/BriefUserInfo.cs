﻿namespace UnbeatableBookingSystem.Controllers.UserInfo.Responses;

public class BriefUserInfo
{
    public required Guid Id { get; set; }
    
    public required string Fio { get; set; }
    
    public required string AvatarImage { get; set; }
}