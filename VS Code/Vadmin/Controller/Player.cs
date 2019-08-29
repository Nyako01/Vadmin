namespace Vadmin.Models
{
    public class Player
    {
        public int ID { get; set; }
        public string[] PlayerData { get; set; }
        public string[] PlayerID { get; set; }
        public string[] PlayerName { get; set; }
        public string[] PlayerIP { get; set; }
        public string[] PlayerPing { get; set; }
        public string PlayerAction { get; set; }
        public string[] BanPlayerName { get; set; }
        public string[] BanPlayerExp { get; set; }
        public string[] BanPlayerReason { get; set; }
    }
}
