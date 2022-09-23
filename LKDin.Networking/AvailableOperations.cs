using System;
namespace LKDin.Networking
{
    public enum AvailableOperation
    {
        ACK = 00,
        ERR = 01,
        CREATE_USER = 11,
        CREATE_WORK_PROFILE = 12,
        ASSIGN_IMAGE_TO_WORK_PROFILE = 13,
        SEND_CHAT_MESSAGE = 14,
        CHECK_CHAT_MESSAGES_BY_RECEIVER_ID = 15,
        CHECK_CHAT_MESSAGES_BY_SENDER_ID = 16,
        MARK_MESSAGE_AS_READ = 17,
        SEARCH_PROFILES_BY_SKILLS = 18,
        SEARCH_PROFILES_BY_DESCRIPTION = 19,
        SHOW_WORK_PROFILE_BY_ID = 20
    }
}

