using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public static class Constants
    {
        public static int MAX_TABLE_SEATS = 6;
        public static string DEFULT_PIC_PLAYER = "emptyPlayer.jpg";
        public static string DEFULT_PIC_PLAYER_PATH = "imgs/players/seat";

        public static string DEFULT_PIC_CARD = "cardFliepd.png";
        public static string DEFULT_PIC_CARD_PATH = "imgs/cards/";

        public static string USER_NAME = "userName";
        public static string PASSWORD = "password";
        public static string EMAIL = "Email";
        public static string CURRENT_USER = "curent_user";
        public static string DUPLEX_CHANNEL = "Duplex_Channel";
        public static string PROXY = "proxy";
        

        //windows names
        public const string LOGIN_WINDOW = "login";
        public const string LOBBY_WINDOW = "loby";
        public const string TABLE_WINDOW = "table";
        public const string NEW_TABLE_WINDOW = "new_table";

        public const int FLOP1 = 0;
        public const int FLOP2 = 1;
        public const int FLOP3 = 2;
        public const int TERN = 3;
        public const int RIVER = 4;
        
    }
}
