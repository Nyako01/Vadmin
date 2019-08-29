CREATE TABLE IF NOT EXISTS `user_banlist` (
  `Name` varchar(50) NOT NULL,
  `SteamID` varchar(50) NOT NULL,
  `ExpiredDate` varchar(50) NOT NULL,
  `Permanent` int(11) NOT NULL,
  `Reason` varchar(50) NOT NULL,
  `Banned` int(11) NOT NULL
);

