using System;
namespace LKDin.Networking
{
    public enum AvailableOperations
    {
        ACK = 00,
        ERR = 01,
        CREATE_USER = 11,
        CREATE_WORK_PROFILE = 12,
        ASSIGN_IMAGE_TO_WORK_PROFILE = 13,
        SEND_CHAT_MESSAGE = 14,
        CHECK_CHAT_MESSAGES = 15,
        SEARCH_PROFILES_BY_SKILS = 16,
        SEARCH_PROFILES_BY_DESCRIPTION = 17,
        SHOW_WORK_PROFILE_BY_ID = 18
    }
}

