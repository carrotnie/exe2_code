using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.Common.Enum
{
    public enum BusinessCode
    {
        Success = 200,
        InputRequired = 400,
        UnknownServerError = 500,
        NotFound = 404,
        ExpireToken = 405,
        GET_DATA_SUCCESSFULLY = 2000,
        UPDATE_SUCCESSFULLY = 2010,
        EXCEPTION = 4000,
        SIGN_UP_SUCCESSFULLY = 2001,
        SIGN_UP_FAILED = 2002,
        EXISTED_USER = 2003,
        AUTH_NOT_FOUND = 401,
        WRONG_PASSWORD = 405,
        ACCESS_DENIED = 403,
        INSERT_SUCESSFULLY = 2005,
        ALREADY_FRIENDSHIP = 2004,
        FRIEND_REQUEST_NOT_FOUND = 402,
        CANCEL_SUCCESSFULLY = 2006,
        FRIENDSHIP_NOT_FOUND = 404,
        ALREADY_BLOCKED = 2007,
        BLOCK_NOT_FOUND = 406,
        NO_FRIEND_REQUESTS = 407,
        CONVERSATION_NOT_FOUND = 2008,
        MESSAGE_NOT_FOUND = 2009,
    }
}
